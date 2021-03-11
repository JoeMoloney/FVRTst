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
    public int currentlyDisplayingText = 0;
    public TextAsset jsonFile;
    public TextAsset QuestJsonFile;
    public string NPCReplyText = null;
    public Text textBox;

    string[] NPCReplies = new string[3];

    public struct RelationshipDetails
    {
        public int Level { get; set; }
        public bool QuestGiven { get; set; }
    }

    public Dictionary<string, RelationshipDetails> RelationshipDictionary = new Dictionary<string, RelationshipDetails>();
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
            RelationshipDetails ThrowMeIn = new RelationshipDetails { Level = 0, QuestGiven = false };
            RelationshipDictionary.Add(npc.Name, ThrowMeIn);
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
                switch (RelationshipDictionary[CharName].Level) //checking to see what relationship level the player has with the specific NPC
                {
                    case 0:
                        NPCReplyText = (npc.Name + ": " + npc.Introduction); //When they meet the first time, read out the introduction
                        break;
                    case 1://if Acquaintance
                        if (RelationshipDictionary[CharName].QuestGiven)
                        {
                            if (npc.Acquaintance2 != "") // included due to Flynn only having one acquaintence reply
                            {
                                NPCReplies[0] = npc.Acquaintance1;
                                NPCReplies[1] = npc.Acquaintance2;
                                NPCReplyText = (npc.Name + ": " + NPCReplies[UnityEngine.Random.Range(0, 2)]);//choose a random one of the two possible replies
                            }
                            else
                                NPCReplyText = (npc.Name + ": " + npc.Acquaintance1);
                        }
                        else
                        {
                            QuestForEach(CharName);
                        }
                        break;
                    case 2:///if NPCand player are friends
                        if (RelationshipDictionary[CharName].QuestGiven)
                        {
                            NPCReplies[0] = npc.Friend1;
                            NPCReplies[1] = npc.Friend2;
                            NPCReplies[2] = npc.Friend3;
                            NPCReplyText = (npc.Name + ": " + NPCReplies[UnityEngine.Random.Range(0, 3)]);//choose a random one of the three possible replies
                        }
                        else
                            QuestForEach(CharName);
                        break;
                    case 3://if they're best friends
                        if (RelationshipDictionary[CharName].QuestGiven) { 
                        NPCReplies[0] = npc.BestFriend1;
                        NPCReplies[1] = npc.BestFriend2;
                        NPCReplyText = (npc.Name + ": " + NPCReplies[UnityEngine.Random.Range(0, 2)]);//choose a random one of the two possible replies
                        }
                        else
                            QuestForEach(CharName);
                        break;
                }
                StartCoroutine(AnimateText());//start the typewriter effect
                break;//stop the foreach loop continuing after we've found our target npc
            }
        }
    }


    public void QuestForEach(string CharName)
    {
        foreach (NPCQuest quest in npcQuestInfo.questInfo)
        {
            if (CharName == quest.Name)
            {
                if (RelationshipDictionary[CharName].Level == 1)
                    NPCReplyText = (quest.Name + ": " + quest.Aquaintance);
                else if (RelationshipDictionary[CharName].Level == 2)
                    NPCReplyText = (quest.Name + ": " + quest.Friend);
                else if (RelationshipDictionary[CharName].Level == 3)
                    NPCReplyText = (quest.Name + ": " + quest.BestFriend);
                else
                    NPCReplyText = "Invalid text";
                RelationshipDetails ThrowMeIn = new RelationshipDetails { Level = RelationshipDictionary[CharName].Level, QuestGiven = true };
                RelationshipDictionary[CharName] = ThrowMeIn;
            }
        }
    }

    
    public void  ButtonChecker(string CharName)
    {
        Debug.Log("Running button checker"); // testing purposes
        Debug.Log("CharName = " + CharName);
        foreach (NPCDialogue npc in npcDialogue.dialogues) //Foreach object within' Json File
        {
            Debug.Log(npc.Name);
            if (CharName == npc.Name)
            { //check if a button has been used and therefore need to set the reply to this first
                Debug.Log("Matched the charname and npc name");
                if (getQuestBool)
                    NPCReplyText = npc.Quest;
                else if (CompletedQuestBool)
                {
                    Debug.Log("CompletedQuestBool is pressed");
                        foreach (NPCQuest quest in npcQuestInfo.questInfo)
                        {
                            if (CharName == quest.Name)
                            {
                                if (RelationshipDictionary[CharName].Level == 1)
                                    NPCReplyText = (quest.Name + ": " + quest.AquaintanceComplete);
                                else if (RelationshipDictionary[CharName].Level == 2)
                                    NPCReplyText = (quest.Name + ": " + quest.FriendComplete);
                                else if (RelationshipDictionary[CharName].Level == 3)
                                    NPCReplyText = (quest.Name + ": " + quest.BestFriendComplete);
                                else
                                    NPCReplyText = "Invalid text";
                            }
                        }
                    if (RelationshipDictionary[CharName].Level < 3)
                    {
                        RelationshipDetails ThrowMeIn = new RelationshipDetails { Level = RelationshipDictionary[CharName].Level + 1, QuestGiven = false };
                        RelationshipDictionary[CharName] = ThrowMeIn;
                    }
                }
                else if (JobBool)
                    NPCReplyText = npc.Job;
                else if (ElectionBool)
                    NPCReplyText = npc.Election;
                else if (BuildingQBool)
                    NPCReplyText = npc.BuildingQ;
                else if (BuildingCompleteBool)
                    NPCReplyText = npc.BuildingComplete;
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
