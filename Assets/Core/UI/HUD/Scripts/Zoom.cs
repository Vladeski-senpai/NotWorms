using UnityEngine;

public class Zoom : MonoBehaviour
{
    [SerializeField] Camera cam;

    float zoomSpeed;
    float minZoom;
    float maxZoom;
    float zoomValue;
    bool buttonPressed;
    bool zoom;

    void Start()
    {
        var gameSettings = Director.Instance.GameSettings;
        zoomSpeed = gameSettings.ZoomSpeed;
        minZoom = gameSettings.MinZoom;
        maxZoom = gameSettings.MaxZoom;
        zoomValue = cam.orthographicSize;
    }

    void Update()
    {
        if (!buttonPressed) return;

        zoomValue += (zoom ? -1 : 1) * zoomSpeed * Time.deltaTime;

        if (zoomValue > maxZoom) zoomValue = maxZoom;
        else if (zoomValue < minZoom) zoomValue = minZoom;

        cam.orthographicSize = zoomValue;
    }

    public void OnUnzoomButtonDown()
    {
        zoom = false;
        buttonPressed = true;
    }

    public void OnZoomButtonDown()
    {
        zoom = true;
        buttonPressed = true;
    }

    public void OnButtonUp()
    {
        buttonPressed = false;
    }
}
