using PlayerManager;
using CellManager;

namespace ElementManager{
    public class Element{
        public string type;
        public string color;
        public int id;
        
        public readonly string[] allowedType = new string[] { "Wood", "Fire", "Earth", "Metal", "Aqua" };
        public readonly string[] allowedColor = new string[] {"Green", "Red", "Brown", "Yellow", "Blue"};

        public Element(int elementId){
            this.id = elementId;
            this.type =  allowedType[id];
            this.color = allowedColor[id];
        }
    }

    public class ElementManager{
        public void Reaction(PlayerInfo player, Cell cell){
            int idx = player.element.id;
            int count = 0;
            while (true){
                if (cell.element.type == player.element.allowedType[idx]) break;
                if (idx == 4){
                    idx = 0;
                    continue;
                }
                idx += 1;
                count += 1;
            }
            int effect = count; 
            switch (effect){
                case 0: // e.g."Wood" - "Wood"
                    //ask insert tokens
                    break;
                case 1: // e.g."Wood" - "Fire"
                    cell.token += 2;
                    player.tokens[player.element] -= 1;
                    player.tokens[cell.element] += 2;
                    player.CheckIsFoul();
                    break;
                case 2: // e.g."Wood" - "Earth"
                    cell.token -= 1;
                    player.tokens[cell.element] -= 1;
                    cell.IsTokenEmpty();
                    break;
                case 3: // e.g."Wood" - "Metal"
                    player.tokens[player.element] += 2;
                    cell.token -= 1;
                    player.tokens[cell.element] -= 2;
                    cell.IsTokenEmpty();
                    break;
                case 4: // e.g."Wood" - "Aqua"
                    player.tokens[player.element] -= 1;
                    player.tokens[cell.element] += 1;
                    player.CheckIsFoul();
                    break;
            }
        }
    }
}
