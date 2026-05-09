using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


[System.Serializable]
public class CardData
{
    public double damage;
    public double defense;
    public string imageName;
}

public class CardManager : MonoBehaviour 
{
    public ArrayList cardTypes = new ArrayList();
    public ArrayList cardImages = new ArrayList();

    void Start()
    {
        string[] cardJsons = Directory.GetFiles(@"Assets\Resources", "*.json");

        foreach(string file in cardJsons)
        {
            string name = file;
            name = file.Substring(file.IndexOf(@"\")+1);
            name = name.Substring(name.IndexOf(@"\")+1);
            name = name.Substring(0,name.IndexOf(@"."));
            cardTypes.Add(JsonUtility.FromJson<CardData>(Resources.Load<TextAsset>(name).ToString()));
            cardImages.Add(Resources.Load<Sprite>(name+"Image").ToString());
        }
    }
}