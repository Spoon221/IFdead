using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;

public class MasterRoom : MonoBehaviourPunCallbacks,IPunObservable
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private SpawnManagerForPlayer forPlayer;

    public List<int> generatedNumbers = new List<int>();
    private bool isManiacSpawned = false;
    [SerializeField] private int randomNumber;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(generatedNumbers);
            stream.SendNext(isManiacSpawned);
        }
        else
        {
            generatedNumbers = (List<int>)stream.ReceiveNext();
            isManiacSpawned = (bool)stream.ReceiveNext();
        }
    }

    private void Awake()
    {
        //PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
        base.OnLeftRoom();
    }

    public void LoadLevel()
    {
        photonView.RPC("AssignRandomNumber", RpcTarget.AllBuffered, randomNumber);
        if (PhotonNetwork.IsMasterClient)
        {
            var props = new ExitGames.Client.Photon.Hashtable();
            props.Add("StartMatch", true);
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }
    }

    [PunRPC]
    private void AssignRandomNumber(int randomNumber)
    {
        randomNumber = GetUniqueRandomNumber();
        if (randomNumber == 1 && !isManiacSpawned)
        {
            PhotonNetwork.LocalPlayer.CustomProperties["NextScenePlayer"] = "Player1";
        }
        else
        {
            PhotonNetwork.LocalPlayer.CustomProperties["NextScenePlayer"] = "Player2";
        }
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("StartMatch"))
        {
            var startMatch = (bool)propertiesThatChanged["StartMatch"];
            if (startMatch)
            {
                PhotonNetwork.LoadLevel("GameArea");
            }
        }
    }

    public int GetUniqueRandomNumber()
    {
        var randomNumber = Random.Range(1, forPlayer.PlayerRoom + 1);
        while (generatedNumbers.Contains(randomNumber))
        {
            randomNumber = Random.Range(1, forPlayer.PlayerRoom + 1);
        }
        generatedNumbers.Add(randomNumber);
        return randomNumber;
    }
}
