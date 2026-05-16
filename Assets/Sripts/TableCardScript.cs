using UnityEngine;
using System.Collections;
using System.Reflection;
using UnityEngine.UI;

/// <summary>
/// Manages the behavior of cards displayed on the table, such as deck cards.
/// </summary>
public class TableCardScript : MonoBehaviour
{
    /// <summary>
    /// Animator component for card animations.
    /// </summary>
    public Animator cardAnimator;

    /// <summary>
    /// Button component for card interaction.
    /// </summary>
    public Button CardButton;

    /// <summary>
    /// Flag indicating if the mouse is hovering over the card.
    /// </summary>
    public bool hovering;

    /// <summary>
    /// Flag indicating if the card is active and interactable.
    /// </summary>
    public bool active;

    /// <summary>
    /// Initializes the table card script.
    /// </summary>
    void Start()
    {

    }

    /// <summary>
    /// Handles click events on the card.
    /// </summary>
    public void click()
    {
        Board.boardScript.ClickDeck();
    }

    /// <summary>
    /// Handles pointer enter events for hover effects.
    /// </summary>
    public void OnPointerEnter()
    {
        if (active)
            cardAnimator.SetBool("hovering", true);
        hovering = true;
    }

    /// <summary>
    /// Handles pointer exit events for hover effects.
    /// </summary>
    public void OnPointerExit()
    {
        if (active)
            cardAnimator.SetBool("hovering", false);
        hovering = false;
    }

    /// <summary>
    /// Deactivates the card, making it non-interactable.
    /// </summary>
    public void Deactivate()
    {
        cardAnimator.SetBool("hovering", false);
        CardButton.interactable = false;
        active = false;
    }

    /// <summary>
    /// Activates the card, making it interactable.
    /// </summary>
    public void Activate()
    {
        CardButton.interactable = true;
        active = true;
        if (hovering)
        {
            cardAnimator.SetBool("hovering", true);
        }
    }

    /// <summary>
    /// Triggers the card removal animation and destroys the game object.
    /// </summary>
    public void RemoveCard()
    {
        cardAnimator.SetTrigger("slideOut");
        Destroy(this.gameObject, 0.5f);
    }
}
