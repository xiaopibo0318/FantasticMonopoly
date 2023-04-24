using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapManager;
using System;
using System.Reflection;
using PlayerManager;
using Photon.Pun;

public class MapGenerator : MonoBehaviourPunCallbacks
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

    public static MapGenerator Instance;
    private void Awake()
    {
        Instance = this;

    }


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
                var myObject = PhotonNetwork.Instantiate(specitalCell.name, pos, Quaternion.identity, 0);
                myObject.transform.SetParent(groundParent);
            }
            else
            {
                Vector3 pos = (mapSize.mapDictS[i] * offset) + normalCell.transform.position;
                var myObject = PhotonNetwork.Instantiate(normalCell.name, pos, Quaternion.identity, 0);
                myObject.transform.SetParent(groundParent);
            }
        }
        Debug.Log($"Build Succeed");
    }


    public void UpdateCeil(Map map, int targetIndex, PlayerInfo player)
    {
        Debug.Log($"is special? :{map.cells[targetIndex].isSpecial}, index is{targetIndex}, playElementId is{player.element.id}");
        var mapSize = new MapSize();
        if (map.cells[targetIndex].isSpecial == false)
        {
            int nowElementID = player.element.id;
            Vector3 pos = (mapSize.mapDictS[targetIndex] * offset) + groundList[nowElementID].transform.position;
            var myObject = PhotonNetwork.Instantiate(groundList[nowElementID].name, pos, Quaternion.identity, 0);
            myObject.transform.SetParent(groundParent);
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