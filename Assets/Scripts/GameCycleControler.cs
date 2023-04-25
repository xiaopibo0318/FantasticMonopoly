using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class GameCycleControler : MonoBehaviourPunCallbacks
{
    public static GameCycleControler Instance;

    // Coroutine for the game cycle
    // Property to check if player has finished their turn
    // List of player names
    Coroutine gameCoroutine = null;
    public bool playerFinish { get; set; }
    List<string> playerNameList = new List<string>();

    // Awake method to set the singleton instance
    private void Awake()
    {
        Instance = this;
    }
    
    // Start method called before the first frame update
    void Start()
    {
        // Get list of player names from PhotonNetwork
        playerNameList = PhotonNetwork.PlayerList.Select(player => player.NickName).ToList();
        // Start the game cycle coroutine
        gameCoroutine = StartCoroutine(GameCycle());
    }

    private IEnumerator GameCycle()
    {
        // Get the number of rounds left from GameController
        int RoundLeft = GameController.Instance.totalRound;
        // Set playerFinish to false initially
        playerFinish = false;

        // Loop while there are rounds left
        while (RoundLeft > 0)
        {
            // Loop through each player name in playerNameList
            foreach (string playerName in playerNameList)
            {
                // If the local player's name matches the current player name, make the dice button interactable
                // Player can roll dice here
                if (PhotonNetwork.LocalPlayer.NickName == playerName)
                    UiController.Instance.DiceButtonInteractable(true);
                else 
                    UiController.Instance.DiceButtonInteractable(false);
                // Wait until playerFinish is true before continuing
                if (!playerFinish)
                    yield return new WaitUntil(() => playerFinish);

                // Set playerFinish to false for next iteration
                playerFinish = false;
            }
            RoundLeft--;
        }

        // Log end of game and make dice button non-interactable
        Debug.Log("End Game");
        UiController.Instance.DiceButtonInteractable(false);
    }

    // Override OnPlayerPropertiesUpdate method from MonoBehaviourPunCallbacks
    // 
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, HashTable changedProps)
    {
        // Detect Validity
        if(!(changedProps.ContainsKey("playerElement") && changedProps.ContainsKey("playerFinish") && changedProps.ContainsKey("playerPos")))
            return;

        // Set playerFinish to value of "playerFinish" key in changedProps and set "playerFinish" key to false in changedProps
        playerFinish = (bool)changedProps["playerFinish"];
        changedProps["playerFinish"] = false;
    }
}
