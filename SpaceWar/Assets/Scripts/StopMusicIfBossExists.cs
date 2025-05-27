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
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name.StartsWith("Boss"))
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
                break;
            }
        }
    }



}
