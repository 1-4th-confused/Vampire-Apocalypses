using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CameraInputManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnGUI()
    {
        if (Event.current.isKey && Event.current.keyCode == KeyCode.Escape)
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }
}
