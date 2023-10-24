using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSource : Singleton<MusicSource>
{
    public Transform target;

    private void LateUpdate() {
        transform.position = target.position;
    }
}
