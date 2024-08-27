using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public abstract class RobotEquipView : MonoBehaviour
{
    public Slot[] Slots;

    [SerializeField] protected UIDocument document;
    [SerializeField] protected StyleSheet stylesheet;

    protected static VisualElement ghostIcon;

    static bool isDragging;
    static Slot originalSlot;

    protected VisualElement root;
    protected VisualElement container;

    public event Action<Slot, Slot> OnDrop;


    IEnumerator Start() 
    {
        yield return StartCoroutine(InitializeView());

        ghostIcon.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        ghostIcon.RegisterCallback<PointerUpEvent>(OnPointerUp);
        foreach (var slot in Slots)
        {
            slot.OnStartDrag += OnPointerDown;
        }
    }

    public abstract IEnumerator InitializeView(int size = 20);

    static void OnPointerDown(Vector2 position, Slot slot)
    {
        isDragging = true;
        originalSlot = slot;

        SetGhostIconPosition(position);

        ghostIcon.style.backgroundImage = originalSlot.BaseSprite.texture;
        originalSlot.Icon.image = null;
        originalSlot.StackLabel.visible = false;

        // ghostIcon.style.opacity = 0.8f;
        ghostIcon.style.visibility = Visibility.Visible;
    }

    static void SetGhostIconPosition(Vector2 position)
    {
        ghostIcon.style.top = position.y - ghostIcon.layout.height / 2;
        ghostIcon.style.left = position.x - ghostIcon.layout.width / 2;
    }

    void OnPointerMove(PointerMoveEvent evt)
    {
        if (!isDragging) return;

        SetGhostIconPosition(evt.position);
    }

    void OnPointerUp(PointerUpEvent evt)
    {
        if (!isDragging) return;
        print("dropping");
        Slot closestSlot = Slots
            .Where(slot => slot.worldBound.Overlaps(ghostIcon.worldBound))
            .OrderBy(slot => Vector2.Distance(slot.worldBound.position, ghostIcon.worldBound.position))
            .FirstOrDefault();
        print("closest => " + closestSlot);

        if (closestSlot != null)
        {
            print("found");
            OnDrop?.Invoke(originalSlot, closestSlot);
        }
        else{
            print("null");
            originalSlot.Icon.image = originalSlot.BaseSprite.texture;
        }

        isDragging = false;
        originalSlot = null;
        ghostIcon.style.visibility = Visibility.Hidden;
    }
}