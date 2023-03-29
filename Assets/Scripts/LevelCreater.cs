using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelCreater : Singleton<LevelCreater>
{
    [Header("¦a¹Ï«H®§")]
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private float groundOffset;
    [SerializeField] private Transform groundParent;




    private void LoadLevel1()
    {
        Vector3 nowPos = Vector3.zero;
        Quaternion nowR = Quaternion.identity;
        Instantiate(groundPrefab, nowPos, nowR, groundParent);
    }

}
