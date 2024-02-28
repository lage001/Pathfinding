using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.EventSystems;


public enum PlaceType
{
    Normal,
    SetStart,
    SetTarget,
}

public enum NormalType
{
    Single,
    Line,
    Rectangle,
}


public class BuildingCreator : SingletonBase<BuildingCreator>
{
    Tilemap previewMap;
    Tilemap dmap;
    Tilemap wmap;
    Tilemap fmap;

    TileBase tileBase;
    BuildingObjectBase selectedObj;
    Tilemap selectedMap;

    List<Tilemap> mapList;
    
    Camera _camera;

    Vector2 mousePos;
    Vector3Int gridPos;
    Vector3Int currentGridPosition;

    BoundsInt bounds;
    PlaceType placeType;
    NormalType normalType;


    bool holdActive;
    bool buildable = true;
    Vector3Int holdStartPosition;

    protected override void Awake()
    {
        base.Awake();
        previewMap = MapManager.Instance.tilemaps["PreviewMap"];

        dmap = MapManager.Instance.tilemaps["DefaultMap"];
        wmap = MapManager.Instance.tilemaps["WallMap"];
        fmap = MapManager.Instance.tilemaps["FloorMap"];
        mapList = new List<Tilemap>() 
        { 
            dmap,wmap,fmap,   
        };
        _camera = Camera.main;
        
    }
    private void OnEnable()
    {
        //playerInput.Enable();
        GameManager.Instance.playerInput.GamePlay.MousePosition.performed += OnMouseMove;
        GameManager.Instance.playerInput.GamePlay.MouseLeftClick.performed += OnLeftClick;
        GameManager.Instance.playerInput.GamePlay.MouseLeftClick.started += OnLeftClick;
        GameManager.Instance.playerInput.GamePlay.MouseLeftClick.canceled += OnLeftClick;
        GameManager.Instance.playerInput.GamePlay.MouseRightClick.performed += OnRightClick;

    }
    private void OnDisable()
    {
        //GameManager.Instance.playerInput.Disable();
        GameManager.Instance.playerInput.GamePlay.MousePosition.performed -= OnMouseMove;
        GameManager.Instance.playerInput.GamePlay.MouseLeftClick.performed -= OnLeftClick;
        GameManager.Instance.playerInput.GamePlay.MouseLeftClick.started -= OnLeftClick;
        GameManager.Instance.playerInput.GamePlay.MouseLeftClick.canceled -= OnLeftClick;
        GameManager.Instance.playerInput.GamePlay.MouseRightClick.performed -= OnRightClick;
    }
    public void SwitchBrush(NormalType type)
    {
        normalType = type;
    }
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.X))
        {
            normalType = NormalType.Rectangle;
            
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            normalType = NormalType.Line;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            normalType = NormalType.Single;
        }
        if (GameManager.Instance.fsm.currentStateType == GameState.Test)
        {
            selectedObj = null;
            previewMap.ClearAllTiles();
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            GameManager.Instance.playerInput.GamePlay.MouseLeftClick.Disable();
        }
        else
        {
            GameManager.Instance.playerInput.GamePlay.MouseLeftClick.Enable();
        }
        // Determine under what conditions to show preview
        //print(holdActive);

        if(selectedObj != null)
        {
            Vector3 pos = _camera.ScreenToWorldPoint(mousePos);
            
            gridPos = previewMap.WorldToCell(pos);

            //  Determine whether the mouse moves across cells, and if so, update the current grid position
            if (gridPos != currentGridPosition)
            {
                //lastGridPosition = currentGridPosition;
                currentGridPosition = gridPos;
                DrawItem(previewMap);
                if (holdActive)
                {
                    HandleDrawing(previewMap);
                }
            }
        }
        
    }
    #region Inputs
    void OnMouseMove(InputAction.CallbackContext ctx)
    {
        mousePos = ctx.ReadValue<Vector2>();
    }

    void OnLeftClick(InputAction.CallbackContext ctx)
    {
        // 
        if (selectedObj != null)
        {
            if (ctx.phase == InputActionPhase.Started)
            {
                holdActive = true;
                holdStartPosition = gridPos;
                HandleDrawing(previewMap);
            }
            else if (holdActive && ctx.phase == InputActionPhase.Canceled)
            {
                
                if (selectedObj == MapManager.Instance.tileDic["Eraser"])
                {
                    HandleDelete();
                    previewMap.ClearAllTiles();
                }
                else
                {
                    if (!buildable)
                    {
                        Functions.SetWarning("不能在起点或者终点处放置 !!");
                        print("不能在起点或者终点处放置！！");
                    }
                    else
                    {
                        HandleDrawing(selectedMap);
                        previewMap.ClearAllTiles();
                    }
                    
                }
                //Exit holding state
                holdActive = false;

                buildable = true;
                //Map has been modified
                MapManager.Instance.currentMap.legal = false;
            }
        }
    }

    void OnRightClick(InputAction.CallbackContext ctx) 
    {
        SelectedObj = null;
        holdActive = false;
    }
    #endregion

    #region Get tilebase and tilemap
    BuildingObjectBase SelectedObj
    {
        set
        {
            selectedObj = value;
            tileBase = selectedObj == null? null : selectedObj.TileBase;

            if(selectedObj == null)
            {
                placeType = PlaceType.Normal;
            }
            else if (selectedObj.Category == Category.Start)
            {
                placeType = PlaceType.SetStart;

            }
            else if(selectedObj.Category == Category.Target)
            {
                placeType = PlaceType.SetTarget;
            }
            else
            {
                placeType = PlaceType.Normal;
            }
            DrawItem(previewMap);
           
        }
    }
    Tilemap SelectedMap
    {
        set
        {
            selectedMap = value;
        }
    }
    public void ObjectSelected(BuildingObjectBase obj)
    {
        SelectedObj = obj;
    }
    public void TilemapSelected(Tilemap map)
    {
        SelectedMap = map;
    }
    #endregion

    #region Different ways of drawing and recording information
    void HandleDrawing(Tilemap map)
    {
        if(selectedObj != null)
        {
            switch (placeType)
            {
                case PlaceType.Normal:
                    switch (normalType)
                    {
                        case NormalType.Single:
                            DrawItem(map);
                            break;
                        case NormalType.Line:
                            DrawLine(map);
                            break;
                        case NormalType.Rectangle:
                            DrawRectangle(map);
                            break;
                        default:
                            break;
                    }
                    break;
                case PlaceType.SetStart:
                    DrawStart();
                    break;
                case PlaceType.SetTarget:
                    DrawTarget();
                    break;
            }
        }
    }

    //画初始位置
    void DrawStart()
    {
        
        if (mapList[1].HasTile(currentGridPosition))
        {
            
            Functions.SetWarning("起点不能在建筑物上 !!");
            print("起点不能在建筑物上！！");
        }
        else
        {   
            //保证初始位置唯一
            if (MapManager.Instance.currentMap.start.isSet)
            {
                DeleteOneTile(MapManager.Instance.currentMap.start.pos);
            }

            DrawOneTile(currentGridPosition, selectedMap);
            MapManager.Instance.currentMap.start.pos = currentGridPosition;
            MapManager.Instance.currentMap.start.isSet = true;
        }
        
    }
    //画目标位置
    void DrawTarget()
    {
        if (mapList[1].HasTile(currentGridPosition))
        {
            Functions.SetWarning("终点不能在建筑物上 !!");
            //createPanel.SetWarning("终点不能在建筑物上 !!");
            print("终点不能在建筑物上！！");
        }
        else
        {
            //保证目标位置唯一
            if (MapManager.Instance.currentMap.target.isSet)
            {
                DeleteOneTile(MapManager.Instance.currentMap.target.pos);
            }
            DrawOneTile(currentGridPosition, selectedMap);
            MapManager.Instance.currentMap.target.pos = currentGridPosition;
            MapManager.Instance.currentMap.target.isSet = true;
        }
        
    }
    void DrawOneTile(Vector3Int pos, Tilemap map)
    {
        MapManager.Instance.DrawOneTile(pos, selectedObj, map.name);
        if(selectedObj != null && selectedObj.Category == Category.Wall && holdActive)
        {
            buildable &= !mapList[0].HasTile(pos);
        }
        RecordTileInfoOnMap(map,pos);
    }
    void DrawBounds(Tilemap map)
    {
        for (int i = bounds.xMin; i <= bounds.xMax; i++)
        {
            for (int j = bounds.yMin; j <= bounds.yMax; j++)
            {
                DrawOneTile(new Vector3Int(i, j, 0), map);

            }
        }
    }
    void DrawItem(Tilemap map)
    {
        previewMap.ClearAllTiles();
        DrawOneTile(currentGridPosition, map);
    }
    void DrawRectangle(Tilemap map)
    {
        previewMap.ClearAllTiles();

        bounds.xMin = holdStartPosition.x < currentGridPosition.x ? holdStartPosition.x : currentGridPosition.x;
        bounds.yMin = holdStartPosition.y < currentGridPosition.y ? holdStartPosition.y : currentGridPosition.y;
        bounds.xMax = holdStartPosition.x > currentGridPosition.x ? holdStartPosition.x : currentGridPosition.x;
        bounds.yMax = holdStartPosition.y > currentGridPosition.y ? holdStartPosition.y : currentGridPosition.y;

        DrawBounds(map);

    }
    void DrawLine(Tilemap map)
    {
        previewMap.ClearAllTiles();
        float diffX = Mathf.Abs(currentGridPosition.x - holdStartPosition.x);
        float diffY = Mathf.Abs(currentGridPosition.y - holdStartPosition.y);
        if (diffX <= diffY)
        {
            bounds.xMin = holdStartPosition.x;
            bounds.xMax = holdStartPosition.x;
            bounds.yMin = holdStartPosition.y < currentGridPosition.y ? holdStartPosition.y : currentGridPosition.y;
            bounds.yMax = holdStartPosition.y > currentGridPosition.y ? holdStartPosition.y : currentGridPosition.y;
        }
        else
        {
            bounds.yMin = holdStartPosition.y;
            bounds.yMax = holdStartPosition.y;
            bounds.xMin = holdStartPosition.x < currentGridPosition.x ? holdStartPosition.x : currentGridPosition.x;
            bounds.xMax = holdStartPosition.x > currentGridPosition.x ? holdStartPosition.x : currentGridPosition.x;
        }

        DrawBounds(map);
    }
    
    void RecordTileInfoOnMap(Tilemap map,Vector3Int pos)
    {
        if (map != previewMap)
        {
            TileInfo tileInfo = new(tileBase.name,currentGridPosition, selectedObj.walkable);
            string strPos = pos.x + "," + pos.y;
            MapManager.Instance.currentMap.tilemapDic[map.name].tiles[strPos] = tileInfo;
        }
    }
    #endregion

    #region Different ways of deleting and deleting information
    void HandleDelete()
    {
        if (selectedObj != null)
        {
            switch (normalType)
            {
                case NormalType.Single:
                    DeleteItem();
                    break;
                case NormalType.Line:
                    DeleteLine();
                    break;
                case NormalType.Rectangle:
                    DeleteRectangle();
                    break;
                default:
                    break;
            }
        }
    }
    
    
    
    void DeleteOneTile(Vector3Int pos)
    {
        foreach (Tilemap map in mapList)
        {
            if (map.HasTile(pos))
            {
                MapManager.Instance.EraseOneTile(pos, map.name);
                DeleteTileInfoFromMap(map,pos);
                return;
            }
        }
    }
    void DeleteBounds()
    {
        for (int i = bounds.xMin; i <= bounds.xMax; i++)
        {
            for (int j = bounds.yMin; j <= bounds.yMax; j++)
            {
                DeleteOneTile(new Vector3Int(i, j, 0));
            }
        }
    }
    void DeleteItem()
    {
        previewMap.ClearAllTiles();
        DeleteOneTile(currentGridPosition);
    }
    void DeleteRectangle()
    {
        previewMap.ClearAllTiles();

        bounds.xMin = holdStartPosition.x < currentGridPosition.x ? holdStartPosition.x : currentGridPosition.x;
        bounds.yMin = holdStartPosition.y < currentGridPosition.y ? holdStartPosition.y : currentGridPosition.y;
        bounds.xMax = holdStartPosition.x > currentGridPosition.x ? holdStartPosition.x : currentGridPosition.x;
        bounds.yMax = holdStartPosition.y > currentGridPosition.y ? holdStartPosition.y : currentGridPosition.y;

        DeleteBounds();

    }
    
    void DeleteLine()
    {
        previewMap.ClearAllTiles();
        float diffX = Mathf.Abs(currentGridPosition.x - holdStartPosition.x);
        float diffY = Mathf.Abs(currentGridPosition.y - holdStartPosition.y);
        if (diffX <= diffY)
        {
            bounds.xMin = holdStartPosition.x;
            bounds.xMax = holdStartPosition.x;
            bounds.yMin = holdStartPosition.y < currentGridPosition.y ? holdStartPosition.y : currentGridPosition.y;
            bounds.yMax = holdStartPosition.y > currentGridPosition.y ? holdStartPosition.y : currentGridPosition.y;
        }
        else
        {
            bounds.yMin = holdStartPosition.y;
            bounds.yMax = holdStartPosition.y;
            bounds.xMin = holdStartPosition.x < currentGridPosition.x ? holdStartPosition.x : currentGridPosition.x;
            bounds.xMax = holdStartPosition.x > currentGridPosition.x ? holdStartPosition.x : currentGridPosition.x;
        }

        DeleteBounds();
    }
    void DeleteTileInfoFromMap(Tilemap map,Vector3Int pos)
    {
        if (map != previewMap)
        {
            string strPos = pos.x + "," + pos.y;
            
            MapManager.Instance.currentMap.tilemapDic[map.name].tiles.Remove(strPos);
            if(pos == MapManager.Instance.currentMap.start.pos)
            {
                MapManager.Instance.currentMap.start.isSet = false;
            }
            if (pos == MapManager.Instance.currentMap.target.pos)
            {
                MapManager.Instance.currentMap.target.isSet = false;
            }
        }
    }
    #endregion
}
