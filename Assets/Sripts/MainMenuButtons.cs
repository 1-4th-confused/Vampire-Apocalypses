using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public static void newGame() {
        Debug.Log("rui is wrong");
        SceneManager.LoadScene("CabinOutside");
    }

    public static void openGame() {
        Debug.Log("*ruis ego takes a hit");
        SceneManager.LoadScene("CabinOutside");
    }
}
