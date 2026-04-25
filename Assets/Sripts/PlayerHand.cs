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

    [SerializeField] public PlayerScript player;
    private Image myImageComponent;
    void Start()
    {
        myImageComponent = GetComponent<Image>();
    }

    void Update()
    {      
        if(player.currentDeck > 0 && player.currentDeck <= deckIndicators.Length){
            myImageComponent.sprite = deckIndicators[player.currentDeck-1];
        }
        for(int i = 0; i < 5; i++){
            if(currentCards[i] != null){
                GameObject card = cardImages.FirstOrDefault(obj => obj.name == currentCards[i]);

                if (card != null)
                {
                    card.transform.position = cardTransforms[i].position;
                    card.transform.rotation = cardTransforms[i].rotation;
                }
            }
        }
    }
}
