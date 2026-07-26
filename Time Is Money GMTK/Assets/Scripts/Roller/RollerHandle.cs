using UnityEngine;
using UnityEngine.Events;

public class RollerHandle : MonoBehaviour
{
    UnityEvent m_PulledEvent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnMouseDown()
    {
        if (m_PulledEvent == null)
            m_PulledEvent = new UnityEvent();
    }
}
