using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    float deltaTime = 0.0f;
    void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0; // VSync kapalý, yoksa FPS sýnýrlar
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;
        GUIStyle style = new GUIStyle();
        Rect rect = new Rect(10, 10, w, h * 2 / 300);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 150;
        style.normal.textColor = Color.white;
        float fps = 1.0f / deltaTime;
        string text = string.Format("FPS: {0:0.}", fps);
        GUI.Label(rect, text, style);
    }
}
