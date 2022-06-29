using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class LightEstimate : MonoBehaviour
{
    [SerializeField] ARCameraManager cameraManager;
    private Light light;

    void Start()
    {
        this.light = GetComponent<Light>();        
        cameraManager.frameReceived += OnCameraFrameReceived;
    }

    void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {
        Color color = Color.white;
        float intensity = 1.0f;

        if (eventArgs.lightEstimation.averageBrightness.HasValue)
        {
            intensity = eventArgs.lightEstimation.averageBrightness.Value;
            intensity *= 2.0f;
            if (intensity > 1) intensity = 1.0f;
        }
        if (eventArgs.lightEstimation.averageColorTemperature.HasValue)
        {
            color = Mathf.CorrelatedColorTemperatureToRGB(eventArgs.lightEstimation.averageColorTemperature.Value);
        }

        Color c = color * intensity;
        light.color = c;
        RenderSettings.ambientSkyColor = c;
    }

    private void OnDisable()
    {
        cameraManager.frameReceived -= OnCameraFrameReceived;
    }
}