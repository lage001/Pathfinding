using UnityEngine;

public static class Functions
{
    public static void SetMaskField(Vector2 center,float width, float height,Material material)
    {
        material.SetFloat("_SliderX", width/2);
        material.SetFloat("_SliderY", height/2);
        material.SetVector("_Center", center);
    }
}
