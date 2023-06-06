using UnityEngine;
using PlayerManager;
using MapManager;
using System.Linq;
using Photon.Pun;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEditor;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using Photon.Realtime;
using HashTable = ExitGames.Client.Photon.Hashtable;
using CellManager;
using UnityEngine.Rendering.VirtualTexturing;

public class GameController : MonoBehaviourPunCallbacks
{
    [Header("All Game Info")]
    public PlayerInfo player1 = null;
    public Map map = null;
    [SerializeField] private GameObject playerPrefab;
    public int nowDiceIndex { get; set; }
    public int totalRound { get; set; }

    public static GameController Instance;

    public List<string> playerNameList = new List<string>();

    private List<Player> sortedPlayerList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
        Destroy(this);
    }



    /// <summary>
    /// Game process devide into n steps, 
    /// </summary>
    public enum GameState
    {
        Menu, Item, Dice, Move, End
    }


    /// <summary>
    /// All player execute this function
    /// </summary>
    public void CreateNewGame()
    {
        CreatePlayer();
        sortedPlayerList = PhotonNetwork.CurrentRoom.Players.Values.OrderBy(player => player.ActorNumber).ToList();

        int i = 2;
        foreach (Player player in sortedPlayerList)
        {
            if (player.NickName == PhotonNetwork.LocalPlayer.NickName)
            {
                player1 = new PlayerInfo(i, 10);// ElmentID = 0, tokenNum = 10;
                PlayerController.LocalPlayerInstance.UpdateMapDataToPlayer(map);
            }
            i++;
        }
    }

    /// <summary>
    /// Only room keeper execute this function
    /// </summary>
    public void InitGame()
    {
        map = new Map(16, 0.25f); // Small Map
        MapGenerator.Instance.MapGenerate(map);
        // gameCoroutine = StartCoroutine(GameCycle());

    }


    //private void UpdatePublicDataToPlayer()
    //{

    //    int i = 2;
    //    foreach (Player player in sortedPlayerList)
    //    {
    //        Debug.Log("Update Public Data");
    //        if (map == null) { Debug.Log("Update Map Data Failed"); }
    //        if (player.NickName == PhotonNetwork.LocalPlayer.NickName)
    //        {
    //            PlayerController.LocalPlayerInstance.UpdateMapDataToPlayer(map);
    //        }
    //        i++;
    //    }
    //}


    public void UpdateMapDataToGameController(Map _map)
    {
        map = _map;
    }


    public void UpdateCeil()
    {
        nowDiceIndex = PlayerController.LocalPlayerInstance.NowPos();
        //MapGenerator.Instance.UpdateCeil(map, nowDiceIndex, player1);
    }

    private void CreatePlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("Can't Create Player, because playerPrefab is empty.");
            return;
        }
        PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0, 50, 0), Quaternion.identity, 0);
    }

    public void AddNameList(string name)
    {
        playerNameList.Add(name);
    }


    





    //  #region GameCycle Region
    //  public bool playerFinish { get; set; }
    //  Coroutine gameCoroutine = null;
    //  private IEnumerator GameCycle()
    //  {
    //      int x = 3;
    //      playerFinish = false;
    //      while (x > 0)
    //      {
    //          for (int i = 0; i < playerNameList.Count; i++) // i is player in active
    //          {
    //              HashTable table = new HashTable();
    //              table.Add("whoseTurn", playerNameList[i]);
    //              Debug.Log($"Now Player Name is:{playerNameList[i]}");
    //              if (PhotonNetwork.LocalPlayer.NickName == playerNameList[i])
    //              {
    //                  UiController.Instance.DiceButtonInteractable(true);
    //              }
    //              else { UiController.Instance.DiceButtonInteractable(false); }
    //
    //              if (!playerFinish)
    //              {
    //                  Debug.Log("Player didn't finished");
    //                  yield return new WaitUntil(() => playerFinish);
    //                  Debug.Log("Player finished");
    //              }
    //              playerFinish = false;
    //          }
    //          x--;
    //      }
    //      Debug.Log("End Game");
    //      SiginalUI.Instance.SiginalText("End Game");
    //  }
    // #endregion



}
