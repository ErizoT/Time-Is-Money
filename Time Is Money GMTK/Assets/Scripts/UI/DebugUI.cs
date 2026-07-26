using TMPro;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI luckDisplay;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Application.isEditor)
        {
            GlobalData.Instance.OnPlayerLuckChanged += OnLuckUpdated;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void OnLuckUpdated(float luck)
    {
        luckDisplay.text = $"Current luck: {luck}";
    }
}
