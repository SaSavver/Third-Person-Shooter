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

    public string DefaultWeaponID;
}
