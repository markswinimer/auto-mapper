using UnityEngine;

public class MapCameraTarget : MonoBehaviour
{

    private void Awake()
    {
        // Set the camera target to the current instance
        // transform.position = new Vector3(0, 0, 0);
    }
    // This is used as a target for the cinemachine camera
    // it is moved by the grid script
    public void MoveToPosition()
    {
        //currently always should be at 0,0,0;
        // transform.position = new Vector3(0, 0, 0);
    }

    public void ScaleToGridSize(float width, float height)
    {
        transform.localScale = new Vector3(width, 1, height);
        transform.position = new Vector3(width / 2, 0, height / 2);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // draw cube around the object size
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

}