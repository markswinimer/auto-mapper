using UnityEngine;

public class HoverDetection : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask hoverLayerMask;
    private GameObject lastHoveredObject = null;

    void Update()
    {
        // Use the utility function to get the GameObject under the mouse cursor
        GameObject hitObject = MouseUtility3D.GetMouseHitObject(mainCamera, hoverLayerMask);

        // Handle hover logic
        if (hitObject != null)
        {
            IInteractable hoverableComponent = hitObject.GetComponent<IInteractable>();

            if (hoverableComponent != null && hoverableComponent.IsHoverable())
            {
                if (hitObject != lastHoveredObject)
                {
                    // Trigger exit on the last hovered object
                    if (lastHoveredObject != null)
                    {
                        IInteractable lastHoverable = lastHoveredObject.GetComponent<IInteractable>();
                        if (lastHoverable != null && lastHoverable.IsHoverable())
                        {
                            lastHoverable.OnHoverExit();
                        }
                    }

                    // Trigger enter on the new hovered object
                    hoverableComponent.OnHoverEnter();
                    lastHoveredObject = hitObject;
                }
            }
        }
        else
        {
            // If no object is hit, trigger exit on the last hovered object
            if (lastHoveredObject != null)
            {
                IInteractable lastHoverable = lastHoveredObject.GetComponent<IInteractable>();
                if (lastHoverable != null && lastHoverable.IsHoverable())
                {
                    lastHoverable.OnHoverExit();
                }
                lastHoveredObject = null;
            }
        }

        // Handle click logic separately
        if (Input.GetMouseButtonDown(0)) // 0 for left mouse button
        {
            if (hitObject != null)
            {

                IInteractable clickableComponent = hitObject.GetComponent<IInteractable>();
                if (clickableComponent != null && clickableComponent.IsClickable())
                {
               Debug.Log("should click");

                    clickableComponent.OnClick();
                }
            }
        }
    }
}
