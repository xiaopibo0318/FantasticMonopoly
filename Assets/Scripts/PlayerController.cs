using MapManager;
using System.Collections;
using System.Collections.Generic;
using CellManager;
using ElementManager;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject player;
    [SerializeField] private TextMeshPro playerName;

    private int nowPosIndex;
    MapSize mapSize = new MapSize();
    PhotonView pv;
    private static int id = 0;
    private Element element{ get; set; }
    private HashTable tokens{ get; set; }
    public bool isFoul = false;
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
        InitPlayer();
    }


    public void LoadGame()
    {
        player.transform.position = new Vector3(0, 50, 0);
        nowPosIndex = 0;
    }

    public void PlayerWalk(int amount) => StartCoroutine(GoWalk(amount));

    private IEnumerator GoWalk(int amount){
        SiginalUI.Instance.SiginalText(pv.Owner.NickName + " " + amount.ToString());
        CameraController.Instance.ViewSwitch("overLook");
        Debug.Log($"NowIndex is {nowPosIndex}");
        var posOffset = new Vector3(0, 50, 0);
        for (int i = 0; i < amount; i++){
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

    private void InitPlayer(){
        UpdateName();
        element = new Element(id);
        int amount = 10;//init amount of the selected element
        for (int i = 0; i <= 4; i++){
            if (i == id){
                tokens.Add("element " + i, amount);
                continue;
            }
            tokens.Add("element " + i, 0);
        }
    }

    public void CheckIsFoul(){
        string playerElement = "element " + id;
        int playerToken = (int)tokens[playerElement];
        if (playerToken <= 0){
            isFoul = true;
        }
    }

    public void Reaction(Cell cell){
        int idx = id;
        int count = 0;
        while (true){
            if (cell.element.type == element.allowedType[idx]) break;
            if (idx == 4){
                idx = 0;
                continue;
            }
            idx += 1;
            count += 1;
        }
        int effect = count;
        string playerElement = "element " + id;
        string cellElement = "element " + cell.element.id;
        int playerTmp = (int)tokens[playerElement];
        int cellTmp = (int)tokens[cellElement];
        switch (effect){
            case 0: // e.g."Wood" - "Wood"
                //ask insert tokens
                break;
            case 1: // e.g."Wood" - "Fire"
                cell.token += 2;
                tokens.Remove(playerElement);
                tokens.Add(playerElement, playerTmp - 1);
                tokens.Remove(cellElement);
                tokens.Add(cellElement, cellTmp + 1);
                CheckIsFoul();
                break;
            case 2: // e.g."Wood" - "Earth"
                cell.token -= 1;
                tokens.Remove(cellElement);
                tokens.Add(cellElement, cellTmp - 1);
                cell.IsTokenEmpty();
                break;
            case 3: // e.g."Wood" - "Metal"
                tokens.Remove(playerElement);
                tokens.Add(playerElement, playerTmp + 1);
                cell.token -= 1;
                tokens.Remove(cellElement);
                tokens.Add(cellElement, cellTmp - 2);
                cell.IsTokenEmpty();
                break;
            case 4: // e.g."Wood" - "Aqua"
                tokens.Remove(playerElement);
                tokens.Add(playerElement, playerTmp - 1);
                tokens.Remove(cellElement);
                tokens.Add(cellElement, cellTmp + 1);
                CheckIsFoul();
                break;
        }
    }
    
}
