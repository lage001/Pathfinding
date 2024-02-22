using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using UnityEditor;
using UnityEngine.Tilemaps;
using Astar;



public class JsonTest
{
    static TestData data;
    static TestData data1;
    static List<TestData> datalist;
    static Dictionary<string, TestData> datadic;
    static string path = Application.streamingAssetsPath + "/TheLastTest.json";
    static GameObject panel;

    [MenuItem("CMCmd/创建数据")]
    public static void CreateObj()
    {
        InitData();
        //datalist = new List<TestData>() { data, data1 };
        datadic = new Dictionary<string, TestData>()
        {
            { "first", data },
            {"second",data1 },
        };
        //MyDic dic = new MyDic();
        //dic.dic = datadic;
        MyFileHandler.SaveToJSON(datadic, "testC.json");

        //MyFileHandler.SaveToJSON<TestData>(dic, "testB.json");
        //string json = JsonConvert.SerializeObject(datalist);
        //Debug.Log(path);
        ////FileStream fileStream = new FileStream(path, FileMode.Create);
        //using (StreamWriter writer = new StreamWriter(path))
        //{
        //    writer.Write(json);
        //}
    }
    [MenuItem("CMCmd/读取数据")]
    public static void ReadObj()
    {
        //string json;
        //using (StreamReader reader = new StreamReader(path))
        //{
        //    json = reader.ReadToEnd();
        //}
        //List<TestData> readData = JsonConvert.DeserializeObject<List<TestData>>(json);
        //List<TestData> readData = MyFileHandler.ReadListFromJSON<TestData>("testA.json");
        //TestData readData = MyFileHandler.ReadFromJSON<TestData>("testB.json");
        Dictionary<string,TestData> readData = MyFileHandler.ReadFromJSON<Dictionary<string, TestData>>("testC.json");
        Debug.Log(readData["first"].dic[new List<int>() { 0, 1, 2 }].name);
    }
    static void  InitData()
    {
        data = new TestData();
        data1 = new TestData();
        data.id = 0;
        data.list = new List<int>()
        {
            1,2,3
        };
        data.dic = new Dictionary<List<int>, MyTest>()
        {
            {new List<int>(){0,1,2 },new MyTest(0,"零")},
            {new List<int>(){0,2,2 } ,new MyTest(1,"一")},
            {new List<int>(){0,1,3 },new MyTest(2,"二")},
        };
        data.isOn = false;

        data1.id = 1;
        data1.list = new List<int>()
        {
            1,2,3
        };
        data1.dic = new Dictionary<List<int>, MyTest>()
        {
            {new List<int>(){0,1,2 },new MyTest(3,"三")},
            {new List<int>(){0,1,3 },new MyTest(4,"四")},
            {new List<int>(){2,1,2 },new MyTest(5,"五")},
        };
        data1.isOn = true;
    }
    [MenuItem("CMCmd/关闭面板")]
    public static void ClosePanel()
    {
        
        Debug.Log(panel == null);
        if (panel.activeInHierarchy)
        {
            panel.SetActive(false);
        }
        else
        {
            panel.SetActive(true);
        }
    }
    [MenuItem("CMCmd/获取面板")]
    public static void GetPanel()
    {
        panel = GameObject.Find("Canvas");
    }
    [MenuItem("CMCmd/测试地图一")]
    public static void TestFunction()
    {
        //Map map = MyFileHandler.ReadFromJSON<Map>("/Maps/mapData" + string.Format("_{0}", 1) + ".json");
        List<Vector2> list = Astar.AStar.AStarFindWay(new Vector2(0, 0), new Vector2(0, 2), Check);
        //foreach (var item in list)
        //{
        //    Debug.Log(item);
        //}
    }
    public static bool Check(Vector2 pos)
    {
        if(pos.x == 0 && pos.y == 1)
        {
            return false;
        }
        return true;
    }
    [MenuItem("CMCmd/创造格子地图")]
    public static void InitGridPlane()
    {
        GameObject planePrefab = Resources.Load<GameObject>("Prefabs/Plane");
        Transform GridPlane = GameObject.Find("GridPlane").transform;
        BoundsInt bound;
        bound = new BoundsInt(-96, -54, 0, 192, 108, 0);
        for (int x = bound.xMin; x < bound.xMax; x++)
        {
            for (int y = bound.yMin; y < bound.yMax; y++)
            {
                var obj = UnityEngine.Object.Instantiate(planePrefab, GridPlane);
                obj.transform.position = new Vector3(x, y, 10);
                obj.transform.rotation = Quaternion.Euler(-90, 0, 0);
                obj.AddComponent<NavMeshModifier>();
                obj.AddComponent<NavMeshSourceTag>();
            }
        }
    }
    [MenuItem("CMCmd/测试A*")]
    public static void TestFunc()
    {
        List<Vector2> list = AStar.AStarFindWay(Vector2.zero, new Vector2(4, 4), Check_1);
        foreach (var item in list)
        {
            Debug.Log(item);
        }
    }

    static bool Check_1(Vector2 pos)
    {
        if (pos.x == 1 && pos.y == 0)
        {
            return false;
        }
        if (pos.x == 0 && pos.y == 1)
        {
            return false;
        }
        return true;
    }
}

[Serializable]
public class TestData
{
    public int id;
    public List<int> list;
    public Dictionary<List<int>, MyTest> dic;
    public bool isOn;
    public TileBase tile;
}
[Serializable]
public class MyTest
{
    public int id;
    public string name;
    public MyTest(int id, string name)
    {
        this.id = id;
        this.name = name;
    }
}
[Serializable]
public class MyDic
{
    public Dictionary<string, TestData> dic;
}
