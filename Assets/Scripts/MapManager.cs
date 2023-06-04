using System;
//using System.Numerics;
using CellManager;
using UnityEngine;
namespace MapManager
{
    public class Map
    {
        public int size;
        public Cell[] cells;

        private void InitNormalCells(int mapSize)
        {
            size = mapSize;
            cells = new Cell[size];
            for (int i = 0; i < size; i++)
            {
                cells[i] = new Cell(false);
            }
        }

        private void SetSpecialCells(int numOfSpecialCells)
        {
            int count = 0;
            while (count <= numOfSpecialCells)
            {
                int rand = UnityEngine.Random.Range(0, size - 1);
                if (cells[rand].isSpecial) continue;
                cells[rand].isSpecial = true;
                count += 1;
            }
        }

        public Map(int mapSize, float specialToNormalRatio)
        {
            InitNormalCells(mapSize);
            int numOfSpecialCells = (int)Math.Round(size * specialToNormalRatio) - 1;
            SetSpecialCells(numOfSpecialCells);
        }

    }
    


    /// <summary>
    /// Define 3 size of Map with Small(16), Midium(25), Large(35),
    /// </summary>
    public class MapSize
    {
        public readonly Vector3[] mapDictS = {
            new Vector3(0, 0, 0), new Vector3(0, 0, 1), new Vector3(0, 0, 2), new Vector3(0, 0, 3),
            new Vector3(1, 0, 3), new Vector3(2, 0, 3), new Vector3(3, 0, 3), new Vector3(4, 0, 3),
            new Vector3(5, 0, 3), new Vector3(5, 0, 2), new Vector3(5, 0, 1), new Vector3(5, 0, 0),
            new Vector3(4, 0, 0), new Vector3(3, 0, 0), new Vector3(2, 0, 0), new Vector3(1, 0, 0)
        };
    }



}