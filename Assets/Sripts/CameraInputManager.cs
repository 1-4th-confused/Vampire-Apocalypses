using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages camera-related input, such as returning to the main menu.
/// </summary>
public class CameraInputManager : MonoBehaviour
{
    /// <summary>
    /// Initializes the camera input manager.
    /// </summary>
    void Start()
    {

    }

    /// <summary>
    /// Handles GUI events, including the escape key to load the main menu.
    /// </summary>
    void OnGUI()
    {
        if (Event.current.isKey && Event.current.keyCode == KeyCode.Escape)
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }
}
