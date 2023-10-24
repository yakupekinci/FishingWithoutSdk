using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookYLimiter : MonoBehaviour
{
    [SerializeField] Float hookLength;
    [SerializeField] RopeTest ropeTest;
    [SerializeField] float delta = 0f;


    void OnEnable()
    {
        float val = hookLength.Value + 0.2f;
        transform.localPosition = new Vector3(transform.localPosition.x, -(val + 1 - delta), transform.localPosition.z);
    }
    
    void OnCollisionStay(Collision other)
    {
        ropeTest.WarnLength();
    }

}
