using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SecondCinematic : MonoBehaviour
{
    public static SecondCinematic Instance;
    public VideoPlayer videoPlayer;
    public GameObject rawImageParent;

    private bool videoPlaying = false;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        rawImageParent.SetActive(false);
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void Update()
    {
        if (videoPlaying && Input.GetMouseButtonDown(0))
        {
            videoPlayer.Stop();
            rawImageParent.SetActive(false);
            StartBattle();
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        rawImageParent.SetActive(false);
        StartBattle();
    }

    public void PlayVideoAndStartBattleAfter()
    {
        rawImageParent.SetActive(true);
        videoPlayer.Play();
        videoPlaying = true;
    }

    public void StartBattle()
    {
        GameManager.Instance.StartGame();
        videoPlaying = false;
    }
}
