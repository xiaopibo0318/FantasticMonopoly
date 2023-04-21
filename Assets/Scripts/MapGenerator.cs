using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapManager;

public class MapGenerator : Singleton<MapGenerator>
{
    [Header("Ground Object")]
    [SerializeField] GameObject[] groundList;
    [SerializeField] Transform groundParent;

    //[Header("Map Size")]
    //Dictionary<string, int> mapSize = new Dictionary<string, int>()
    //{{"S",15},{"M",25 },{"L",35 }};




    public void MapGenerate()
    {
        int offset = 150;
        var mapSize = new MapSize();
        for (int i = 0; i < mapSize.mapDictS.Length; i++)
        {
            int index = Random.Range(0, groundList.Length);
            //Vector3 pos = new Vector3(offset, 0, 0) + groundList[index].transform.position;
            Vector3 pos = (mapSize.mapDictS[i] * offset) + groundList[index].transform.position;
            Instantiate(groundList[index], pos, Quaternion.identity, groundParent);
        }
        Debug.Log($"Build Succeed");
    }

}
