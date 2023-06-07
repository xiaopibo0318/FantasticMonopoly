using Photon.Pun;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using MapManager;
using PlayerManager;
using System.Linq.Expressions;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;

public class UiController : Singleton<UiController>
{
    [SerializeField] private Button overLookButton;
    [SerializeField] private Button frontLookButton;
    [SerializeField] private Button backLookButton;

    [SerializeField] private Button diceButton;

    [SerializeField] private Text roundText;
    public int totalRound { get; set; }

    [SerializeField] private GameObject playerInfoUI;
    [SerializeField] private Transform playerInfoParent;
    private GameObject viewObject;

    private List<GameObject> playerInfoList = new List<GameObject>();

    private void Start()
    {
        InitButton();
        StartCoroutine(CountDown(.5f, InitPlayerInfo));
    }


    public void UpdateInfo(int roundLeft, int total)
    {
        roundText.text = "回合數：" + (total - roundLeft) + " / " + total;
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
            diceButton.interactable = false;
            PlayerController.LocalPlayerInstance.SignalTextToEveryPlayer("玩家擲骰子中");
        });
        diceButton.interactable = false;
    }


    public void DiceButtonInteractable(bool b)
    {
        diceButton.interactable = b;
    }


    public void InitPlayerInfo()
    {
        List<Player> sortedPlayerList = PhotonNetwork.CurrentRoom.Players.Values.OrderBy(player => player.ActorNumber).ToList();
        foreach (Player player in sortedPlayerList)
        {
            var myObject = Instantiate(playerInfoUI, playerInfoParent);
            //var myObject = PhotonNetwork.Instantiate(playerInfoUI.name, Vector3.zero, Quaternion.identity, 0);
            myObject.GetComponent<PlayerViewManager>().playerName.text = player.NickName;

            if (player.NickName == PhotonNetwork.LocalPlayer.NickName)
            {
                for (int i = 0; i < myObject.GetComponent<PlayerViewManager>().elementNum.Length; i++)
                {
                    myObject.GetComponent<PlayerViewManager>().elementNum[i].text = PlayerController.LocalPlayerInstance.GetPlayerElementData(i);
                }
            }
            viewObject = myObject;
            playerInfoList.Add(myObject);
        }
    }



    private IEnumerator CountDown(float time, UnityAction action = null)
    {
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        action?.Invoke();
    }

    public void UpdatePlayerViewData(int elementIndex, int num, int targetPlayerID)
    {
        var x = playerInfoList[targetPlayerID].GetComponent<PlayerViewManager>();
        x.elementNum[elementIndex].text = num.ToString();
    }



}
