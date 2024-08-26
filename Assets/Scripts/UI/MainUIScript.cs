using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class MainUI : MonoBehaviour
{
    [SerializeField] private UIDocument _document;
    [SerializeField] private StyleSheet _styleSheet;

    public static event Action<float> ScaleChanged;
    public static event Action SpinClicked;
    public static event Action ToggleUnitsMenu;
    public static event Action ToggleFramesMenu;

    void Start() {
        StartCoroutine(Generate());
    }

    // we need to remove this if we build the project?
    void OnValidate()
    {
        if (Application.isPlaying) return;
        StartCoroutine(Generate());
    }

    // using IEnum allows for animations
    private IEnumerator Generate()
    {
        yield return null;
        //actual canvas things are being put on
        var root = _document.rootVisualElement;
        root.Clear();

        root.styleSheets.Add(_styleSheet);

        var mainWindow = Create("main-window");
        var topBar = Create("top-bar");
        var gameLog = Create("game-log");

        // bottom icons
        // loop through data as there will be very similar repeated icons
        // consider creating a library to get the id from index
        // that would be useful for seeing the order in the ui and referencing code?
        var bottomBar = Create("bottom-bar");
        
        // string[] bottomButtonData = { "mapButton", "menuButton" };
        // for (int i = 0; i < bottomButtonData.Length; i++)
        // {
        //     var icon = Create("bottom-bar__icon",$"bottom-bar__icon-{i + 1}");
        //     icon.name = bottomButtonData[i];
        //     bottomBar.Add(icon);
        // }

        var unitsMenu = Create<Button>("units-menu", "bottom-bar__icon", "bottom-bar__icon-1");
        unitsMenu.name = "units-menu";
        unitsMenu.text = "Units";
        unitsMenu.clicked += ToggleUnitsMenu;

        var framesMenu = Create<Button>("frames-menu", "bottom-bar__icon", "bottom-bar__icon-2");
        framesMenu.name = "frames-menu";
        framesMenu.text = "Frames";
        framesMenu.clicked += ToggleFramesMenu;

        bottomBar.Add(unitsMenu);
        bottomBar.Add(framesMenu);

        root.Add(topBar);
        root.Add(mainWindow);
        mainWindow.Add(gameLog);
        mainWindow.Add(bottomBar);

        // example of using tweening engine, still in experimental
       
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
