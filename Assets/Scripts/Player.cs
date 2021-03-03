using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private GameObject NPCTalkingTo = null;
    
    private void OnTriggerEnter(Collider other)
    {
        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.CanMove = false;
        TextAnimation.Instance.canvas.enabled = true;
        Debug.Log(other.gameObject.GetComponent<NPCs>().Relationship);
        if (other.gameObject.CompareTag("Peter"))
        {
            NPCTalkingTo = other.gameObject;
            TextAnimation.Instance.NameChecker(other.name, other.gameObject.GetComponent<NPCs>().Relationship);
            other.gameObject.GetComponent<NPCs>().Relationship++;
        }
        else if (other.gameObject.CompareTag("Flynn"))
        {
            TextAnimation.Instance.NameChecker(other.name, other.gameObject.GetComponent<NPCs>().Relationship);
            other.gameObject.GetComponent<NPCs>().Relationship++;
        }
    }

    public void GetQuest()
    {
        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.CanMove = false;

        TextAnimation.Instance.getQuestBool = true;

        TextAnimation.Instance.NameChecker(NPCTalkingTo.name, NPCTalkingTo.gameObject.GetComponent<NPCs>().Relationship);
    }

    public void CompleteQuest()
    {
        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.CanMove = false;

        TextAnimation.Instance.CompletedQuestBool = true;
        TextAnimation.Instance.getQuestBool = false;
        TextAnimation.Instance.JobBool = false;
        TextAnimation.Instance.ElectionBool = false;

        TextAnimation.Instance.NameChecker(NPCTalkingTo.name, NPCTalkingTo.gameObject.GetComponent<NPCs>().Relationship);
    }

    public void Job()
    {
        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.CanMove = false;

        TextAnimation.Instance.CompletedQuestBool = false;
        TextAnimation.Instance.getQuestBool = false;
        TextAnimation.Instance.JobBool = true;
        TextAnimation.Instance.ElectionBool = false;

        TextAnimation.Instance.NameChecker(NPCTalkingTo.name, NPCTalkingTo.gameObject.GetComponent<NPCs>().Relationship);
    }

    public void Election()
    {
        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.CanMove = false;

        TextAnimation.Instance.CompletedQuestBool = false;
        TextAnimation.Instance.getQuestBool = false;
        TextAnimation.Instance.JobBool = false;
        TextAnimation.Instance.ElectionBool = true;

        TextAnimation.Instance.NameChecker(NPCTalkingTo.name, NPCTalkingTo.gameObject.GetComponent<NPCs>().Relationship);
    }



    private void OnTriggerExit(Collider other)
    {
        TextAnimation.Instance.StopAllCoroutines();
        TextAnimation.Instance.canvas.enabled = false;
        TextAnimation.Instance.currentlyDisplayingText = 0;
        for (int i = 0; i<TextAnimation.Instance.goatText.Length; i++)
        {
            TextAnimation.Instance.goatText[i] = null;
            
        }
    }
}
