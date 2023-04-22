using System.Collections.Generic;
using ElementManager;
using Unity.VisualScripting;
using UnityEngine;
//using InventoryManager;

namespace PlayerManager
{

    public class Player
    {
        public Element element;
        public bool isFoul = false;
        public Vector3 position = Vector3.zero;
        public Inventory inventory = new Inventory();
        public Dictionary<Element, int> tokens = new Dictionary<Element, int>(){
            {new Element(0), 0},
            {new Element(1), 0},
            {new Element(2), 0},
            {new Element(3), 0},
            {new Element(4), 0}
        };

        //public void Walk(Vector3 amount)
        //{
        //    position += amount;
        //}

        public bool CheckIsFoul()
        {
            if (tokens[element] == 0) return false;
            isFoul = true;
            tokens[element] = 0;
            return true;
        }

        public Player(int elementId, int tokenAmount) //This part couldn't use, 
        {
            //element = new Element(elementId); // because the element is new, so it won't be found.
            //tokens[element] += tokenAmount;   // this will error: keynotfoundexception

            Element targetKey = null;
            foreach (var item in tokens)
            {
                if (item.Key.id == elementId)
                {
                    targetKey = item.Key;
                }
            }
            if (targetKey != null) { tokens[targetKey] += tokenAmount; }
            else { Debug.Log("Fail To set TokenNum , because of targetKey is null"); }

        }
    }
}

