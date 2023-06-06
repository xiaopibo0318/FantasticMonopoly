using MapManager;
using System.Collections;
using ElementManager;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using HashTable = ExitGames.Client.Photon.Hashtable;
using CellManager;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject player;
    [SerializeField] private TextMeshPro playerName;

    private int nowPosIndex;
    MapSize mapSize = new MapSize();
    PhotonView pv;
    private static Element element;
    private static int id;
    private HashTable tokens = new Hashtable();
    public bool isFoul = false;
    public static PlayerController LocalPlayerInstance; //Local Instance
    private Map map;

    public Animator playerAnimator;


    private void Awake()
    {
        pv = this.gameObject.GetComponent<PhotonView>();
        if (pv.IsMine) // use Photon to distinguish the player you control
        {
            LocalPlayerInstance = this;
        }
    }

    private void Start()
    {
        InitPlayer();
    }

    public void LoadGame()
    {
        player.transform.position = new Vector3(-10, -20, 0);
        nowPosIndex = 0;
    }

    public void PlayerWalk(int amount) => StartCoroutine(GoWalk(amount));

    private IEnumerator GoWalk(int amount)
    {
        SignalUI.Instance.SignalText($"{pv.Owner.NickName} {amount.ToString()}");
        CameraController.Instance.ViewSwitch("overLook");
        Debug.Log($"NowIndex is {nowPosIndex}");
        var posOffset = new Vector3(-10, -20, 0);

        for (int i = 0; i < amount; i++)
        {
            playerAnimator.SetBool("walk", true);
            yield return new WaitForSeconds(1.05f);
            playerAnimator.SetBool("walk", false);

            player.transform.Rotate(0, -4.2f, 0);
            //player.transform.Translate(1,0,0);
            int cPos = (nowPosIndex + 1 + i) % 16;

            if (cPos == 3 || cPos == 8 || cPos == 11 || cPos == 0)
            {
                player.transform.position = (mapSize.mapDictS[(nowPosIndex + 1 + i) % 16] * 150) + posOffset;
                player.transform.Rotate(0, 90, 0);
            }

        }

        nowPosIndex += amount;
        Debug.Log($"AfterGoIndex is {nowPosIndex}");
        CameraController.Instance.ViewSwitch("backLook");
        //GameController.Instance.UpdateCeil();
        Debug.Log($"Idx is {id}");
        Debug.Log($"tokens is{tokens.Count}");
        if (map == null) { Debug.Log("mapNull"); }
        else Debug.Log(map.cells.Length);
        new ElementManager.ElementManager().Reaction(id, tokens, map.cells[nowPosIndex]);
        string playerElement = $"element {id}";
        if ((int)tokens[$"element {id}"] <= 0)
        {
            isFoul = true;
            tokens.Remove($"element {id}");
            tokens.Add($"element {id}", 0);
        }
        GameCycleControler.Instance.playerFinish = true;
        HashTable table = new HashTable();
        table.Add("playerElement", GameController.Instance.player1.element.id);
        table.Add("playerPos", nowPosIndex);
        table.Add("tokens", tokens);
        table.Add("isFoul", isFoul);
        table.Add("playerFinish", GameCycleControler.Instance.playerFinish);
        PhotonNetwork.LocalPlayer.SetCustomProperties(table);
        MapGenerator.Instance.UpdateCeil(map, nowPosIndex, id);
        GameController.Instance.UpdateMapDataToGameController(map);
        //SerializeMapData(map,map.cells);
        GameController.Instance.ChangeMapData(map,map.cells);
    }

    public int NowPos() { return nowPosIndex % 16; }

    private void UpdateName()
    {
        player.gameObject.name = "Player" + pv.Owner.NickName;
        playerName.text = pv.Owner.NickName;
    }

    private void InitPlayer()
    {
        UpdateName();
        element = GameController.Instance.player1.element;
        map = GameController.Instance.map;
        id = element.id;
        int amount = 10;//init amount of the selected element
        for (int i = 0; i <= 4; i++)
        {
            if (i == id)
            {
                tokens.Add($"element {i}", amount);
                continue;
            }
            tokens.Add($"element {i}", 0);
        }
    }

    public void UpdateMapDataToPlayer(Map _map)
    {
        map = _map;
    }


    private void SerializeMapData(Map _map, Cell _cells)
    {
        string jsonData = JsonUtility.ToJson(_map);
        byte[] byteData = System.Text.Encoding.UTF8.GetBytes(jsonData);
    }


}