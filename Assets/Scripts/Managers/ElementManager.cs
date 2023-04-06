using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ElementManager : MonoBehaviour{
    private string Element;
     

    public void InitElement(){
        if (this.Element != null){
            return;
        }
        string[] AllowedElements = {"metal", "wood", "aqua", "fire", "earth"};
        this.Element = AllowedElements[Random.Range(0, AllowedElements.Length)];

}

/*
    金 = Metal
    木 = Wood
    水 = Aqua
    火 = Fire
    土 = Earth
*/

