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
        dice.transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        diceRb.velocity = new Vector3(Random.Range(1, 5), Random.Range(1, 5), Random.Range(1, 5));
        diceRb.maxAngularVelocity = 1000;
        Vector3 torque = new Vector3(Random.Range(1, 5), Random.Range(1, 5), Random.Range(1, 5));
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
        Debug.Log($"目前得數值為{maxIndex + 1}");
        return (maxIndex + 1);
    }
}
