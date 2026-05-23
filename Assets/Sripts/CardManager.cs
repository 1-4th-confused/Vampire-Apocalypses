using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Serializable data structure for card and unit information.
/// </summary>
[System.Serializable]
public class CardData
{
    // General properties
    /// <summary>
    public string type;
    public string range;
    public float maxHealth;
    public Sprite image;
    public Sprite greyImage;
    public string name;
    public double damage;
    public double defense;
    public double magicDamage;
    public double meleeDamage;
    public double rangedDamage;
    public string description;
}

/// <summary>
/// Manages loading and storing card and unit data from JSON files.
/// </summary>
public class CardManager : MonoBehaviour
{
    /// <summary>
    /// Static list of card types loaded from JSON.
    /// </summary>
    public static ArrayList cardTypes = new ArrayList();

    /// <summary>
    /// Static list of unit types loaded from JSON.
    /// </summary>
    public static ArrayList unitTypes = new ArrayList();

    /// <summary>
    /// Loads card data from JSON files in Resources folder.
    /// </summary>
    public static void ReadCardJSON()
    {
        string[] cardJsons = Directory.GetFiles(@"Assets/Resources", "*.json");

        foreach (string file in cardJsons)
        {
            string name = file;
            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            {
                name = file.Substring(file.IndexOf(@"\") + 1);
                name = name.Substring(name.IndexOf(@"\") + 1);
                name = name.Substring(0, name.IndexOf(@"."));
            }
            else
            {
                name = file.Substring(file.IndexOf(@"/") + 1);
                name = name.Substring(name.IndexOf(@"/") + 1);
                name = name.Substring(0, name.IndexOf(@"."));
            }
            CardData cardDataTemp = JsonUtility.FromJson<CardData>(Resources.Load<TextAsset>(name).ToString());
            if (cardDataTemp.type == "card")
            {
                cardTypes.Add(cardDataTemp);
                cardDataTemp.image = Resources.Load<Sprite>(name + "Image");
            }
        }
    }

    /// <summary>
    /// Loads unit data from JSON files in Resources folder.
    /// </summary>
    public static void ReadUnitsJSON()
    {
        string[] cardJsons = Directory.GetFiles(@"Assets/Resources", "*.json");

        foreach (string file in cardJsons)
        {
            string name = file;
            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            {
                name = file.Substring(file.IndexOf(@"\") + 1);
                name = name.Substring(name.IndexOf(@"\") + 1);
                name = name.Substring(0, name.IndexOf(@"."));
            }
            else
            {
                name = file.Substring(file.IndexOf(@"/") + 1);
                name = name.Substring(name.IndexOf(@"/") + 1);
                name = name.Substring(0, name.IndexOf(@"."));
            }
            CardData unitDataTemp = JsonUtility.FromJson<CardData>(Resources.Load<TextAsset>(name).ToString());
            if (unitDataTemp.type == "unit")
            {
                unitTypes.Add(unitDataTemp);
                unitDataTemp.image = Resources.Load<Sprite>(name + "Image");
                unitDataTemp.greyImage = Resources.Load<Sprite>(name + "ImageGrey");
                // ((CardData) (unitTypes[unitTypes.Count-1])).image = Resources.Load<Sprite>(name+"Image");
            }
        }
    }
}