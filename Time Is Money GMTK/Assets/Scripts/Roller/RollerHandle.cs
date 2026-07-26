using System;
using UnityEngine;
using UnityEngine.Events;

public class RollerHandle : MonoBehaviour
{
    [SerializeField] RollerDisplay rollerDisplay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnMouseDown()
    {
        rollerDisplay.DoRoll();
    }
}
