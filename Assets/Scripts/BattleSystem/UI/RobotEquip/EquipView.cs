using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class EquipView : RobotEquipView
{
    [SerializeField] string panelName = "Robot Equipment";

    public override IEnumerator InitializeView(int size = 20)
    {
        Slots = new Slot[size];

        root = document.rootVisualElement;
        root.Clear();
        
        root.styleSheets.Add(stylesheet);

        container = root.CreateChild("container");
        
        var inventory = container.CreateChild("inventory");

        inventory.CreateChild("inventoryFrame");
        inventory.CreateChild("inventoryHeader").Add(new Label(panelName));

        var slotsContainer = inventory.CreateChild("slotsContainer");
        for (int i = 0; i < size; i++)
        {
            var slot = slotsContainer.CreateChild<Slot>("slot");
            Slots[i] = slot;
        }

        ghostIcon = container.CreateChild("ghostIcon");
        ghostIcon.BringToFront();

        yield return null;
    }

}