using UnityEngine;

public class StopMusicIfBossExists : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        GameObject bossObject = GameObject.Find("Boss(Clone)");

        if (bossObject != null)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                Debug.Log("Music stopped because Boss exists.");
            }
        }
        //else
        //{
        //    if (!audioSource.isPlaying)
        //    {
        //        audioSource.Play();
        //        Debug.Log("Music resumed because Boss is gone.");
        //    }
        //}
    }
}
