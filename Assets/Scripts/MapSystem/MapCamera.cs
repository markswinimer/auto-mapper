using Cinemachine;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    public static MapCamera instance;
    // put logic here for adjusting cenemachine camera
    // currently settings are handled on cinemachine object
    // camera tracks camera target and that is moved by grid script
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] MapCameraTarget mapCameraTarget;

    private float targetFieldOfView = 50f;
    private float fieldOfViewMinimum = 20f;
    private float fieldOfViewMaximum = 100f;

    void Awake()
    {
        instance = this;
    }
    private void Update() {
        HandleCameraZoom();
    }

    private void HandleCameraZoom() {
        float fieldOfViewIncreaseAmount = 5f;

        if ( Input.mouseScrollDelta.y < 0 )
        {
            targetFieldOfView += fieldOfViewIncreaseAmount;
        }
        if ( Input.mouseScrollDelta.y > 0 )
        {
            targetFieldOfView -= fieldOfViewIncreaseAmount;
        }

        targetFieldOfView = Mathf.Clamp(targetFieldOfView, fieldOfViewMinimum, fieldOfViewMaximum);

        float zoomSpeed = 3f;

        // smooths the scroll between the two values
        cinemachineVirtualCamera.m_Lens.FieldOfView = 
            Mathf.Lerp(cinemachineVirtualCamera.m_Lens.FieldOfView, targetFieldOfView, Time.deltaTime * zoomSpeed);
    }
    public void SetCameraTargetPosition(float width, float height)
    {
        // Adjust the z position by subtracting a fraction of the bounds' depth
        // offsetPosition.z -= southwardOffsetFactor * bounds.size.z;
        // Set the camera target position
        if (mapCameraTarget != null)
        {
            mapCameraTarget.ScaleToGridSize(width, height);
        }
        else
        {
            Debug.LogError("Camera Target Controller is not assigned!");
        }
    }
}
