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
    public int totalRound { get; set; }

    private void Start()
    {
        InitButton();
    }


    public void UpdateInfo(int x = 0)
    {
        roundText.text = "¦^¦X¼Æ¡G" + x + " / " + totalRound.ToString();
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
        diceButton.interactable = false;
    }


    public void DiceButtonInteractable(bool b)
    {
        diceButton.interactable = b;
    }

}
