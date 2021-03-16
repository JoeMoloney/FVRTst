using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    [SerializeField] public GameObject QuestBox = null;
    [SerializeField] public Text ActiveQuests = null;
    public TextAsset QuestJsonFile;
    QuestInfo npcQuestInfo;
    public List<Quest> listOfQuests = new List<Quest>();

    public struct Quest
    {
        public string NPCName;
        public string AquaintanceQuest;
        public string AquaintanceComplete;
        public string ASummary;
        public string FriendQuest;
        public string FriendComplete;
        public string FSummary;
        public string BestFriendQuest;
        public string BestFriendComplete;
        public string BFFSummary;
    }

    //creaing an Instance
    private static QuestManager instance;

    public static QuestManager Instance
    {
        get
        {
            return instance;
        }
    }

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
        QuestBox.SetActive(false);
        npcQuestInfo = JsonUtility.FromJson<QuestInfo>(QuestJsonFile.text);
        foreach (NPCQuest quest in npcQuestInfo.questInfo)
        {
            Quest newQuestToAdd = new Quest
            {
                NPCName = quest.Name,
                AquaintanceQuest = quest.Aquaintance,
                AquaintanceComplete = quest.AquaintanceComplete,
                ASummary = quest.ASummary,
                FriendQuest = quest.Friend,
                FriendComplete = quest.FriendComplete,
                FSummary = quest.FSummary,
                BestFriendQuest = quest.BestFriend,
                BestFriendComplete = quest.BestFriendComplete,
                BFFSummary = quest.BFFSummary
            };
            listOfQuests.Add(newQuestToAdd);
        }
    }
}
