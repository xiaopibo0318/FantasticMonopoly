using ElementManager;

namespace CellManager{
    public class Cell{
        public bool isSpecial = false;
        public Element element = null;
        public int token;

        public bool IsTokenEmpty(){
            if (token > 0) return false;
            element = null;
            token = 0;
            return true;
        }

        public Cell(bool cellType){
            isSpecial = cellType;
            element = null;
            token = 0;
        }
    }
}
