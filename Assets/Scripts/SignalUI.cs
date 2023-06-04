using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SignalUI : MonoBehaviour
{
    public GameObject signalContent;
    public Text signalText;
    public Button confirm;
    public Button dontDo;
    private UnityAction nextAction;
    private Coroutine coroutine;

    [SerializeField] private Button skipButton;

    public static SignalUI Instance;
    public void Awake()
    {
        Instance = this;
        skipButton.onClick.AddListener(SkipStory);
        ResetSignal();
    }

    public void SignalText(string myText, int signalTime = 5, int textFontSize = 20, bool canSkip = true)
    {
        if (canSkip == true) skipButton.gameObject.SetActive(true);
        else skipButton.gameObject.SetActive(false);

        if (coroutine != null) StopCoroutine(coroutine);
        Instance.signalContent.SetActive(true);
        signalText.text = myText;
        signalText.fontSize = textFontSize;
        //panel.SetActive(true);
        coroutine = StartCoroutine(CloseSignal(signalTime));
    }

    //public void TextInterectvie(string myText, UnityAction unityAction1 = null, UnityAction unityAction2 = null)
    //{
    //    signalContent.SetActive(true);
    //    signalText.text = myText;
    //    confirm.gameObject.SetActive(true);
    //    dontDo.gameObject.SetActive(true);
    //    nextAction = unityAction1;
    //    confirm.onClick.AddListener(delegate { ExecuteOptionYes(nextAction); });
    //    UnityAction temp = unityAction2;
    //    dontDo.onClick.AddListener(delegate { ExecuteOptionNo(temp); });
    //}


    //private void ExecuteOptionYes(UnityAction unityAction = null)
    //{
    //    ResetSignal();
    //    unityAction?.Invoke();
    //    nextAction = null;
    //}
    //private void ExecuteOptionNo(UnityAction unityAction = null)
    //{
    //    ResetSignal();
    //    unityAction?.Invoke();
    //    nextAction = null;
    //}

    IEnumerator CloseSignal(float delayTime)
    {
        while (delayTime > 0)
        {
            yield return new WaitForSeconds(1);
            delayTime -= 1;
        }
        ResetSignal();
    }

    public void ResetSignal()
    {
        if (coroutine != null) StopCoroutine(coroutine);

        coroutine = null;
        Instance.signalContent.SetActive(false);
        signalText.text = "";
        //confirm.gameObject.SetActive(false);
        //dontDo.gameObject.SetActive(false);
    }


    private void SkipStory()
    {
        StopCoroutine(coroutine);
        coroutine = null;
        SignalUI.Instance.ResetSignal();
        skipButton.gameObject.SetActive(false);
    }

}
