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
        Vector3 targetDir = (targetPos - transform.position).normalized;//���Ŀ�귽��λ����
        Vector3 forwardDir = forward.normalized;//���forward����ĵ�λ����
        float theta = GetAngle(forwardDir, targetDir);//������ת�ǶȺͷ���
        transform.rotation *= Quaternion.Euler(0, 0, theta);//����z����תtheta�Ƕ�
    }

    float GetAngle(Vector3 forward, Vector3 targetDir)
    {
        float cosTheta = Mathf.Clamp(Vector3.Dot(forward, targetDir), -1, 1);//���Ʒ�Χ,������ܻᳬ����Χ������һ������  
        float Theta = Mathf.Acos(cosTheta) * 180 / Mathf.PI;//������ת�Ƕ���
        Vector3 right = new Vector3(forward.y, -forward.x, 0);//�õ��ҷ���
        float rotDir = Mathf.Sign(Vector3.Dot(targetDir, -right));//�õ���ת����
        return Theta * rotDir;
    }
}
