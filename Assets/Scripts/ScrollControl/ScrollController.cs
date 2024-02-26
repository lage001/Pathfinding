using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScrollController : MonoBehaviour
{
    ScrollRect scrollrect;
    
    public float posnum;
    public int initPos;
    protected float target
    {
        get
        {
            return intPos / (posnum - 1);
        }
    }
    float smoothing = 1;
    bool isShowing = false;

    protected float intPos;
    private void Awake()
    {
        scrollrect = GetComponent<ScrollRect>();
    }
    private void OnEnable()
    {
        intPos = initPos;
    }
    void FixedUpdate()
    {
        if(Mathf.Abs(scrollrect.horizontalNormalizedPosition - target)>0.001f)
        {
            smoothing += 2;
        }
        else
        {
            smoothing = 1;
        }
        scrollrect.horizontalNormalizedPosition = Mathf.Lerp(scrollrect.horizontalNormalizedPosition, target, Time.deltaTime * smoothing);

    }
    public void Onclick()
    {
        if (!isShowing)
        {
            intPos = 0;
            isShowing = true;
        }
        else
        {
            intPos = 1;
            isShowing = false;
        }
    }
}
