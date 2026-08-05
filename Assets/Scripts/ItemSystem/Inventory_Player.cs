using System.Collections.Generic;
using UnityEngine;

public class Inventory_Player : Inventory_Base
{
    private Stats_System playerStats;
    public List<Inventory_EquipmentSlot> equipList;

    protected override void Awake()
    {
        base.Awake();
        playerStats = GetComponent<Stats_System>();
    }

    public void TryEquipItem(Inventory_Item item)
    {
        var inventoryItem = FindItem(item.itemData);
        var matchingSlots = equipList.FindAll(slot => slot.slotType == item.itemData.itemType);

        // STEP 1 :Try to find empty slot and equip item
        foreach (var slot in matchingSlots)
        {
            if (slot.HasItem() == false)
            {
                EquipItem(inventoryItem, slot);
                return;
            }
        }

        var slotToReplace = matchingSlots[0];
        var itemToUneqip = slotToReplace.equipedItem;

        EquipItem(inventoryItem, slotToReplace);
        UnequipItem(itemToUneqip);
    }

    private void EquipItem(Inventory_Item itemToEquip, Inventory_EquipmentSlot slot)
    {
        slot.equipedItem = itemToEquip;
        slot.equipedItem.AddModifiers(playerStats);

        RemoveItem(itemToEquip);
    }

    public void UnequipItem(Inventory_Item itemToUnequip)
    {
        if (CanAddItem() == false)
        {
            Debug.Log("No space!");
            return;
        }

        foreach (var slot in equipList)
        {
            if (slot.equipedItem == itemToUnequip)
            {
                slot.equipedItem = null;
                break;
            }
        }

        itemToUnequip.RemoveModifiers(playerStats);
        AddItem(itemToUnequip);
    }
}
