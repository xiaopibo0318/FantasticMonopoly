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
    Coroutine gameCoroutine = null;
    public bool playerFinish { get; set; }
    public List<string> playerNameList = new List<string>();


    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else Destroy(this);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerNameList.Add(player.NickName);
        }
        gameCoroutine = StartCoroutine(GameCycle());
    }

    
    // public void AddNameList(string name)
    // {
    //     playerNameList.Add(name);
    // }


    private IEnumerator GameCycle()
    {
        int x = 3;
        Debug.Log("totalRound = " + GameController.Instance.totalRound);
        playerFinish = false;
        for (int i = 0; i < playerNameList.Count; i++) 
        {
            Debug.Log(playerNameList[i]); 
        }
        while (x > 0)
        {
            for (int i = 0; i < playerNameList.Count; i++) // i is player in active
            {
                Debug.Log($"Now Player Name is:{playerNameList[i]}");
                if (PhotonNetwork.LocalPlayer.NickName == playerNameList[i])
                {
                    UiController.Instance.DiceButtonInteractable(true);
                }
                else { UiController.Instance.DiceButtonInteractable(false); }

                if (!playerFinish)
                {
                    Debug.Log("Player" + playerNameList[i] + " didn't finished");
                    yield return new WaitUntil(() => playerFinish);
                    Debug.Log("Player" + playerNameList[i] + "finished");
                }
                playerFinish = false;
            }
            x--;
        }
        Debug.Log("End Game");
        SiginalUI.Instance.SiginalText("End Game");
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, HashTable changedProps)
    {
        Debug.Log("In player finish detect");
        foreach (DictionaryEntry entry in changedProps)
        {
            Debug.Log( "targetPlayer : " + targetPlayer.NickName + " key :  " + entry.Key + " value :  "+ entry.Value);
        }

        if(!(changedProps.ContainsKey("playerFinish")))
            return;
        if(targetPlayer.NickName != PhotonNetwork.LocalPlayer.NickName){
            playerFinish = (bool)changedProps["playerFinish"];
            changedProps["playerFinish"] = false;
        }
        
    }

}
