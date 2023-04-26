using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerViewData : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private Image image;
    [SerializeField] private List<TextMeshProUGUI> elementNumList = new List<TextMeshProUGUI>(); // fire water wood gold dirt


    public void InfoInit(string name, Image newImage = null)
    {
        playerName.text = name;
        //image = newImage;

        for (int i = 0; i < elementNumList.Count; i++)
        {
            elementNumList[i].text = "0";
        }
    }

}
