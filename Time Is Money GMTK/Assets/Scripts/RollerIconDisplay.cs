using Unity.VisualScripting;
using UnityEngine;

public class RollerIconDisplay : MonoBehaviour
{
    [SerializeField] SpriteRenderer iconRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetRollerSymbol(RollerSymbol symbol)
    {
        iconRenderer.sprite = symbol.SymbolSprite;
    }
}
