using UnityEngine;
using UnityEngine.EventSystems;

public class CardScript : MonoBehaviour
{
    public CardData cardType;
    public Animator cardAnimator;
    public GameObject cardObject;
    public int index;
    public static PlayerHand playerHandScript;
    public bool selected = false;
    void Start()
    {
        cardObject.GetComponent<UnityEngine.UI.Image>().sprite = cardType.image;
    }
    public void SetSelection(bool selection)
    {
        selected = selection;
        cardAnimator.SetBool("selected", selection);
    }
    public void click()
    {
        playerHandScript.ClickCard(index);
    }

    public void OnPointerEnter()
    {
        cardAnimator.SetBool("hovering", true);
        this.gameObject.transform.SetAsLastSibling();
    }

    public void OnPointerExit()
    {
        cardAnimator.SetBool("hovering", false);
    }
}
