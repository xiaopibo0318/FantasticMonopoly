using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneHandler : Singleton<SceneHandler>
{
    public void GoToNextScene(string name)
    {
        SceneManager.LoadScene(name);
    }



}
