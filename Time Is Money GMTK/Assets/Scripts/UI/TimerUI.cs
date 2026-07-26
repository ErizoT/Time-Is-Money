using System;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    public TextMeshProUGUI textField;


    private void OnEnable()
    {
        GlobalData.Instance.OnPlayerTimeChanged += OnTimeChanged;
      
    }

    private void OnDisable()
    {
        GlobalData.Instance.OnPlayerTimeChanged -= OnTimeChanged;

    }




    void OnTimeChanged(float time)
    {
        TimeSpan ts = TimeSpan.FromSeconds(time);
        textField.text = string.Format("{0}:{1:00}", (int)ts.Minutes, ts.Seconds);
    }
}
