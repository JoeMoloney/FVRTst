using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.CanMove = false;
        TextAnimation.Instance.canvas.enabled = true;
        Debug.Log(other.gameObject.GetComponent<NPCs>().Relationship);
        if (other.gameObject.CompareTag("Peter"))
        {
            TextAnimation.Instance.NameChecker(other.name, other.gameObject.GetComponent<NPCs>().Relationship);
            other.gameObject.GetComponent<NPCs>().Relationship++;
        }
        else if (other.gameObject.CompareTag("Flynn"))
        {
            TextAnimation.Instance.NameChecker(other.name, other.gameObject.GetComponent<NPCs>().Relationship);
            other.gameObject.GetComponent<NPCs>().Relationship++;
        }
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
