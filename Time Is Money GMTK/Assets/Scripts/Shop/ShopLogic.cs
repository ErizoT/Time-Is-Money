using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopLogic : MonoBehaviour
{
    public struct ShopOption
    {
        public string Name;
        public int Cost;
        public float CostMultiplier;
        public int BuyLimit;
        public void Apply() => ApplierDelegate.Invoke();
        private readonly Action ApplierDelegate;
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

    public GlobalData playerData => GlobalData.Instance; // The player data (current money, time, luck, etc)

    public GameObject upgrade1, upgrade2, upgrade3, upgrade4;


    void Start()
    {
        shopOptions = new Dictionary<string, ShopOption>()
        {
            { "slowTime", new ShopOption("slowTime", 1, 3, 2, SlowTime) },
            { "speedLuck", new ShopOption("speedLuck", 1, 3, 2, SpeedLuck) },
            { "buyLuck", new ShopOption("buyLuck", 1, 3, 2, BuyLuck) },
            { "increaseItemWeight", new ShopOption("increaseItemWeight", 1, 3, 2, () => IncreaseItemWeight(UnityEngine.Random.Range(0, 8)))},
        };
        
    }

    public void Reroll()
    {
        // Reroll the shop


    }

    public void SlowTime()
    {
        // Subtract the cost from player money
        // Half the current timeTickRate
        // Multiply timeSlowCost by 3x

        // Player can spend money to slow time by 50%. Cost of it will increase 3x

        var slowTime = shopOptions["slowTime"];

        playerData.playerMoney -= slowTime.Cost;
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
        playerData.playerLuck *= 2;
    }

   public void BuyLuck()
    {
        // Buy some luck!
        // More good symbols
        // Less bad symbols
        // Greater likelihood of better combinations

        // Player can buy general luck for better item combos, better symbols, and less bad symbols

        var buyLuck = shopOptions["buyLuck"];

        playerData.playerMoney -= buyLuck.Cost;
        playerData.playerLuck += 0.5f;
    }

    public void IncreaseItemWeight(int symbolID)
    {
        // Player can spend money to make a symbol appear more

        var symbolBuy = shopOptions["increaseItemWeight"];

        playerData.playerMoney -= symbolBuy.Cost;
        // Insert code here to make a symbol appear more
    }


}
