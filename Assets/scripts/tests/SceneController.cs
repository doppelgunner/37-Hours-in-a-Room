using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour {

    public const string GAME = "Game",
                        MENU = "Menu";

    public static void GoToScene(string name) {
        SceneManager.LoadScene(name);
    }
}
