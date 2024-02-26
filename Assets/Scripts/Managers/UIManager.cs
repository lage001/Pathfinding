using System.Collections.Generic;
using UnityEngine;


public class UIManager : SingletonBase<UIManager>
{
    
    //界面预制体路径缓存字典
    public Dictionary<string, string> pathDict;
    //界面预制体缓存字典
    public Dictionary<string, GameObject> prefabDict;
    //已打开界面的缓存字典
    public Dictionary<string, BasePanel> panelDict;
    Transform _uiRoot;

    protected override void  Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

    }
    UIManager()
    {
        InitDicts();
    }

    public Transform UIRoot()
    {
        _uiRoot = GameObject.Find("Canvas").transform;
        return _uiRoot;
    }
    void InitDicts()
    {
        prefabDict = new Dictionary<string, GameObject>();
        panelDict = new Dictionary<string, BasePanel>();
        pathDict = new Dictionary<string, string>()
        {
            {UIConst.StartPanel,"Menu/StartPanel"},
            {UIConst.LoadPanel,"Game/LoadPanel"},
            {UIConst.SavePanel,"Game/SavePanel"},
            {UIConst.CreatePanel,"Game/CreatePanel"},
            {UIConst.TestPanel,"Game/TestPanel"},
            {UIConst.PlayPanel,"Game/PlayPanel"},
        };
    }
    public BasePanel OpenPanel(string name)
    {
        //检查是否已经打开
        if (panelDict.TryGetValue(name, out BasePanel panel))
        {
            return panel;
        }
        //检查路径是否有配置
        if (!pathDict.TryGetValue(name, out string path))
        {
            Debug.LogError("界面名称错误，或者未配置路径：" + name);
            return null;
        }
        //使用缓存的预制体
        if (!prefabDict.TryGetValue(name, out GameObject panelPrefab))
        {   
            string realPath = "Prefabs/Panels/" + path;
            panelPrefab = Resources.Load<GameObject>(realPath);
            prefabDict.Add(name, panelPrefab);
        }
        //打开界面
        GameObject panelObject = Instantiate(panelPrefab, UIRoot(), false);
        panelObject.SetActive(true);
        panel = panelObject.GetComponent<BasePanel>();
        panelDict.Add(name, panel);
        return panel;
    }


    public void ClosePanel(string name)
    {
        if(!panelDict.ContainsKey(name))
        {
            Debug.LogError("界面未打开：" + name);
            return;
        }
        GameObject gameObject = GameObject.Find(name + "(Clone)");
        gameObject.SetActive(false);
        Destroy(gameObject);
        panelDict.Remove(name);
    }

}
public class UIConst
{
    public const string StartPanel = "StartPanel";
    public const string LoadPanel = "LoadPanel";
    public const string SavePanel = "SavePanel";
    public const string CreatePanel = "CreatePanel";
    public const string TestPanel = "TestPanel";
    public const string PlayPanel = "PlayPanel";

}



