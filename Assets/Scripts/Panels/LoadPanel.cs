using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadPanel : BasePanel
{
    Button loadBtn;
    Button deleteBtn;
    Button cancelBtn;

    Transform root;
    GameObject BtnPrefab;

    string selectedMapName;
    Map selectedMap;
    private void Awake()
    {
        
        Transform items = transform.Find("Image/Display/items");
        
        cancelBtn = transform.Find("Image/Buttons/CancelBtn").GetComponent<Button>();
        deleteBtn = transform.Find("Image/Buttons/DeleteBtn").GetComponent<Button>();
        loadBtn = transform.Find("Image/Buttons/LoadBtn").GetComponent<Button>();


        root = transform.Find("Image/Display/items");
        BtnPrefab = Resources.Load<GameObject>("Prefabs/Panels/Game/OpenMapBtn");

        cancelBtn.onClick.AddListener(OnClickCancel);
        deleteBtn.onClick.AddListener(OnClickDelete);
        loadBtn.onClick.AddListener(OnClickLoad);
    }

    private void OnClickDelete()
    {
        if(selectedMapName != null)
        {
            MapManager.Instance.OnDelete(selectedMapName);
            MapManager.Instance.ClearMap(selectedMap, true);
            Refresh();
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.playerInput.CameraControl.Disable();
        Refresh();
    }
    private void OnDisable()
    {
        GameManager.Instance.playerInput.CameraControl.Enable();
    }
    void Refresh()
    {
        Clear();
        //string[] dirs = Directory.GetFileSystemEntries(path);

        int length = MapManager.Instance.mapNameList.Count;
        //print(length);
        for (int i = 0; i < length; i++)
        {
            int mapIndex = i;
            Button btn = Instantiate(BtnPrefab, root, false).GetComponent<Button>();
            string name = MapManager.Instance.mapNameList[mapIndex];
            
            btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "¡¶"+ name + "¡·";
            btn.onClick.AddListener(() => 
            {
                selectedMapName = name;
                selectedMap = MapManager.Instance.DrawSelectedMap(name,true);
            });
        }
    }

    void Clear()
    {
        Transform items = transform.Find("Image/Display/items");
        for (int i = 0; i < items.childCount; i++)
        {
            Destroy(items.GetChild(i).gameObject);
        }
    }
    void OnClickCancel()
    {
        UIManager.Instance.ClosePanel(UIConst.LoadPanel);
        //MapManager.Instance.ClearMap();
        MapManager.Instance.DrawMap(MapManager.Instance.currentMap,true);

    }
    void OnClickLoad()
    {
        if (selectedMapName!=null)
        {
            MapManager.Instance.OnLoad(selectedMapName, true);
            UIManager.Instance.ClosePanel(UIConst.LoadPanel);
        }
    }
}
