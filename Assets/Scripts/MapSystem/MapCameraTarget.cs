using UnityEngine;

public class MapCameraTarget : MonoBehaviour
{
    // This is used as a target for the cinemachine camera
    // it is moved by the grid script
    public void MoveToPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
    public void ScaleToGridSize(Bounds bounds)
    {
        transform.localScale = new Vector3(bounds.size.x, bounds.size.y, bounds.size.z);
    }

}