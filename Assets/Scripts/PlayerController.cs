using MapManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private GameObject player;

    private int nowPosIndex;
    MapSize mapSize = new MapSize();


    public void LoadGame()
    {
        player.transform.position = new Vector3(0, 50, 0);
        nowPosIndex = 0;
    }

    public void PlayerWalk(int amount) => StartCoroutine(GoWalk(amount));

    private IEnumerator GoWalk(int amount)
    {
        CameraController.Instance.ViewSwitch("overLook");
        Debug.Log($"NowIndex is {nowPosIndex}");
        for (int i = nowPosIndex; i < nowPosIndex + amount; i++)
        {
            yield return new WaitForSeconds(1);
            player.transform.position = (mapSize.mapDictS[nowPosIndex + i] * 150);
        }
        nowPosIndex += amount;
        Debug.Log($"AfterGoIndex is {nowPosIndex}");
        CameraController.Instance.ViewSwitch("backLook");
    }


}
