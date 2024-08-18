using UnityEngine;

public class MapCameraTarget : MonoBehaviour
{

    private void Awake()
    {
        // Set the camera target to the current instance
        transform.position = Map.Instance.MapCenter;
    }

    // This is used as a target for the cinemachine camera
    // it is moved by the grid script
    public void MoveToPosition()
    {
    }

    public void ScaleToGridSize(Vector2Int dimensions)
    {
        // transform.localScale = new Vector3(dimensions.x, 1, dimensions.y);
        // transform.position = new Vector3(dimensions.x / 2, 0, dimensions.y / 2);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // draw cube around the object size
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

}