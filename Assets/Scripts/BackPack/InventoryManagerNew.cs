using System.Collection.Generic;
using System.Collection;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager>
{
    public Inventory Backpack;
    public GameObject SlotGrid;
    public GameObject EmptySlot;
    
    private List<GameObject> Slots = new List<GameObject>();
    
    private void OnEnable(){
    }

    public void UpdateItemInfo(string Description)
    {
        
    }

    public void AddItem(Item NewItem)
    {
    }
}