using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public static void newGame() {
        Debug.Log("rui is right");
        SceneManager.LoadScene("Battle", LoadSceneMode.Single);
    }

    public static void openGame() {
        Debug.Log("rui's ego grows");
        SceneManager.LoadScene("Battle", LoadSceneMode.Single);
    }
}
