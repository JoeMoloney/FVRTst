using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        TextAnimation.Instance.canvas.enabled = true;
        if (other.gameObject.CompareTag("Peter"))
        {
            TextAnimation.Instance.NameChecker(other.name);
        }
        else if (other.gameObject.CompareTag("Flynn"))
        {
            TextAnimation.Instance.NameChecker(other.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        TextAnimation.Instance.StopAllCoroutines();
        TextAnimation.Instance.canvas.enabled = false;
        for(int i = 0; i<TextAnimation.Instance.goatText.Length; i++)
        {
            TextAnimation.Instance.goatText[i] = null;
        }
    }
}
