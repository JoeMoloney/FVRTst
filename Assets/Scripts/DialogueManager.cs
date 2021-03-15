using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
    [SerializeField] public GameObject ReplyBoxAndButtons;
    [SerializeField] float TextSpeed = 0.5f;
    [SerializeField] AudioSource audioSource;
    Dialogues npcDialogue;
    QuestInfo npcQuestInfo;
    
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
        ReplyBoxAndButtons.SetActive(false); //disable the canvas from displaying
        foreach (NPCDialogue npc in npcDialogue.dialogues) //Foreach object within' Json File
        {
            RelationshipDetails ThrowMeIn = new RelationshipDetails { Level = 0, QuestGiven = false };
            RelationshipDictionary.Add(npc.Name, ThrowMeIn);
        }
    }

    public void NameChecker(string CharName)
    {
        Debug.Log(CharName); //for testing purposes
        switch (RelationshipDictionary[CharName].Level) //checking to see what relationship level the player has with the specific NPC
        {
            case 0:
                IfNPCIsOnTheirIntroduction(CharName);
                break;
            case 1://if Acquaintance
                if (RelationshipDictionary[CharName].QuestGiven)
                    IfNPCIsAcquaintance(CharName);
                else
                    QuestForEach(CharName);
                break;
            case 2:///if NPCand player are friends
                if (RelationshipDictionary[CharName].QuestGiven)
                    IfNPCIsFriend(CharName);
                else
                    QuestForEach(CharName);
                break;
            case 3://if they're best friends
                if (RelationshipDictionary[CharName].QuestGiven)
                    ifNPCIsBestFriend(CharName);
                else
                    QuestForEach(CharName);
                break;
        }
        StartCoroutine(AnimateText());//start the typewriter effect
    }

    public void IfNPCIsOnTheirIntroduction(string CharName)
    {
        foreach (NPCDialogue npc in npcDialogue.dialogues) //Foreach object within' Json File
        {
            if (CharName == npc.Name)
            {
                NPCReplyText = (npc.Name + ": " + npc.Introduction); //When they meet the first time, read out the introduction
            }
        }
    }

    public void IfNPCIsAcquaintance(string CharName)
    {
        foreach (NPCDialogue npc in npcDialogue.dialogues) //Foreach object within' Json File
        {
            if (CharName == npc.Name)
            {
                if (npc.Acquaintance2 != "") // included due to Flynn only having one acquaintence reply
                {
                    NPCReplies[0] = npc.Acquaintance1;
                    NPCReplies[1] = npc.Acquaintance2;
                    NPCReplyText = (npc.Name + ": " + NPCReplies[UnityEngine.Random.Range(0, 2)]);//choose a random one of the two possible replies
                }
                else
                    NPCReplyText = (npc.Name + ": " + npc.Acquaintance1);
                break;
            }
        }
    }

    public void IfNPCIsFriend(string CharName)
    {
        foreach (NPCDialogue npc in npcDialogue.dialogues) //Foreach object within' Json File
        {
            if (CharName == npc.Name)
            {
                NPCReplies[0] = npc.Friend1;
                NPCReplies[1] = npc.Friend2;
                NPCReplies[2] = npc.Friend3;
                NPCReplyText = (npc.Name + ": " + NPCReplies[UnityEngine.Random.Range(0, 3)]);//choose a random one of the three possible replies
                break;
            }
        }
    }

    public void ifNPCIsBestFriend(string CharName)
    {
        foreach (NPCDialogue npc in npcDialogue.dialogues) //Foreach object within' Json File
        {
            if (CharName == npc.Name)
            {
                NPCReplies[0] = npc.BestFriend1;
                NPCReplies[1] = npc.BestFriend2;
                NPCReplyText = (npc.Name + ": " + NPCReplies[UnityEngine.Random.Range(0, 2)]);//choose a random one of the two possible replies
                break;
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
                {
                    NPCReplyText = (quest.Name + ": " + quest.Aquaintance);
                    if (QuestManager.Instance.ActiveQuests.text == "")
                        QuestManager.Instance.ActiveQuests.text = quest.Name + ": " + quest.ASummary;
                    else
                        QuestManager.Instance.ActiveQuests.text += "\n" + quest.Name + ": " + quest.ASummary;
                }
                else if (RelationshipDictionary[CharName].Level == 2)
                {
                    NPCReplyText = (quest.Name + ": " + quest.Friend);
                    if (QuestManager.Instance.ActiveQuests.text == "")
                        QuestManager.Instance.ActiveQuests.text = quest.Name + ": " + quest.FSummary;
                    else
                        QuestManager.Instance.ActiveQuests.text += "\n" + quest.Name + ": " + quest.FSummary;
                }
                else if (RelationshipDictionary[CharName].Level == 3)
                {
                    NPCReplyText = (quest.Name + ": " + quest.BestFriend);
                    if (QuestManager.Instance.ActiveQuests.text == "")
                        QuestManager.Instance.ActiveQuests.text = quest.Name + ": " + quest.BFFSummary;
                    else
                        QuestManager.Instance.ActiveQuests.text += "\n" + quest.Name + ": " + quest.BFFSummary;
                }
                else
                    NPCReplyText = "Invalid text";
                RelationshipDetails ThrowMeIn = new RelationshipDetails { Level = RelationshipDictionary[CharName].Level, QuestGiven = true };
                RelationshipDictionary[CharName] = ThrowMeIn;
                QuestManager.Instance.QuestBox.SetActive(true);
                
                break;
            }
            
        }
    }

    public void ButtonChecker(string CharName)
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
                            {
                                QuestManager.Instance.ActiveQuests.text = QuestManager.Instance.ActiveQuests.text.Replace(quest.Name + ": " + quest.ASummary, string.Empty);
                                NPCReplyText = (quest.Name + ": " + quest.AquaintanceComplete);
                            }
                            else if (RelationshipDictionary[CharName].Level == 2)
                            {
                                QuestManager.Instance.ActiveQuests.text = QuestManager.Instance.ActiveQuests.text.Replace(quest.Name + ": " + quest.FSummary, string.Empty);
                                NPCReplyText = (quest.Name + ": " + quest.FriendComplete);
                            }
                            else if (RelationshipDictionary[CharName].Level == 3)
                            {
                                QuestManager.Instance.ActiveQuests.text = QuestManager.Instance.ActiveQuests.text.Replace(quest.Name + ": " + quest.BFFSummary, string.Empty);
                                NPCReplyText = (quest.Name + ": " + quest.BestFriendComplete);
                            }
                            else
                                NPCReplyText = "Invalid text";
                            if (!StringIncludesLetters(QuestManager.Instance.ActiveQuests.text))
                            {
                                QuestManager.Instance.ActiveQuests.text = QuestManager.Instance.ActiveQuests.text.Replace("\n", string.Empty);
                            }
                        }
                        if (QuestManager.Instance.ActiveQuests.text == string.Empty)
                            QuestManager.Instance.QuestBox.SetActive(false);
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

    private static bool StringIncludesLetters(String str)
    {
        return QuestManager.Instance.ActiveQuests.text.Any(x => char.IsLetter(x));
        //return Regex.IsMatch(str, @"^[a-zA-Z]+$");
    }

    public string GetRelationshipLevel(string NPCName)
    {
        string relationshipLevel = null;
        switch (RelationshipDictionary[NPCName].Level)
        {
            case 0:
                relationshipLevel = "Introduction";
                break;
            case 1:
                relationshipLevel = "Acquaintance";
                break;
            case 2:
                relationshipLevel = "Friend";
                break;
            case 3:
                relationshipLevel = "BestFriend";
                break;
        }
        return relationshipLevel;
    }

    public bool GetQuestGiven(string NPCName)
    {
        return RelationshipDictionary[NPCName].QuestGiven;
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
