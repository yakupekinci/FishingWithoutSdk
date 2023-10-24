using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PpinterDouble : MonoBehaviour
{
    public Transform playerPos;
    public Transform otherPointer;
    public GameObject gfx;

    private void Awake()
    {
        int phase = PlayerPrefs.GetInt("pointerPhase", 0);
        if (phase == 14)
        {
            gameObject.SetActive(false);
            //  Destroy(gameObject);
        }
    }

    void LateUpdate()
    {
        if (!otherPointer.gameObject.activeSelf)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 pos = playerPos.position;
        transform.position = pos;
        pos.y = otherPointer.position.y;
        Vector3 delta = otherPointer.position - pos;
        if (delta.magnitude < 8f)
        {
            gfx.SetActive(false);
        }
        else
        {
            gfx.SetActive(true);
        }
        transform.rotation = Quaternion.LookRotation(delta.normalized, Vector3.up);
    }
}
