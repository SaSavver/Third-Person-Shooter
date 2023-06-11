using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UIConfig", menuName = "Configs/UIConfig")]
public class UIConfig : ScriptableObject
{
    public float HPBarLerpSpeed = 5f;
    public HealthbarView HealthbarPrefab;
}
