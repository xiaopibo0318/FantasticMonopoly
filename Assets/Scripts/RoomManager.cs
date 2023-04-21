using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class RoomManager : MonoBehaviour
{
    [Header("Title inform")]
    [SerializeField] private Button goToRoomButton;

    [Header("Round inform")]
    public readonly int[] roundNum = { 3, 5, 8, 10 };
    int nowRoundIndex;
    [SerializeField] private Text roundText;
    [SerializeField] private Button leftRoundButton;
    [SerializeField] private Button rightRoundButton;


    [SerializeField] private Button goBackButton;
    [SerializeField] private Button startGameButton;


    private void Start()
    {
        InitRoom();
    }

    private void InitRoom()
    {
        nowRoundIndex = 0;
        ButtonInit();
        SiginalRoomText("Round");
    }

    private void ButtonInit()
    {
        leftRoundButton.onClick.AddListener(() => ChangeStatus("Round", -1));
        rightRoundButton.onClick.AddListener(() => ChangeStatus("Round", 1));
        startGameButton.onClick.AddListener(() => StartGame());
        goToRoomButton.onClick.AddListener(() => SwitchRoomManager.Instance.SwitchView("Room"));
        goBackButton.onClick.AddListener(() => SwitchRoomManager.Instance.SwitchView("Title"));
    }

    private void ChangeStatus(string name, int num)
    {
        switch (name)
        {
            case "Round":
                int roomSize = roundNum.Length;
                nowRoundIndex = (nowRoundIndex + num + roomSize) % roomSize;
                SiginalRoomText("Round");
                break;
        }
    }

    private void SiginalRoomText(string name)
    {
        switch (name)
        {
            case "Round":
                roundText.text = roundNum[nowRoundIndex].ToString();
                break;
        }
    }

    private void StartGame()
    {
        SceneHandler.Instance.GoToNextScene("MainGame");
        TimeManager.Instance.Delay(1, () => LoadMainGame());

    }

    private void LoadMainGame()
    {
        MapGenerator.Instance.MapGenerate();
        PlayerController.Instance.LoadGame();
        UiController.Instance.totalRound = roundNum[nowRoundIndex];
    }



}