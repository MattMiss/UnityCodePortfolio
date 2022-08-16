using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    [SerializeField]
    private Transform target;
    private float StartFOV, targetFOV;
    public float zoomSpeed = 1f;
    private Camera fpsCamera;


    void Awake()
    {
        instance = this;
        fpsCamera = GetComponent<Camera>();
    }

    void Start()
    {
        StartFOV = fpsCamera.fieldOfView;
        targetFOV = StartFOV;
    }

    void LateUpdate()
    {
        //transform.position = target.position;
        //transform.rotation = target.rotation;

        fpsCamera.fieldOfView = Mathf.Lerp(fpsCamera.fieldOfView, targetFOV, zoomSpeed * Time.deltaTime);
    }

    public void ZoomIn(float newZoom)
    {
        targetFOV = newZoom;
    }

    public void ZoomOut()
    {
        targetFOV = StartFOV;
    }
}
