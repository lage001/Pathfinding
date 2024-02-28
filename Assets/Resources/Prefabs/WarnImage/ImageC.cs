using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageC : MonoBehaviour
{
    public float dis;
    public float speed;
    RectTransform rect;

    private void Start()
    {
        rect = transform.GetComponent<RectTransform>();
        Destroy(gameObject,2);
    }
    public void FixedUpdate()
    {
        rect.localPosition += Vector3.up * 40 * Time.fixedDeltaTime;
    }
}
