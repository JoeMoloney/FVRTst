using UnityEngine;

public class Player : MonoBehaviour
{
    private GameObject NPCTalkingTo = null;
    #region Triggers
    private void OnTriggerEnter(Collider other)//as the player collides with one of the box npcs.
    {
        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.CanMove = false; //don't allow the player to move anymore
        DialogueManager.Instance.ReplyBoxAndButtons.SetActive(true); // show the canvas for text box, text and buttons
        Debug.Log(DialogueManager.Instance.RelationshipDictionary[other.name].Level); // for testing
        if (other.gameObject.CompareTag("Peter"))//are we talking to peter?
        {
            NPCTalkingTo = other.gameObject;
            DialogueManager.Instance.NPCInteraction(other.name); //run GeneralDialogueReply
        }
        else if (other.gameObject.CompareTag("Flynn"))//are we talking to flynn?
        {
            NPCTalkingTo = other.gameObject;
            DialogueManager.Instance.NPCInteraction(other.name);
        }
        else if (other.gameObject.CompareTag("Poppy"))//are we talking to poppy?
        {
            NPCTalkingTo = other.gameObject;
            DialogueManager.Instance.NPCInteraction(other.name);
        }
        //if (DialogueManager.Instance.RelationshipDictionary[other.name].Level == 0)
        //{
        //    DialogueManager.RelationshipDetails ThrowMeIn = new DialogueManager.RelationshipDetails { Level = DialogueManager.Instance.RelationshipDictionary[other.name].Level + 1, QuestGiven = false };
        //    DialogueManager.Instance.RelationshipDictionary[other.name] = ThrowMeIn;
        //}
    }

    private void OnTriggerExit(Collider other) //when the player leaves the box that they were colliding with in the first place
    {
        //in here I'm resetting the text that was in the textbox so it doesn't cause bugs. I'm also turning the canvas off so it's not always on screen, only when they're talking
        DialogueManager.Instance.NPCReplyText = null;
        DialogueManager.Instance.StopAllCoroutines();
        DialogueManager.Instance.ReplyBoxAndButtons.SetActive(false);
        DialogueManager.Instance.currentlyDisplayingText = 0;
    }
    #endregion

    #region button methods
    public void GetQuest() //pressing the get quest button to sim this situation happening in the game
    {
        DialogueManager.Instance.StopAllCoroutines();

        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.CanMove = false;

        DialogueManager.Instance.getQuestBool = true;

        DialogueManager.Instance.SimulatorWithButtons(NPCTalkingTo.name);
    }

    public void CompleteQuest()//pressing the completed quest button to sim this situation happening in the game
    {
        DialogueManager.Instance.StopAllCoroutines();

        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.CanMove = false;

        DialogueManager.Instance.CompletedQuestBool = true;
        DialogueManager.Instance.getQuestBool = false;

        DialogueManager.Instance.SimulatorWithButtons(NPCTalkingTo.name);
    }

    public void Job()//pressing the job button to sim this situation happening in the game
    {
        DialogueManager.Instance.StopAllCoroutines();

        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.CanMove = false;

        DialogueManager.Instance.CompletedQuestBool = false;
        DialogueManager.Instance.getQuestBool = false;
        DialogueManager.Instance.JobBool = true;

        DialogueManager.Instance.SimulatorWithButtons(NPCTalkingTo.name);
    }

    public void Election()//pressing the election button to sim this situation happening in the game
    {
        DialogueManager.Instance.StopAllCoroutines();

        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.CanMove = false;

        DialogueManager.Instance.CompletedQuestBool = false;
        DialogueManager.Instance.getQuestBool = false;
        DialogueManager.Instance.JobBool = false;
        DialogueManager.Instance.ElectionBool = true;

        DialogueManager.Instance.SimulatorWithButtons(NPCTalkingTo.name);
    }

    public void BuildingQ()//pressing the get the building quest button to sim this situation happening in the game
    {
        DialogueManager.Instance.StopAllCoroutines();

        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.CanMove = false;

        DialogueManager.Instance.CompletedQuestBool = false;
        DialogueManager.Instance.getQuestBool = false;
        DialogueManager.Instance.JobBool = false;
        DialogueManager.Instance.ElectionBool = false;
        DialogueManager.Instance.BuildingQBool = true;

        DialogueManager.Instance.SimulatorWithButtons(NPCTalkingTo.name);
    }

    public void BuildingComplete()//pressing the building quest complete button to sim this situation happening in the game
    {
        DialogueManager.Instance.StopAllCoroutines();

        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.CanMove = false;

        DialogueManager.Instance.CompletedQuestBool = false;
        DialogueManager.Instance.getQuestBool = false;
        DialogueManager.Instance.JobBool = false;
        DialogueManager.Instance.ElectionBool = false;
        DialogueManager.Instance.BuildingQBool = false;
        DialogueManager.Instance.BuildingCompleteBool = true;

        DialogueManager.Instance.SimulatorWithButtons(NPCTalkingTo.name);
    }

    #endregion

}
