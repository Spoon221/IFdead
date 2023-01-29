using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float maxSpeed = 4f;
    private const float correctorIateralSpeed = 14f;
    private const float correctorFrontSpeed = 18f;

    private Rigidbody physicsPlayer;

    private float axisX;
    private float axisZ;

    private void Awake()
    {
        physicsPlayer = transform.parent.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        axisX = Input.GetAxis("HorizontalX");
        axisZ = Input.GetAxis("HorizontalZ");

        if (physicsPlayer.velocity.magnitude < maxSpeed) 
        {
            physicsPlayer.AddRelativeForce(axisX * correctorIateralSpeed, 0f, axisZ * correctorFrontSpeed);
        }
    }
}
