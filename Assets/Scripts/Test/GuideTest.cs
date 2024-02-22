using UnityEngine;
using UnityEngine.UI;

public class GuideTest : MonoBehaviour
{
    Material material;
    public GameObject obj;
    Vector2 screenPos;
    Camera c;
    public Canvas canvas;
    private void Start()
    {
        material = GetComponent<Image>().material;
        c = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        BoxSelect(obj.GetComponent<RectTransform>());
        print(screenPos);
    }

    void BoxSelect(RectTransform rect)
    {

        //print(rect.localPosition);
        screenPos = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, rect.position)-new Vector2(960,540);
        material.SetVector("_Center", screenPos);
        material.SetFloat("_SliderX", rect.rect.width/2);
        material.SetFloat("_SliderY", rect.rect.height/2);
    }
}
