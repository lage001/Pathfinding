using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using NavMeshPlus.Components;


public class CreatePanel : BasePanel
{
    Button setStartBtn;
    Button setTargetBtn;
    Button saveBtn;
    Button loadBtn;
    Button clearBtn;

    Button pointBtn;
    Button lineBtn;
    Button surfaceBtn;

    Button wallTileBtn;
    Button floorTileBtn;
    Button eraserBtn;

    Button testBtn;
    Button hintBtn;
    Button homeBtn;

    Canvas canvas;

    public Material material;
    Transform mask;
    GameObject warnImagePrefab;

    NavMeshSurface navMesh;
    private void Awake()
    {
        InitUI();
        InitLiseners();
        InitGuideSys();
        warnImagePrefab = Resources.Load<GameObject>("Prefabs/WarnImage/Image");
        
    }
    private void OnEnable()
    {
        navMesh = GameObject.Find("Navmesh").GetComponent<NavMeshSurface>();
    }
    void InitUI()
    {
        canvas = GetComponentInParent<Canvas>();

        setStartBtn = transform.Find("UpRight/LoadAndSave/SetStartBtn").GetComponent<Button>();
        setTargetBtn = transform.Find("UpRight/LoadAndSave/SetTargetBtn").GetComponent<Button>();
        saveBtn = transform.Find("UpRight/LoadAndSave/SaveBtn").GetComponent<Button>();
        loadBtn = transform.Find("UpRight/LoadAndSave/LoadBtn").GetComponent<Button>();
        clearBtn = transform.Find("UpRight/LoadAndSave/ClearBtn").GetComponent<Button>();

        pointBtn = transform.Find("MidLeft/SwitchBrushes/PointBtn").GetComponent<Button>();
        lineBtn = transform.Find("MidLeft/SwitchBrushes/LineBtn").GetComponent<Button>();
        surfaceBtn = transform.Find("MidLeft/SwitchBrushes/SurfaceBtn").GetComponent<Button>();

        testBtn = transform.Find("TestBtn").GetComponent<Button>();
        hintBtn = transform.Find("TopMid/Hint/HintBtn").GetComponent<Button>();
        homeBtn = transform.Find("DownRight/HomeBtn").GetComponent<Button>();


        wallTileBtn = transform.Find("Categories/Wall/Display/items/WallTileBtn").GetComponent<Button>();
        floorTileBtn = transform.Find("Categories/Floor/Display/items/FloorTileBtn").GetComponent<Button>();
        eraserBtn = transform.Find("Categories/Eraser").GetComponent<Button>();

        mask = transform.Find("Mask");
    }
    void InitLiseners()
    {
        setStartBtn.onClick.AddListener(OnClickSetStartBtn);
        setTargetBtn.onClick.AddListener(OnClickSetTargetBtn);
        wallTileBtn.onClick.AddListener(OnClickWallTileBtn);
        floorTileBtn.onClick.AddListener(OnClickFloorTileBtn);
        eraserBtn.onClick.AddListener(OnClickEraserBtn);

        pointBtn.onClick.AddListener(() => { BuildingCreator.Instance.SwitchBrush(NormalType.Single); });
        lineBtn.onClick.AddListener(() => { BuildingCreator.Instance.SwitchBrush(NormalType.Line); });
        surfaceBtn.onClick.AddListener(() => { BuildingCreator.Instance.SwitchBrush(NormalType.Rectangle); });

        homeBtn.onClick.AddListener(OnClickHomeBtn);
        hintBtn.onClick.AddListener(OnClickHintBtn);
        testBtn.onClick.AddListener(OnClickTestBtn);

        saveBtn.onClick.AddListener(OnClickSaveBtn);
        loadBtn.onClick.AddListener(OnClickLoadBtn);
        clearBtn.onClick.AddListener(OnClickClearBtn);
    }

    void InitGuideSys()
    {
        mask.gameObject.SetActive(false);
        if (GameManager.Instance.playerData.firstTimeCreate)
        {
            mask.gameObject.SetActive(true);
            Functions.SetMaskField(Vector2.zero, 0, 0, material);
        }
        
    }

    void OnClickSetStartBtn()
    {
        Tilemap tilemap = MapManager.Instance.tilemaps["DefaultMap"];
        BuildingObjectBase item = MapManager.Instance.tileDic["Start"];
        BuildingCreator.Instance.ObjectSelected(item);
        BuildingCreator.Instance.TilemapSelected(tilemap);
    }
    void OnClickSetTargetBtn()
    {
        Tilemap tilemap = MapManager.Instance.tilemaps["DefaultMap"];
        BuildingObjectBase item = MapManager.Instance.tileDic["Target"];
        BuildingCreator.Instance.ObjectSelected(item);
        BuildingCreator.Instance.TilemapSelected(tilemap);
    }
    void OnClickLoadBtn()
    {
        UIManager.Instance.OpenPanel(UIConst.LoadPanel);

    }
    void OnClickSaveBtn()
    {
        if(MapManager.Instance.currentMap.target.isSet && MapManager.Instance.currentMap.start.isSet)
        {  
            UIManager.Instance.OpenPanel(UIConst.SavePanel);
        }
        if (!MapManager.Instance.currentMap.target.isSet)
        {
            SetWarning("请设置终点 !!");
            Debug.Log("请设置终点");
        }
        if (!MapManager.Instance.currentMap.start.isSet)
        {
            SetWarning("请设置出发点 !!", -350);
            Debug.Log("请设置出发点");
        }
        
    }
    void OnClickClearBtn()
    {
        MapManager.Instance.Clear();
    }

    void OnClickTestBtn()
    {
        if (!MapManager.Instance.currentMap.start.isSet)
        {
            SetWarning("请设置出发点 !!", -350);
            Debug.Log("请设置出发点");
        }
        else
        {
            GameManager.Instance.fsm.SwitchState(GameState.Test);
            navMesh.BuildNavMesh();
        }
    }
    private void OnClickHintBtn()
    {
        mask.gameObject.SetActive(true);
        Functions.SetMaskField(Vector2.zero, 0, 0, material);
    }
    void OnClickHomeBtn()
    {
        GameManager.Instance.fsm.SwitchState(GameState.Menu);
    }

    void OnClickWallTileBtn()
    {
        Tilemap tilemap = MapManager.Instance.tilemaps["WallMap"];
        BuildingObjectBase item = MapManager.Instance.tileDic["White"];
        BuildingCreator.Instance.ObjectSelected(item);
        BuildingCreator.Instance.TilemapSelected(tilemap);
    }
    void OnClickFloorTileBtn()
    {
        Tilemap tilemap = MapManager.Instance.tilemaps["FloorMap"];
        BuildingObjectBase item = MapManager.Instance.tileDic["Black"];
        BuildingCreator.Instance.ObjectSelected(item);
        BuildingCreator.Instance.TilemapSelected(tilemap);
    }
    void OnClickEraserBtn()
    {
        BuildingObjectBase item = MapManager.Instance.tileDic["Eraser"];
        BuildingCreator.Instance.ObjectSelected(item);
    }

    public void BoxSelectUI(RectTransform UItoSelect)
    {
        Vector3[] coners = new Vector3[4];
        UItoSelect.GetWorldCorners(coners);
        Vector3 topLeft = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, coners[1]);
        Vector3 bottomRight = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, coners[3]);

        Vector2 screenPos = (new Vector2((topLeft.x+bottomRight.x)/2, (topLeft.y + bottomRight.y)/2) - new Vector2(Screen.width/2, Screen.height/2))* (1920f/ (float)Screen.width);
        
        Functions.SetMaskField(screenPos, UItoSelect.rect.width * UItoSelect.localScale.x, UItoSelect.rect.height * UItoSelect.localScale.x, material);
    }

    public void SetWarning(string content,int pos=default)
    {
        if (pos == default)
            pos = -Screen.height / 2 + 80;
        else
            pos = -Screen.height / 2 + 540 + pos;
        GameObject obj = Instantiate(warnImagePrefab, transform);
        obj.GetComponent<RectTransform>().localPosition = Vector3.up * pos;
        TMPro.TextMeshProUGUI text = obj.transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>();
        text.text = content;
    }
}
