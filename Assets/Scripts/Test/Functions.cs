using UnityEngine;

public static class Functions
{
    public static void SetMaskField(Vector2 center,float width, float height,Material material)
    {
        material.SetFloat("_SliderX", width/2);
        material.SetFloat("_SliderY", height/2);
        material.SetVector("_Center", center);
    }
    public static void SetWarning(string content, int pos = default)
    {
        if (pos == default)
            pos = -Screen.height / 2 + 80;
        else
            pos = -Screen.height / 2 + 540 + pos;
        Transform root = GameObject.Find("Canvas").transform;
        GameObject obj = Object.Instantiate(ConfigManager.Instance.warnPref,root);
        obj.GetComponent<RectTransform>().localPosition = Vector3.up * pos;
        TMPro.TextMeshProUGUI text = obj.transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>();
        text.text = content;
    }
}
