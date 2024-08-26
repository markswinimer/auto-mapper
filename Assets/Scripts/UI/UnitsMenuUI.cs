using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

// we need to create an Interface for ui menus (with Create(), action listeners, onclick etc)
public class UnitsMenuUI : MonoBehaviour
{
    public static UnitsMenuUI Instance { get; private set; }

    [SerializeField] private UIDocument _document;

    private VisualElement _unitsMenu;
    public static event Action ToggleMenu;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeMenu();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        MainUI.ToggleUnitsMenu += ToggleMenuClass; // Subscribe to the event
    }

    void OnDestroy()
    {
        MainUI.ToggleUnitsMenu -= ToggleMenuClass; // Unsubscribe from the event
    }

    public void ToggleMenuClass()
    {
        print("clicked units menu" + _unitsMenu);
        // Toggle a class, for example "hidden" which might control visibility via CSS
        _unitsMenu.ToggleInClassList("menu-hidden");
    }
    

    public void InitializeMenu()
    {
        StartCoroutine(GenerateMenu());
    }

    // we need to remove this if we build the project?
    void OnValidate()
    {
        if (Application.isPlaying) return;
        StartCoroutine(GenerateMenu());
    }

    // using IEnum allows for animations
    private IEnumerator GenerateMenu()
    {
        yield return null;
        // Assuming your units menu will be added to the root element of the document
        var root = _document.rootVisualElement;

        // Create and configure the units menu
        _unitsMenu = Create("units-menu", "menu-hidden", "popup-menu");

        var unitsMenuBox = Create("menu-box");
        var unitsMenuBoxInner = Create("menu-box-inner");

        unitsMenuBox.Add(unitsMenuBoxInner);
        _unitsMenu.Add(unitsMenuBox);

        // Populate the menu with content
        var titleLabel = Create<Label>("menu-box-label");
        titleLabel.text = "Units";
        unitsMenuBoxInner.Add(titleLabel);

        var closeMenuButton = Create<Button>("close-menu");

        closeMenuButton.text = "X";
        closeMenuButton.clicked += ToggleMenuClass;
        unitsMenuBoxInner.Add(closeMenuButton);

        // Add the menu to the root element
        root.Add(_unitsMenu);
    }

    //helper function to quicken writing and adding elements
    VisualElement Create(params string[] classNames)
    {
        return Create<VisualElement>(classNames);
    }

    // makes the create function generic so that it can create more than visual elements base
    T Create<T>(params string[] classNames) where T : VisualElement, new()
    {
        var element = new T();
        foreach (var className in classNames)
        {
            element.AddToClassList(className);
        }
        return element;
    }
}
