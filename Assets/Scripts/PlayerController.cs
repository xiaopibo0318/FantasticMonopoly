using MapManager;
using System.Collections;
using System.Text;
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
        Debug.Log($"MapLength:{map.size}");
        map = GameController.Instance.map;
        Debug.Log($"MapLength:{map.size}");
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
        //SerializeMapData(map,map.cells);
        ChangeMapData(map, map.cells);
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




    [PunRPC]
    public void UpdateMapData(byte[] mapDataBytes, byte[] cellsDataBytes, string originalHash)
    {
        string receivedHash = CalculateHash(mapDataBytes);
        if (receivedHash == originalHash)
        {
            string jsonData = System.Text.Encoding.UTF8.GetString(mapDataBytes);
            Map receivedMapData = JsonUtility.FromJson<Map>(jsonData);
            map = receivedMapData;
            Debug.Log($"Data.Lengthis:{jsonData.Length}");
            Debug.Log($"Data:{jsonData}");
            GameController.Instance.UpdateMapDataToGameController(map);
        }
        else
        {
            Debug.Log("Serialize and Deserialize Fail");
            Debug.Log($"original file length is :{originalHash.Length},recieve file is :{receivedHash.Length}");
        }
        //string cellsData = System.Text.Encoding.UTF8.GetString(cellsDataBytes);
        //map.cells = JsonUtility.FromJson<Cell[]>(cellsData);
        Debug.Log($"map:{map.size},ceil:{map.cells.Length}");
    }

    public void ChangeMapData(Map newMapData, Cell[] cellData)
    {
        string jsonData = JsonUtility.ToJson(newMapData);
        byte[] byteData = System.Text.Encoding.UTF8.GetBytes(jsonData);

        string cellsData = JsonUtility.ToJson(cellData);
        byte[] cellsByteData = System.Text.Encoding.UTF8.GetBytes(cellsData);
        string originalHash = CalculateHash(byteData);
        photonView.RPC("UpdateMapData", RpcTarget.Others, byteData, cellsByteData, originalHash);
    }

    string CalculateHash(byte[] data)
    {
        using (var md5 = System.Security.Cryptography.MD5.Create())
        {
            byte[] hashBytes = md5.ComputeHash(data);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2")); // Convert each byte to two digits of hexadecimal
            }
            return sb.ToString();
        }
    }


    public void SignalTextToEveryPlayer(string text)
    {
        photonView.RPC("SynchronizeText", RpcTarget.Others, text);
    }

    [PunRPC]
    private void SynchronizeText(string text)
    {
        SignalUI.Instance.SignalText(text);
    }


    private void SerializeMapData(Map _map, Cell _cells)
    {
        string jsonData = JsonUtility.ToJson(_map);
        byte[] byteData = System.Text.Encoding.UTF8.GetBytes(jsonData);
    }


}