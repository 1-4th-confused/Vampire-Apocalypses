using UnityEngine;
using UnityEngine.UI;

public class WaveInterface : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Text waveText; 
    public Board board;

    // Update is called once per frame
    void FixedUpdate()
    {
        waveText.text = "Wave: " + board.currentWave.ToString() + "/" + board.totalWaves.ToString();    
    }
}
