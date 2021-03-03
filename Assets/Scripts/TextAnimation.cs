using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
                
                if (RelationshipLevel == 0)
                {
                    goatText[0] = (npc.Name + ": " + npc.Introduction);
                    
                }else if(RelationshipLevel == 1)
                {
                    if (npc.Acquaintance2 != null)
                    {
                        string[] AcquaintanceReplies = new string[] { npc.Acquaintance1, npc.Acquaintance2 };
                        goatText[0] = AcquaintanceReplies[Random.Range(0, 2)];
                    }
                }
                else if(RelationshipLevel == 2)
                {
                    string[] FriendReplies = new string[] { npc.Friend1, npc.Friend2, npc.Friend3 };
                    goatText[0] = FriendReplies[Random.Range(0, 3)];
                }
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
    public string[] goatText = new string[] { "1. Hey There", "2. Any Plans for the day", "3.Skipping Rope", "4. bin", "5. Microphone" };
    //public string[] goatText = new string[5];
    public int currentlyDisplayingText = 0;
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
        //Debug.Log(goatText.Length);
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
