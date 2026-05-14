using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.InputSystem;


public class PlayerHand : MonoBehaviour
{
    [SerializeField] private Sprite[] deckIndicators;
    private List<GameObject> currentCardObjs;
    public List<CardData> currentCards;
    public GameObject cardPrefab;
    public GameObject SelectedCard = null;
    public double cardSpacing = 10;
    public float maxcardRotation = 30;
    public double maxcarddepth = -50;
    public GameObject handParent;
    public bool cardsMovedDown = false;
    public Animator handAnimator;
    public double threasholdForCardsToMoveDown;
    public double threasholdForCardsToMoveUp;
    void Start()
    {
        CardScript.playerHandScript = this;
        CardManager.ReadCardJSON();
        currentCardObjs = new List<GameObject>();
        currentCards = new List<CardData>();
        currentCards.Add((CardData) CardManager.cardTypes[0]);
        currentCards.Add((CardData) CardManager.cardTypes[1]);
        currentCards.Add((CardData) CardManager.cardTypes[0]);
        currentCards.Add((CardData) CardManager.cardTypes[1]);
        currentCards.Add((CardData) CardManager.cardTypes[0]);
        RefreshHand();
    }
    
    void Update()
    {      
        HandleCardsMoveingDown();
    }
    public void RefreshHand()
    {
        for (int i = 0;i < currentCardObjs.Count;i++) {
            if (currentCardObjs[i]!=null) {
                Destroy(currentCardObjs[i]);
            }
        }
        currentCardObjs.Clear();
        for (int i = 0; i < currentCards.Count; i++) {
            if (currentCards[i] != null) {
                GameObject newCard = Instantiate(
                    cardPrefab,
                    handParent.transform.position + new Vector3(
                        (float) (
                            (0.5+i - currentCards.Count/2.0f) * cardSpacing
                        ),
                        (float) (
                            maxcarddepth
                            *
                            (2f * (0.5f+i - currentCards.Count/2.0f) / currentCards.Count)
                            * 
                            (2f * (0.5f+i - currentCards.Count/2.0f) / currentCards.Count)
                        ),
                        0f),
                    Quaternion.identity,
                    handParent.transform
                );
                newCard.transform.Rotate(new Vector3(0f,0f,-maxcardRotation * 2f * (0.5f + i - currentCards.Count/2.0f) / currentCards.Count));
                newCard.transform.SetParent(handParent.transform, true);
                newCard.GetComponent<CardScript>().index = i;
                currentCardObjs.Add(newCard);
                newCard.GetComponent<CardScript>().cardType = currentCards[i];
                
            }
        }
    }

    public void ClickCard(int card) {
        if (!currentCardObjs[card].GetComponent<CardScript>().selected){
            for (int i = 0; i < currentCards.Count;i++) {
                if (i != card) {
                    currentCardObjs[i].GetComponent<CardScript>().SetSelection(false);
                } else {
                    currentCardObjs[i].GetComponent<CardScript>().SetSelection(true);
                }
            }
            SelectedCard = currentCardObjs[card];
        } else {
            for (int i = 0; i < currentCards.Count;i++) {
                currentCardObjs[i].GetComponent<CardScript>().SetSelection(false);
            }
            SelectedCard = null;
        }
        
    }

    public void HandleCardsMoveingDown(){
        if (Mouse.current.position.ReadValue().y/Screen.height > threasholdForCardsToMoveDown && SelectedCard != null && !cardsMovedDown){
            handAnimator.SetBool("cardsDown",true);
            cardsMovedDown = true;
        } else if (Mouse.current.position.ReadValue().y/Screen.height < threasholdForCardsToMoveUp && cardsMovedDown){
            handAnimator.SetBool("cardsDown",false);
            cardsMovedDown = false;
        }
    }
}
