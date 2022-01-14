using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    public void PlayGame() {
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
