using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Animator transition;

    public void HostGame() {
        print("HOST!");
    }

    public void JoinGame() {
        print("JOIN!");
    }

    public void PlaySolo() {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex+1));
    }

    IEnumerator LoadScene(int sceneIndex) {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneIndex);
    }
    public void QuitGame() {
        print("QUIT!");
        Application.Quit();
    }
}
