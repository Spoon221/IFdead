using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CounerLeftGame : MonoBehaviourPun
{
    [SerializeField] private List<GameObject> Player;
    [SerializeField] private List<GameObject> Maniac;

    void Start()
    {
        var maniac = GameObject.FindGameObjectsWithTag("Maniac");
        foreach (var maniacInScene in maniac)
        {
            Maniac.Add(maniacInScene);
        }

        var player = GameObject.FindGameObjectsWithTag("Player");
        foreach (var playerInScene in player)
        {
            Maniac.Add(playerInScene);
        }
    }

    void Update()
    {
        
    }
}
