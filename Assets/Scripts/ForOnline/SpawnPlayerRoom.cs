using UnityEngine;
using Photon.Pun;
using Cinemachine;

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

    public void GameCamera(GameObject spawnPlayer)
    {
        //Подумать над реализацией
        spawnPlayer.GetComponentInChildren<CinemachineFreeLook>().enabled = true;
    }
}
