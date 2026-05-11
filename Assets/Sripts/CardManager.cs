using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class CardData
{
    //general
    public string type;
    public Sprite image;
    public string name;
    //for cards
    public double damage;
    public double defense;
    //for units
    public double magicDamage;
    public double melaeDamage;
    public double rangedDamage;
}

public class CardManager : MonoBehaviour 
{
    public static ArrayList cardTypes = new ArrayList();
    public static ArrayList unitTypes = new ArrayList();

    public static void ReadCardJSON()
    {
        string[] cardJsons = Directory.GetFiles(@"Assets/Resources", "*.json");

        foreach(string file in cardJsons)
        {
            string name = file;
            name = file.Substring(file.IndexOf(@"\")+1);
            name = name.Substring(name.IndexOf(@"\")+1);
            name = name.Substring(0,name.IndexOf(@"."));
            CardData cardDataTemp = JsonUtility.FromJson<CardData>(Resources.Load<TextAsset>(name).ToString());
            if (cardDataTemp.type == "card")
            {
                cardTypes.Add(
                    cardDataTemp
                );
                ((CardData) (cardTypes[cardTypes.Count-1])).image = Resources.Load<Sprite>(name+"Image");
            }
        }
    }

    public static void ReadUnitsJSON()
    {
        string[] cardJsons = Directory.GetFiles(@"Assets/Resources", "*.json");

        foreach(string file in cardJsons)
        {
            string name = file;
            name = file.Substring(file.IndexOf(@"\")+1);
            name = name.Substring(name.IndexOf(@"\")+1);
            name = name.Substring(0,name.IndexOf(@"."));
            CardData unitDataTemp = JsonUtility.FromJson<CardData>(Resources.Load<TextAsset>(name).ToString());
            if (unitDataTemp.type == "unit")
            {
                unitTypes.Add(
                    unitDataTemp
                );
                unitDataTemp.image = Resources.Load<Sprite>(name+"Image");
                // ((CardData) (unitTypes[unitTypes.Count-1])).image = Resources.Load<Sprite>(name+"Image");
            }
        }
    }
}