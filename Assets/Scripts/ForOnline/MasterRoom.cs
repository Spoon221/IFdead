using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MasterRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private SpawnManagerForPlayer forPlayer;

    public List<int> generatedNumbers = new List<int>();
    public bool isManiacSpawned = false;
    public int randomNumber;

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
        photonView.RPC("AssignRandomNumber", RpcTarget.AllBuffered);

        if (PhotonNetwork.IsMasterClient)
        {
            var props = new ExitGames.Client.Photon.Hashtable();
            props.Add("StartMatch", true);
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("IsManiacSpawned"))
        {
            isManiacSpawned = (bool)PhotonNetwork.CurrentRoom.CustomProperties["IsManiacSpawned"];
        }

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("GeneratedNumbers"))
        {
            generatedNumbers = new List<int>((int[])PhotonNetwork.CurrentRoom.CustomProperties["GeneratedNumbers"]);
        }
    }

    [PunRPC]
    private void AssignRandomNumber()
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

        var props = new ExitGames.Client.Photon.Hashtable();
        props.Add("IsManiacSpawned", isManiacSpawned);
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);

        photonView.RPC("SyncGeneratedNumbers", RpcTarget.AllBuffered, randomNumber);
    }

    [PunRPC]
    private void SyncGeneratedNumbers(int randomNumber)
    {
        generatedNumbers.Add(randomNumber);
        if (generatedNumbers.Count == PhotonNetwork.PlayerList.Length)
        {
            var props = new ExitGames.Client.Photon.Hashtable();
            props.Add("GeneratedNumbers", generatedNumbers.ToArray());
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }
    }

    public int GetUniqueRandomNumber()
    {
        var roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
        if (roomProperties.TryGetValue("GeneratedNumbers", out object generatedNumbersObj))
        {
            generatedNumbers = new List<int>((int[])generatedNumbersObj);
        }
        var randomNumber = Random.Range(1, forPlayer.PlayerRoom + 1);
        while (generatedNumbers.Exists(number => number == randomNumber))
        {
            randomNumber = Random.Range(1, forPlayer.PlayerRoom + 1);
        }
        generatedNumbers.Add(randomNumber);
        return randomNumber;
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
}
