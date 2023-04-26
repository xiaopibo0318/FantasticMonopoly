using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerManager;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using ElementManager;

public class UiController : Singleton<UiController>
{
    [SerializeField] private Button overLookButton;
    [SerializeField] private Button frontLookButton;
    [SerializeField] private Button backLookButton;

    [SerializeField] private Button diceButton;

    [SerializeField] private Text roundText;
    public int totalRound { get; set; }

    [SerializeField] private GameObject playerInfoPrefab;
    [SerializeField] private Transform playerInfoParent;

    private Player player = null;

    private void Start()
    {
        InitButton();
    }


    public void UpdateInfo(int x = 0)
    {
        roundText.text = "¦^¦X¼Æ¡G" + x + " / " + totalRound.ToString();
    }


    private void InitButton()
    {
        overLookButton.onClick.AddListener(() => { CameraController.Instance.ViewSwitch("overLook"); });
        frontLookButton.onClick.AddListener(() => { CameraController.Instance.ViewSwitch("frontLook"); });
        backLookButton.onClick.AddListener(() => { CameraController.Instance.ViewSwitch("backLook"); });
        diceButton.onClick.AddListener(() =>
        {
            CameraController.Instance.ViewSwitch("Dice");
            DiceSystem.Instance.RollDice();
        });
        diceButton.interactable = false;
    }


    public void DiceButtonInteractable(bool b)
    {
        diceButton.interactable = b;
    }

    /// <summary>
    /// From GameController to catch the playerDataList, and get the player, and set the value to UI.
    /// </summary>
    /// <param name="playerList"></param>
    public void InitUI(List<Player> playerList)
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            var playerInfo = Instantiate(playerInfoPrefab, playerInfoParent);
            if (playerList[i].NickName == PhotonNetwork.LocalPlayer.NickName)
            {
                player = playerList[i];
            }
            var playerView = playerInfo.GetComponent<PlayerViewData>();
            playerView.InfoInit(player.NickName);
        }
    }

}
