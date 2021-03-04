using UnityEngine;

public class Player : MonoBehaviour
{
    private GameObject NPCTalkingTo = null;
    
    private void OnTriggerEnter(Collider other)//as the player collides with one of the box npcs.
    {
        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.CanMove = false; //don't allow the player to move anymore
        DialogueManager.Instance.canvas.enabled = true; // show the canvas for text box, text and buttons
        Debug.Log(other.gameObject.GetComponent<NPCs>().Relationship); // for testing
        if (other.gameObject.CompareTag("Peter"))//are we talking to peter?
        {
            NPCTalkingTo = other.gameObject;
            DialogueManager.Instance.NameChecker(other.name, other.gameObject.GetComponent<NPCs>().Relationship); //run namechecker
            other.gameObject.GetComponent<NPCs>().Relationship++; // simulate increasing the relationship level
        }
        else if (other.gameObject.CompareTag("Flynn"))//are we talking to flynn?
        {
            NPCTalkingTo = other.gameObject;
            DialogueManager.Instance.NameChecker(other.name, other.gameObject.GetComponent<NPCs>().Relationship);
            other.gameObject.GetComponent<NPCs>().Relationship++;
        }
    }

    public void GetQuest() //pressing the get quest button to sim this situation happening in the game
    {
        DialogueManager.Instance.StopAllCoroutines();

        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.CanMove = false;

        DialogueManager.Instance.getQuestBool = true;

        DialogueManager.Instance.NameChecker(NPCTalkingTo.name, NPCTalkingTo.gameObject.GetComponent<NPCs>().Relationship);
    }

    public void CompleteQuest()//pressing the completed quest button to sim this situation happening in the game
    {
        DialogueManager.Instance.StopAllCoroutines();

        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.CanMove = false;

        DialogueManager.Instance.CompletedQuestBool = true;
        DialogueManager.Instance.getQuestBool = false;

        DialogueManager.Instance.NameChecker(NPCTalkingTo.name, NPCTalkingTo.gameObject.GetComponent<NPCs>().Relationship);
    }

    public void Job()//pressing the job button to sim this situation happening in the game
    {
        DialogueManager.Instance.StopAllCoroutines();

        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.CanMove = false;

        DialogueManager.Instance.CompletedQuestBool = false;
        DialogueManager.Instance.getQuestBool = false;
        DialogueManager.Instance.JobBool = true;

        DialogueManager.Instance.NameChecker(NPCTalkingTo.gameObject.name, NPCTalkingTo.gameObject.GetComponent<NPCs>().Relationship);
    }

    public void Election()//pressing the election button to sim this situation happening in the game
    {
        DialogueManager.Instance.StopAllCoroutines();

        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.CanMove = false;

        DialogueManager.Instance.CompletedQuestBool = false;
        DialogueManager.Instance.getQuestBool = false;
        DialogueManager.Instance.JobBool = false;
        DialogueManager.Instance.ElectionBool = true;

        DialogueManager.Instance.NameChecker(NPCTalkingTo.gameObject.name, NPCTalkingTo.gameObject.GetComponent<NPCs>().Relationship);
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

        DialogueManager.Instance.NameChecker(NPCTalkingTo.gameObject.name, NPCTalkingTo.gameObject.GetComponent<NPCs>().Relationship);
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

        DialogueManager.Instance.NameChecker(NPCTalkingTo.gameObject.name, NPCTalkingTo.gameObject.GetComponent<NPCs>().Relationship);
    }



    private void OnTriggerExit(Collider other) //when the player leaves the box that they were colliding with in the first place
    {
        //in here I'm resetting the text that was in the textbox so it doesn't cause bugs. I'm also turning the canvas off so it's not always on screen, only when they're talking
        DialogueManager.Instance.NPCReplyText = null;
        DialogueManager.Instance.StopAllCoroutines();
        DialogueManager.Instance.canvas.enabled = false;
        DialogueManager.Instance.currentlyDisplayingText = 0;
    }
}
