using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public Backpack Backpack;
    public GameObject SlotGrid;
    public GameObject EmptySlot;

    private List<GameObject> Slots = new List<GameObject>();

    private void OnEnable()
    {
        for (int i = 0; i < 8; i++)
        {
            Backpack.Tracker.Add(i);
        }
    }

    public void UpdateItemInfo(string Description)
    {
        
    }

    private void RefreshUI()
    {
        if (SlotGrid.transform.childCount != 0)
        {
            for (int i = 0; i < SlotGrid.transform.childCount; i++)
            {
                Destroy(SlotGrid.transform.GetChild(i).gameObject);
            }
            Slots.Clear();
        }

        for (int i = 0; i < Backpack.Inventory.Count; i++)
        {
            Slots.Add(Instantiate(EmptySlot));
            var ItemSlot = Slots[i];
            ItemSlot.transform.SetParent(SlotGrid.transform);
            ItemSlot.transform.localScale = new Vector3(1, 1, 1);
            var Component = ItemSlot.GetComponent<Slot>();
            Component.slotID = i;
            Component.slotItem = Backpack.Inventory[i];
            Component.SetupSlot(Backpack.Inventory[i]);
        }
    }

    private bool IsBackpackFull()
    {
        return Backpack.Inventory.Count == 8;
    }

    public void AddItem(Item Item)
    {
        if (Backpack.Inventory.Contains(Item))
        {
            Item.ItemCount += 1;
            RefreshUI();
            return;
        }

        int Indx = Backpack.Tracker[0];
        Backpack.Inventory[Indx] = Item;
        Slots[Indx].GetComponent<Slot>().slotItem = Item;
        Item.ItemCount += 1;
        Backpack.Tracker.RemoveAt(0);
        Backpack.Tracker.Sort();
        RefreshUI();
    }

    public void RemoveItem(Item Item, int Num = 1)
    {
        if (!Backpack.Inventory.Contains(Item)) return;
        Item.ItemCount -= Num;
        if (Item.ItemCount < 0)
        {
            int ItemIndx = Backpack.Inventory.IndexOf(Item);
            Backpack.Tracker.Add(ItemIndx);
            Backpack.Tracker.Sort();
            Backpack.Inventory[ItemIndx] = null;
        }
        RefreshUI();
    }
}