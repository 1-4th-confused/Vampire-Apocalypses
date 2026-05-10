using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


[System.Serializable]
public class CardData
{
    public double damage;
    public double defense;
    public string name;
    public Sprite image;
}

public class CardManager : MonoBehaviour 
{
    public static ArrayList cardTypes = new ArrayList();

    public static void ReadJSON()
    {
        string[] cardJsons = Directory.GetFiles(@"Assets/Resources", "*.json");

        foreach(string file in cardJsons)
        {
            string name = file;
            name = file.Substring(file.IndexOf(@"\")+1);
            name = name.Substring(name.IndexOf(@"\")+1);
            name = name.Substring(0,name.IndexOf(@"."));
            cardTypes.Add(
                JsonUtility.FromJson<CardData>(Resources.Load<TextAsset>(name).ToString())
            );
            ((CardData) (cardTypes[cardTypes.Count-1])).image = Resources.Load<Sprite>(name+"Image");
        }
    }
}