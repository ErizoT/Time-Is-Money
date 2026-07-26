using UnityEngine;
using TMPro;

public class DepositScript : MonoBehaviour
{
    public GlobalData playerData => GlobalData.Instance;
    public TextMeshProUGUI costText;

    public void Update()
    {
        costText.text = "$" + playerData.PlayerMoney.ToString();
    }

    public void Deposit()
    {
        playerData.PaidDebt += playerData.PlayerMoney;
        playerData.PlayerMoney = 0;
        Debug.Log("Money Deposited");
    }
}
