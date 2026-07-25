using System;
using UnityEngine;

public class RollerScript : MonoBehaviour
{
    public int id;
    
    public class LuckData
    {
        private readonly struct SymbolData
        {
            public readonly float SymbolWeight;
            public readonly float LuckWeight;
            public SymbolData(float symbolWeight, float luckWeight)
            {
                SymbolWeight = symbolWeight;
                LuckWeight = luckWeight;
            }
        }
        private readonly (RollerSymbols.Enum, SymbolData)[] Data = new (RollerSymbols.Enum, SymbolData)[]
        {
            new ( RollerSymbols.Enum.Coin, new SymbolData() ),
            new ( RollerSymbols.Enum.Coin, new SymbolData() ),
            new ( RollerSymbols.Enum.Coin, new SymbolData() ),
            new ( RollerSymbols.Enum.Coin, new SymbolData() ),
            new ( RollerSymbols.Enum.Coin, new SymbolData() ),
            new ( RollerSymbols.Enum.Coin, new SymbolData() ),
            new ( RollerSymbols.Enum.Coin, new SymbolData() ),
            new ( RollerSymbols.Enum.Coin, new SymbolData() )
        };
        public (RollerSymbols.Enum, int)[] SymbolCounts;
        public LuckData()
        {

        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
