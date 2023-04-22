
using UnityEngine;
using PlayerManager;
using MapManager;

public class GameController : Singleton<GameController>
{
    [Header("All Game Info")]
    Player player1 = null;
    Map map = null;


    /// <summary>
    /// Game process devide into n steps, 
    /// </summary>
    public enum GameState
    {
        Menu, Item, Dice, Move, End
    }


    public void CreateNewGame()
    {
        player1 = new Player(0, 10); // ElmentID = 0, tokenNum = 10;
        map = new Map(16, 0.25f); // Small Map
        MapGenerator.Instance.MapGenerate(map);
    }


}
