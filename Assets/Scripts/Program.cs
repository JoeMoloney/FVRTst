using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Program : MonoBehaviour
{
    //Path to JSON file
    [SerializeField] TextAsset keyedJson = null; //keyedJSON
    [SerializeField] Text uiText = null;
    private string[] repliesForFlynn;
    private int amount = 0;

    private void Start()
    {
        StreamReader file = File.OpenText(AssetDatabase.GetAssetPath(keyedJson)); //Keyed JSON File Path
        JsonTextReader reader = new JsonTextReader(file); //Keyed JSON File Reader
        
        JObject o2 = (JObject)JToken.ReadFrom(reader); //Read JSON Data To JObject
        uiText.text = ("Name: " + (string)o2.SelectToken("Flynn.Friend"));

        foreach (string reply in o2.SelectToken("Flynn"))
        {
            amount++;
        }

        for(int i=0; i < amount; i++)
        {
            //repliesForFlynn[i] = o2.SelectToken("Flynn").First;
            Debug.Log("" + i + ": " + repliesForFlynn[i]);
        }
    }

}

