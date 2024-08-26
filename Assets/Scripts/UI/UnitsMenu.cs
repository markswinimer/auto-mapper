using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class UnitsMenuUI : MonoBehaviour
{
    [SerializeField] private UIDocument _document;

    private VisualElement _unitsMenu;

    void Start()
    {
        InitializeMenu();
    }

    private void InitializeMenu()
    {
        // Assuming your units menu will be added to the root element of the document
        var root = _document.rootVisualElement;

        // Create and configure the units menu
        _unitsMenu = new VisualElement();
        _unitsMenu.AddToClassList("units-menu-popup");

        // Populate the menu with content
        var titleLabel = new Label("Units Menu");
        _unitsMenu.Add(titleLabel);

        // Initially hide the menu
        _unitsMenu.style.display = DisplayStyle.None;

        // Add the menu to the root element
        root.Add(_unitsMenu);
    }

    public void ToggleMenu()
    {
        _unitsMenu.style.display = _unitsMenu.style.display == DisplayStyle.None
            ? DisplayStyle.Flex
            : DisplayStyle.None;
    }
}
