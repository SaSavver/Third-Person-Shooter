using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    public float PlayerMoveSpeed;
    public float PlayerInputVectorToTurn;

    public float PlayerMaxHealth = 100f;

    public float DistanceToPickUpItem;
    public float DefaultDistanceToMagnetItem;

    public float DefaulMaxExp;

    public string DefaultWeaponID;

    public float GetNewMaxXp(float lvl)
    {
        var a = (Mathf.Exp(lvl) * Mathf.Log10(lvl)) / lvl;
        var output = Mathf.Clamp(a, 1, float.MaxValue) * 100;
        return output;
    }
}
