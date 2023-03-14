using System;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    private const float maxSpeed = 4f;
    private const float correctorIateralSpeed = 14f;
    private const float correctorFrontSpeed = 18f;

    [SerializeField] private Transform camera;

    [field: SerializeField] public int MaxHealth { get; private set; }
    private int currentHealth;
    [field: SerializeField] public float MaxMana { get; private set; }
    private float currentMana;

    private Rigidbody physicsPlayer;
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    public UnityEvent<int> OnHealthChanged = new UnityEvent<int>();
    public UnityEvent<float> OnManaChanged = new UnityEvent<float>();

    //PhotonView View;
    //public GameObject Camera;
    //public PlayerController scriptPlayerController;

    private void Awake()
    {
        //View = GetComponent<PhotonView>();
        //if (!View.IsMine)
        //{
        //Camera.SetActive(false);
        //scriptPlayerController.enabled = false;
        //}

        physicsPlayer = transform.parent.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        var axisX = Input.GetAxis("HorizontalX");
        var axisZ = Input.GetAxis("HorizontalZ");
        var direction = new Vector3(axisX, 0f, axisZ).normalized;
        var moveDirection = Vector3.zero; 

        if (direction.magnitude >= 0.1f)
        {
            var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,
                turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }

        if (physicsPlayer.velocity.magnitude < maxSpeed)
            physicsPlayer.AddRelativeForce(moveDirection.normalized * correctorFrontSpeed);
    }

    private void RestoreHealth(int amountOfHealth)
    {
        currentHealth += amountOfHealth;
        OnHealthChanged.Invoke(currentHealth);
    }

    private void GetDamage(int amountOfDamage)
    {
        currentHealth -= amountOfDamage;
        OnHealthChanged.Invoke(currentHealth);
    }
}