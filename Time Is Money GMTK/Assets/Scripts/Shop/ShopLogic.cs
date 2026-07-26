using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopLogic : MonoBehaviour
{
    public struct ShopOption
    {
        public string Name;
        public int Cost;
        public float CostMultiplier;
        public int BuyLimit;
        public void Apply() => ApplierDelegate.Invoke();
        public readonly Action ApplierDelegate;
        public ShopOption(string name, int cost, float costMultiplier, int buyLimit, Action applierDelegate)
        {
            Name = name;
            Cost = cost;
            CostMultiplier = costMultiplier;
            BuyLimit = buyLimit;
            ApplierDelegate = applierDelegate;
        }
    }
    public Dictionary<string, ShopOption> shopOptions;
    public ButtonDetails[] buttons;
    public GlobalData playerData => GlobalData.Instance; // The player data (current money, time, luck, etc)

    public GlobalRollerData rollerData; // The script that contains stuff regarding the slot machine.

    void Start()
    {
        buttons = GetComponentsInChildren<ButtonDetails>();
        shopOptions = new Dictionary<string, ShopOption>()
        {
            { "slowTime", new ShopOption("slowTime", 300, 1.5f, 2, SlowTime) },
            { "speedLuck", new ShopOption("speedLuck", 1, 3, 2, SpeedLuck) },
            { "buyLuck", new ShopOption("buyLuck", 1, 3, 2, BuyLuck) },
            { "increaseItemWeight", new ShopOption("increaseItemWeight", 1, 3, 2, () => IncreaseItemWeight(UnityEngine.Random.Range(0, 8)))},
            { "freeDoubleTime", new ShopOption("freeDoubleTime", 1, 3, 1, FreeDoubleTime) },
            { "decreaseItemWeight", new ShopOption("decreaseItemWeigh", 1, 3, 2, () => DecreaseItemWeight(UnityEngine.Random.Range(0, 8)))},
        };
        
    }

    public void Reroll()
    {
        // Reroll the shop
        List<ShopOption> list = shopOptions.Values.ToList();
        int[] ints = new int[list.Count];
        for (int i = 0; i < ints.Length; i++)
        {
            ints[i] = i;
        }
        for (int i = ints.Length - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (ints[j], ints[i]) = (ints[i], ints[j]);
        }
        for (int i = 0; i < buttons.Length; i++)
        {
            //Button button = buttons[i];
            ShopOption shopOption = list[ints[i]];

            buttons[i].text.text = shopOption.Name;
            buttons[i].action = shopOption.ApplierDelegate;

        }

    }

    public void SlowTime()
    {
        // Subtract the cost from player money
        // Half the current timeTickRate
        // Multiply timeSlowCost by 3x

        // Player can spend money to slow time by 50%. Cost of it will increase 3x

        var slowTime = shopOptions["slowTime"];

        playerData.PlayerMoney -= slowTime.Cost;
        playerData.timeTickRate /= 2;
        slowTime.Cost = Mathf.FloorToInt(slowTime.CostMultiplier * slowTime.Cost);
    }

    public void SpeedLuck()
    {
        // Subtract cost from player money
        // Increase tickrate by .25
        // Double player luck

        // Player makes time tick faster in exchange for MASSIVE luck boost

        var speedLuck = shopOptions["speedLuck"];

        playerData.timeTickRate *= 1.25f;
        playerData.PlayerLuck *= 2;
    }

   public void BuyLuck()
    {
        // Buy some luck!
        // More good symbols
        // Less bad symbols
        // Greater likelihood of better combinations

        // Player can buy general luck for better item combos, better symbols, and less bad symbols

        var buyLuck = shopOptions["buyLuck"];

        playerData.PlayerMoney -= buyLuck.Cost;
        playerData.PlayerLuck += 0.5f;
    }

    public void IncreaseItemWeight(int symbolID)
    {
        // Player can spend money to make a symbol appear more

        var symbolBuy = shopOptions["increaseItemWeight"];

        playerData.PlayerMoney -= symbolBuy.Cost;
        // Insert code here to make a symbol appear more
    }

    public void FreeDoubleTime()
    {
        // The player can make time go double time in exchange for free respins

        var freeDoubleTime = shopOptions["freeDoubleTime"];
        playerData.timeTickRate = 2f;
        // Re-spins become free

    }

    public void DecreaseItemWeight(int symbolID)
    {
        // Player can spend money to remove a symbol from the slot machine

        var removeSymbol = shopOptions["removeSymbol"];

        playerData.PlayerMoney -= removeSymbol.Cost;
        // Remove a symbol from the slot machine
        rollerData.RollerSymbols[symbolID].Weight 
    }
}
