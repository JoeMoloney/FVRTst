using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private GameObject NPCTalkingTo = null;
    
    private void OnTriggerEnter(Collider other)
    {
        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.CanMove = false;
        DialogueManager.Instance.canvas.enabled = true;
        Debug.Log(other.gameObject.GetComponent<NPCs>().Relationship);
        if (other.gameObject.CompareTag("Peter"))
        {
            NPCTalkingTo = other.gameObject;
            DialogueManager.Instance.NameChecker(other.name, other.gameObject.GetComponent<NPCs>().Relationship);
            other.gameObject.GetComponent<NPCs>().Relationship++;
        }
        else if (other.gameObject.CompareTag("Flynn"))
        {
            NPCTalkingTo = other.gameObject;
            DialogueManager.Instance.NameChecker(other.name, other.gameObject.GetComponent<NPCs>().Relationship);
            other.gameObject.GetComponent<NPCs>().Relationship++;
        }
    }

    public void GetQuest()
    {
        DialogueManager.Instance.StopAllCoroutines();

        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.CanMove = false;

        DialogueManager.Instance.getQuestBool = true;

        DialogueManager.Instance.NameChecker(NPCTalkingTo.name, NPCTalkingTo.gameObject.GetComponent<NPCs>().Relationship);
    }

    public void CompleteQuest()
    {
        DialogueManager.Instance.StopAllCoroutines();

        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.CanMove = false;

        DialogueManager.Instance.CompletedQuestBool = true;
        DialogueManager.Instance.getQuestBool = false;

        DialogueManager.Instance.NameChecker(NPCTalkingTo.name, NPCTalkingTo.gameObject.GetComponent<NPCs>().Relationship);
    }

    public void Job()
    {
        DialogueManager.Instance.StopAllCoroutines();

        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.CanMove = false;

        DialogueManager.Instance.CompletedQuestBool = false;
        DialogueManager.Instance.getQuestBool = false;
        DialogueManager.Instance.JobBool = true;

        DialogueManager.Instance.NameChecker(NPCTalkingTo.gameObject.name, NPCTalkingTo.gameObject.GetComponent<NPCs>().Relationship);
    }

    public void Election()
    {
        DialogueManager.Instance.StopAllCoroutines();

        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.CanMove = false;

        DialogueManager.Instance.CompletedQuestBool = false;
        DialogueManager.Instance.getQuestBool = false;
        DialogueManager.Instance.JobBool = false;
        DialogueManager.Instance.ElectionBool = true;

        DialogueManager.Instance.NameChecker(NPCTalkingTo.gameObject.name, NPCTalkingTo.gameObject.GetComponent<NPCs>().Relationship);
    }

    public void BuildingQ()
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

    public void BuildingComplete()
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



    private void OnTriggerExit(Collider other)
    {
        DialogueManager.Instance.NPCReplyText = null;
        DialogueManager.Instance.StopAllCoroutines();
        DialogueManager.Instance.canvas.enabled = false;
        DialogueManager.Instance.currentlyDisplayingText = 0;
    }
}
