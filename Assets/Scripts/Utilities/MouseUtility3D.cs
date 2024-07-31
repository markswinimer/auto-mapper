using UnityEngine;

public static class MouseUtility3D
{
    public static Vector3 GetMouseWorldPosition(Camera mainCamera, LayerMask mouseColliderLayerMask)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, mouseColliderLayerMask))
        {
            // Debug.DrawRay(ray.origin, ray.direction * 999f, Color.red); // Draw the ray in the scene view
            // Debug.Log("Raycast hit point: " + raycastHit.point + ", Hit Object: " + raycastHit.collider.gameObject.name); // Log the hit point and object
            return raycastHit.point;
        }
        else
        {
            // Debug.Log("Raycast did not hit any collider");
            return Vector3.zero;
        }
    }

    public static GameObject GetMouseHitObject(Camera mainCamera, LayerMask mouseObjectLayerMask)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, mouseObjectLayerMask))
        {
            // Debug.DrawRay(ray.origin, ray.direction * 999f, Color.red); // Draw the ray in the scene view
            // Debug.Log("Raycast hit point: " + raycastHit.point + ", Hit Object: " + raycastHit.collider.gameObject.name); // Log the hit point and object
            return raycastHit.collider.gameObject;
        }
        else
        {
            Debug.Log("Raycast did not hit any collider");
            return null;
        }
    }
}
