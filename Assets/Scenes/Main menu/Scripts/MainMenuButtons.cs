using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles main menu button interactions.
/// </summary>
public class MainMenuButtons : MonoBehaviour
{
    /// <summary>
    /// Starts a new game by loading the battle scene.
    /// </summary>
    public static void newGame()
    {
        SceneManager.LoadScene("Battle", LoadSceneMode.Single);
    }

    /// <summary>
    /// Opens an existing game by loading the battle scene.
    /// </summary>
    public static void openGame()
    {
        SceneManager.LoadScene("Battle", LoadSceneMode.Single);
    }
}
