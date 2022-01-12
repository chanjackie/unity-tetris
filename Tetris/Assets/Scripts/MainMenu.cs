using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // private void Start() {
    //     StartCoroutine(LoadAsyncGame());
    // }

    // private IEnumerator LoadAsyncGame() {
    //     AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Tetris");
    //     while (!asyncLoad.isDone) {
    //         yield return null;
    //     }
    // }
    public void PlayGame() {
        SceneManager.LoadScene("Tetris");
    }

    public void QuitGame() {
        print("QUIT!");
        Application.Quit();
    }
}
