using Cinemachine;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;


namespace KeyDownForPlayers
{
    public class KeyDownForPlayers : MonoBehaviour
    {
        [SerializeField] private Settings settingsMenu;
        [SerializeField] private ThirdPersonCameraController cameraController;
        [SerializeField] private PlayerMovementController playerMovementController;
        [SerializeField] private Camera cameraRender;
        [SerializeField] private ManiacMovementController maniacMovementController;
        [SerializeField] private Camera CameraManiac;
        [SerializeField] private Canvas canvasRender;

        [SerializeField] private PhotonView view;
        [SerializeField] private Text TextLobbyE;
        [SerializeField] private CinemachineVirtualCamera cameraOnTable;

        private void Start()
        {
            settingsMenu = GameObject.Find("Esc").GetComponent<Settings>();
            canvasRender = settingsMenu.GetComponent<Canvas>();
            canvasRender.renderMode = RenderMode.ScreenSpaceCamera;
            canvasRender.worldCamera = cameraRender;
        }

        public void ResumePlayers()
        {
            settingsMenu.SettingsClose();
            cameraController.cinemachineVirtualCamera.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            playerMovementController.enabled = true;
        }

        public void PausPlayers()
        {
            settingsMenu.SettingsOpen();
            cameraController.cinemachineVirtualCamera.enabled = false;
            playerMovementController.enabled = false;
        }

        public void ResumeManiac()
        {
            settingsMenu.SettingsClose();
            playerMovementController.enabled = true;
            CameraManiac.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void PausManiac()
        {
            settingsMenu.SettingsOpen();
            CameraManiac.enabled = false;
            playerMovementController.enabled = false;
        }


        public void ResumeItermediateScene()
        {
            TextLobbyE.enabled = true;
            playerMovementController.enabled = true;
            cameraController.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;

        }

        public void PauseItermediateScene()
        {
            TextLobbyE.enabled = false;
            playerMovementController.enabled = false;
            cameraController.enabled = false;
            Cursor.lockState = CursorLockMode.None;
        }

        public void SubsequentCanvasItermediateScene()
        {
            TextLobbyE.enabled = false;
            playerMovementController.enabled = false;
            cameraController.enabled = false;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}