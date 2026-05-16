using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.InputSystem;

/// <summary>
/// Manages the player's hand of cards, including layout, selection, and animations.
/// </summary>
public class PlayerHand : MonoBehaviour
{
    /// <summary>
    /// Array of sprites for deck indicators.
    /// </summary>
    [SerializeField] private Sprite[] deckIndicators;

    /// <summary>
    /// List of current card game objects in the hand.
    /// </summary>
    private List<GameObject> currentCardObjs;

    /// <summary>
    /// List of current card data in the hand.
    /// </summary>
    public List<CardData> currentCards;

    /// <summary>
    /// Prefab for instantiating cards.
    /// </summary>
    public GameObject cardPrefab;

    /// <summary>
    /// Currently selected card game object.
    /// </summary>
    public GameObject SelectedCard = null;

    /// <summary>
    /// Spacing between cards in the hand.
    /// </summary>
    public double cardSpacing = 10;

    /// <summary>
    /// Maximum rotation angle for cards in the hand.
    /// </summary>
    public float maxcardRotation = 30;

    /// <summary>
    /// Maximum depth offset for cards in the hand.
    /// </summary>
    public double maxcarddepth = -50;

    /// <summary>
    /// Parent transform for hand cards.
    /// </summary>
    public GameObject handParent;

    /// <summary>
    /// Flag indicating if cards have been moved down.
    /// </summary>
    public bool cardsMovedDown = false;

    /// <summary>
    /// Animator for hand animations.
    /// </summary>
    public Animator handAnimator;

    /// <summary>
    /// Threshold for moving cards down based on mouse Y position.
    /// </summary>
    public double threasholdForCardsToMoveDown;

    /// <summary>
    /// Threshold for moving cards up based on mouse Y position.
    /// </summary>
    public double threasholdForCardsToMoveUp;

    /// <summary>
    /// Initializes the player hand with initial cards.
    /// </summary>
    void Start()
    {
        CardScript.playerHandScript = this;
        CardManager.ReadCardJSON();
        currentCardObjs = new List<GameObject>();
        currentCards = new List<CardData>();
        currentCards.Add((CardData)CardManager.cardTypes[0]);
        currentCards.Add((CardData)CardManager.cardTypes[1]);
        currentCards.Add((CardData)CardManager.cardTypes[0]);
        currentCards.Add((CardData)CardManager.cardTypes[1]);
        currentCards.Add((CardData)CardManager.cardTypes[0]);
        CreateHand();
    }

    /// <summary>
    /// Updates the hand each frame, handling card movement animations.
    /// </summary>
    void Update()
    {
        HandleCardsMoveingDown();
        if(SelectedCard != null){
            Board.boardScript.UpdatePeiceInteractability();
        }
    }
    public void placeCard(){
    }

    /// <summary>
    /// Adds a new card to the hand and repositions existing cards.
    /// </summary>
    /// <param name="card">The card data to add.</param>
    public void AddCardToHand(CardData card)
    {
        // Reposition existing cards
        for (int i = 0; i < currentCardObjs.Count; i++)
        {
            currentCardObjs[i].GetComponent<CardScript>().MoveCard(handParent.transform.position + new Vector3(
                (float)(
                    (0.5 + i - (currentCards.Count + 1f) / 2.0f) * cardSpacing
                ),
                (float)(
                    maxcarddepth
                    *
                    (2f * (0.5f + i - (currentCards.Count + 1f) / 2.0f) / (currentCards.Count + 1f))
                    *
                    (2f * (0.5f + i - (currentCards.Count + 1f) / 2.0f) / (currentCards.Count + 1f))
                ),
                0f
            ),
            new Vector3(0f, 0f, -maxcardRotation * 2f * (0.5f + i - (currentCards.Count + 1f) / 2.0f) / (currentCards.Count + 1f)));
        }

        // Instantiate new card
        GameObject newCard = Instantiate(
            cardPrefab,
            handParent.transform.position + new Vector3(
                (float)(
                    (0.5f + currentCards.Count - (currentCards.Count + 1f) / 2.0f) * cardSpacing
                ),
                (float)(
                    maxcarddepth
                    *
                    (2f * (0.5f + currentCards.Count - (currentCards.Count + 1f) / 2.0f) / (currentCards.Count + 1))
                    *
                    (2f * (0.5f + currentCards.Count - (currentCards.Count + 1f) / 2.0f) / (currentCards.Count + 1))
                ),
                0f),
            Quaternion.identity,
            handParent.transform
        );
        newCard.transform.Rotate(new Vector3(0f, 0f, -maxcardRotation * 2f * (0.5f + currentCards.Count - (currentCards.Count + 1) / 2.0f) / (currentCards.Count + 1)));
        newCard.transform.SetParent(handParent.transform, true);
        newCard.GetComponent<CardScript>().index = currentCards.Count;
        currentCardObjs.Add(newCard);
        currentCards.Add(card);
        newCard.GetComponent<CardScript>().cardType = card;
    }

    /// <summary>
    /// Creates the initial hand layout by instantiating card objects.
    /// </summary>
    public void CreateHand()
    {
        // Destroy existing cards
        for (int i = 0; i < currentCardObjs.Count; i++)
        {
            if (currentCardObjs[i] != null)
            {
                Destroy(currentCardObjs[i]);
            }
        }
        currentCardObjs.Clear();

        // Instantiate new cards
        for (int i = 0; i < currentCards.Count; i++)
        {
            if (currentCards[i] != null)
            {
                GameObject newCard = Instantiate(
                    cardPrefab,
                    handParent.transform.position + new Vector3(
                        (float)(
                            (0.5 + i - currentCards.Count / 2.0f) * cardSpacing
                        ),
                        (float)(
                            maxcarddepth
                            *
                            (2f * (0.5f + i - currentCards.Count / 2.0f) / currentCards.Count)
                            *
                            (2f * (0.5f + i - currentCards.Count / 2.0f) / currentCards.Count)
                        ),
                        0f),
                    Quaternion.identity,
                    handParent.transform
                );
                newCard.transform.Rotate(new Vector3(0f, 0f, -maxcardRotation * 2f * (0.5f + i - currentCards.Count / 2.0f) / currentCards.Count));
                newCard.transform.SetParent(handParent.transform, true);
                newCard.GetComponent<CardScript>().index = i;
                currentCardObjs.Add(newCard);
                newCard.GetComponent<CardScript>().cardType = currentCards[i];
            }
        }
    }

    /// <summary>
    /// Handles card selection/deselection when a card is clicked.
    /// </summary>
    /// <param name="card">Index of the clicked card.</param>
    public void ClickCard(int card)
    {
        if (!currentCardObjs[card].GetComponent<CardScript>().selected)
        {
            // Select the card and deselect others
            for (int i = 0; i < currentCards.Count; i++)
            {
                if (i != card)
                {
                    currentCardObjs[i].GetComponent<CardScript>().SetSelection(false);
                }
                else
                {
                    currentCardObjs[i].GetComponent<CardScript>().SetSelection(true);
                }
            }
            SelectedCard = currentCardObjs[card];
        }
        else
        {
            // Deselect all cards
            for (int i = 0; i < currentCards.Count; i++)
            {
                currentCardObjs[i].GetComponent<CardScript>().SetSelection(false);
            }
            SelectedCard = null;
        }
    }

    /// <summary>
    /// Handles moving cards down or up based on mouse position.
    /// </summary>
    public void HandleCardsMoveingDown()
    {
        if (Mouse.current.position.ReadValue().y / Screen.height > threasholdForCardsToMoveDown && SelectedCard != null && !cardsMovedDown)
        {
            if (handAnimator != null)
                handAnimator.SetBool("cardsDown", true);
                cardsMovedDown = true;
        }
        else if (Mouse.current.position.ReadValue().y / Screen.height < threasholdForCardsToMoveUp && cardsMovedDown)
        {
            if (handAnimator != null)
                handAnimator.SetBool("cardsDown", false);
                cardsMovedDown = false;
        }
    }
}
