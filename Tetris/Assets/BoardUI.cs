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
    public TMP_Text clearText;
    public VertexGradient yellowToOrange;
    public VertexGradient blueToPurple;

    public ParticleSystem pS;

    public IEnumerator LoadScene(int sceneIndex) {
        gameOverAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(2);
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneIndex);
    }

    public void UpdateScoreUI(long score) {
        this.scoreText.text = score.ToString();
    }

    public void UpdateUI(Data.ClearType clearType, int linesCleared, int level) {
        this.linesClearedText.text = linesCleared.ToString();
        this.levelText.text = level.ToString();
        this.clearText.text = clearType.ToString();
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
                burst.count = 60;
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
