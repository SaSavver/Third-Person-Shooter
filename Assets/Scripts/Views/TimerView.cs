using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerView : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _timerText;

    public void UpdateTimer(int min, int sec)
    {
        _timerText.text = $"{min:00}:{sec:00}";
    }
}
