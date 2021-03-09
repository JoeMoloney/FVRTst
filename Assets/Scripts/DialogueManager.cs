using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] AudioSource audioSource;
    Dialogues npcDialogue;
    
    private string characterInteractedWith = null;
    private int placeInArray = 0;
    public int currentlyDisplayingText = 0;
    public TextAsset jsonFile;
    public string NPCReplyText = null;
    public Text textBox;

    string[] NPCReplies = new string[3];
    //[SerializeField] List<Tuple<string, int, bool>> RelationshipStuff = new List<Tuple<string, int, bool>>();
    //[SerializeField] List<RelationshipData> RelationshipStuff2 = new List<RelationshipData>();

    public struct RelationshipDetails
    {
        public string NPCName { get; set; }
        public int Level { get; set; }
        public bool QuestGiven { get; set; }
    }

    public struct RelationshipDetails2
    {
        public int Level { get; set; }
        public bool QuestGiven { get; set; }
    }

    public List<RelationshipDetails> RelationshipStuff3 = new List<RelationshipDetails>();

    public Dictionary<string, RelationshipDetails2> RelationshipStuff4 = new Dictionary<string, RelationshipDetails2>();
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
        foreach (NPCDialogue npc in npcDialogue.dialogues) //Foreach object within' Json File
        {
            //RelationshipStuff.Add(new Tuple<string, int, bool>(npc.Name, 0, false));
            //RelationshipStuff2.Add(new RelationshipData() { NPCName = npc.Name,RelationshipLevel = 0, QuestGiven =  false});
            RelationshipDetails nextThingInList = new RelationshipDetails { NPCName = npc.Name, Level = 0, QuestGiven = false };
            RelationshipStuff3.Add(nextThingInList);

            RelationshipDetails2 ThrowMeIn = new RelationshipDetails2 { Level = 0, QuestGiven = false };
            RelationshipStuff4.Add(npc.Name, ThrowMeIn);
        }
        for(int i = 0; i<RelationshipStuff3.Count; i++)
        {
            Debug.Log(RelationshipStuff3[i].NPCName + ", " + RelationshipStuff3[i].Level + ", " + RelationshipStuff3[i].QuestGiven);

            Debug.Log("Dict: " + RelationshipStuff3[i].NPCName + RelationshipStuff4[RelationshipStuff3[i].NPCName].Level + ", " + RelationshipStuff3[i].NPCName + RelationshipStuff4[RelationshipStuff3[i].NPCName].QuestGiven);
        }
    }

    public void NameChecker(string CharName, int RelationshipLevel)
    {
        foreach (NPCDialogue npc in npcDialogue.dialogues) //Foreach object within' Json File
        {
            if (CharName == npc.Name)
            {
                Debug.Log(CharName); //for testing purposes
                characterInteractedWith = CharName;
                switch (RelationshipLevel) //checking to see what relationship level the player has with the specific NPC
                {
                    case 0:
                        NPCReplyText = (npc.Name + ": " + npc.Introduction); //When they meet the first time, read out the introduction
                        break;
                    case 1://if Acquaintance
                        if (npc.Acquaintance2 != "") // included due to Flynn only having one acquaintence reply
                        {
                            NPCReplies[0] = npc.Acquaintance1;
                            NPCReplies[1] = npc.Acquaintance2;
                            NPCReplyText = (npc.Name + ": " + NPCReplies[UnityEngine.Random.Range(0, 2)]);//choose a random one of the two possible replies
                        }
                        else
                        {
                            NPCReplyText = (npc.Name + ": " + npc.Acquaintance1); 
                        }
                        break;
                    case 2:///if NPCand player are friends
                        NPCReplies[0] = npc.Friend1;
                        NPCReplies[1] = npc.Friend2;
                        NPCReplies[2] = npc.Friend3;
                        NPCReplyText = (npc.Name + ": " + NPCReplies[UnityEngine.Random.Range(0, 3)]);//choose a random one of the three possible replies
                        break;
                    case 3://if they're best friends
                        NPCReplies[0] = npc.BestFriend1;
                        NPCReplies[1] = npc.BestFriend2;
                        NPCReplyText = (npc.Name + ": " + NPCReplies[UnityEngine.Random.Range(0, 2)]);//choose a random one of the two possible replies
                        break;
                }
                StartCoroutine(AnimateText());//start the typewriter effect
                break;//stop the foreach loop continuing after we've found our target npc
            }
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
            audioSource.Play();
            
            yield return new WaitForSeconds(TextSpeed);
        }
    }

    public class RelationshipData : MonoBehaviour
    {
        public string NPCName = null;
        public int RelationshipLevel = 0;
        public bool QuestGiven = false;
    }


}












//*********************************************//
//------------- RIP TO THE OLD IF'S :'( -------------------------
//    if (RelationshipLevel == 0)//if they've never met
//{
//        NPCReplyText = (npc.Name + ": " + npc.Introduction);

//    }
//    else if (RelationshipLevel == 1)//met once - Acquaintance
//{
//        if (npc.Acquaintance2 != null)
//        {
//            string[] AcquaintanceReplies = new string[] { npc.Acquaintance1, npc.Acquaintance2 }; //add all possible replies into an array for randomisation if there are 2 replies
//            NPCReplyText = AcquaintanceReplies[UnityEngine.Random.Range(0, 2)];
//        }
//        else
//        {
//            NPCReplyText = npc.Acquaintance1; // if the npc only has one acquaintance reply
//        }
//    }
//    else if (RelationshipLevel == 2)//met twice - friend
//{
//        string[] FriendReplies = new string[] { npc.Friend1, npc.Friend2, npc.Friend3 };
//        NPCReplyText = FriendReplies[UnityEngine.Random.Range(0, 3)];
//    }
//    else if(RelationshipLevel >= 3)//met 3+ times - best friend
//    {
//        string[] BestFriendReplies = new string[] { npc.BestFriend1, npc.BestFriend2 };
//        NPCReplyText = BestFriendReplies[UnityEngine.Random.Range(0, 2)];
//    }
//**************************************************************************//
