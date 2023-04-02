using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour{
    [SerializeField] private Button overLookButton;
    [SerializeField] private Button frontLookButton;
    [SerializeField] private Button backLookButton;

    private void Start(){
        InitButton();
    }


    private void InitButton(){
        overLookButton.onClick.AddListener(() => { CameraController.Instance.ViewSwitch("overLook"); });
        frontLookButton.onClick.AddListener(() => { CameraController.Instance.ViewSwitch("frontLook"); });
        backLookButton.onClick.AddListener(() => { CameraController.Instance.ViewSwitch("backLook"); });
    }




}
