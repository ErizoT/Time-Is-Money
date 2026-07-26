using System;
using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    public TextMeshProUGUI textField;


    private void OnEnable()
    {
        OnMoneyChanged(GlobalData.Instance.PlayerMoney);
        GlobalData.Instance.OnPlayerMoneyChanged += OnMoneyChanged;
      
    }

    private void OnDisable()
    {
        GlobalData.Instance.OnPlayerMoneyChanged -= OnMoneyChanged;

    }




    void OnMoneyChanged(int money)
    {
        textField.text = "$"+ money.ToString();
    }
}
