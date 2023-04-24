using MapManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;
using TMPro.Examples;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject player;

    private int nowPosIndex;
    MapSize mapSize = new MapSize();

    public static PlayerController LocalPlayerInstance; //Local Instance

    private void Awake()
    {
        if (photonView.IsMine) // use Photon to distinguish the player you control
        {
            LocalPlayerInstance = this;
        }
        DontDestroyOnLoad(this);
        
    }


    private void Start()
    {

    }


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
        var posOffset = new Vector3(0, 50, 0);
        for (int i = 0; i < amount; i++)
        {
            yield return new WaitForSeconds(1);
            Debug.Log($"nowPos is {(nowPosIndex + 1 + i) % 16}");
            player.transform.position = (mapSize.mapDictS[(nowPosIndex + 1 + i) % 16] * 150) + posOffset;
        }
        nowPosIndex += amount;
        Debug.Log($"AfterGoIndex is {nowPosIndex}");
        CameraController.Instance.ViewSwitch("backLook");
        GameController.Instance.UpdateCeil();
    }

    public int NowPos() { return nowPosIndex % 16; }

}
