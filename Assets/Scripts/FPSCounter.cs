using UnityEngine;
using TMPro;

// Calculates and displays a smoothed Frames Per Second (FPS) counter.
public class FPSCounter : MonoBehaviour
{
    public TMP_Text fpsText;

    private float smoothedDeltaTime = 0.0f;

    void Update()
    {
        // Smooth deltaTime to prevent erratic UI updates at high mobile framerates
        smoothedDeltaTime += (Time.unscaledDeltaTime - smoothedDeltaTime) * 0.1f;

        // Threshold check to prevent divide-by-zero errors when frames render too quickly
        if (smoothedDeltaTime > 0.0001f)
        {
            float currentFPS = 1.0f / smoothedDeltaTime;
            fpsText.text = "FPS: " + Mathf.RoundToInt(currentFPS).ToString();
        }
    }
}