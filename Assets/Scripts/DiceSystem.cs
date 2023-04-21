using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceSystem{
    //[SerializeField] private GameObject dice;
    //[SerializeField] private List<Transform> diceFaceList;
    //private static Rigidbody diceRb;
    //private int diceNum = 0;

    public class Dice{
        public int id;
        public string name;
        private static GameObject gameObj;
        private static Rigidbody rigidbd = gameObj.GetComponent<Rigidbody>();
        private List<Transform> faces = new List<Transform>(6);
        public int result;

        private bool IsStopped(){
            return (rigidbd.velocity == Vector3.zero && rigidbd.angularVelocity == Vector3.zero);
        }
    
        //Result() call only IsStopped == True
        private int Result(){
            if (!IsStopped()) Result();
            int max = 0;
            for (int i = 0; i < faces.Count; i++){
                if (faces[i].position.y > faces[max].position.y) max = i;
            }
            return (max + 1);
        }
        
        public void Roll(){
            gameObj.transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            rigidbd.velocity = new Vector3(Random.Range(1, 5), Random.Range(1, 5), Random.Range(1, 5));
            rigidbd.maxAngularVelocity = 1000;
            Vector3 torque = new Vector3(Random.Range(1, 5), Random.Range(1, 5), Random.Range(1, 5));
            rigidbd.AddTorque(torque, ForceMode.VelocityChange);
            while (!IsStopped()){
                continue;
            }
            result = Result();
        }

        
    }
    
    //private void Start(){
    //    Dice_Init();
    //    Roll_Dice();
    //}

    //private void FixedUpdate(){
    //    if (IsDiceStopped()){
    //        diceNum = FindDiceResult();
    //    }
    //}

    //private void Dice_Init(){
    //    diceRb = dice.GetComponent<Rigidbody>();
    //}

    //private void Roll_Dice(){
    //    dice.transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
    //    diceRb.velocity = new Vector3(Random.Range(1, 5), Random.Range(1, 5), Random.Range(1, 5));
    //    diceRb.maxAngularVelocity = 1000;
    //    Vector3 torque = new Vector3(Random.Range(1, 5), Random.Range(1, 5), Random.Range(1, 5));
    //    diceRb.AddTorque(torque, ForceMode.VelocityChange);
    //}


    //private bool IsDiceStopped(){
    //    return (diceRb.velocity == Vector3.zero && diceRb.angularVelocity == Vector3.zero);
    //}


    //private int FindDiceResult(){
    //    int maxIndex = 0;
    //    for (int i = 0; i < diceFaceList.Count; i++){
    //        if (diceFaceList[i].position.y > diceFaceList[maxIndex].position.y) maxIndex = i;
    //    }
    //    Debug.Log($"目前得數值為{maxIndex + 1}");
    //    return (maxIndex + 1);
    //}
}
