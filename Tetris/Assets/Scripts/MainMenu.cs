using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public InputField roomCodeInput;
    public Animator transition;
    public GameObject playMenu;
    public GameObject lobbyMenu;

    public void HostGame() {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex+2));
    }

    public void EnterLobby() {
        if (roomCodeInput.text == "ABCD") {
            playMenu.SetActive(false);
            lobbyMenu.SetActive(true);
        }
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
