using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchRoomManager : Singleton<SwitchRoomManager>
{
    [SerializeField] private GameObject[] gameObjectsList;

    private void Start()
    {
        SwitchView("Title");
    }


    public void SwitchView(string name)
    {
        for (int i = 0; i < gameObjectsList.Length; i++)
        {
            gameObjectsList[i].SetActive(false);
        }
        for (int i = 0; i < gameObjectsList.Length; i++)
        {
            if (gameObjectsList[i].name == name)
            {
                gameObjectsList[i].SetActive(true);
            }
        }

    }
}
