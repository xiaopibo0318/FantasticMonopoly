using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 繼承原本 Monobehavior且在Awake中使用Singleton
/// </summary>
public class LoadingManager : Singleton<LoadingManager>
{
    [SerializeField] private GameObject loadingImage;
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    private float fadeDuration = 3;
    //private bool isFade = true;
    [SerializeField] private RectTransform idle;

    private Vector2 startPos = new Vector2(80, 20);
    private Vector2 endPos = new Vector2(600, 20);

    private IEnumerator Fade(float targetAlpha)
    {
        //isFade = true;
        fadeCanvasGroup.blocksRaycasts = true;
        idle.position = startPos;
        float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / fadeDuration;

        float posChange = 1;
        while (!Mathf.Approximately(posChange, targetAlpha)) //If you want to get the "Fade" effect, just need to change "posChange" to "fadCanvasGroup.alpha"
        {
            //fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
            posChange = Mathf.MoveTowards(posChange, targetAlpha, speed * Time.deltaTime);
            idle.position = (endPos - startPos) * (1 - posChange);
            yield return null;
        }
        fadeCanvasGroup.blocksRaycasts = false;
        //isFade = false;
        loadingImage.SetActive(false);
    }


    public void LobbyLoading() => StartCoroutine(WaitLobbyLoading());
    private IEnumerator WaitLobbyLoading()
    {
        loadingImage.SetActive(true);
        yield return Fade(0);
    }



    public void HandleBlackProblem()
    {
        fadeCanvasGroup.blocksRaycasts = false;
        fadeCanvasGroup.alpha = 0;
    }

}
