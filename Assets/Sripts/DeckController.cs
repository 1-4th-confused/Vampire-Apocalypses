using UnityEngine;
using UnityEngine.UI;

public class DeckController : MonoBehaviour 
{

    [SerializeField] private Sprite[] images;

    [SerializeField] public PlayerScript player;

    private Image myImageComponent;

    void Start()
    {
        myImageComponent = GetComponent<Image>();
    }

    void Update()
    {      
        if(player.currentDeck > 0 && player.currentDeck!=null){
            myImageComponent.sprite = images[player.currentDeck-1];
        }
    }
}
