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

                HashTable table = new HashTable();
                table.Add("PlayerName", player1.playerName);
                //table.Add("PlayerTokens", player1.tokens);
                PhotonNetwork.LocalPlayer.SetCustomProperties(table);
            }
            i++;
        }

        UiController.Instance.InitUI(sortedPlayerList);
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


    List<string> playerNames = new List<string>();
    List<Dictionary<Element, int>> playerTokens = new List<Dictionary<Element, int>>();

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, HashTable changedProps)
    {
        string _name = (string)changedProps["PlayerName"];
        //Dictionary<Element, int> _tokenDict = (Dictionary<Element, int>)changedProps["PlayerToken"];

    }

}

