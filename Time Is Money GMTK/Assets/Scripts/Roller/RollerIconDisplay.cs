using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RollerIconDisplay : MonoBehaviour
{
    public Vector3 forwardsVector;
    [SerializeField] SpriteRenderer iconRenderer;
    [SerializeField] Sprite QueuedSprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void QueueRollerSymbol(RollerSymbol symbol)
    {
        QueuedSprite = symbol.SymbolSprite;
    }
    public void RollStep(float rollAngle)
    {
        if (Vector3.Dot(transform.forward, forwardsVector) <= 0)
        {
            iconRenderer.sprite = QueuedSprite;
        }
    }
}
