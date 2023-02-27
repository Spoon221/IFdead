using System;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    

    private const float maxSpeed = 4f;
    private const float correctorIateralSpeed = 14f;
    private const float correctorFrontSpeed = 18f;

    [field: SerializeField] public int MaxHealth { get; private set; }
    private int currentHealth;

    private Rigidbody physicsPlayer;

    private float axisX;
    private float axisZ;
    
    public UnityEvent<int> OnHealthChanged = new UnityEvent<int>();

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
        axisX = Input.GetAxis("HorizontalX");
        axisZ = Input.GetAxis("HorizontalZ");

        if (physicsPlayer.velocity.magnitude < maxSpeed)
        {
            physicsPlayer.AddRelativeForce(axisX * correctorIateralSpeed, 0f, axisZ * correctorFrontSpeed);
        }
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