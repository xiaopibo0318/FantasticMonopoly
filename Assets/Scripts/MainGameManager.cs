using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InitGame();
    }

    // Update is called once per frame
    private void InitGame()
    {
        GameController.Instance.CreateNewGame();
       
        PlayerController.LocalPlayerInstance.LoadGame();
    }
}
