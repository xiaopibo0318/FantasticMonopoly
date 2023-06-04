using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Inventory")]
public class Backpack : ScriptableObject{
    public List<Item> Inventory = new List<Item>();
    public List<int> Tracker = new List<int>();
}