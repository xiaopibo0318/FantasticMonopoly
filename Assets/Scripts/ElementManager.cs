using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerManager;
using MapManager;
using CellManager;

namespace ElementManager{
    public class Element{
        public readonly string type;
        public string color;
        public readonly int id;
        
        public readonly string[] allowedType = new string[] { "Wood", "Fire", "Earth", "Metal", "Aqua" };
        public readonly string[] allowedColor = new string[] {"Green", "Red", "Brown", "Yellow", "Blue"};

        public Element(int elementId){
            id = elementId;
            type =  allowedType[id];
            color = allowedColor[id];
        }
    }

    public class ElementManager{
        public void Reaction(Player player, Cell cell){
            int idx = player.element.id;
            int count = 0;
            while (true){
                if (cell.element.type == player.element.allowedType[idx]) break;
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
}
