using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerManager;
using MapManager;
using CellManager;

namespace ElementManager{
    public class Element{
        public string type;
        public string color;
        public int id;
        
        public string[] AllowedType = new string[] { "Wood", "Fire", "Earth", "Metal", "Aqua" };
        public string[] AllowedColor = new string[] {"Green", "Red", "Brown", "Yellow", "Blue"};

        public Element(int elementId){
            id = elementId;
            type =  AllowedType[id];
            color = AllowedColor[id];
        }
    }

    public void Reaction(Player player, Cell cell){
        string[] AllowedType = new string[] { "Wood", "Fire", "Earth", "Metal", "Aqua" };
        int idx = player.element.id;
        int count = 0;
        while (true){
            if (cell.element.type == player.element.AllowedType[idx]) break;
            if (idx == 4){
                idx = 0;
                continue;
            }
            idx += 1;
            count += 1;
        }
        int effect = count; 
        switch (effect){
            case 0:
                //ask insert tokens
                break;
            case 1:
                cell.token += 2;
                player.tokens[player.element] -= 1;
                player.CheckIsFoul();
                break;
            case 2:
                cell.token -= 1;
                cell.IsTokenEmpty();
                break;
            case 3: 
                player.tokens[player.element] += 2;
                cell.token -= 1;
                cell.IsTokenEmpty();
                break;
            case 4:
                player.tokens[player.element] -= 1;
                player.CheckIsFoul();
                break;
        }
    }
}
