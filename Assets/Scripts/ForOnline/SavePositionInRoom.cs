using Cinemachine;
using Photon.Pun;
using UnityEngine;

public static class PlayerHelper
{
    public const string PlayerPositionKey = "PlayerPosition";
    public const string PlayerRotationKey = "PlayerRotation";

    public static Quaternion GetPlayerRotation()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(PlayerRotationKey, out object rotation) && rotation is Quaternion)
        {
            return (Quaternion)rotation;
        }

        return Quaternion.identity;
    }

    public static Vector3 GetPlayerPosition()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(PlayerPositionKey, out object position) && position is Vector3)
        {
            return (Vector3)position;
        }

        return Vector3.zero;
    }

    public static void SpawnPlayerLobby(GameObject player, CinemachineVirtualCamera cameraOnTable, GameObject playerModel)
    {
        Cursor.lockState = CursorLockMode.None;
        var spawnPosition = GetPlayerPosition();
        var spawnRotation = GetPlayerRotation();
        player.transform.position = spawnPosition;
        playerModel.transform.rotation = spawnRotation;
        cameraOnTable.enabled = true;
    }

    public static void SavePlayerPosition(GameObject player, GameObject playerModel)
    {
        var playerPosition = player.transform.position;
        PhotonNetwork.LocalPlayer.CustomProperties[PlayerPositionKey] = playerPosition;
        var playerRotation = playerModel.transform.rotation;
        PhotonNetwork.LocalPlayer.CustomProperties[PlayerRotationKey] = playerRotation;
    }
}