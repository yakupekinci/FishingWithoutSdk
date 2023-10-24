using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Collider coll;

    public float moveSpeed = 2f;
    // bool isFacingRight;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();

        bool isFacingRight = Random.Range(0, 2) == 1;
        transform.eulerAngles = isFacingRight ? new Vector3(0, 90f, 0) : new Vector3(0, -90f, 0);
        rb.velocity = transform.forward * moveSpeed;
    }
    void OnEnable()
    {
        coll.enabled = true;
        transform.SetParent(AreaManager.Instance.transform);

        transform.localScale = Vector3.one;
        // isFacingRight = transform.eulerAngles.y == 90;
        // transform.eulerAngles = Random.Range(0, 2) == 1 ? new Vector3(0, 90f, 0) : new Vector3(0, -90f, 0);
        rb.isKinematic = false;
        rb.velocity = transform.forward * Random.Range(moveSpeed-0.75f, moveSpeed+0.75f);

    }

    public void StopMovement()
    {
        coll.enabled = false;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            transform.eulerAngles = new Vector3(0, 90, 0);
            rb.velocity = transform.forward * Random.Range(moveSpeed-0.75f, moveSpeed+0.75f);
            return;
        }
        else if (other.CompareTag("WallRight"))
        {
            transform.eulerAngles = new Vector3(0, -90, 0);
            rb.velocity = transform.forward * Random.Range(moveSpeed-0.75f, moveSpeed+0.75f);
            return;
        }
    }
}
