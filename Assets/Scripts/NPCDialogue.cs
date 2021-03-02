using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCDialogue //Objects Attributes
{
    //Attributes found in Json file relating to NPC Dialogues
    public string Name, Description, Building, Pastry,
        Introduction, Acquaintance1, Acquaintance2, Friend1,
        Friend2, Friend3, Election, BestFriend1,
        BestFriend2, BuildingQ, BuildingComplete, NPC,
        Job, Quest, QuestComplete;

    public static bool firstEncounter = true;

    public override string ToString()
    {
        return string.Format($"Name: {Name}, Description: {Description}, Building: {Building}, Pastry: {Pastry},\n" +
            $" Introduction: {Introduction}, Acquaintance1: {Acquaintance1}, Acquaintance2: {Acquaintance2}, Friend1: {Friend1},\n" +
            $" Friend2: {Friend2}, Friend3: {Friend3}, Election: {Election}, BestFriend1: {BestFriend1},\n" +
            $" BestFriend2: {BestFriend2}, BuildingQ: {BuildingQ}, BuildingComplete: {BuildingComplete}, NPC: {NPC},\n" +
            $" Job: {Job}, Quest: {Quest}, QuestComplete: {QuestComplete}");
    }

    public string NPCDetails()
    { 
        return string.Format($"NPC Name: {Name}, \nNPC Description: {Description}, \nHome Building: {Building}, \nFavorite Pastry: {Pastry}, \nFirst Encounter?: {firstEncounter}");
    }
}
