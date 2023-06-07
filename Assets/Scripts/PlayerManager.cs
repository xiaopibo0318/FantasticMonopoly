using System.Collections.Generic;
using ElementManager;
using Unity.VisualScripting;
using UnityEngine;

namespace PlayerManager
{

    public class PlayerInfo
    {
        public Element element;
        public bool isFoul = false;
        public Vector3 position = Vector3.zero;
        public Backpack Invnetory = ScriptableObject.CreateInstance<Backpack>();
        public Dictionary<Element, int> tokens = new Dictionary<Element, int>(){
            {new Element(0), 0},
            {new Element(1), 0},
            {new Element(2), 0},
            {new Element(3), 0},
            {new Element(4), 0}
        };

        public bool CheckIsFoul()
        {
            if (tokens[element] == 0) return false;
            isFoul = true;
            tokens[element] = 0;
            return true;
        }

        public PlayerInfo(int elementId, int tokenAmount)
        {

            Element targetKey = null;
            foreach (var item in tokens)
            {
                if (item.Key.id == elementId)
                {
                    targetKey = item.Key;
                }
            }
            if (targetKey != null) { tokens[targetKey] += tokenAmount; }
            else { Debug.Log("Fail To set TokenNum , because targetKey is null"); }
            this.element = targetKey;
        }
    }
}

