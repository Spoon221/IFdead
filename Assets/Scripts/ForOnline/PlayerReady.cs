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
    [SerializeField] private SpawnManagerForPlayer spawnManagerForPlayer;

    private void Update()
    {
        StartCoroutine(LateUpdateCountPlayers());
    }

    IEnumerator LateUpdateCountPlayers()
    {
        var readyCount = playerReadyStatus.Count;
        countText.text = $"{readyCount}/{spawnManagerForPlayer.PlayerRoom}";
        yield return new WaitForSeconds(1.5f);
    }

    // ����� ��� ��������� ������ ���� ������� �������
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

    // ���������� ��� ������� ������ ���������� ������
    public void ToggleReady()
    {
        readyButton.interactable = false;
        var isLocalPlayerReady = !playerReadyStatus.ContainsKey(PhotonNetwork.LocalPlayer) || !playerReadyStatus[PhotonNetwork.LocalPlayer];
        photonView.RPC("SetPlayerReady", RpcTarget.All, PhotonNetwork.LocalPlayer, isLocalPlayerReady);
    }

    // RPC-����� ��� ��������� ���������� ������
    [PunRPC]
    private void SetPlayerReady(Player player, bool ready)
    {
        // ��������� ������ ���������� ������
        playerReadyStatus[player] = ready;

        // ���������, ��� �� ������ ������
        CheckAllPlayersReady();
    }

    // ���������, ��� �� ������ ������
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

        countText.text = $"{readyCount}/{spawnManagerForPlayer.PlayerRoom}";

        // ���� ��� ������ ������, ����� ��������� ����
        if (allPlayersReady && PhotonNetwork.IsMasterClient)
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

        countText.text = $"{0}/{spawnManagerForPlayer.PlayerRoom}";
        readyButton.interactable = true;
        startButton.interactable = false;

    }
}