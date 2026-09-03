using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages the player's deck of cards.
/// </summary>
public class PlayerDeck : MonoBehaviour
{
    /// <summary>
    /// List of game objects representing cards in the deck.
    /// </summary>
    List<GameObject> deck = new List<GameObject>();

    /// <summary>
    /// List of possible card prefabs that can be in the deck.
    /// </summary>
    [SerializeField] List<GameObject> possibleCards = new List<GameObject>();

    /// <summary>
    /// Initializes the player deck.
    /// </summary>
    void Start()
    {

    }

    /// <summary>
    /// Updates the player deck each frame.
    /// </summary>
    void Update()
    {

    }
}
