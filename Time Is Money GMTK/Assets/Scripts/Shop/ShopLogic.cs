using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static ShopLogic;

public class ShopLogic : MonoBehaviour
{
    public class ShopOptionFactory
    {
        public int BuyCount;
        public Func<ShopOption> Factory;
        public ShopOptionFactory(int buyCount, Func<ShopOption> factory)
        {
            BuyCount = buyCount;
            Factory = factory;
        }
    }
    public struct ShopOption
    {
        public string Name;
        public int Cost;
        public float CostMultiplier;
        public int BuyLimit;
        public string Description;
        public void Apply() => ApplierDelegate.Invoke();
        public readonly Action ApplierDelegate;
        public ShopOption(string name, string description, int cost, float costMultiplier, int buyLimit, Action applierDelegate)
        {
            Name = name;
            Description = description;
            Cost = cost;
            CostMultiplier = costMultiplier;
            BuyLimit = buyLimit;
            ApplierDelegate = applierDelegate;
        }
    }
    //public Dictionary<string, ShopOption> shopOptions;
    public Dictionary<string, ShopOptionFactory> shopOptionFactories;
    public ButtonDetails[] buttons;
    public GlobalData playerData => GlobalData.Instance; // The player data (current money, time, luck, etc)

    public GlobalRollerData rollerData => GlobalRollerData.Instance; // The script that contains stuff regarding the slot machine.

    private int timesRerolled; // The number of time rerolled. Basically used for one interaction just for rerolling the first time
    private bool boughtAbility; // If the player has bought an ability or not

    void Start()
    {
        buttons = GetComponentsInChildren<ButtonDetails>();

        shopOptionFactories = new Dictionary<string, ShopOptionFactory>()
        {
            { "slowTime", new ShopOptionFactory(0, () => new ShopOption("slowTime", "Slow time by 50%", 300, 1.5f, 2, SlowTime)) },
            { "speedLuck", new ShopOptionFactory(0, () => new ShopOption("speedLuck", "Increase your luck, but time speeds up 25%!", 10, 3, 2, SpeedLuck)) },
            { "buyLuck", new ShopOptionFactory(0, () => new ShopOption("buyLuck", "Increase your luck by 0.5", 75, 1.25f, 2, BuyLuck)) },
            { "increaseItemWeight",
                new ShopOptionFactory(0, () => {
                    int rand = UnityEngine.Random.Range(0, rollerData.RollerSymbols.Length);
                    return new ShopOption("increaseItemWeight", "Increase the likelihood of " + rollerData.RollerSymbols[rand].SymbolId, 50, 5, 2, () => IncreaseItemWeight(rand));
                } )
            },
            //{ "freeDoubleTime", () => new ShopOption("freeDoubleTime", "Time speeds up 200%, but respins become free.", 10, 3, 1, FreeDoubleTime) },
            { "decreaseItemWeight",
                new ShopOptionFactory(0, () => {
                    int rand = UnityEngine.Random.Range(0, rollerData.RollerSymbols.Length); 
                    return new ShopOption("decreaseItemWeight", "Decrease the likelihood of " + rollerData.RollerSymbols[rand].SymbolId, 50, 5, 2, () => DecreaseItemWeight(rand));
                })
            },
            { "speedSlots", new ShopOptionFactory(0, () => new ShopOption("speedSlots", "Slots finish spinning 1 second faster", 75, 1.25f, 2, SpeedSlots)) },
        };
        // On start, reroll items
        Reroll();
        
    }

    public void Reroll()
    {
        // Reroll the shop

        if (timesRerolled == 0 || playerData.PlayerMoney >= 50 || boughtAbility == true)
        {
            if (playerData.PlayerMoney >= 50 && timesRerolled > 0 && boughtAbility == false) { playerData.PlayerMoney -= 50; } // If the player has enough money for a reroll, pay 50 bucks
            boughtAbility = false;
            timesRerolled += 1;

            List<ShopOptionFactory> list = shopOptionFactories.Values.ToList();
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
                ShopOption shopOption = list[ints[i]].Factory.Invoke();

                buttons[i].text.text = shopOption.Description;
                int calculatedCost = (int)(shopOption.Cost + Mathf.Pow(shopOption.CostMultiplier, list[ints[i]].BuyCount));
                buttons[i].costText.text = "$" + shopOption.Cost.ToString();
                buttons[i].action = shopOption.ApplierDelegate;
            }
        } else
        {
            Debug.LogError("Reroll didn't work for some reason");
        }
    }
    public void SlowTime()
    {
        // Subtract the cost from player money
        // Half the current timeTickRate
        // Multiply timeSlowCost by 3x

        // Player can spend money to slow time by 50%. Cost of it will increase 3x

        var slowTime = shopOptionFactories["slowTime"].Factory.Invoke();
        int cost = (int)(slowTime.Cost + Mathf.Pow(slowTime.CostMultiplier, shopOptionFactories["slowTime"].BuyCount));
        if (playerData.PlayerMoney >= cost)
        {
            playerData.PlayerMoney -= cost;
            playerData.timeTickRate /= 2;
            shopOptionFactories["slowTime"].BuyCount += 1;
            boughtAbility = true;
            Reroll();
        }
        else
        {
            Debug.Log("Don't have enough money!");
        }
    }

    public void SpeedLuck()
    {
        // Subtract cost from player money
        // Increase tickrate by .25
        // Double player luck

        // Player makes time tick faster in exchange for MASSIVE luck boost

        var speedLuck = shopOptionFactories["speedLuck"].Factory.Invoke();
        int cost = (int)(speedLuck.Cost + Mathf.Pow(speedLuck.CostMultiplier, shopOptionFactories["speedLuck"].BuyCount));
        if (playerData.PlayerMoney >= cost)
        {
            playerData.PlayerMoney -= cost;
            playerData.timeTickRate *= 1.25f;
            playerData.PlayerLuck *= 2;
            shopOptionFactories["speedLuck"].BuyCount += 1;
            boughtAbility = true;
            Reroll();
        }
        else
        {
            Debug.Log("Don't have enough money!");
        }
    }

   public void BuyLuck()
    {
        // Buy some luck!
        // More good symbols
        // Less bad symbols
        // Greater likelihood of better combinations

        // Player can buy general luck for better item combos, better symbols, and less bad symbols

        var buyLuck = shopOptionFactories["buyLuck"].Factory.Invoke();
        int cost = (int)(buyLuck.Cost + Mathf.Pow(buyLuck.CostMultiplier, shopOptionFactories["buyLuck"].BuyCount));
        if (playerData.PlayerMoney >= cost)
        {
            playerData.PlayerMoney -= cost;
            playerData.PlayerLuck += 0.5f;
            shopOptionFactories["buyLuck"].BuyCount += 1;
            boughtAbility = true;
            Reroll();
        }
        else
        {
            Debug.Log("Don't have enough money!");
        }
    }

    public void IncreaseItemWeight(int symbolID)
    {
        // Player can spend money to make a symbol appear more

        var symbolBuy = shopOptionFactories["increaseItemWeight"].Factory.Invoke();
        int cost = (int)(symbolBuy.Cost + Mathf.Pow(symbolBuy.CostMultiplier, shopOptionFactories["increaseItemWeight"].BuyCount));
        if (playerData.PlayerMoney >= cost)
        {
            playerData.PlayerMoney -= cost;
            rollerData.RollerSymbols[symbolID].Weight = Mathf.Max(rollerData.RollerSymbols[symbolID].Weight + 10, 0);
            shopOptionFactories["increaseItemWeight"].BuyCount += 1;
            boughtAbility = true; 
            Reroll();
        }
        else
        {
            Debug.Log("Don't have enough money!");
        }
    }

    /*
    public void FreeDoubleTime()
    {
        // The player can make time go double time in exchange for free spins

        var freeDoubleTime = shopOptionFactories["freeDoubleTime"].Invoke();
        playerData.timeTickRate = 2f;
        

    }*/

    public void DecreaseItemWeight(int symbolID)
    {
        // Player can spend money to remove a symbol from the slot machine
        var decreaseItemWeight = shopOptionFactories["decreaseItemWeight"].Factory.Invoke();
        int cost = (int)(decreaseItemWeight.Cost + Mathf.Pow(decreaseItemWeight.CostMultiplier, shopOptionFactories["decreaseItemWeight"].BuyCount));
        if (playerData.PlayerMoney >= cost)
        {
            playerData.PlayerMoney -= cost;
            // Decrease symbol weight
            rollerData.RollerSymbols[symbolID].Weight = Mathf.Max(rollerData.RollerSymbols[symbolID].Weight - 10, 0);
            shopOptionFactories["decreaseItemWeight"].BuyCount += 1;
            boughtAbility = true; 
            Reroll();
        }
        else
        {
            Debug.Log("Don't have enough money!");
        }
    }

    public void SpeedSlots()
    {
        // Player can spend money to remove a symbol from the slot machine
        var speedSlots = shopOptionFactories["speedSlots"].Factory.Invoke();
        int cost = (int)(speedSlots.Cost + Mathf.Pow(speedSlots.CostMultiplier, shopOptionFactories["speedSlots"].BuyCount));
        if (playerData.PlayerMoney >= cost)
        {
            playerData.PlayerMoney -= cost;
            rollerData.MachineData.RollerDelay -= 1;
            rollerData.MachineData.CountdownDelay -= 0.1f;
            shopOptionFactories["speedSlots"].BuyCount += 1;
            boughtAbility = true; 
            Reroll();
        }
        else
        {
            Debug.Log("Don't have enough money!");
        }
    }
}
