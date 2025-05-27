using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class FirstCinematicScript : MonoBehaviour
{
    public static FirstCinematicScript Instance;
    public VideoPlayer videoPlayer;     // VideoPlayer component
    public GameObject rawImageParent;   // RawImage'in ba�l� oldu�u GameObject
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        rawImageParent.SetActive(true);

        videoPlayer.loopPointReached += OnVideoEnd;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))  // Ekrana t�klama
        {
            rawImageParent.SetActive(false);
            videoPlayer.Stop();
        }
    }

    

    void OnVideoEnd(VideoPlayer vp)
    {
        rawImageParent.SetActive(false);
    }
    public void NextScene()
    {
        SceneManager.LoadScene(1);    
    }
}
