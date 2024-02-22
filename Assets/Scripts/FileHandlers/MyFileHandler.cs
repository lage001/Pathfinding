using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public static class MyFileHandler
{
    public static T GetFile<T>(string filename) where T : new()
    {
        T item;
        if (!File.Exists(GetPath(filename)))
        {
            item = new T();
        }
        else
        {
            item = ReadFromJSON<T>(filename);
        }
        return item;
    }
    public static void SaveToJSON<T>(List<T> toSave, string filename)
    {
        string json = JsonConvert.SerializeObject(toSave);
        
        WriteFile(GetPath(filename), json);
    }
    public static void SaveToJSON<T>(T toSave, string filename)
    {
        string json = JsonConvert.SerializeObject(toSave);
        WriteFile(GetPath(filename), json);
    }
    public static void DeleteFile(string filename)
    {
        File.Delete(GetPath(filename));
    }
    public static List<T> ReadListFromJSON<T>(string filename)
    {
        string json = ReadFile(GetPath(filename));
        if (string.IsNullOrEmpty(json) || json == "{}")
        {
            return new List<T>();
        }
        List<T> res = JsonConvert.DeserializeObject<List<T>>(json);
        return res;
    }
    public static T ReadFromJSON<T>(string filename)
    {
        string json = ReadFile(GetPath(filename));
        if (string.IsNullOrEmpty(json) || json == "{}")
        {
            return default;
        }
        T res = JsonConvert.DeserializeObject<T>(json);
        return res;
    }


    private static string GetPath(string filename)
    {
        return Application.streamingAssetsPath + "/" + filename;
    }
    private static void WriteFile(string path, string json)
    {
        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.Write(json);
        }

    }
    private static string ReadFile(string path)
    {
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string content = reader.ReadToEnd();
                return content;
            }
        }
        return "";
    }
}
