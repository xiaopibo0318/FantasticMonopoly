using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    [SerializeField] private List<GameObject> cameras = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else Destroy(this);
    }


    [SerializeField] private GameObject target;


    [Header("Camera座標")]
    private Vector3 offset = new Vector3(0, 180, -135);
    private Vector3 overLookP = new Vector3(400, 1000, 200);
    private Vector3 overLookR = new Vector3(90, 0, 0);
    private Vector3 frontLookP = new Vector3(0, 0, 40);
    private Vector3 frontLookR = new Vector3(0, 180, 0);
    private Vector3 backLookR = new Vector3(45, 0, 0);
    private void Start()
    {
        cameras[0].gameObject.SetActive(true);
        cameras[1].gameObject.SetActive(false);
        ViewSwitch("backLook");
    }

    public void ViewSwitch(string name)
    {

        if (name.Contains("overLook"))
        {
            cameras[0].transform.position = overLookP;
            cameras[0].transform.localEulerAngles = overLookR;
        }
        else if (name.Contains("backLook"))
        {
            cameras[0].transform.position = target.transform.position + offset;
            cameras[0].transform.localEulerAngles = backLookR;
        }
        else if (name.Contains("frontLook"))
        {
            cameras[0].transform.position = frontLookP;
            cameras[0].transform.localEulerAngles = frontLookR;
        }
        else if (name.Contains("Dice"))
        {
            cameras[0].gameObject.SetActive(false);
            cameras[1].gameObject.SetActive(true);
        }
        else if (name.Contains("Main"))
        {
            cameras[1].gameObject.SetActive(false);
            cameras[0].gameObject.SetActive(true);
        }
    }


    private void MouseEvent()
    {

    }



}
