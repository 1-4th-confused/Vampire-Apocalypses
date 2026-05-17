using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Manages the behavior, animation, and interaction of individual cards in the player's hand.
/// </summary>
public class CardScript : MonoBehaviour
{
    /// <summary>
    /// Data for this card.
    /// </summary>
    public CardData cardType;

    /// <summary>
    /// Animator component for card animations.
    /// </summary>
    public Animator cardAnimator;

    /// <summary>
    /// GameObject containing the card's image.
    /// </summary>
    public GameObject cardObject;

    /// <summary>
    /// Index of this card in the hand.
    /// </summary>
    public int index;
    
    /// <summary>
    /// Reference to the player's hand script.
    /// </summary>
    public static PlayerHand playerHandScript;

    /// <summary>
    /// Flag indicating if the card is selected.
    /// </summary>
    public bool selected = false;

    /// <summary>
    /// Flag indicating if the card is moving.
    /// </summary>
    public bool moved;

    /// <summary>
    /// Time when the card started moving.
    /// </summary>
    private double timeMoved;

    /// <summary>
    /// Current position of the card.
    /// </summary>
    public Vector3 CurrentPos;

    /// <summary>
    /// Current rotation of the card.
    /// </summary>
    public Vector3 CurrentRot;

    /// <summary>
    /// Target position for movement.
    /// </summary>
    public Vector3 wantedPos;

    /// <summary>
    /// Target rotation for movement.
    /// </summary>
    public Vector3 wantedRot;

    /// <summary>
    /// Initializes the card's image.
    /// </summary>
    void Start()
    {
        cardObject.GetComponent<UnityEngine.UI.Image>().sprite = cardType.image;
    }

    /// <summary>
    /// Updates the card's position and rotation during movement animation.
    /// </summary>
    void Update()
    {
        if (moved)
        {
            // Calculate normalized time along the movement path with easing
            double timeAlongPathNormalized = 5 * (Time.realtimeSinceStartupAsDouble - timeMoved);
            timeAlongPathNormalized = System.Math.Pow(timeAlongPathNormalized, 1.0 / 3.0) * (2.0 / 3.0) + timeAlongPathNormalized * (1.0 / 3.0);
            if (timeAlongPathNormalized > 1)
            {
                // Movement complete
                this.gameObject.transform.position = wantedPos;
                this.gameObject.transform.rotation = Quaternion.Euler(wantedRot);
                moved = false;
            }
            else
            {
                // Interpolate position
                this.gameObject.transform.position = new Vector3(
                    (float)(CurrentPos.x + (timeAlongPathNormalized * (wantedPos.x - CurrentPos.x))),
                    (float)(CurrentPos.y + (timeAlongPathNormalized * (wantedPos.y - CurrentPos.y))),
                    (float)(CurrentPos.z + (timeAlongPathNormalized * (wantedPos.z - CurrentPos.z)))
                );

                // Handle rotation interpolation, accounting for angle wrapping
                float wantedZ = wantedRot.z;
                float currentZ = CurrentRot.z;
                if (wantedZ > 180)
                {
                    wantedZ -= 360;
                }
                if (currentZ > 180)
                {
                    currentZ -= 360;
                }
                float tempZrot = (float)(currentZ + (timeAlongPathNormalized * (wantedZ - currentZ)));

                this.gameObject.transform.rotation = Quaternion.Euler(
                    (float)(CurrentRot.x + (timeAlongPathNormalized * (wantedRot.x - CurrentRot.x))),
                    (float)(CurrentRot.y + (timeAlongPathNormalized * (wantedRot.y - CurrentRot.y))),
                    tempZrot
                );
            }
        }
    }

    /// <summary>
    /// Initiates movement to a new position and rotation.
    /// </summary>
    /// <param name="wantedPostion">Target position.</param>
    /// <param name="wantedrotation">Target rotation.</param>
    public void MoveCard(Vector3 wantedPostion, Vector3 wantedrotation)
    {
        CurrentPos = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        CurrentRot = this.gameObject.transform.rotation.eulerAngles;

        wantedPos = wantedPostion;
        wantedRot = wantedrotation;
        moved = true;
        timeMoved = Time.realtimeSinceStartupAsDouble;
    }

    /// <summary>
    /// Sets the selection state of the card.
    /// </summary>
    /// <param name="selection">True to select, false to deselect.</param>
    public void SetSelection(bool selection)
    {
        selected = selection;
        cardAnimator.SetBool("selected", selection);
    }

    /// <summary>
    /// Handles click events on the card.
    /// </summary>
    public void click()
    {
        playerHandScript.ClickCard(index);
        Board.boardScript.UpdatePeiceInteractability();
    }

    /// <summary>
    /// Handles pointer enter events for hover effects.
    /// </summary>
    public void OnPointerEnter()
    {
        cardAnimator.SetBool("hovering", true);
        this.gameObject.transform.SetAsLastSibling();
        InfoTileScript.infoTileScript.MoveOut(cardType);
    }

    /// <summary>
    /// Handles pointer exit events for hover effects.
    /// </summary>
    public void OnPointerExit()
    {
        cardAnimator.SetBool("hovering", false);
        InfoTileScript.infoTileScript.MoveIn(cardType);
    }
}
