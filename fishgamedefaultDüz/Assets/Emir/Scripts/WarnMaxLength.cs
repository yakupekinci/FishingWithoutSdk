using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarnMaxLength : MonoBehaviour
{
    [SerializeField] RopeTest ropeTest;
    void OnCollisionStay(Collision other)
    {
        ropeTest.WarnLength();
    }
}
