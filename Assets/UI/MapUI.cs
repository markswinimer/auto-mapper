using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    // public MapController mapcontroller;
    private Label mapLog;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button buttonMenu = root.Q<Button>("ButtonMenu");
        Button buttonHelp = root.Q<Button>("ButtonHelp");
        VisualElement leftSide = root.Q<VisualElement>("LeftSide");
        // Then query the mapLog label within the LeftSide container
        mapLog = leftSide.Q<Label>("MapLog");

        buttonMenu.clicked += () => Debug.Log("Menu Button Clicked");
        buttonHelp.clicked += () => Debug.Log("Help Button Clicked");
    }

    public void UpdateMapLog(string newText)
    {
        mapLog.text += "\n" + newText; // Appends new text to the existing text in the mapLog
    }


}
