using UnityEngine;

public class CardScript : MonoBehaviour
{
    public CardData cardType;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.gameObject.GetComponent<UnityEngine.UI.Image>().sprite = cardType.image;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
