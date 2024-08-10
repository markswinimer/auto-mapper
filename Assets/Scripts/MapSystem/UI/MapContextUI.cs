using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MapContextUI : MonoBehaviour
{
    VisualElement root;

    public Transform room1;
    public Transform room2;

    VisualElement roomIcons1;
    VisualElement roomIcons2;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        
    }
}