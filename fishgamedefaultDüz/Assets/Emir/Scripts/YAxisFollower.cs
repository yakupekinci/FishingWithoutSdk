using UnityEngine;

public class YAxisFollower : MonoBehaviour
{
    public Transform target; // Assign your character's Transform to this field
    void LateUpdate()
    {
        if (target != null)
        {
            // Only update the Y-position of the follower object
            transform.position = new Vector3(transform.position.x, target.position.y, transform.position.z);
        }
    }
}
