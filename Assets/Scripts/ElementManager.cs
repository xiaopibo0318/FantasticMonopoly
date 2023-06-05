using CellManager;
using HashTable = ExitGames.Client.Photon.Hashtable;

namespace ElementManager
{
    public class Element
    {
        public string type;
        public string color;
        public int id;

        public readonly string[] allowedType = new string[] { "Wood", "Fire", "Earth", "Metal", "Aqua" };
        public readonly string[] allowedColor = new string[] { "Green", "Red", "Brown", "Yellow", "Blue" };

        public Element(int elementId)
        {
            this.id = elementId;
            this.type = allowedType[id];
            this.color = allowedColor[id];
        }
    }

    public class ElementManager
    {
        public HashTable Reaction(int playerElementId, HashTable playerTokens, Cell cell)
        {
            HashTable tokens = playerTokens;
            int idx = playerElementId;
            int count = 0;
            while (true)
            {
                if (cell.element == null) break;
                if (cell.element.id == idx) break;
                if (idx == 4)
                {
                    idx = 0;
                    continue;
                }
                idx += 1;
                count += 1;
            }
            int effect = count;
            string playerElement = "element " + playerElementId;
            string cellElement;
            if (cell.element == null)
            {
                cellElement = playerElement;
            }
            else
            {
                cellElement = "element " + cell.element.id;
            }
            int playerTmp = (int)playerTokens[playerElement];
            int cellTmp = (int)playerTokens[cellElement];
            switch (effect)
            {
                case 0: // e.g."Wood" - "Wood"
                    cell.token += 2;
                    break;
                case 1: // e.g."Wood" - "Fire"
                    cell.token += 2;
                    tokens.Remove(playerElement);
                    tokens.Add(playerElement, playerTmp - 1);
                    tokens.Remove(cellElement);
                    tokens.Add(cellElement, cellTmp + 2);
                    break;
                case 2: // e.g."Wood" - "Earth"
                    cell.token -= 1;
                    tokens.Remove(cellElement);
                    tokens.Add(cellElement, cellTmp - 1);
                    cell.IsTokenEmpty();
                    break;
                case 3: // e.g."Wood" - "Metal"
                    tokens.Remove(playerElement);
                    tokens.Add(playerElement, playerTmp + 2);
                    cell.token -= 1;
                    tokens.Remove(cellElement);
                    tokens.Add(cellElement, cellTmp - 2);
                    cell.IsTokenEmpty();
                    break;
                case 4: // e.g."Wood" - "Aqua"
                    tokens.Remove(playerElement);
                    tokens.Add(playerElement, playerTmp - 1);
                    tokens.Remove(cellElement);
                    tokens.Add(cellElement, cellTmp + 1);
                    break;
            }

            return tokens;
        }
    }
}
