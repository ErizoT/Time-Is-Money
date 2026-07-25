using UnityEngine;

[CreateAssetMenu(fileName = "RollerSymbol", menuName = "Scriptable Objects/RollerSymbol")]
public class RollerSymbol : ScriptableObject, IWeightedListEntry
{
    [SerializeField]
    private string symbolId;
    public string SymbolId => symbolId;
    public Texture2D SymbolTex;

    [SerializeField, Tooltip("The default number of symbols that are added to the pool.")]
    private int symbolCount;
    [HideInInspector]
    public int SymbolCount;
    public int Weight => SymbolCount;
    [SerializeField, Tooltip("The current luck value is multiplied by this in order to calculate the current pool.")]
    private float luckWeight;
    [HideInInspector]
    public float LuckWeight;

    public void Awake()
    {
        SymbolCount = symbolCount;
        LuckWeight = luckWeight;
    }
}
