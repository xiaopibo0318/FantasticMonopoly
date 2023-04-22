using UnityEngine;
using PlayerManager;
using MapManager;

public class GameController : Singleton<GameController>
{
    [Header("All Game Info")]
    private Player player1 = null;
    private Map map = null;
    public int nowDiceIndex { get; set; }
    public int totalRound;


    /// <summary>
    /// Game process devide into n steps, 
    /// </summary>
    public enum GameState
    {
        Menu, Item, Dice, Move, End
    }


    public void CreateNewGame()
    {
        player1 = new Player(2, 10); // ElmentID = 0, tokenNum = 10;
        map = new Map(16, 0.25f); // Small Map
        MapGenerator.Instance.MapGenerate(map);
    }

    public void UpdateCeil()
    {
        nowDiceIndex = PlayerController.Instance.NowPos();
        MapGenerator.Instance.UpdateCeil(map, nowDiceIndex, player1);
    }
}
