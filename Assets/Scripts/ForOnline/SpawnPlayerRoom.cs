using UnityEngine;
using Photon.Pun;

public class SpawnPlayerRoom : MonoBehaviour
{
    public GameObject[] Spawns;
    public GameObject Player;
    void Start()
    {
        var randomIndex = Random.Range(0, Spawns.Length);
        var randomPosition = Spawns[randomIndex].transform.position;
        var spawnPlayer = PhotonNetwork.Instantiate(Player.name, randomPosition, Quaternion.identity);
    }
}
