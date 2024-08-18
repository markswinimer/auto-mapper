using Cinemachine;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    public static MapCamera instance;
    // put logic here for adjusting cenemachine camera
    // currently settings are handled on cinemachine object
    // camera tracks camera target and that is moved by grid script
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    private MapCameraTarget _mapCameraTarget;

    private float targetFieldOfView = 50f;
    private float fieldOfViewMinimum = 10f;
    private float fieldOfViewMaximum = 100f;

    void Awake()
    {
        instance = this;
    }
    
    private void Update() {
        HandleCameraZoom();
    }

    public void SetCameraFollow(Transform target)
    {
        // cinemachineVirtualCamera.Follow = target;
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
    public void SetCameraTargetPosition(Vector2Int dimensions)
    {
        // Adjust the z position by subtracting a fraction of the bounds' depth
        // offsetPosition.z -= southwardOffsetFactor * bounds.size.z;
        // Set the camera target position
        GameObject mapCameraTarget = new GameObject("MapCameraTarget");
        // consider breaking this into a map script init function 
        // Add the Map script to the new GameObject
        print("map > > " + mapCameraTarget.transform.eulerAngles.y); // Prints the Y rotation in degrees (initially 0)
        _mapCameraTarget = mapCameraTarget.AddComponent<MapCameraTarget>();
        mapCameraTarget.transform.rotation = Quaternion.Euler(0, -180, 0);
        print("map > > " + mapCameraTarget.transform.eulerAngles.y); // Should print 210

        // Add the Grid component to the same GameObject

        if (mapCameraTarget != null)
        {
            // mapCameraTarget.ScaleToGridSize(dimensions);
            cinemachineVirtualCamera.Follow = _mapCameraTarget.transform;
            cinemachineVirtualCamera.LookAt = _mapCameraTarget.transform;
            _mapCameraTarget.MoveToPosition();
        }
        else
        {
            Debug.LogError("Camera Target Controller is not assigned!");
        }
    }
}
