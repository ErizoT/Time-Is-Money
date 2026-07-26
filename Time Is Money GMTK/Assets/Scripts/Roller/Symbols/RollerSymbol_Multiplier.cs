using UnityEngine;

[CreateAssetMenu(fileName = "RollerSymbol_Multiplier", menuName = "Scriptable Objects/RollerSymbol_Multiplier")]
public class RollerSymbol_Multiplier : RollerSymbol, IWeightedListEntry
{
    public override string SymbolDescription
    {
        get
        {
            string singleDescription = SymbolSingleValue < 1 ?
                $"On single rolled: divide current money by {(Mathf.RoundToInt((1 / SymbolSingleValue )*10) / 10).ToString() + '.' + (Mathf.RoundToInt((1 / SymbolSingleValue) * 10) % 10).ToString()}" :
                $"On single rolled: multiply current money by {SymbolSingleValue}";
            string tripleDescription = SymbolTripleValue < 1 ?
                $"On single rolled: divide current money by {(Mathf.RoundToInt((1 / SymbolTripleValue) * 10) / 10).ToString() + '.' + (Mathf.RoundToInt((1 / SymbolSingleValue) * 10) % 10).ToString()}" :
                $"On single rolled: multiply current money by {SymbolTripleValue}";
            return symbolDescription + '\n' +
            singleDescription + '\n' +
            tripleDescription;
        }
    }
    public override void DoSingleEffect()
    {
        GlobalData.Instance.PlayerMoney *= SymbolSingleValue;
    }
    public override void DoTripleEffect()
    {
        GlobalData.Instance.PlayerMoney *= SymbolTripleValue;
    }
}
