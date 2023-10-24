using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeChild : MonoBehaviour
{
    public Transform fakeParent;

    void LateUpdate()
    {
        transform.position = fakeParent.position;
        transform.eulerAngles = fakeParent.eulerAngles;
    }
}
