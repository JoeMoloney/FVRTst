using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class TextAnimation : MonoBehaviour
{
    private static TextAnimation instance;

    public static TextAnimation Instance
    {
        get
        {
            return instance;
        }
    }

    

    public TextAsset jsonFile;
    private int placeInArray = 0;
    Dialogues npcDialogue;
    [SerializeField] public Canvas canvas;

    //Notes:
    //Dialogues - Array DataStructure to hold Objects with attributes of NPCDialogue


    private void Start()
    {
        npcDialogue = JsonUtility.FromJson<Dialogues>(jsonFile.text); //Convert Json Data into Dialogues Array
        canvas.enabled = false;
    }

    public void NameChecker(string CharName, int RelationshipLevel)
    {
        foreach (NPCDialogue npc in npcDialogue.dialogues) //Foreach object within' Json File
        {
            if (CharName == npc.Name)
            {
                //Debug.Log(npc.NPCDetails());
                Debug.Log(CharName);
                goatText[0] = npc.Name;
                if(RelationshipLevel == 0)
                goatText[1] = npc.Acquaintance1;
                StartCoroutine(AnimateText());
            }
        }
    }

    private void Interact()
    {
        //string name = "Peter";
    }

    public Text textBox;
    [SerializeField] float TextSpeed = 0.5f;
    //Store all your text in this string array
    //string[] goatText = new string[] { "1. Hey There", "2. Any Plans for the day", "3.Skipping Rope", "4. bin", "5. Microphone" };
    public string[] goatText = new string[2];
    int currentlyDisplayingText = 0;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

       // StartCoroutine(AnimateText());
    }
    //This is a function for a button you press to skip to the next text
    public void SkipToNextText()
    {
        StopAllCoroutines();
        currentlyDisplayingText++;
        //If we've reached the end of the array, do anything you want. I just restart the example text
        if (currentlyDisplayingText > goatText.Length)
        {
            currentlyDisplayingText = 0;
        }
        StartCoroutine(AnimateText());
    }
    //Note that the speed you want the typewriter effect to be going at is the yield waitforseconds (in my case it's 1 letter for every      0.03 seconds, replace this with a public float if you want to experiment with speed in from the editor)
    IEnumerator AnimateText()
    {

        for (int i = 0; i < (goatText[currentlyDisplayingText].Length + 1); i++)
        {
            if(i < (goatText[currentlyDisplayingText].Length)){
                textBox.text = goatText[currentlyDisplayingText].Substring(0, i) + "|";
            }
            else
            {
                textBox.text = goatText[currentlyDisplayingText].Substring(0, i);
            }
            
            yield return new WaitForSeconds(TextSpeed);
        }
    }

    
}
