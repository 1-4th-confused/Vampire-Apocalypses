using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public static void newGame() {
        Debug.Log("rui is right");
        SceneManager.LoadScene("Battle");
    }

    public static void openGame() {
        Debug.Log("rui ego takes a hit");
        SceneManager.LoadScene("Battle");
    }
}
