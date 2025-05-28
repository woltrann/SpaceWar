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



    public void NextScene()
    {
        SceneManager.LoadScene(1);    
    }
}
