using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public static void newGame() {
        Debug.Log("rui is right");
        SceneManager.LoadScene("CabinOutside");
    }

    public static void openGame() {
        Debug.Log("*olive oil's ego takes a hit");
        SceneManager.LoadScene("CabinOutside");
    }
}
