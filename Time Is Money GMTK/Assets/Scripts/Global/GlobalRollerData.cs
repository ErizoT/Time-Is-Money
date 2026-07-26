using System.Collections.Generic;
using UnityEngine;

public class GlobalRollerData : MonoBehaviour
{
    public static GlobalRollerData Instance => GlobalData.Instance.rollerData;
    [SerializeField]
    private int SlotMachineCount = 3;
    public RollerSymbol[] RollerSymbols;
    public Dictionary<string, RollerSymbol> RollerSymbolsPerId = new Dictionary<string, RollerSymbol>();
    public LuckData[] LuckPerMachine;
    public class LuckData
    {
        public int RollCost = 10;
        private float _rollerLuck;
        public float RollerLuck 
            { get => _rollerLuck; 
            set 
            {
                _rollerLuck = value;
                Recalculate(value);
            } 
        }
        public readonly struct RollerOption : IWeightedListEntry
        {
            public readonly int SymbolId { get => _symbolId; }
            private readonly int _symbolId;
            public readonly int Weight { get => _weight; }
            private readonly int _weight;

            public RollerOption(int symbolId, int weight)
            {
                _symbolId = symbolId;
                _weight = weight;
            }
        }
        private WeightedList<RollerOption> RollerSymbols;
        public int RollRandomSymbolId()
        {
            int randomInt = UnityEngine.Random.Range(0, RollerSymbols.TotalWeight);
            return RollerSymbols.GetRandom(randomInt).SymbolId;
        }
        public RollerSymbol RollRandomSymbol()
        {
            return GlobalData.RollerData.RollerSymbols[RollRandomSymbolId()];
        }
        public void AddSymbolWithWeight()
        {

        }
        public LuckData(float rollerLuck)
        {
            Recalculate(rollerLuck);
        }
        public void Recalculate(float rollerLuck)
        {
            RollerSymbol[] rollerSymbols = Instance.RollerSymbols;
            _rollerLuck = rollerLuck;
            RollerOption[] rollerOptions = new RollerOption[rollerSymbols.Length];
            float luckCoefficient = rollerLuck * GlobalData.Instance.PlayerLuck;
            for (int i = 0; i < rollerSymbols.Length; i++)
            {
                int calculatedWeight = rollerSymbols[i].Weight;
                float floatWeight = calculatedWeight + (rollerSymbols[i].LuckWeight * luckCoefficient);
                int bonusSymbol = UnityEngine.Random.Range(0, 1) < floatWeight % 1 ? 1 : 0; 

                calculatedWeight = Mathf.FloorToInt(floatWeight + bonusSymbol * Mathf.Sign(rollerSymbols[i].LuckWeight));
                
                rollerOptions[i] = new RollerOption(i, calculatedWeight);
            }
            RollerSymbols = new WeightedList<RollerOption>(rollerOptions);
        }
    }
    public void RecalculateAll()
    {
        foreach (LuckData luckData in LuckPerMachine) {
            luckData.Recalculate(luckData.RollerLuck);
        }
    }
    public void Initialise()
    {
        foreach (var rollerSymbol in RollerSymbols)
        {
            RollerSymbolsPerId.Add(rollerSymbol.SymbolId, rollerSymbol);
        }
        LuckPerMachine = new LuckData[SlotMachineCount];
        for (int i = 0; i < SlotMachineCount; i++)
        {
            LuckPerMachine[i] = new LuckData(Random.Range(0.5f, 1.5f));
        }
    }
}
