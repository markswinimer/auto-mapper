using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition3D : MonoBehaviour
{
    public static MousePosition3D Instance { get; private set; }
    [SerializeField] private Camera mainCamera = null;
    [SerializeField] private LayerMask mouseColliderLayerMask = new LayerMask(); // default is everything
    [SerializeField] private LayerMask mouseObjectLayerMask = new LayerMask(); // default is everything

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        // Visual debugging of the raycast
        MouseUtility3D.GetMouseWorldPosition(mainCamera, mouseColliderLayerMask);
    }

    public static Vector3 GetMouseWorldPosition()
    {
        return MouseUtility3D.GetMouseWorldPosition(Instance.mainCamera, Instance.mouseColliderLayerMask);
    }

    public static GameObject GetMouseHitObject()
    {
        return MouseUtility3D.GetMouseHitObject(Instance.mainCamera, Instance.mouseObjectLayerMask);
    }
}
