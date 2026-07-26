using UnityEngine;

[CreateAssetMenu(fileName = "RollerSymbol", menuName = "Scriptable Objects/RollerSymbol")]
public class RollerSymbol : ScriptableObject, IWeightedListEntry
{
    [SerializeField]
    protected string symbolId;
    public string SymbolId => symbolId;
    [SerializeField]
    protected string symbolDescription;
    public virtual string SymbolDescription { 
        get {
            return symbolDescription + '\n' +
            $"On single rolled: {GainOrLoseSingle} ${SymbolSingleValue}" + '\n' +
            $"On triple rolled: {GainOrLoseTriple} ${SymbolTripleValue}";
        } 
    }
    public virtual string GainOrLoseSingle => SymbolSingleValue < 0 ? "gain" : "lose";
    public virtual string GainOrLoseTriple => SymbolTripleValue < 0 ? "gain" : "lose";
    public Sprite SymbolSprite;

    [SerializeField, Tooltip("The default number of symbols that are added to the pool.")]
    protected int symbolCount;
    [HideInInspector]
    public int SymbolCount;
    public int Weight { get => SymbolCount; set => symbolCount = value; }
    [SerializeField]
    protected float symbolSingleValue = 0;
    [HideInInspector]
    public float SymbolSingleValue;
    [SerializeField]
    protected float symbolTripleValue = 5;
    [HideInInspector]
    public float SymbolTripleValue;
    [SerializeField, Tooltip("The current luck value is multiplied by this in order to calculate the current pool.")]
    protected float luckWeight;
    [HideInInspector]
    public float LuckWeight;

    public virtual void DoSingleEffect()
    {
        GlobalData.Instance.PlayerMoney += (int)SymbolSingleValue;
    }
    public virtual void DoTripleEffect()
    {
        GlobalData.Instance.PlayerMoney += (int)SymbolTripleValue;
    }

    public void OnEnable()
    {
        SymbolCount = symbolCount;
        SymbolSingleValue = symbolSingleValue;
        SymbolTripleValue = symbolTripleValue;
        LuckWeight = luckWeight;
    }
}
