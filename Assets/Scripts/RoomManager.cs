using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using HashTable = ExitGames.Client.Photon.Hashtable;
using TMPro;



public class RoomManager : MonoBehaviourPunCallbacks
{
    [Header("Title")]
    [SerializeField] Button buttonJoinLobby;

    [Header("Lobby")]
    [SerializeField] Button buttonCreateRoom;
    [SerializeField] Button buttonJoinRoom;
    [SerializeField] TextMeshProUGUI inputRoomName;
    [SerializeField] TextMeshProUGUI inputPlayerName;
    [SerializeField] TextMeshProUGUI textRoomList;

    [Header("Room")]
    [SerializeField] TextMeshProUGUI textRoomName;
    [SerializeField] TextMeshProUGUI textPlayerList;
    [SerializeField] Button leaveRoomButton;
    [SerializeField] TextMeshProUGUI textRound;
    [SerializeField] Button buttonLeftRound;
    [SerializeField] Button buttonRightRound;
    [SerializeField] Button buttonStartGame;

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
        buttonJoinLobby.onClick.AddListener(JoinLobby);
        // lobby
        buttonCreateRoom.onClick.AddListener(CreateRoom);
        buttonJoinRoom.onClick.AddListener(_JoinRoom);
        // room
        leaveRoomButton.onClick.AddListener(leaveRoom);
        buttonLeftRound.onClick.AddListener(() => ChangeStatus("Round", -1));
        buttonRightRound.onClick.AddListener(() => ChangeStatus("Round", 1));
        buttonStartGame.onClick.AddListener(StartGame);
    }

    private void JoinLobby()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        SwitchRoomManager.Instance.SwitchView("Lobby");
        LoadingManager.Instance.LobbyLoading();
    }


    private void ChangeStatus(string name, int num)
    {
        switch (name)
        {
            case "Round":
                int roomSize = roundNum.Length;
                nowRoundIndex = (nowRoundIndex + num + roomSize) % roomSize;
                textRound.text = roundNum[nowRoundIndex].ToString();
                //SiginalRoomText("Round");
                break;
        }
    }

    private void SiginalRoomText(string Name)
    {
        switch (Name)
        {
            case "Round":
                textRound.text = roundNum[nowRoundIndex].ToString();
                break;
        }
    }

    private void StartGame()
    {
        HashTable table = new HashTable();
        table.Add("totalRound", roundNum[nowRoundIndex]);
        PhotonNetwork.LocalPlayer.SetCustomProperties(table);
        GameController.Instance.totalRound = roundNum[nowRoundIndex];
        SceneHandler.Instance.GoToNextScene("MainGame");
        TimeManager.Instance.Delay(1, () => LoadMainGame());


    }

    private void LoadMainGame()
    {
        GameController.Instance.InitGame();
        UiController.Instance.totalRound = roundNum[nowRoundIndex];
        UiController.Instance.UpdateInfo(roundNum[nowRoundIndex], roundNum[nowRoundIndex]);
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, HashTable changedProps)
    {
        if (GameController.Instance.totalRound == (int)changedProps["totalRound"])
            return;
        GameController.Instance.totalRound = (int)changedProps["totalRound"];
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
            Debug.LogWarning("Invalid RoomName or PlayerName!");
    }

    private bool IsValidName(string PlayerName)
    {
        Regex AllowedChars = new Regex(@"[A-Za-z0-9_-]+");
        return AllowedChars.IsMatch(PlayerName);
    }

    public void _JoinRoom()
    {
        string roomName = GetRoomName();
        string playerName = GetPlayerName();
        if (roomName.Length < 1)
        {
            Debug.LogWarning("Invalid RoomName");
            return;
        }

        if ((playerName.Length < 3 || playerName.Length > 15) && !IsValidName(playerName))
        {
            Debug.LogWarning("Invalid PlayerName");
            return;
        }
        PhotonNetwork.JoinRoom(roomName);
        PhotonNetwork.LocalPlayer.NickName = playerName;
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
            Debug.LogWarning("No room selected!");
            return;
        }
        SwitchRoomManager.Instance.SwitchView("Room");
        if (PhotonNetwork.CurrentRoom == null)
        {
            Debug.LogWarning($"Unable to join room with name: {PhotonNetwork.CurrentRoom.Name}");
            SwitchRoomManager.Instance.SwitchView("Lobby");
            return;
        }
        textRoomName.text = PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();
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
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        buttonStartGame.interactable = PhotonNetwork.IsMasterClient;
    }

    public void UpdatePlayerList()
    {
        StringBuilder sb = new StringBuilder();
        List<Player> sortedPlayerList = PhotonNetwork.CurrentRoom.Players.Values.OrderBy(player => player.ActorNumber).ToList();
        foreach (Player player in sortedPlayerList)
        {
            sb.AppendLine("->" + player.NickName);
        }
        textPlayerList.text = sb.ToString();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    public void leaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        SwitchRoomManager.Instance.SwitchView("Lobby");
    }

    public override void OnLeftRoom()
    {
        SwitchRoomManager.Instance.SwitchView("Lobby");
    }
}