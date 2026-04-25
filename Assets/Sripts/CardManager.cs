using UnityEngine;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class CardData
{
    public double damage;
    public double defense;
    public string imagePath;
}

public class CardManager : MonoBehaviour 
{
    [SerializeField] private string cardName;
    
    private CardData currentCard;
    private Sprite cardSprite;

    void Start()
    {
        LoadCardData(cardName);
    }

    void LoadCardData(string name)
    {
        // 1. Define the path to the JSON file
        // Using Resources.Load is easiest for cross-platform builds
        string jsonFilePath = "Cards/JSON/" + name; 
        TextAsset targetFile = Resources.Load<TextAsset>(jsonFilePath);

        if (targetFile != null)
        {
            // 2. Deserialize the JSON
            currentCard = JsonUtility.FromJson<CardData>(targetFile.text);
            Debug.Log($"Loaded {name}: Damage {currentCard.damage}, Defense {currentCard.defense}");

            // 3. Load the Image based on the path stored inside the JSON
            // Note: imagePath in JSON should not include the extension (.png)
            string spritePath = "Cards/Images/" + currentCard.imagePath;
            cardSprite = Resources.Load<Sprite>(spritePath);

            if (cardSprite != null)
            {
                Debug.Log("Card Sprite successfully loaded!");
                // If you have a SpriteRenderer or Image component, assign it here:
                // GetComponent<SpriteRenderer>().sprite = cardSprite;
            }
            else
            {
                Debug.LogError($"Sprite not found at: {spritePath}");
            }
        }
        else
        {
            Debug.LogError($"JSON file not found for: {name} at {jsonFilePath}");
        }
    }
}