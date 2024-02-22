using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class MapManager : SingletonBase<MapManager>
{
    public Map currentMap;
    
    public Vector3Int startPos;
    public Vector3Int targetPos;

    string filename = "/Maps/mapData";
    string path = Application.streamingAssetsPath + "/Maps";

    // 存储场上的所有tilemap
    public Dictionary<string, Tilemap> tilemaps;

    // 存储所有要用到的tile信息
    public Dictionary<string, BuildingObjectBase> tileDic;
    //public Dictionary<string, >

    //Dictionary<int, Map> mapDic;
    public List<string> mapNameList;
    public Transform VirtualMap;
    public Dictionary<Vector3, GameObject> obstacleDic;
    GameObject obstaclePrefab;
    protected override void Awake()
    {
        base.Awake();
        InitMap();
        InitVirtualMap();
        GetTilemapDic();
        GetTileDic();
        GetMapNameList();
    }
    private void Update()
    {
        //print(tilemaps.Count)
    }
    void InitVirtualMap()
    {
        if (GameManager.Instance.fsm.currentStateType == GameState.Create)
        {
            VirtualMap = GameObject.Find("VirtualMap").transform;
            obstaclePrefab = Resources.Load<GameObject>("Prefabs/Cube");
        }
        
        obstacleDic = new Dictionary<Vector3, GameObject>();
    }
    void InitMap()
    {
        TilemapInfo floorMap = new TilemapInfo("floorMap", new Dictionary<string, TileInfo>());
        TilemapInfo wallMap = new TilemapInfo("wallMap", new Dictionary<string, TileInfo>());
        TilemapInfo defaultMap = new TilemapInfo("defaultMap", new Dictionary<string, TileInfo>());
        Dictionary<string, TilemapInfo> tilemapDic = new Dictionary<string, TilemapInfo>
        {
            {"FloorMap",floorMap},
            {"WallMap",wallMap},
            {"DefaultMap",defaultMap},
        };
        currentMap = new Map(tilemapDic);
    }
    void GetTilemapDic()
    {
        tilemaps = new Dictionary<string, Tilemap>();
        Tilemap[] maps = FindObjectsOfType<Tilemap>();
        print(maps.Length);
        foreach (var map in maps)
        {
            tilemaps.Add(map.name, map);
        }
    }
    void GetTileDic()
    {
        tileDic = new Dictionary<string, BuildingObjectBase>();
        BuildingObjectBase tile_1 = Resources.Load<BuildingObjectBase>("Scriptables/Buildables/Floor/BlackFloor");
        BuildingObjectBase tile_2 = Resources.Load<BuildingObjectBase>("Scriptables/Buildables/Wall/WhiteWall");
        BuildingObjectBase tile_3 = Resources.Load<BuildingObjectBase>("Scriptables/Eraser");
        BuildingObjectBase tile_4 = Resources.Load<BuildingObjectBase>("Scriptables/Start");
        BuildingObjectBase tile_5 = Resources.Load<BuildingObjectBase>("Scriptables/Target");
        tileDic.Add("Black", tile_1);
        tileDic.Add("White", tile_2);
        tileDic.Add("Eraser", tile_3);
        tileDic.Add("Start", tile_4);
        tileDic.Add("Target", tile_5);
    }
    
    void GetMapNameList()
    {
        mapNameList = MyFileHandler.GetFile<List<string>>("MapNameList");
    }
    public void ClearMap(Map map,bool drawVirtualMap)
    {
        foreach (var (tilemapName, tilemapInfo) in map.tilemapDic)
        {
            foreach (var strPos in tilemapInfo.tiles.Keys)
            {
                string[] posList = strPos.Split(",");
                Vector3Int pos = new Vector3Int(int.Parse(posList[0]), int.Parse(posList[1]), 0);
                EraseOneTile(pos, tilemapName, drawVirtualMap);
            }
        }
    }
    public void Clear(bool drawVirtualMap)
    {
        ClearMap(currentMap, drawVirtualMap);
        InitMap();
        obstacleDic = new Dictionary<Vector3, GameObject>();
    }
    public void OnSave(string mapName)
    {
        if (mapNameList.Contains(mapName) && (mapName != currentMap.name))
        {
            print("这个名字已经用过了!!");
        }
        else 
        {
            if (!mapNameList.Contains(mapName))
                mapNameList.Add(mapName);
            currentMap.name = mapName;
            MyFileHandler.SaveToJSON(mapNameList, "MapNameList");
            MyFileHandler.SaveToJSON(currentMap, GetFileName(mapName));
        }
    }
    public void OnDelete(string mapName)
    {
        if (!mapNameList.Contains(mapName))
        {
            Debug.Log("找不到这个地图名!!");
        }
        else
        {
            mapNameList.Remove(mapName);
            MyFileHandler.SaveToJSON(mapNameList, "MapNameList");
            MyFileHandler.DeleteFile(GetFileName(mapName));
        }
    }

    public Map OnLoad(string mapName, bool drawVirtualMap)
    {
        currentMap = MyFileHandler.ReadFromJSON<Map>(GetFileName(mapName));
        DrawMap(currentMap, drawVirtualMap);
        return currentMap;
        //GameManager.Instance.panel.SetActive(false);
    }

    public void DrawMap(Map map,bool drawVirtualMap)
    {
        ClearMap(map, drawVirtualMap);
        foreach (var (tilemapName,tileMapData) in map.tilemapDic)
        {
            Dictionary<string, TileInfo> tiles = tileMapData.tiles;

            //Delete all tiles in scene
            tilemaps[tilemapName].ClearAllTiles();

            foreach (var strPos in tiles.Keys)
            {
                TileInfo tileInfo = tiles[strPos];
                string[] posList = strPos.Split(",");
                Vector3Int pos = new Vector3Int(int.Parse(posList[0]), int.Parse(posList[1]), 0);
                DrawOneTile(pos, tileDic[tileInfo.name], tilemapName, drawVirtualMap);
            }
        }
    }
    public Map DrawSelectedMap(string name,bool drawVirtualMap)
    {
        Map map = MyFileHandler.ReadFromJSON<Map>(GetFileName(name));
        DrawMap(map, drawVirtualMap);
        return map;
    }
    string GetFileName(string mapName)
    {
        return filename + string.Format("_{0}", mapName) + ".json";
    }

    public void DrawOneTile(Vector3Int pos,BuildingObjectBase objectBase, string tilemapName,bool drawVirtualMap)
    {
        Tilemap tilemap = tilemaps[tilemapName];
        TileBase tileBase = objectBase == null ? null : objectBase.TileBase;
        tilemap.SetTile(pos, tileBase);

        if(drawVirtualMap && tilemapName != "PreviewMap" && !objectBase.walkable && !obstacleDic.ContainsKey(pos))
        {
            Vector3 obstaclePos = pos + new Vector3(0.5f, 0.5f, 0);
            GameObject obstacle = Instantiate(obstaclePrefab, VirtualMap);
            obstacle.transform.position = obstaclePos;
            obstacleDic[pos] = obstacle;
        }
    }
    public void EraseOneTile(Vector3Int pos,string tilemapName,bool drawVirtualMap)
    {
        Tilemap tilemap = tilemaps[tilemapName];
        tilemap.SetTile(pos, null);
        if (drawVirtualMap)
        {
            GameObject obstacle;
            if (obstacleDic.TryGetValue(pos, out obstacle))
            {
                Destroy(obstacle);
                obstacleDic.Remove(pos);
            }
        }
    }
}

[Serializable]
public class SpecialTile
{
    public Vector3Int pos;
    public bool isSet = false;
}
[Serializable]
public class Map
{
    public int id;
    public string name;
    public SpecialTile start;
    public SpecialTile target;

    public Dictionary<string, TilemapInfo> tilemapDic;
    public Map(Dictionary<string, TilemapInfo> tilemapDic)
    {
        this.tilemapDic = tilemapDic;
        start = new SpecialTile();
        target = new SpecialTile();
    }
    public bool CheckWalkable(Vector2 pos)
    {
        //string strPos = pos.x + "," + pos.y;
        TileInfo tileInfo;
        foreach (var tilemap in tilemapDic.Values)
        {
            if (tilemap.TryGetTile(pos, out tileInfo) && !tileInfo.walkable)
                return false;
        }
        return true;
    }
}

[Serializable]
public class TilemapInfo
{
    // Every tilemap has properties of key which is the name of itself and tiles on it.
    public string key;
    public Dictionary<string, TileInfo> tiles;
    public TilemapInfo(String key, Dictionary<string, TileInfo> tiles)
    {
        this.key = key;
        this.tiles = tiles;
    }
    public bool TryGetTile(Vector3 pos,out TileInfo tileInfo)
    {
        string strPos = pos.x + "," + pos.y;
        return tiles.TryGetValue(strPos, out tileInfo);
    }
}
[Serializable]
public class TileInfo
{
    //  Every single tile has properties of tilebase and position.
    public string name;
    public Vector3Int pos;
    public bool walkable;

    //  Initialize the tilebase and position of this tile.
    public TileInfo(string name, Vector3Int pos, bool walkable)
    {
        this.name = name;
        this.pos = pos;
        this.walkable = walkable;
    }
}
