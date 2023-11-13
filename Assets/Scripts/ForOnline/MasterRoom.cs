using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Cinemachine;
using System.Collections;

public class MasterRoom : MonoBehaviourPunCallbacks
{
    public Button startButton;
    public Button exitButton;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
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
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(LoadRoomSceneAsync());
        }
    }

    private IEnumerator LoadRoomSceneAsync()
    {
        var asyncLoad = SceneManager.LoadSceneAsync("GameArea");
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            Debug.Log("Загрузка сцены... Прогресс: " + (progress * 100) + "%");
            yield return null;
        }
        PhotonNetwork.LoadLevel("GameArea");
    }
}
