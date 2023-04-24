using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapManager;
using System;
using System.Reflection;
using PlayerManager;

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
    int offset = 150;

    public void MapGenerate(Map map = null)
    {
        if (map == null)
        {
            Debug.Log($"Map Generate Fail, cause map is null");
            return;
        }

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


    public void UpdateCeil(Map map, int targetIndex, Player player)
    {
        Debug.Log($"is special? :{map.cells[targetIndex].isSpecial}, index is{targetIndex}, playElementId is{player.element.id}");
        var mapSize = new MapSize();
        if (map.cells[targetIndex].isSpecial == false)
        {
            int nowElementID = player.element.id;
            Vector3 pos = (mapSize.mapDictS[targetIndex] * offset) + groundList[nowElementID].transform.position;
            Instantiate(groundList[nowElementID], pos, Quaternion.identity, groundParent);
        }
    }

}

/* Generate 5 elment map
 * 
 * int index = Random.Range(0, groundList.Length);
   Vector3 pos = (mapSize.mapDictS[i] * offset) + groundList[index].transform.position;
   Instantiate(groundList[index], pos, Quaternion.identity, groundParent);
 * 
*/