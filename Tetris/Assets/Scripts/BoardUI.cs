using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class BoardUI : MonoBehaviour
{
    public Animator transition;
    public Animator gameOverAnimator;
    public Animator clearDisplayAnimator;

    public Text linesClearedText;
    public Text scoreText;
    public Text levelText;
    public TMP_Text finalScoreText;
    public TMP_Text clearText;
    public TMP_Text comboText;
    public VertexGradient yellowToOrange;
    public VertexGradient blueToPurple;

    public Button retryButton;
    public Button quitButton;

    public ParticleSystem pS;

    public IEnumerator LoadGameOverUI() {
        gameOverAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(2);
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        retryButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
    }

    public void Quit() {
        gameOverAnimator.SetTrigger("QuitPressed");
        StartCoroutine(LoadScene(0));
    }

    public void Retry() {
        gameOverAnimator.SetTrigger("RetryPressed");
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex));
    }

    public IEnumerator LoadScene(int sceneIndex) {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneIndex);
    }

    public void UpdateScoreUI(long score) {
        this.scoreText.text = score.ToString();
        this.finalScoreText.text = score.ToString();
    }

    public void UpdateUI(Data.ClearType clearType, int linesCleared, int level, int comboCount) {
        this.linesClearedText.text = linesCleared.ToString();
        this.levelText.text = level.ToString();
        this.clearText.text = clearType.ToString();
        if (comboCount >= 1) {
            this.comboText.text = comboCount.ToString() + " COMBO";
        } else {
            this.comboText.text = "";
        }
        ParticleSystem.EmissionModule em = this.pS.emission;
        ParticleSystem.Burst burst = new ParticleSystem.Burst(0.0f, 30);
        ParticleSystem.MainModule main = this.pS.main;
        this.clearText.enableVertexGradient = false;
        this.clearText.color = Color.white;
        main.startColor = new ParticleSystem.MinMaxGradient(Color.white);
        switch (clearType) {
            case Data.ClearType.TETRIS:
                this.clearText.enableVertexGradient = true;
                this.clearText.color = Color.yellow;
                this.clearText.colorGradient = this.yellowToOrange;
                burst.count = 75;
                Color orange = new Color(1.0f, 0.4f, 0.0f);
                main.startColor = new ParticleSystem.MinMaxGradient(Color.yellow, orange);
                break;
            case Data.ClearType.TSPIN:
            case Data.ClearType.TSPIN_SINGLE:
            case Data.ClearType.TSPIN_DOUBLE:
            case Data.ClearType.TSPIN_TRIPLE:
                this.clearText.enableVertexGradient = true;
                this.clearText.color = Color.blue;
                this.clearText.colorGradient = this.blueToPurple;
                burst.count = 60;
                main.startColor = new ParticleSystem.MinMaxGradient(Color.blue);
                break;
            case Data.ClearType.SINGLE:
                burst.count = 15;
                break;
            case Data.ClearType.DOUBLE:
                burst.count = 25;
                break;
            case Data.ClearType.TRIPLE:
                burst.count = 35;
                break;
            default:
                burst.count = 30;
                break;
        }

        em.SetBurst(0, burst);
        this.pS.Play(true);
        this.clearDisplayAnimator.SetTrigger("Display");
    }

}
