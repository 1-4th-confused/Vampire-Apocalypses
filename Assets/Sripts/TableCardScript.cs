using UnityEngine;
using System.Collections;
using System.Reflection;
using UnityEngine.UI;

public class TableCardScript : MonoBehaviour
{
    public Animator cardAnimator;
    public Button CardButton;
    public bool hovering;
    public bool active;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    public void click()
    {
        Board.boardScript.ClickDeck();
        Debug.Log("clicked card!");
    }
    public void OnPointerEnter()
    {
        if (active)
            cardAnimator.SetBool("hovering", true);
        hovering = true;
    }

    public void OnPointerExit()
    {
        if (active)
            cardAnimator.SetBool("hovering", false);
        hovering = false;
    }

    public void Deactivate()
    {
        cardAnimator.SetBool("hovering", false);
        CardButton.interactable = false;
        active = false;
    }
    public void Activate()
    {
        CardButton.interactable = true;
        active = true;
        if (hovering)
        {
            cardAnimator.SetBool("hovering", true);
        }
    }
    public void RemoveCard()
    {
        cardAnimator.SetTrigger("slideOut");
        Destroy(this.gameObject, 0.5f);
    }
}
