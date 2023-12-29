using UnityEngine;
using UnityEngine.SceneManagement;

public class backgroundSound : MonoBehaviour
{
    private string gameScnemeName = "NewGameArea";
    [SerializeField] private GameObject backgroundAudio;
    void Start()
    {
        if (SceneManager.GetActiveScene().name != gameScnemeName)
        {
            DontDestroyOnLoad(backgroundAudio);
        }
    }

    public void OnDestroy()
    {
        Destroy(backgroundAudio);
    }
}
