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

        if (hitObject != null)
        {
            IHoverable hoverableComponent = hitObject.GetComponent<IHoverable>();

            if (hoverableComponent != null && hoverableComponent.IsHoverable())
            {
                if (hitObject != lastHoveredObject)
                {
                    if (lastHoveredObject != null)
                    {
                        // If a different object was previously hovered, trigger exit on it
                        IHoverable lastHoverable = lastHoveredObject.GetComponent<IHoverable>();
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
            // If no object is hit and there was a previously hovered object, trigger exit
            if (lastHoveredObject != null)
            {
                IHoverable lastHoverable = lastHoveredObject.GetComponent<IHoverable>();
                if (lastHoverable != null && lastHoverable.IsHoverable())
                {
                    lastHoverable.OnHoverExit();
                }
                lastHoveredObject = null;
            }
        }
    }
}
