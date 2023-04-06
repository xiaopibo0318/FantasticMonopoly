using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ElementManager : MonoBehaviour{
    class Element{
        string type;
        string color;

        void Element(string elementType, string elementColor){
            type =  elementType;
            color = elementColor;
        }
    }
     

    public void InitElement(){
        string[] AllowedType = {"metal", "wood", "aqua", "fire", "earth"};
        string[] AllowedColor = {"Yellow", "Green", "Blue", "Red", "Brown"};
        int rand = Random.Range(0, AllowedType.Length);
        Element = new Element(AllowedType[rand], AllowedColor[rand]);
    }
}

/*
    金 = Metal
    木 = Wood
    水 = Aqua
    火 = Fire
    土 = Earth
*/

