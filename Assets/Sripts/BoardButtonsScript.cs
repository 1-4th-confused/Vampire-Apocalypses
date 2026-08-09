using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Reflection;

/// <summary>
/// Handles interaction and selection for individual board tiles.
/// </summary>
public class BoardButtonsScript : MonoBehaviour
{
    /// <summary>
    /// Flag indicating if this tile is currently selected.
    /// </summary>
    public bool isSelected = false;
    public bool isMultiSelected;
    public bool isAttackPosibility;

    /// <summary>
    /// Image component for displaying selection highlight.
    /// </summary>
    public Image selection;
    public Image buttonImage;

    /// <summary>
    /// Grid position of this tile.
    /// </summary>
    public (int x, int y) position;

    /// <summary>
    /// Initializes the tile button.
    /// </summary>
    void Start()
    {
        this.gameObject.GetComponent<Animator>().SetInteger("SelectionType", 0);
    }

    /// <summary>
    /// Sets the selection state of the tile.
    /// </summary>
    /// <param name="selected">True to select, false to deselect.</param>


    public void setSelected(int color)
    {
        this.gameObject.GetComponent<Animator>().SetInteger("SelectionType", color);
        isSelected = color != 0;
    }
    public void SetImageActive(bool state)
    {
        if (state)
            buttonImage.color = new Color(1, 1, 1, 1);
        else
            buttonImage.color = new Color(1, 1, 1, 0);
    }

    /// <summary>
    /// Handles click events on the tile.
    /// </summary>
    public void Click()
    {
        Board.boardScript.ClickTile(position);
    }

    /// <summary>
    /// Handles pointer enter events for hover effects.
    /// </summary>
    public void OnPointerEnter()
    {
        if (isAttackPosibility && !isSelected)
        {
            Board.boardScript.HoveringTileAttack(position);
        }
    }

    /// <summary>
    /// Handles pointer exit events for hover effects.
    /// </summary>
    public void OnPointerExit()
    {
        if (!isSelected)
        {
            Board.boardScript.UnHoveringTileAttack(position);
        }
    }
}
