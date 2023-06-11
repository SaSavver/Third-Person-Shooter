using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarView : MonoBehaviour
{
    [SerializeField]
    private Image _healthSlider;

    public void ChangeHealthSliderValue(float currentHealth, float maxHealth)
    {
        var progress = currentHealth / maxHealth;
        _healthSlider.fillAmount = progress;
    }
}
