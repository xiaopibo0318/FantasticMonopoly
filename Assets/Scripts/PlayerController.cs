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

    public Animator playerAnimator;

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
        player.transform.position = new Vector3(-10, -20, 0);
        nowPosIndex = 0;
    }

    public void PlayerWalk(int amount) => StartCoroutine(GoWalk(amount));

    private IEnumerator GoWalk(int amount)
    {
        SiginalUI.Instance.SiginalText(pv.Owner.NickName + amount.ToString());
        CameraController.Instance.ViewSwitch("overLook");
        Debug.Log($"NowIndex is {nowPosIndex}");
        var posOffset = new Vector3(-10, -20, 0);
        
        for (int i = 0; i < amount; i++)
        {
            playerAnimator.SetBool("walk",true);
            yield return new WaitForSeconds(1.05f);
            playerAnimator.SetBool("walk",false);
            
            player.transform.Rotate(0,-4.2f,0);
            //player.transform.Translate(1,0,0);
            int cPos = (nowPosIndex + 1 + i)% 16;

            if(cPos == 3 || cPos == 8 || cPos == 11 || cPos==0)
            {
                player.transform.position = (mapSize.mapDictS[(nowPosIndex + 1 + i) % 16] * 150) + posOffset;
                player.transform.Rotate(0,90,0);
            }
            
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
