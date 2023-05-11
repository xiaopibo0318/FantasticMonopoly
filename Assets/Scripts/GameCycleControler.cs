using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using TMPro;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class GameCycleControler : MonoBehaviourPunCallbacks
{
    public static GameCycleControler Instance;

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

    private IEnumerator GameCycle(){
        Debug.Log("Start Cycle");
        // Get the number of rounds left from GameController
        int totalRound = GameController.Instance.totalRound;
        int roundLeft = totalRound;
        // Set playerFinish to false initially
        playerFinish = false;

        // Loop while there are rounds left
        while (roundLeft > 0)
        {
            UiController.Instance.UpdateInfo(roundLeft, totalRound);
            // Loop through each player name in playerNameList
            foreach (string playerName in playerNameList)
            {
                // If the local player's name matches the current player name, make the dice button interactable
                // Player can roll dice here
                UiController.Instance.DiceButtonInteractable(PhotonNetwork.LocalPlayer.NickName == playerName);
                // Wait until playerFinish is true before continuing
                if (!playerFinish)
                    yield return new WaitUntil(() => playerFinish);

                // Set playerFinish to false for next iteration
                playerFinish = false;
            }
            roundLeft--;
        }

        // Log end of game and make dice button non-interactable
        UiController.Instance.UpdateInfo(roundLeft, totalRound);
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

        if(targetPlayer.NickName != PhotonNetwork.LocalPlayer.NickName){
            playerFinish = (bool)changedProps["playerFinish"];
            changedProps["playerFinish"] = false;
        }
    }
}
