using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManager:SingletonBase<ConfigManager>
{
    public GameObject warnPref;
    public const string warnPath = "Prefabs/WarnImage/Image";
    public const string agentPath = "Prefabs/Agent";
    public const string playerPath = "Prefabs/Player";
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        GetPrefab();
    }
    void GetPrefab()
    {
        warnPref = Resources.Load<GameObject>(warnPath);
    }

}
