using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Text;



public class RoomManager : MonoBehaviourPunCallbacks
{
    [Header("Title")]
    [SerializeField] Button goToLobbyButton;

    [Header("Lobby")]
    [SerializeField] Button createRoomButton;
    [SerializeField] Button joinRoomButton;
    [SerializeField] TextMeshProUGUI inputRoomName;
    [SerializeField] TextMeshProUGUI inputPlayerName;
    [SerializeField] TextMeshProUGUI textRoomList;

    [Header("Room")]
    [SerializeField] Button goBackButton;
    [SerializeField] Text roundText;
    [SerializeField] Button leftRoundButton;
    [SerializeField] Button rightRoundButton;
    [SerializeField] Button startGameButton;

    public readonly int[] roundNum = { 3, 5, 8, 10 };
    int nowRoundIndex;


    private void Start()
    {
        InitLogin();
    }

    private void InitLogin()
    {
        nowRoundIndex = 0;
        ButtonInit();
        SiginalRoomText("Round");
    }

    private void ButtonInit()
    {
        // login
        goToLobbyButton.onClick.AddListener(JoinLobby);
        // lobby
        createRoomButton.onClick.AddListener(CreateRoom);
        joinRoomButton.onClick.AddListener(JoinRoom);
        // room
        goBackButton.onClick.AddListener(() => SwitchRoomManager.Instance.SwitchView("Title"));
        leftRoundButton.onClick.AddListener(() => ChangeStatus("Round", -1));
        rightRoundButton.onClick.AddListener(() => ChangeStatus("Round", 1));
        startGameButton.onClick.AddListener(() => StartGame());
    }

    private void JoinLobby()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        SwitchRoomManager.Instance.SwitchView("Lobby");
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


    public override void OnConnectedToMaster()
    {
        print("Connected to master");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        print("Lobby joined.");
    }
    public string GetRoomName()
    {
        string roomName = inputRoomName.text;
        print("roomLength: " + roomName.Length);
        return roomName.Trim();
    }
    public string GetPlayerName()
    {
        string playerName = inputPlayerName.text;
        return playerName.Trim();
    }

    public void CreateRoom()
    {
        string roomName = GetRoomName();
        string playerName = GetPlayerName();
        if (roomName.Length > 1 && playerName.Length > 1)
        {
            PhotonNetwork.CreateRoom(roomName);
            PhotonNetwork.LocalPlayer.NickName = playerName;
        }
        else
            print("Invalid RoomName or PlayerName!");
    }

    public void JoinRoom()
    {
        string roomName = GetRoomName();
        string playerName = GetPlayerName();
        if (roomName.Length > 1 && playerName.Length > 1)
        {
            PhotonNetwork.JoinRoom(roomName);
            PhotonNetwork.LocalPlayer.NickName = playerName;
        }
        else
            print("Invalid RoomName or PlayerName!");

    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            SwitchRoomManager.Instance.SwitchView("Room");
        }
        else
        {
            Debug.LogWarning("CurrentRoom is null.");
        }
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        StringBuilder sb = new StringBuilder();
        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.PlayerCount > 0)
            {
                sb.AppendLine("->" + roomInfo.Name);
            }
        }
        textRoomList.text = sb.ToString();
    }

}