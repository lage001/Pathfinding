using System.Collections.Generic;
using UnityEngine;


public class UIManager : SingletonBase<UIManager>
{
    
    //����Ԥ����·�������ֵ�
    public Dictionary<string, string> pathDict;
    //����Ԥ���建���ֵ�
    public Dictionary<string, GameObject> prefabDict;
    //�Ѵ򿪽���Ļ����ֵ�
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
        //����Ƿ��Ѿ���
        if (panelDict.TryGetValue(name, out BasePanel panel))
        {
            return panel;
        }
        //���·���Ƿ�������
        if (!pathDict.TryGetValue(name, out string path))
        {
            Debug.LogError("�������ƴ��󣬻���δ����·����" + name);
            return null;
        }
        //ʹ�û����Ԥ����
        if (!prefabDict.TryGetValue(name, out GameObject panelPrefab))
        {   
            string realPath = "Prefabs/Panels/" + path;
            panelPrefab = Resources.Load<GameObject>(realPath);
            prefabDict.Add(name, panelPrefab);
        }
        //�򿪽���
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
            Debug.LogError("����δ�򿪣�" + name);
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



