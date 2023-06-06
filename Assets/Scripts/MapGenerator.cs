using System.Collections;
using UnityEngine;
using MapManager;
using Photon.Pun;
using Photon.Realtime;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class MapGenerator : MonoBehaviourPunCallbacks
{
    [Header("Ground Object")]
    [SerializeField] GameObject[] groundList;
    [SerializeField] Transform groundParent;
    [SerializeField] GameObject normalCell;
    [SerializeField] GameObject specitalCell;

    //[Header("Map Size")]
    //Dictionary<string, int> mapSize = new Dictionary<string, int>()
    //{{"S",15},{"M",25 },{"L",35 }};
    int offset = 150;

    public static MapGenerator Instance;
    private void Awake()
    {
        Instance = this;

    }


    public void MapGenerate(Map map = null)
    {
        if (map == null)
        {
            Debug.Log($"Map Generate Fail, cause map is null");
            return;
        }

        var mapSize = new MapSize();
        for (int i = 0; i < mapSize.mapDictS.Length; i++)
        {
            if (map.cells[i].isSpecial)
            {
                Vector3 pos = (mapSize.mapDictS[i] * offset) + specitalCell.transform.position;
                var myObject = PhotonNetwork.Instantiate(specitalCell.name, pos, Quaternion.identity, 0);
                myObject.transform.SetParent(groundParent);
            }
            else
            {
                Vector3 pos = (mapSize.mapDictS[i] * offset) + normalCell.transform.position;
                var myObject = PhotonNetwork.Instantiate(normalCell.name, pos, Quaternion.identity, 0);
                myObject.transform.SetParent(groundParent);
            }
        }
        PlayerController.LocalPlayerInstance.UpdateMapDataToPlayer(map);
        Debug.Log($"Build Succeed");
    }


    public void UpdateCeil(Map map, int targetIndex, int elementIdInt)
    {
        Debug.Log($"is special? :{map.cells[targetIndex].isSpecial}, index is{targetIndex}, playElementId is{elementIdInt}");
        var mapSize = new MapSize();
        if (map.cells[targetIndex].isSpecial == false)
        {
            Vector3 pos = (mapSize.mapDictS[targetIndex] * offset) + groundList[elementIdInt].transform.position;
            var myObject = PhotonNetwork.Instantiate(groundList[elementIdInt].name, pos, Quaternion.identity, 0);
            myObject.transform.SetParent(groundParent);
        }
        GameController.Instance.UpdateMapDataToGameController(map);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, HashTable changedProps)
    {
        Debug.Log("detect map Update => targetPlayer : " + targetPlayer.NickName);
        foreach (DictionaryEntry entry in changedProps)
            Debug.Log($"Key: {entry.Key} | Value: {entry.Value}");
        if(!(changedProps.ContainsKey("playerElement") && changedProps.ContainsKey("playerFinish") && changedProps.ContainsKey("playerPos")))  return;

        int targetIndex = (int)changedProps["playerPos"];

        if(PhotonNetwork.IsMasterClient)
        {
            Map map = GameController.Instance.map;
            var mapSize = new MapSize();
            
            if (map.cells[targetIndex].isSpecial == false && map.cells[targetIndex].IsTokenEmpty()){
                int nowElementID = (int)changedProps["playerElement"];
                Vector3 pos = (mapSize.mapDictS[targetIndex] * offset) + groundList[nowElementID].transform.position;
                var myObject = PhotonNetwork.Instantiate(groundList[nowElementID].name, pos, Quaternion.identity, 0);
                myObject.transform.SetParent(groundParent);
                
            }
        }
        
    }


}

/* Generate 5 elment map
 * 
 * int index = Random.Range(0, groundList.Length);
   Vector3 pos = (mapSize.mapDictS[i] * offset) + groundList[index].transform.position;
   Instantiate(groundList[index], pos, Quaternion.identity, groundParent);
 * 
*/