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
    private Image myImageComponent;
    void Start()
    {
        myImageComponent = GetComponent<Image>();

        previousCards = new string[currentCards.Length];
        Array.Copy(currentCards, previousCards, currentCards.Length);
        
        RefreshHand();
    }
    
    void Update()
    {      
        if(player.currentDeck > 0 && player.currentDeck <= deckIndicators.Length) {
            myImageComponent.sprite = deckIndicators[player.currentDeck-1];
        }

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
    public void draw(){
        
    }

    public void place(){

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
                GameObject prefab = cardImages.FirstOrDefault(obj => obj.name == currentCards[i]);
                if (prefab != null) {
                    GameObject newCard = Instantiate(prefab, cardTransforms[i].position, cardTransforms[i].rotation);
                    newCard.transform.SetParent(this.transform, true);
                    newCard.name = prefab.name;
                }
            }
        }
    }
}
