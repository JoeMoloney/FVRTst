using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{ 
    //creaing an Instance
    private static DialogueManager instance;

    public static DialogueManager Instance
    {
        get
        {
            return instance;
        }
    }

    //global Variables
    [SerializeField] public Canvas canvas;
    [SerializeField] float TextSpeed = 0.5f;
    Dialogues npcDialogue;
    
    private string characterInteractedWith = null;
    private int placeInArray = 0;
    public int currentlyDisplayingText = 0;
    public TextAsset jsonFile;
    public string NPCReplyText = null;
    public Text textBox;
    #region buttonBools
    public bool getQuestBool = false;
    public bool CompletedQuestBool = false;
    public bool JobBool = false;
    public bool ElectionBool = false;
    public bool BuildingQBool = false;
    public bool BuildingCompleteBool = false;
    #endregion


    void Awake()
    {
        //ensure that there's one and only one instance
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        npcDialogue = JsonUtility.FromJson<Dialogues>(jsonFile.text); //Convert Json Data into Dialogues Array
        canvas.enabled = false; //disable the canvas from displaying
    }

    public void NameChecker(string CharName, int RelationshipLevel)
    {
        foreach (NPCDialogue npc in npcDialogue.dialogues) //Foreach object within' Json File
        {
            if (CharName == npc.Name)
            {
                //check if a button has been used and therefore need to set the reply to this first
                    Debug.Log(CharName); //for testing purposes
                    characterInteractedWith = CharName;
                    if (RelationshipLevel == 0)//if they've never met
                {
                        NPCReplyText = (npc.Name + ": " + npc.Introduction);

                    }
                    else if (RelationshipLevel == 1)//met once - Acquaintance
                {
                        if (npc.Acquaintance2 != null)
                        {
                            string[] AcquaintanceReplies = new string[] { npc.Acquaintance1, npc.Acquaintance2 }; //add all possible replies into an array for randomisation if there are 2 replies
                            NPCReplyText = AcquaintanceReplies[UnityEngine.Random.Range(0, 2)];
                        }
                        else
                        {
                            NPCReplyText = npc.Acquaintance1; // if the npc only has one acquaintance reply
                        }
                    }
                    else if (RelationshipLevel == 2)//met twice - friend
                {
                        string[] FriendReplies = new string[] { npc.Friend1, npc.Friend2, npc.Friend3 };
                        NPCReplyText = FriendReplies[UnityEngine.Random.Range(0, 3)];
                    }
                    else if(RelationshipLevel >= 3)//met 3+ times - best friend
                    {
                        string[] BestFriendReplies = new string[] { npc.BestFriend1, npc.BestFriend2 };
                        NPCReplyText = BestFriendReplies[UnityEngine.Random.Range(0, 2)];
                    }
            }
                StartCoroutine(AnimateText());//start the typewriter effect
                break;//stop the foreach loop continuing after we've found our target npc
        }
    }

    public void  ButtonChecker(string CharName)
    {
        foreach (NPCDialogue npc in npcDialogue.dialogues) //Foreach object within' Json File
        {
            if (CharName == npc.Name)
            { //check if a button has been used and therefore need to set the reply to this first
                if (getQuestBool)
                    NPCReplyText = npc.Quest;
                else if (CompletedQuestBool)
                    NPCReplyText = npc.QuestComplete;
                else if (JobBool)
                    NPCReplyText = npc.Job;
                else if (ElectionBool)
                    NPCReplyText = npc.Election;
                else if (BuildingQBool)
                    NPCReplyText = npc.BuildingQ;
                else if (BuildingCompleteBool)
                    NPCReplyText = npc.BuildingComplete;
            }
            StartCoroutine(AnimateText());//start the typewriter effect
            break;//stop the foreach loop continuing after we've found our target npc
        }
    }

    IEnumerator AnimateText() //typewriter effect
    {
        for (int i = 0; i < (NPCReplyText.Length + 1); i++)
        {
            if(i < (NPCReplyText.Length)){
                textBox.text = NPCReplyText.Substring(0, i) + "|";
            }
            else
            {
                textBox.text = NPCReplyText.Substring(0, i);
                UnityStandardAssets.Characters.FirstPerson.FirstPersonController.CanMove = true; //allowing player movement after all text has appeared on the screen
            }
            
            yield return new WaitForSeconds(TextSpeed);
        }
    }

    
}
