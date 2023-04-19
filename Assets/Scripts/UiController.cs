using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : Singleton<UiController>
{
    [SerializeField] private Button overLookButton;
    [SerializeField] private Button frontLookButton;
    [SerializeField] private Button backLookButton;

    [SerializeField] private Button diceButton;

    [SerializeField] private Text roundText;
    [HideInInspector] public int totalRound { get; set; }

    private void Start()
    {
        InitButton();
        roundText.text = "¦^¦X¼Æ¡G 0 / " + totalRound.ToString();
    }


    private void InitButton()
    {
        overLookButton.onClick.AddListener(() => { CameraController.Instance.ViewSwitch("overLook"); });
        frontLookButton.onClick.AddListener(() => { CameraController.Instance.ViewSwitch("frontLook"); });
        backLookButton.onClick.AddListener(() => { CameraController.Instance.ViewSwitch("backLook"); });
        diceButton.onClick.AddListener(() =>
        {
            CameraController.Instance.ViewSwitch("Dice");
            DiceSystem.Instance.RollDice();
        });
    }




}
