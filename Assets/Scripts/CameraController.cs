using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    [SerializeField] private List<GameObject> cameras = new List<GameObject>();
    
    private void Awake(){
        if (Instance == null){
            Instance = this;
            return;
        }
        Destroy(this);
    }


    [SerializeField] private GameObject target;
    private static readonly Vector3 targetPos = new Vector3(-271, 0, -661);


    [Header("Camera座標")]
    private Vector3 offset = new Vector3(270, 150, 0);
    private Vector3 overLookP = new Vector3(0, 5000, 0);
    private Vector3 overLookR = new Vector3(90, 0, 0);
    private Vector3 frontLookP = new Vector3(-200, 150, 0);
    private Vector3 frontLookR = new Vector3(0, 180, 0);
    private Vector3 backLookR = new Vector3(0, 0, 0);
    private void Start(){
        //target.transform.position = targetPos;
        cameras[0].transform.position = target.transform.position + offset;
        ViewSwitch("backLook");
    }

    public void ViewSwitch(string name){
        switch (name){
            case "overLook":
                cameras[0].transform.position = overLookP;
                cameras[0].transform.localEulerAngles = overLookR;
                return;
            case "backLook":
                cameras[0].transform.position = target.transform.position + offset;
                cameras[0].transform.localEulerAngles = backLookR;
                return;
            case "frontLook":
                cameras[0].transform.position = frontLookP;
                cameras[0].transform.localEulerAngles = frontLookR;
                return;
        }
    }


    private void MouseEvent(){
        
    }

    

}
