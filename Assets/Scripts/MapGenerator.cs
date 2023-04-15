using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : Singleton<MapGenerator>
{
    [Header("地形物件")]
    [SerializeField] GameObject[] groundList;
    [SerializeField] Transform groundParent;


    public void MapGenerate(int groundSum = 20)
    {
        int offset = 150;
        for (int i = 0; i < groundSum; i++)
        {
            int index = Random.Range(0, groundList.Length);
            Vector3 pos = new Vector3(offset, 0, 0) + groundList[index].transform.position;
            Instantiate(groundList[index], pos, Quaternion.identity, groundParent);
            offset += 100;
        }
        Debug.Log($"建置完畢");
    }

}
