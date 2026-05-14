using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class BoardButtonsScript : MonoBehaviour
{
    private bool isSelected = false;
    public Image selection;
    public (int x, int y) position;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void setSelected(bool selected)
    {
        selection.enabled = selected;
        isSelected = selected;
    }

    public void Click()
    {
        Board.boardScript.ClickTile(position);
    }

    // Update is called once per frame

}
