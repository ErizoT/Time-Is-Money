using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    public Dictionary<string, Func<ShopOption>> shopOptionFactories;
    public ButtonDetails[] buttons;
    public GlobalData playerData => GlobalData.Instance; // The player data (current money, time, luck, etc)

    public GlobalRollerData rollerData => GlobalRollerData.Instance; // The script that contains stuff regarding the slot machine.

    private int timesRerolled; // The number of time rerolled. Basically used for one interaction just for rerolling the first time
    private bool boughtAbility; // If the player has bought an ability or not

    void Start()
    {
        buttons = GetComponentsInChildren<ButtonDetails>();

        shopOptionFactories = new Dictionary<string, Func<ShopOption>>()
        {
            { "slowTime", () => new ShopOption("slowTime", "Slow time by 50%", 300, 1.5f, 2, SlowTime) },
            { "speedLuck", () => new ShopOption("speedLuck", "Increase your luck, but time speeds up 25%!", 10, 3, 2, SpeedLuck) },
            { "buyLuck", () => new ShopOption("buyLuck", "Increase your luck by 0.5", 75, 1.25f, 2, BuyLuck) },
            { "increaseItemWeight", 
                () => {
                    int rand = UnityEngine.Random.Range(0, rollerData.RollerSymbols.Length);
                    return new ShopOption("increaseItemWeight", "Increase the likelihood of " + rollerData.RollerSymbols[rand].SymbolId, 50, 5, 2, () => IncreaseItemWeight(rand));
                } 
            },
            //{ "freeDoubleTime", () => new ShopOption("freeDoubleTime", "Time speeds up 200%, but respins become free.", 10, 3, 1, FreeDoubleTime) },
            { "decreaseItemWeight",
                () => {
                    int rand = UnityEngine.Random.Range(0, rollerData.RollerSymbols.Length); 
                    return new ShopOption("decreaseItemWeight", "Decrease the likelihood of " + rollerData.RollerSymbols[rand].SymbolId, 50, 5, 2, () => DecreaseItemWeight(rand));
                }
            },
            { "speedSlots", () => new ShopOption("speedSlots", "Slots finish spinning 1 second faster", 75, 1.25f, 2, SpeedSlots) },
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

            List<Func<ShopOption>> list = shopOptionFactories.Values.ToList();
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
                ShopOption shopOption = list[ints[i]].Invoke();

                buttons[i].text.text = shopOption.Description;
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

        var slowTime = shopOptionFactories["slowTime"].Invoke();

        if (playerData.PlayerMoney >= slowTime.Cost)
        {
            playerData.PlayerMoney -= slowTime.Cost;
            playerData.timeTickRate /= 2;
            slowTime.Cost = Mathf.FloorToInt(slowTime.CostMultiplier * slowTime.Cost);
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

        var speedLuck = shopOptionFactories["speedLuck"].Invoke();

        if (playerData.PlayerMoney >= speedLuck.Cost)
        {
            playerData.timeTickRate *= 1.25f;
            playerData.PlayerLuck *= 2;
            boughtAbility = true; Reroll();
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

        var buyLuck = shopOptionFactories["buyLuck"].Invoke();

        if (playerData.PlayerMoney >= buyLuck.Cost)
        {
            playerData.PlayerMoney -= buyLuck.Cost;
            playerData.PlayerLuck += 0.5f;
            boughtAbility = true; Reroll();
        }
        else
        {
            Debug.Log("Don't have enough money!");
        }
    }

    public void IncreaseItemWeight(int symbolID)
    {
        // Player can spend money to make a symbol appear more

        var symbolBuy = shopOptionFactories["increaseItemWeight"].Invoke();

        if (playerData.PlayerMoney >= symbolBuy.Cost)
        {
            playerData.PlayerMoney -= symbolBuy.Cost;
            rollerData.RollerSymbols[symbolID].Weight = Mathf.Max(rollerData.RollerSymbols[symbolID].Weight + 10, 0);
            boughtAbility = true; Reroll();
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
        var decreaseItemWeight = shopOptionFactories["decreaseItemWeight"].Invoke();

        if (playerData.PlayerMoney >= decreaseItemWeight.Cost)
        {
            playerData.PlayerMoney -= decreaseItemWeight.Cost;
            // Decrease symbol weight
            rollerData.RollerSymbols[symbolID].Weight = Mathf.Max(rollerData.RollerSymbols[symbolID].Weight - 10, 0);
            boughtAbility = true; Reroll();
        }
        else
        {
            Debug.Log("Don't have enough money!");
        }
    }

    public void SpeedSlots()
    {
        // Player can spend money to remove a symbol from the slot machine
        var speedSlots = shopOptionFactories["speedSlots"].Invoke();

        if (playerData.PlayerMoney >= speedSlots.Cost)
        {
            playerData.PlayerMoney -= speedSlots.Cost;
            rollerData.MachineData.RollerDelay -= 4;
            boughtAbility = true; Reroll();
        }
        else
        {
            Debug.Log("Don't have enough money!");
        }
    }
}
