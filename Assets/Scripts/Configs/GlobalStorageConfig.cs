using UnityEngine;

[CreateAssetMenu(fileName = "GlobalStorageConfig", menuName = "Configs/GlobalStorageConfig")]
public class GlobalStorageConfig : ScriptableObject
{
    public PlayerConfig PlayerConfig;
    public UIConfig UIConfig;
    public EnemiesConfig EnemiesConfig;
    public WeaponConfig WeaponConfig;
    public DropConfig DropConfig;
    public ScenesConfig ScenesConfig;
}
