using UnityEngine;
using UnityEngine.UI;

public class GoldInterface : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Text goldText; 
    public Board board;

    // Update is called once per frame
    void FixedUpdate()
    {
        goldText.text = "Gold: " + board.gameGold.ToString();    
    }
}
