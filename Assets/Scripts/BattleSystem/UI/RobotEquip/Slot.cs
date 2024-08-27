using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Slot : VisualElement
{
    public Image Icon;
    public Label StackLabel;
    public int Index => parent.IndexOf(this);
    public SerializableGuid ItemId { get; private set; } = SerializableGuid.Empty;
    public Sprite BaseSprite;

    public event Action<Vector2, Slot> OnStartDrag = delegate { };

    public Slot()
    {
        Icon = this.CreateChild<Image>("slotIcon");
        StackLabel = this.CreateChild("slotFrame").CreateChild<Label>("stackCount");
        RegisterCallback<PointerDownEvent>(OnPointerDown);
    }

    void OnPointerDown(PointerDownEvent evt)
    {
        if (evt.button != 0) return;
        
        OnStartDrag.Invoke(evt.position, this);
        evt.StopPropagation();
    }

    public void Set(String name, Sprite icon, bool isEquipped)
    {
        // ItemId = id;
        BaseSprite = icon;

        Icon.image = BaseSprite != null ? icon.texture : null;

        // StackLabel.text = name;
    }

    public void Remove()
    {
        // ItemId = SerializableGuid.Empty;
        Icon.image = null;
    }
}