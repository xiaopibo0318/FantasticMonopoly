using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapManager;
using static UnityEditor.PlayerSettings;
using System;
using System.Reflection;

public class MapGenerator : Singleton<MapGenerator>
{
    [Header("Ground Object")]
    [SerializeField] GameObject[] groundList;
    [SerializeField] Transform groundParent;
    [SerializeField] GameObject normalCell;
    [SerializeField] GameObject specitalCell;

    //[Header("Map Size")]
    //Dictionary<string, int> mapSize = new Dictionary<string, int>()
    //{{"S",15},{"M",25 },{"L",35 }};


    public void MapGenerate(Map map = null)
    {
        if (map == null)
        {
            Debug.Log($"Map Generate Fail, cause map is null");
            return;
        }
        int offset = 150;
        var mapSize = new MapSize();
        for (int i = 0; i < mapSize.mapDictS.Length; i++)
        {
            if (map.cells[i].isSpecial)
            {
                Vector3 pos = (mapSize.mapDictS[i] * offset) + specitalCell.transform.position;
                Instantiate(specitalCell, pos, Quaternion.identity, groundParent);
            }
            else
            {
                Vector3 pos = (mapSize.mapDictS[i] * offset) + normalCell.transform.position;
                Instantiate(normalCell, pos, Quaternion.identity, groundParent);
            }
        }
        Debug.Log($"Build Succeed");
    }

}

/* Generate 5 elment map
 * 
 * int index = Random.Range(0, groundList.Length);
   Vector3 pos = (mapSize.mapDictS[i] * offset) + groundList[index].transform.position;
   Instantiate(groundList[index], pos, Quaternion.identity, groundParent);
 * 
*/