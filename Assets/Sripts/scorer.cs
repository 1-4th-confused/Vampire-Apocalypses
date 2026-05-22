using UnityEngine;
using UnityEngine.UI;

public class scorer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Text scoreText; 
    public Board board;

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + board.score2.ToString();    
    }
}
