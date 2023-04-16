using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCenter : Singleton<GameCenter>
{
    private void Start()
    {
        Instance.TimeManager.Instance.Delay(1, () => SceneHandler.Instance.GoToNextScene("LobbyAndRoom"));
    }

}
