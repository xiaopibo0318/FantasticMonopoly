using MapManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;
using TMPro;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject player;
    [SerializeField] private TextMeshPro playerName;

    private int nowPosIndex;
    MapSize mapSize = new MapSize();
    PhotonView pv;

    public static PlayerController LocalPlayerInstance; //Local Instance

    public enum PlayerGameState
    {
        Menu, Item, Dice, Move, End
    }


    private void Awake()
    {
        pv = this.gameObject.GetComponent<PhotonView>();
        if (pv.IsMine) // use Photon to distinguish the player you control
        {
            LocalPlayerInstance = this;
        }
    }




    private void Start()
    {
        UpdateName();
    }


    public void LoadGame()
    {
        player.transform.position = new Vector3(0, 50, 0);
        nowPosIndex = 0;
    }

    public void PlayerWalk(int amount) => StartCoroutine(GoWalk(amount));

    private IEnumerator GoWalk(int amount)
    {
        SiginalUI.Instance.SiginalText(pv.Owner.NickName + amount.ToString());
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
        GameCycleControler.Instance.playerFinish = true;
        HashTable table = new HashTable();
        table.Add("playerElement", GameController.Instance.player1.element.id);
        table.Add("playerPos", nowPosIndex);
        table.Add("playerFinish", GameCycleControler.Instance.playerFinish);
        PhotonNetwork.LocalPlayer.SetCustomProperties(table);
    }

    public int NowPos() { return nowPosIndex % 16; }

    private void UpdateName()
    {
        player.gameObject.name = "Player" + pv.Owner.NickName;
        playerName.text = pv.Owner.NickName;
    }

}
