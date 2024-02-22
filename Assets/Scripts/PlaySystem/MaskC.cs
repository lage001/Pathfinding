using System.Collections;
using UnityEngine;

public class MaskC : MonoBehaviour
{
    public Material material;
    public bool isMenu = false;
    bool open;
    float timerF;
    int timerI;
    float realPixelSize;
    // Update is called once per frame
    private void Start()
    {
        open = false;
    }
    private void Update()
    {
        realPixelSize = 60 * ((float)Screen.height / (float)Screen.width) * (1920f / 1080f);
    }
    void FixedUpdate()
    {
        if (isMenu)
        {
            timerF += Time.fixedDeltaTime;
            timerI = (int)Mathf.Floor(timerF);
            
            if (timerI % 6 == 0)
            {
                open = false;
            }
            if (timerI % 6 == 3)
            {
                open = true;
            }

            if (open)
            {
                Functions.SetMaskField(new Vector2(transform.position.x, transform.position.y) * realPixelSize, 3500f, 3500f, material);
            }
            else
            {
                Functions.SetMaskField(new Vector2(transform.position.x, transform.position.y) * realPixelSize, 3* realPixelSize, 3* realPixelSize, material);
            }
        }
        //print(transform.position);

    }
    public void MaskFollow(Vector3 screenPos)
    {
        Vector2 target = new Vector2(screenPos.x - Screen.width / 2, screenPos.y - Screen.height / 2) * (1920f / (float)Screen.width);
        if(!isMenu && !open)
            Functions.SetMaskField(target, 3 * realPixelSize, 3 * realPixelSize, material);
    }
    public void StartMaskAnim()
    {
        StartCoroutine(MaskAnim());
    }
    IEnumerator MaskAnim()
    {
        open = true;
        float width = material.GetFloat("_SliderX");
        float height = material.GetFloat("_SliderY");
        Vector2 center = material.GetVector("_Center");
        while (width < 3500 || height < 3500)
        {
            width += 1;
            height += 1;
            Functions.SetMaskField(center, width, height, material);
            yield return null;
        }
        
    }
}
