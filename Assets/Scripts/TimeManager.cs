using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeManager : Singleton<TimeManager>
{
    Coroutine nowCoroutine = null;

    public void Delay(float time, UnityAction action = null)
    {
        if (nowCoroutine != null) StopCoroutine(nowCoroutine);
        nowCoroutine = null;
        nowCoroutine = StartCoroutine(CountDown(time, action));
    }

    private IEnumerator CountDown(float time, UnityAction action = null)
    {
        while (time > 0)
        {
            time-= Time.deltaTime;
            yield return null;
        }
        action?.Invoke();
    }

}
