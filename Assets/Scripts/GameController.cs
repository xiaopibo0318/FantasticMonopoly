using UnityEngine;
using PlayerManager;
using MapManager;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Events;
using System.Collections.Generic;
using HashTable = ExitGames.Client.Photon.Hashtable;
using ElementManager;

public class GameController : MonoBehaviourPunCallbacks
{
    [Header("All Game Info")]
    public PlayerInfo player1 = null;
    public Map map = null;
    [SerializeField] private GameObject playerPrefab;
    public int nowDiceIndex { get; set; }
    public int totalRound { get; set; }

    public static GameController Instance;

    List<Player> sortedPlayerList = new List<Player>();
    List<PlayerInfo> playerDataList = new List<PlayerInfo>();


    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else Destroy(this);
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
                player1 = new PlayerInfo(i, 10, player.NickName);// ElmentID = 0, tokenNum = 10;
            }
            i++;
        }

        UiController.Instance.InitUI();
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


    public void UpdateCeil()
    {
        nowDiceIndex = PlayerController.LocalPlayerInstance.NowPos();
        //MapGenerator.Instance.UpdateCeil(map, nowDiceIndex, player1);
    }

    private void CreatePlayer()
    {
        if (playerPrefab == null) { Debug.LogError("Can't Create Player, because playerPrefab is empty."); }
        else
        {
            PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0, 50, 0), Quaternion.identity, 0);
        }

    }


}

