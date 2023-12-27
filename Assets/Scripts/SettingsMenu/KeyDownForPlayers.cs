using Cinemachine;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;


namespace KeyDownForPlayers
{
    public class KeyDownForPlayers : MonoBehaviour
    {
        [SerializeField] private Settings settingsMenu;
        [SerializeField] private CinemachineFreeLook cameraController;
        [SerializeField] private ThirdPersonCameraController thirdPerson;
        [SerializeField] private PlayerMovementController playerMovementController;
        [SerializeField] private Camera cameraRender;
        [SerializeField] private ManiacMovementController maniacMovementController;
        [SerializeField] private FisrtPersonCameraController maniacCamera;
        [SerializeField] private Canvas canvasRender;

        [SerializeField] private PhotonView view;
        [SerializeField] private Text TextLobbyE;

        private void Start()
        {
            if (view.IsMine)
            {
                settingsMenu = GameObject.Find("Esc").GetComponent<Settings>();
                canvasRender = settingsMenu.GetComponent<Canvas>();
                canvasRender.renderMode = RenderMode.ScreenSpaceCamera;
                canvasRender.worldCamera = cameraRender;
            }
        }

        public void ResumePlayers()
        {
            settingsMenu.SettingsClose();
            thirdPerson.enabled = true;
            playerMovementController.enabled = true;
        }

        public void PausPlayers()
        {
            settingsMenu.SettingsOpen();
            thirdPerson.enabled = false;
            playerMovementController.enabled = false;
        }

        public void ResumeManiac()
        {
            maniacMovementController.enabled = true;
            maniacCamera.enabled = true;
            settingsMenu.SettingsClose();
        }

        public void PausManiac()
        {
            maniacCamera.enabled = false;
            maniacMovementController.enabled = false;
            settingsMenu.SettingsOpen();
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