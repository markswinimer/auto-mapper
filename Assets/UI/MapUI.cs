using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    public MapController mapcontroller;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button buttonMenu = root.Q<Button>("ButtonMenu");
        Button buttonHelp = root.Q<Button>("ButtonHelp");

        buttonMenu.clicked += () => Debug.Log("Menu Button Clicked");
        buttonHelp.clicked += () => Debug.Log("Help Button Clicked");
    }

}
