using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JSONReader : MonoBehaviour
{
    public TextAsset jsonFile;

    //Notes:
    //Dialogues - Array DataStructure to hold Objects with attributes of NPCDialogue

    private void Start()
    {
        Dialogues npcDialogue = JsonUtility.FromJson<Dialogues>(jsonFile.text); //Convert Json Data into Dialogues Array

        string name = "Peter";
        
        foreach (NPCDialogue npc in npcDialogue.dialogues) //Foreach object within' Json File
        {
            if (name.Equals(npc.Name))
                Debug.Log(npc.NPCDetails());
        }
    }

    private void Interact()
    {
        string name = "Peter";      
    }

   
}
