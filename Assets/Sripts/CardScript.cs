using UnityEngine;
using UnityEngine.EventSystems;

public class CardScript : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public CardData cardType;
    void Start()
    {
        this.gameObject.GetComponent<UnityEngine.UI.Image>().sprite = cardType.image;
    }

    public void OnSelect(BaseEventData eventData)
    {
        transform.position += Vector3.up * 50;
        
    }

    public void OnDeselect(BaseEventData eventData)
    {
        transform.position += Vector3.down * 50;
    }
}
