using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IDropItem
{
    public string ID { get; }
    public DropItemView DropItemPrefab { get; }
    public DropItemType DropItemType { get; }
}

public enum DropItemType
{
    Experience,
    Healthpack,
    PowerUp
}

public class HealthKitItemData : IDropItem
{
    public string ItemID;

    public DropItemType DropItemTypeLocal;

    public float RestoreHealthAmount;
    public float ItemDropChance;

    public DropItemView ItemPrefab;

    public string ID => ItemID;
    public float DropChance => ItemDropChance;
    public DropItemView DropItemPrefab => ItemPrefab;
    public DropItemType DropItemType => DropItemTypeLocal;
}

public class ExperienceItemData : IDropItem
{
    public string ItemID;

    public DropItemType DropItemTypeLocal;

    public float ExpToGive;
    public float ItemDropChance;

    public DropItemView ItemPrefab;

    public string ID => ItemID;
    public DropItemView DropItemPrefab => ItemPrefab;
    public DropItemType DropItemType => DropItemTypeLocal;
}

[CreateAssetMenu(fileName = "DropConfig", menuName = "Configs/DropConfig")]
public class DropConfig : ScriptableObject
{
    [SerializeReference] public IDropItem[] DropItems;

    public float ItemMagnetSpeed;

    public IDropItem GetDropItemByID(string id)
    {
       return DropItems.FirstOrDefault(itm => itm.ID == id);
    }
}
