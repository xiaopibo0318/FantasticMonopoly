using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DiceSystem : Singleton<DiceSystem>
{
    [SerializeField] private GameObject dice;
    [SerializeField] private List<Transform> diceFaceList;
    private static Rigidbody diceRb;
    private int diceNum = 0;

    Coroutine coroutine;

    private void Start()
    {
        diceRb = dice.GetComponent<Rigidbody>();
        //Roll();
    }

    private void ResetDice()
    {
        //wtf where is the dice's init pos???
    }


    public void RollDice()
    {
        if (coroutine != null) { StopCoroutine(coroutine); }
        coroutine = null;
        coroutine = StartCoroutine(Rolling());
    }

    private IEnumerator Rolling()
    {
        float random1 = Random.Range(0, 360);
        float random2 = Random.Range(1, 5);
        float random3 = Random.Range(1, 5);
        dice.transform.rotation = Quaternion.Euler(random1, random1, random1);
        diceRb.velocity = new Vector3(random2, random2, random2);
        diceRb.maxAngularVelocity = 1000;
        Vector3 torque = new Vector3(random3, random3, random3);
        diceRb.AddTorque(torque, ForceMode.VelocityChange);

        while (!IsDiceStopped())
        {
            yield return null;
        }
        diceNum = FindDiceResult();
        AfterDiceDone();
    }


    private bool IsDiceStopped()
    {
        if (diceRb.velocity == Vector3.zero && diceRb.angularVelocity == Vector3.zero)
        {
            return true;
        }
        else return false;
    }


    private int FindDiceResult()
    {
        int maxIndex = 0;
        for (int i = 0; i < diceFaceList.Count; i++)
        {
            if (diceFaceList[i].position.y > diceFaceList[maxIndex].position.y) maxIndex = i;
        }
        return (maxIndex + 1);
    }


    private void AfterDiceDone()
    {
        
        CameraController.Instance.ViewSwitch("Main");
        CameraController.Instance.ViewSwitch("backLook");
        PlayerController.LocalPlayerInstance.PlayerWalk(diceNum);
        
    }


}
