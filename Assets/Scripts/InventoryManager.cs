using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemManager;

namespace InventoryManager{
    public class Inventory{
        public int maximumInvSize;
        public List<Item> inv = new List<Item>();

        public void Add(Item item){
            inv.Add(item);
        }

        public void Use(Item item){
            try{
                inv.Remove(item);
            }
            catch (System.Exception){
                //Item 404
                return;
            }
            item.Use(item.name);
        }
    }

}
