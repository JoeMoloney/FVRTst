using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OtherWay : MonoBehaviour
{
    [SerializeField] TextAsset JsonFile = null;


    private void Start()
    {
        CreateFromJSON(JsonFile.text);
        Debug.Log("" + CreateFromJSON(JsonFile.text));
    }

    public OtherWay CreateFromJSON(string JsonString)
    {
        return JsonUtility.FromJson<OtherWay>(JsonString);
    }
}