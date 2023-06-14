using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpBarView : MonoBehaviour
{
    [SerializeField]
    private Image _expProgressionBar;

    [SerializeField]
    private TMP_Text _currentLevelText,
                     _nextLevelText;

    public void UpdateExpBarProgression(float current, float max, int level)
    {
        var progression = current / max;
        _expProgressionBar.fillAmount = progression;

        _currentLevelText.text = $"{level}";
        _nextLevelText.text = $"{level+1}";
    }
}
