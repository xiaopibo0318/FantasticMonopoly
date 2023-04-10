using System;
using CellManager;

namespace MapManager{
    public class Map{
        public int size;
        public Cell[] cells;

        private void InitNormalCells(int mapSize){
            size = mapSize;
            cells = new Cell[size];
            for (int i = 0; i < size; i++){
                cells[i] = new Cell(false);
            }
        }

        private void SetSpecialCells(int numOfSpecialCells){
            int count = 0;
            while (count <= numOfSpecialCells)
            {
                int rand = UnityEngine.Random.Range(0, size - 1);
                if (cells[rand].isSpecial) continue;
                cells[rand].isSpecial = true;
                count += 1;
            }
        }

        public Map(int mapSize, float specialToNormalRatio){
            InitNormalCells(mapSize);
            int numOfSpecialCells = (int)Math.Round(size * specialToNormalRatio);
            SetSpecialCells(numOfSpecialCells);
        }

        public Map(int mapSize, int numOfSpecialCells){
            InitNormalCells(mapSize);
            SetSpecialCells(numOfSpecialCells);
        }
    }
}