using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerReady : MonoBehaviourPunCallbacks
{
    private Dictionary<Player, bool> playerReadyStatus = new();

    [SerializeField] private Button startButton;
    [SerializeField] private Button readyButton;
    [SerializeField] private TMP_Text countText;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Update()
    {
        var readyCount = playerReadyStatus.Count;
        foreach (var entry in playerReadyStatus)
        {
            if (entry.Value) continue;
            readyCount--;
        }
        countText.text = $"{readyCount}/{PhotonNetwork.PlayerList.Length}";
        
        //CheckAllPlayersReady();
    }

    // Метод для получения списка имен готовых игроков
    public List<string> GetReadyPlayerNames()
    {
        List<string> readyPlayerNames = new();

        foreach (var entry in playerReadyStatus)
        {
            if (entry.Value)
            {
                readyPlayerNames.Add(entry.Key.NickName);
            }
        }

        return readyPlayerNames;
    }

    // Вызывается при нажатии кнопки готовности игрока
    public void ToggleReady()
    {
        readyButton.interactable = false;
        var isLocalPlayerReady = !playerReadyStatus.ContainsKey(PhotonNetwork.LocalPlayer) || !playerReadyStatus[PhotonNetwork.LocalPlayer];
        photonView.RPC("SetPlayerReady", RpcTarget.All, PhotonNetwork.LocalPlayer, isLocalPlayerReady);
    }

    // RPC-метод для установки готовности игрока
    [PunRPC]
    private void SetPlayerReady(Player player, bool ready)
    {
        // Обновляем статус готовности игрока
        playerReadyStatus[player] = ready;

        // Проверяем, все ли игроки готовы
        CheckAllPlayersReady();
    }

    // Проверяет, все ли игроки готовы
    private void CheckAllPlayersReady()
    {
        var allPlayersReady = true;
        var readyCount = playerReadyStatus.Count;

        foreach (var entry in playerReadyStatus)
        {
            if (entry.Value) continue;
            allPlayersReady = false;
            readyCount--;
        }

        countText.text = $"{readyCount}/{PhotonNetwork.PlayerList.Length}";

        // Если все игроки готовы, можно запустить игру
        if (allPlayersReady && PhotonNetwork.IsMasterClient
//#if !DEBUG
             //&& PhotonNetwork.PlayerList.Length > 1
//#endif
            )
        {
            startButton.interactable = true;
        }
    }

    public override void OnJoinedRoom()
    {
        //playerReadyStatus.TryAdd(newPlayer, false);

        foreach (var player in PhotonNetwork.PlayerList)
        {
            playerReadyStatus.TryAdd(player, false);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        countText.text = $"{playerReadyStatus.Count}/{PhotonNetwork.PlayerList.Length}";
        playerReadyStatus.TryAdd(newPlayer, false);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (playerReadyStatus.ContainsKey(otherPlayer))
        {
            playerReadyStatus.Remove(otherPlayer);
        }

        foreach (var player in playerReadyStatus.Keys.ToList())
        {
            playerReadyStatus[player] = false;
        }

        countText.text = $"{0}/{PhotonNetwork.PlayerList.Length}";
        readyButton.interactable = true;
        startButton.interactable = false;

    }
}