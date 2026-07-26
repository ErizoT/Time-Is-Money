using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDetails : MonoBehaviour
{
    public string itemDescription;
    public string itemCost;
    public Action action;

    // UI elements
    public Button button;
    public TextMeshProUGUI text;
    public TextMeshProUGUI costText;

    private void Start()
    {
        button = GetComponent<Button>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void PerformUpgrade()
    {
        action();
        //Debug.Log("just performed " + action);
    }
}
