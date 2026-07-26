using TMPro;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI luckDisplay;
    [SerializeField] TextMeshProUGUI moneyDisplay;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Application.isEditor)
        {
            GlobalData.Instance.OnPlayerLuckChanged += OnLuckUpdated;
            OnLuckUpdated(GlobalData.Instance.PlayerLuck);
            GlobalData.Instance.OnPlayerMoneyChanged += OnMoneyUpdated;
            OnMoneyUpdated(GlobalData.Instance.PlayerMoney);
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
    void OnMoneyUpdated(int amount)
    {
        moneyDisplay.text = $"Current money: {amount}";
    }
    public void AddMoney() { GlobalData.Instance.PlayerMoney += 100; }
    public void AddLuck() { GlobalData.Instance.PlayerLuck += 1; }
    public void RemoveAllMoney() { GlobalData.Instance.PlayerMoney = 0; }

}
