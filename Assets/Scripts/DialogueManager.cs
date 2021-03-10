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
    QuestInfo npcQuestInfo;
    
    private string characterInteractedWith = null;
    private int placeInArray = 0;
    public int currentlyDisplayingText = 0;
    public TextAsset jsonFile;
    public TextAsset QuestJsonFile;
    public string NPCReplyText = null;
    public Text textBox;

    string[] NPCReplies = new string[3];

    public struct RelationshipDetails2
    {
        public int Level { get; set; }
        public bool QuestGiven { get; set; }
    }

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
        npcQuestInfo = JsonUtility.FromJson<QuestInfo>(QuestJsonFile.text);
        canvas.enabled = false; //disable the canvas from displaying
        foreach (NPCDialogue npc in npcDialogue.dialogues) //Foreach object within' Json File
        {
            RelationshipDetails2 ThrowMeIn = new RelationshipDetails2 { Level = 0, QuestGiven = false };
            RelationshipStuff4.Add(npc.Name, ThrowMeIn);
        }
    }

    public void NameChecker(string CharName)
    {
        foreach (NPCDialogue npc in npcDialogue.dialogues) //Foreach object within' Json File
        {
            if (CharName == npc.Name)
            {
                Debug.Log(CharName); //for testing purposes
                characterInteractedWith = CharName;
                switch (RelationshipStuff4[CharName].Level) //checking to see what relationship level the player has with the specific NPC
                {
                    case 0:
                            NPCReplyText = (npc.Name + ": " + npc.Introduction); //When they meet the first time, read out the introduction
                        break;
                    case 1://if Acquaintance
                        if (RelationshipStuff4[CharName].QuestGiven)
                        {
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
                        }
                        else
                        {
                            foreach(NPCQuest quest in npcQuestInfo.questInfo)
                            {
                                if(CharName == quest.Name)
                                {
                                    NPCReplyText = (quest.Name + ": " + quest.Aquaintance);
                                    RelationshipDetails2 ThrowMeIn = new RelationshipDetails2 { Level = RelationshipStuff4[CharName].Level, QuestGiven = true};
                                    Instance.RelationshipStuff4[CharName] = ThrowMeIn;
                                }
                            }
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
