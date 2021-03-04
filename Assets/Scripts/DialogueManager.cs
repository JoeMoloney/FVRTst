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
    public TextAsset jsonFile;
    private int placeInArray = 0;
    Dialogues npcDialogue;
    [SerializeField] public Canvas canvas;
    public Text textBox;
    [SerializeField] float TextSpeed = 0.5f;
    public string NPCReplyText = null;
    public int currentlyDisplayingText = 0;
    private string characterInteractedWith = null;
    public bool getQuestBool = false;
    public bool CompletedQuestBool = false;
    public bool JobBool = false;
    public bool ElectionBool = false;
    public bool BuildingQBool = false;
    public bool BuildingCompleteBool = false;

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
                if (getQuestBool)
                {
                    NPCReplyText = npc.Quest;
                }else if(CompletedQuestBool)
                {
                    NPCReplyText = npc.QuestComplete;
                }else if (JobBool)
                {
                    NPCReplyText = npc.Job;
                }
                else if (ElectionBool)
                {
                    NPCReplyText = npc.Election;
                }
                else if (BuildingQBool)
                {
                    NPCReplyText = npc.BuildingQ;
                }
                else if (BuildingCompleteBool)
                {
                    NPCReplyText = npc.BuildingComplete;
                }
                else
                //if it's not a button, what relationship level are we at, and what do we need to reply with?
                {
                    Debug.Log(CharName); //for testing purposes
                    characterInteractedWith = CharName;
                    //if they've never met
                    if (RelationshipLevel == 0)
                    {
                        NPCReplyText = (npc.Name + ": " + npc.Introduction);

                    }
                    //met once - Acquaintance
                    else if (RelationshipLevel == 1)
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
                    else if (RelationshipLevel == 2)
                        //met twice - friend
                    {
                        string[] FriendReplies = new string[] { npc.Friend1, npc.Friend2, npc.Friend3 };
                        NPCReplyText = FriendReplies[UnityEngine.Random.Range(0, 3)];
                    }
                    else if(RelationshipLevel >= 3)
                        //met 3+ times - best friend
                    {
                        string[] BestFriendReplies = new string[] { npc.BestFriend1, npc.BestFriend2 };
                        NPCReplyText = BestFriendReplies[UnityEngine.Random.Range(0, 2)];
                    }
                }
                StartCoroutine(AnimateText());//start the typewriter effect
                break;//stop the foreach loop continuing after we've found our target npc
            }
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
