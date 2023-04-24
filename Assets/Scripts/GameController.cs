using UnityEngine;
using PlayerManager;
using MapManager;
using Photon.Pun;

public class GameController : MonoBehaviourPunCallbacks
{
    [Header("All Game Info")]
    private Player player1 = null;
    private Map map = null;
    [SerializeField] private GameObject playerPrefab;
    public int nowDiceIndex { get; set; }
    public int totalRound;

    public static GameController Instance;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else Destroy(this);
    }

    private void Start()
    {
        
    }


    /// <summary>
    /// Game process devide into n steps, 
    /// </summary>
    public enum GameState
    {
        Menu, Item, Dice, Move, End
    }


    public void CreateNewGame()
    {
        CreatePlayer();
        player1 = new Player(2, 10); // ElmentID = 0, tokenNum = 10;
        map = new Map(16, 0.25f); // Small Map
        MapGenerator.Instance.MapGenerate(map);
    }

    public void UpdateCeil()
    {
        nowDiceIndex = PlayerController.LocalPlayerInstance.NowPos();
        MapGenerator.Instance.UpdateCeil(map, nowDiceIndex, player1);
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
