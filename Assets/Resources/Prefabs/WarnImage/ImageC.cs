using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageC : MonoBehaviour
{
    Vector3 target;
    public float dis;
    public float speed;
    RectTransform rect;

    private void Start()
    {
        rect = transform.GetComponent<RectTransform>();
        target = rect.localPosition + Vector3.up * dis;
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
    public void FixedUpdate()
    {
        rect.localPosition += Vector3.up * 40 * Time.fixedDeltaTime;
    }
}
