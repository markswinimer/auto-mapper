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

    void Start() {
        Generate();    
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
        // root.Clear();
        var root = _document.rootVisualElement;

        root.styleSheets.Add(_styleSheet);

        var container = Create("container");

        var viewBox = Create("view-box", "bordered-box");
        container.Add(viewBox);

        var controlBox = Create("control-box", "bordered-box");
        container.Add(controlBox);

        var spinButton = Create<Button>();
        spinButton.text = "Spin";
        spinButton.clicked += SpinClicked;
        controlBox.Add(spinButton);

        // can look up how to access these values in the unity files 
        var scaleSlider = Create<Slider>();
        scaleSlider.lowValue = 0.5f;
        scaleSlider.highValue = 2f;
        scaleSlider.value = 1f;

        scaleSlider.RegisterValueChangedCallback(value => ScaleChanged?.Invoke(value.newValue));
        controlBox.Add(scaleSlider);
        
        root.Add(container);

        // example of using tweening engine, still in experimental
        if (Application.isPlaying)
        {
            var targetPosition = container.worldTransform.GetPosition();
            var startPosition = targetPosition + Vector3.up * 100;

            controlBox.experimental.animation.Position(targetPosition, 2000).from = startPosition;
            //where you start, where you end, the duration, and what happens each tic of the event
            controlBox.experimental.animation.Start(0, 1, 2000, (e,v) => e.style.opacity = new StyleFloat(v));
        }
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
