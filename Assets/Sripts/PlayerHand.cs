using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;


public class PlayerHand : MonoBehaviour
{

    [SerializeField] 
    private List<Transform> cardTransforms = new List<Transform>();

    [SerializeField] private Sprite[] deckIndicators;

    [SerializeField] private GameObject[] cardImages;

    [SerializeField] private string[] currentCards;

    private string[] previousCards = new string[5];

    [SerializeField] public PlayerScript player;
    public GameObject cardPrefab;
    void Start()
    {

        previousCards = new string[currentCards.Length];
        Array.Copy(currentCards, previousCards, currentCards.Length);
        
        CardManager.ReadCardJSON();

        RefreshHand();
    }
    
    void Update()
    {      

        bool hasChanged = false;
        for (int i = 0; i < currentCards.Length; i++) {
            if (currentCards[i] != previousCards[i]) {
                hasChanged = true;
                break;
            }
        }

        if (hasChanged) {
            RefreshHand();
            Array.Copy(currentCards, previousCards, currentCards.Length);
        }
    }
    public void RefreshHand()
    {
        foreach (Transform child in transform) {
            if (!cardTransforms.Contains(child)) {
                Destroy(child.gameObject);
            }
        }

        for (int i = 0; i < currentCards.Length; i++) {
            if (!string.IsNullOrEmpty(currentCards[i])) {
                // GameObject cardPrefab = cardImages.FirstOrDefault(obj => obj.name == currentCards[i]);
                if (cardPrefab != null) {
                    GameObject newCard = Instantiate(cardPrefab, cardTransforms[i].position, cardTransforms[i].rotation);
                    newCard.transform.SetParent(this.transform, true);
                    CardData tempCard = null;
                    for(int j = 0;j<CardManager.cardTypes.Count;j++)
                        if(((CardData) (CardManager.cardTypes[j])).name == currentCards[i])
                            tempCard = (CardData)(CardManager.cardTypes[j]);
                    if(tempCard!=null)
                        newCard.GetComponent<CardScript>().cardType = tempCard;
                    else
                        newCard.GetComponent<CardScript>().cardType = (CardData)(CardManager.cardTypes[0]);
                }
            }
        }
    }
}
