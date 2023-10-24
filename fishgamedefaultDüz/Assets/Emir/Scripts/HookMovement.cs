using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class HookMovement : MonoBehaviour
{
    public Action<bool> OnWorkingStateChanged;

    public Rigidbody rb;
    Joystick joystick;


    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] Float hookMoveSpeedSO;
    bool isWorking;

    void Awake()
    {
        joystick = FindObjectOfType<Joystick>();
    }

    void Update()
    {
        
        Vector2 dir = new Vector2(joystick.Horizontal, joystick.Vertical);

        if (dir.x == 0 && dir.y == 0)
        {
            dir.x = Input.GetAxisRaw("Horizontal");
            dir.y = Input.GetAxisRaw("Vertical");
        }

        HandleWorkState(dir);

        if (!isWorking)
            return;


        HandleWork(dir);
    }



    private void HandleWork(Vector2 dir)
    {
        dir.Normalize();

        // Calculate the angle in degrees between the up direction and the dir vector
        float targetRotation = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Calculate the new rotation by interpolating between the current rotation and the target rotation
        Quaternion targetQuaternion = Quaternion.Euler(0, 0, targetRotation);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetQuaternion, Time.deltaTime * rotationSpeed);

        // Set the velocity to move in the dir direction
        rb.velocity = dir * hookMoveSpeedSO.Value;
    }

    private void HandleWorkState(Vector2 dir)
    {
        if (dir.x != 0 || dir.y != 0)
        {
            if (!isWorking)
                StartWorking();
        }
        else
        {
            if (isWorking)
                StopWorking();
        }
    }

    private void StartWorking()
    {
        isWorking = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        rb.position = transform.position;
        OnWorkingStateChanged?.Invoke(isWorking);
    }

    public void StopWorking()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        isWorking = false;
        OnWorkingStateChanged?.Invoke(isWorking);
        rb.velocity = Vector3.zero;
    }

    public void DisableMovement()
    {
        enabled = false;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void EnableMovement()
    {
        enabled = true;
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.position = transform.position;
    }


}
