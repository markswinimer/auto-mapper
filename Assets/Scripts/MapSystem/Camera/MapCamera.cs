using Cinemachine;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    public static MapCamera instance;

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private Vector2Int _mapWorldDimensions;
    private Vector3 initialCameraPosition;
    private float initialOrthographicSize;

    private float targetOrthographicSize;
    private Vector3 targetCameraPosition;

    private float zoomSpeed = 5f;
    private float zoomMomentum = 0f;
    private float positionLerpSpeed = 10f;

    private bool zoomingIsFixed = false;
    private Vector3 _cursorWorldPosition;

    void Awake()
    {
        instance = this;
    }

    public void SetupIsometricCamera()
    {
        _mapWorldDimensions = Map.Instance.MapWorldDimensions;

        // Ensure the camera is set to orthographic mode
        cinemachineVirtualCamera.m_Lens.Orthographic = true;

        // Set the isometric angle with the preferred X rotation
        var cameraTransform = cinemachineVirtualCamera.transform;
        cameraTransform.rotation = Quaternion.Euler(-315f, 45f, 0f);

        // Calculate the orthographic size to fit the entire map's height as a diamond
        float mapSize = _mapWorldDimensions.x;
        initialOrthographicSize = mapSize / 2f * 1.2f;
        cinemachineVirtualCamera.m_Lens.OrthographicSize = initialOrthographicSize;

        // Position the camera directly above the map center, adjusted for the isometric angle
        Vector3 cameraOffset = new Vector3(0, initialOrthographicSize * Mathf.Sqrt(2), 0);
        initialCameraPosition = new Vector3(-1, cameraOffset.y, -1);
        cameraTransform.position = initialCameraPosition;

        // Initialize the target values
        targetOrthographicSize = initialOrthographicSize;
        targetCameraPosition = initialCameraPosition;
    }

    void Update()
    {
        HandleCameraZoom();
    }

    private void HandleCameraZoom()
    {
        float scrollInput = Input.mouseScrollDelta.y;

        if (Mathf.Abs(scrollInput) > 0.01f)
        {
            // Adjust zoom momentum based on scroll input
            zoomMomentum += scrollInput * zoomSpeed;
        }

        // Apply zoom momentum
        targetOrthographicSize -= zoomMomentum;
        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, initialOrthographicSize / 2, initialOrthographicSize * 2);

        // Convert cursor position to world position
        Vector3 mousePosition = Input.mousePosition;

        if (!zoomingIsFixed)
        {
            _cursorWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, -Camera.main.transform.position.y));
        }

        // Determine the target position based on whether zooming in or out
        if (zoomMomentum > 0) // Zoom in
        {
            zoomingIsFixed = true;
            targetCameraPosition = Vector3.Lerp(targetCameraPosition, _cursorWorldPosition, 0.1f);
        }
        else if (zoomMomentum < 0) // Zoom out
        {
            zoomingIsFixed = false;
            targetCameraPosition = Vector3.Lerp(targetCameraPosition, initialCameraPosition, 0.1f);

            // Force reset orthographic size and camera position when zooming out reaches near the initial values
            if (targetOrthographicSize >= initialOrthographicSize * 0.99f)
            {
                targetOrthographicSize = initialOrthographicSize;
                targetCameraPosition = initialCameraPosition;
            }
        }

        // Apply the target orthographic size and position smoothly
        cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(cinemachineVirtualCamera.m_Lens.OrthographicSize, targetOrthographicSize, Time.deltaTime * positionLerpSpeed);
        cinemachineVirtualCamera.transform.position = Vector3.Lerp(cinemachineVirtualCamera.transform.position, targetCameraPosition, Time.deltaTime * positionLerpSpeed);

        // Decay the zoom momentum over time for smooth stop
        zoomMomentum = Mathf.Lerp(zoomMomentum, 0f, Time.deltaTime * positionLerpSpeed);
    }
}
