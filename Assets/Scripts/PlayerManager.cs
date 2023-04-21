using System.Collections.Generic;
using System.Numerics;
using ElementManager;
//using InventoryManager;

namespace PlayerManager
{

    public class Player
    {
        public Element element;
        public bool isFoul = false;
        public Vector3 position = Vector3.Zero;
        public Inventory inventory = new Inventory();
        public Dictionary<Element, int> tokens = new Dictionary<Element, int>(){
            {new Element(0), 0},
            {new Element(1), 0},
            {new Element(2), 0},
            {new Element(3), 0},
            {new Element(4), 0}
        };

        public void Walk(Vector3 amount)
        {
            position += amount;
        }

        public bool CheckIsFoul()
        {
            if (tokens[element] == 0) return false;
            isFoul = true;
            tokens[element] = 0;
            return true;
        }

        public Player(int elementId, int tokenAmount)
        {
            element = new Element(elementId);
            tokens[element] += tokenAmount;
        }
    }
}