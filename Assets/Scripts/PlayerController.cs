using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private GameObject player;

    public void LoadGame()
    {
        player.transform.position = new Vector3(150, 50, 0);
    }
}
