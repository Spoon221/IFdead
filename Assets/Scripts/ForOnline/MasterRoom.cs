using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MasterRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private SpawnManagerForPlayer forPlayer;

    private Dictionary<int, int> generatedNumbers = new Dictionary<int, int>();
    public int randomNumber;

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
        photonView.RPC("AssignRandomNumber", RpcTarget.All);

        if (PhotonNetwork.IsMasterClient)
        {
            var props = new ExitGames.Client.Photon.Hashtable();
            props.Add("StartMatch", true);
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }
    }


    [PunRPC]
    private void AssignRandomNumber()
    {
        if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("IsPlayer1Assigned"))
        {
            var player1ActorNumber = GetPlayerWithLowestActorNumber();
            if (PhotonNetwork.LocalPlayer.ActorNumber == player1ActorNumber)
            {
                PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "IsPlayer1Assigned", true } });
                PhotonNetwork.LocalPlayer.CustomProperties["NextScenePlayer"] = "Player1";
            }
            else
            {
                PhotonNetwork.LocalPlayer.CustomProperties["NextScenePlayer"] = "Player2";
            }
        }
        else
        {
            PhotonNetwork.LocalPlayer.CustomProperties["NextScenePlayer"] = "Player2";
        }
    }

    private int GetPlayerWithLowestActorNumber()
    {
        var lowestPlayerActorNumber = int.MaxValue;
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.ActorNumber == lowestPlayerActorNumber || player.ActorNumber > lowestPlayerActorNumber || player.ActorNumber < lowestPlayerActorNumber)
            {
                lowestPlayerActorNumber = player.ActorNumber;
            }
        }
        return lowestPlayerActorNumber;
    }

    [PunRPC]
    private void SyncGeneratedNumbers(int playerActorNumber, int randomNumber)
    {
        generatedNumbers[playerActorNumber] = randomNumber;

        if (generatedNumbers.Count == PhotonNetwork.PlayerList.Length)
        {
            var props = new ExitGames.Client.Photon.Hashtable();
            props.Add("GeneratedNumbers", generatedNumbers);
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }
    }

    //public int GetUniqueRandomNumber()
    //{
    //    var roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
    //    if (roomProperties.TryGetValue("GeneratedNumbers", out object generatedNumbersObj))
    //    {
    //        generatedNumbers = (Dictionary<int, int>)generatedNumbersObj;
    //    }
    //    else
    //    {
    //        generatedNumbers = new Dictionary<int, int>();
    //    }

    //    var randomNumber = Random.Range(1, forPlayer.PlayerRoom + 1);
    //    while (generatedNumbers.ContainsValue(randomNumber))
    //    {
    //        randomNumber = Random.Range(1, forPlayer.PlayerRoom + 1);
    //    }

    //    generatedNumbers[PhotonNetwork.LocalPlayer.ActorNumber] = randomNumber;
    //    return randomNumber;
    //}

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
