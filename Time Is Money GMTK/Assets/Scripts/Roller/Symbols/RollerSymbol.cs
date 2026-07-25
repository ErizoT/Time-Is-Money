using UnityEngine;

[CreateAssetMenu(fileName = "RollerSymbol", menuName = "Scriptable Objects/RollerSymbol")]
public class RollerSymbol : ScriptableObject, IWeightedListEntry
{
    [SerializeField]
    protected string symbolId;
    public string SymbolId => symbolId;
    public Sprite SymbolSprite;

    [SerializeField, Tooltip("The default number of symbols that are added to the pool.")]
    protected int symbolCount;
    [HideInInspector]
    public int SymbolCount;
    public int Weight => SymbolCount;
    [SerializeField]
    protected int symbolSingleValue = 0;
    [HideInInspector]
    public int SymbolSingleValue;
    [SerializeField]
    protected int symbolTripleValue = 5;
    [HideInInspector]
    public int SymbolTripleValue;
    [SerializeField, Tooltip("The current luck value is multiplied by this in order to calculate the current pool.")]
    protected float luckWeight;
    [HideInInspector]
    public float LuckWeight;

    public virtual void DoSingleEffect()
    {
        GlobalData.Instance.playerMoney += SymbolSingleValue;
    }
    public virtual void DoTripleEffect()
    {
        GlobalData.Instance.playerMoney += SymbolTripleValue;
    }

    public void OnEnable()
    {
        SymbolCount = symbolCount;
        SymbolSingleValue = symbolSingleValue;
        SymbolTripleValue = symbolTripleValue;
        LuckWeight = luckWeight;
    }
}
