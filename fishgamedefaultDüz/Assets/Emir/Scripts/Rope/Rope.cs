using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] Transform[] targetPosList;
    [SerializeField] Transform hookPos;
    [SerializeField] LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer.positionCount = targetPosList.Length + 1;    
    }

    void LateUpdate()
    {
        Vector3 targetPos = targetPosList[targetPosList.Length - 1].position;
        targetPos.y = ((hookPos.position - targetPosList[0].position) / 2).y;
        targetPosList[targetPosList.Length - 1].position = targetPos;

        int i = 0;
        for (i = 0; i < targetPosList.Length; i++)
        {
            lineRenderer.SetPosition(i, targetPosList[i].position);
        }
        
        lineRenderer.SetPosition(i, hookPos.position);
    }

    
}
