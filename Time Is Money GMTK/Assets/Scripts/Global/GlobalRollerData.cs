using System.Collections.Generic;
using UnityEngine;

public class GlobalRollerData : MonoBehaviour
{
    [SerializeField]
    private int SlotMachineCount = 3;
    public RollerSymbol[] RollerSymbols;
    public Dictionary<string, RollerSymbol> RollerSymbolsPerId;
    public LuckData[] LuckPerMachine;
    public class LuckData
    {
        private float _rollerLuck;
        public float RollerLuck => _rollerLuck;
        private readonly struct RollerOption : IWeightedListEntry
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
        public int RollRandomSymbol()
        {
            int randomInt = UnityEngine.Random.Range(0, RollerSymbols.TotalWeight);
            return RollerSymbols.GetRandom(randomInt).SymbolId;
        }
        public LuckData(RollerSymbol[] rollerSymbols, float rollerLuck)
        {
            Recalculate(rollerSymbols, rollerLuck);
        }
        public void Recalculate(RollerSymbol[] rollerSymbols, float rollerLuck)
        {
            _rollerLuck = rollerLuck;
            RollerOption[] rollerOptions = new RollerOption[rollerSymbols.Length];
            float luckCoefficient = rollerLuck * GlobalData.Instance.playerLuck;
            for (int i = 0; i < rollerSymbols.Length; i++)
            {
                int calculatedWeight = rollerSymbols[i].Weight;
                calculatedWeight = Mathf.FloorToInt(calculatedWeight + rollerSymbols[i].LuckWeight * luckCoefficient);
                rollerOptions[i] = new RollerOption(i, calculatedWeight);
            }
            RollerSymbols = new WeightedList<RollerOption>();
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
            LuckPerMachine[i] = new LuckData(RollerSymbols, Random.Range(0.5f, 1.5f));
        }
    }
}
