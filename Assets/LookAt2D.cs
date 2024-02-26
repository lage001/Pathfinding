using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt2D : MonoBehaviour
{

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 wordPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 targetPos = new Vector3(wordPoint.x, wordPoint.y, 0);
            Vector3 forward = transform.up + transform.right;
            LookAt2Dfunc(transform, forward, targetPos);
        }
        
    }
    void LookAt2Dfunc(Transform transform, Vector3 forward, Vector3 targetPos)
    {
        Vector3 targetDir = (targetPos - transform.position).normalized;//获得目标方向单位向量
        Vector3 forwardDir = forward.normalized;//获得forward反向的单位向量
        float theta = GetAngle(forwardDir, targetDir);//计算旋转角度和方向
        transform.rotation *= Quaternion.Euler(0, 0, theta);//绕着z轴旋转theta角度
    }

    float GetAngle(Vector3 forward, Vector3 targetDir)
    {
        float cosTheta = Mathf.Clamp(Vector3.Dot(forward, targetDir), -1, 1);//限制范围,否则可能会超出范围导致下一步报错  
        float Theta = Mathf.Acos(cosTheta) * 180 / Mathf.PI;//弧度制转角度制
        Vector3 right = new Vector3(forward.y, -forward.x, 0);//得到右方向
        float rotDir = Mathf.Sign(Vector3.Dot(targetDir, -right));//得到旋转方向
        return Theta * rotDir;
    }
}
