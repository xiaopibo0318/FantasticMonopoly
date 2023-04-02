using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DiceSystem : MonoBehaviour{
    [SerializeField] private GameObject dice;
    [SerializeField] private List<Transform> diceFaceList;
    private static Rigidbody diceRb;
    private int diceNum = 0;

    private void Start(){
        diceRb = dice.GetComponent<Rigidbody>();
        Dice_Init();
    }

    private void FixedUpdate(){
        if (IsDiceStopped()){
            diceNum = FindDiceResult();
        }
    }

    private void Dice_Init(){
        float random1 = Random.Range(0, 360);
        float random2 = Random.Range(1, 5);
        float random3 = Random.Range(1, 5);
        dice.transform.rotation = Quaternion.Euler(random1, random1, random1);
        diceRb.velocity = new Vector3(random2, random2, random2);
        diceRb.maxAngularVelocity = 1000;
        Vector3 torque = new Vector3(random3, random3, random3);
        diceRb.AddTorque(torque, ForceMode.VelocityChange);
    }


    private bool IsDiceStopped(){
        return diceRb.velocity == Vector3.zero && diceRb.angularVelocity == Vector3.zero;
    }


    private int FindDiceResult(){
        int maxIndex = 0;
        for (int i = 0; i < diceFaceList.Count; i++){
            if (diceFaceList[i].position.y > diceFaceList[maxIndex].position.y) maxIndex = i;
        }
        Debug.Log($"�ثe�o�ƭȬ�{maxIndex + 1}");
        return (maxIndex + 1);
    }

}
