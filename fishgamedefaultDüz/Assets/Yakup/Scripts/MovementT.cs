using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class MovementT : MonoBehaviour
{
    public float moveSpeed = 50;
    public float moveDistance = 400;
    public float slowSpeed = 20;
    public float slowRangeMin = 100;
    public float slowRangeMax = 300;
    public Vector3 GoAngles;
    public Vector3 TurnAngles;
    private Vector3 initialPosition;
    private float targetPositionX;
    private bool movingRight = true;
    private Rigidbody boatRigidBody;


    private void Start()
    {
        initialPosition = transform.position;
        targetPositionX = initialPosition.x + moveDistance;
        boatRigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        if (movingRight)
        {
            MoveRight();
        }
        else
        {
            MoveLeft();
        }
    }

    private void MoveRight()
    {
        if (transform.position.x > slowRangeMin && transform.position.x < slowRangeMax)
        {
            boatRigidBody.velocity = Vector3.right * slowSpeed;

        }
        else
        {
            boatRigidBody.velocity = Vector3.right * moveSpeed;
            transform.eulerAngles = GoAngles;
        }
        if (transform.position.x >= targetPositionX)
        {
            movingRight = false;
        }
    }
    private void MoveLeft()
    {
        if (transform.position.x < slowRangeMax && transform.position.x > slowRangeMin)
        {
            boatRigidBody.velocity = Vector3.left * slowSpeed;

        }
        else
        {
            boatRigidBody.velocity = Vector3.left * moveSpeed;
            transform.eulerAngles = TurnAngles;
        }

        if (transform.position.x <= initialPosition.x)
        {
            movingRight = true;
        }
    }
}
