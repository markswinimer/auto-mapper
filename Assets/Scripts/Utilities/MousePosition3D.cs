using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition3D : MonoBehaviour
{
    public static MousePosition3D Instance { get; private set; }
    [SerializeField] private Camera mainCamera = null;
    [SerializeField] private LayerMask mouseColliderLayerMask = new LayerMask(); // default is everything

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, mouseColliderLayerMask))
        {
            transform.position = raycastHit.point;
        }
    }
    public static Vector3 GetMouseWorldPosition() => Instance.GetMouseWorldPosition_Instance();
    private Vector3 GetMouseWorldPosition_Instance()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, mouseColliderLayerMask))
        {
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }
}