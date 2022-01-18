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
    public float transitionTime = 1f;

    public Text linesClearedText;
    public Text scoreText;
    public Text levelText;
    public TMP_Text clearText;
    public VertexGradient yellowToOrange;
    public VertexGradient blueToPurple;

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
        switch (clearType) {
            case Data.ClearType.TETRIS:
                this.clearText.enableVertexGradient = true;
                this.clearText.color = Color.yellow;
                this.clearText.colorGradient = this.yellowToOrange;
                break;
            case Data.ClearType.TSPIN:
            case Data.ClearType.TSPIN_SINGLE:
            case Data.ClearType.TSPIN_DOUBLE:
            case Data.ClearType.TSPIN_TRIPLE:
                this.clearText.enableVertexGradient = true;
                this.clearText.color = Color.blue;
                this.clearText.colorGradient = this.blueToPurple;
                break;
            default:
                this.clearText.enableVertexGradient = false;
                this.clearText.color = Color.white;
                break;
        }
        this.clearDisplayAnimator.SetTrigger("Display");
    }

}
