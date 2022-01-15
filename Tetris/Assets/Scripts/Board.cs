using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set;}
    public Queue queue { get; private set;}
    public Hold hold { get; private set;}
    public Piece activePiece { get; private set;}
    public TetrominoData[] tetrominos;
    public Vector3Int spawnPos;
    public Vector2Int boardSize = new Vector2Int(10, 22);

    public AudioSource BGM;
    public AudioSource effectSource;
    public AudioSource gameOverAudio;
    public AudioSource comboSource;
    public AudioClip nextBGM;
    public AudioClip lockClip;
    public AudioClip clearClip;
    public AudioClip moveClip;

    public Animator transition;
    public Animator gameOverAnimator;
    public float transitionTime = 1f;

    public Text linesClearedText;
    public Text scoreText;
    public Text levelText;

    public int totalLinesCleared { get; private set;}
    public int level { get; private set;}
    public long score { get; private set;}
    public int comboCount { get; private set;}
    public Data.ClearType lastClear { get; private set;}
    // nextThreshold determines at which level the music will shift to the next BGM
    public int nextBGMLevel;

    public RectInt Bounds {
        get {
            Vector2Int position = new Vector2Int(-this.boardSize.x/2, -this.boardSize.y/2+1);
            Vector2Int playBounds = new Vector2Int(this.boardSize.x, this.boardSize.y+1);
            return new RectInt(position, playBounds);
        }
    }

    private void Awake() {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();
        this.queue = GetComponentInChildren<Queue>();
        this.hold = GetComponentInChildren<Hold>();
        this.totalLinesCleared = 0;
        this.level = 0;
        this.score = 0;
        this.lastClear = Data.ClearType.NONE;
        for (int i=0; i<this.tetrominos.Length; i++) {
            this.tetrominos[i].Initialize();
        }
    }


    private void Start() {
        this.queue.Initialize(tilemap, tetrominos);
        this.hold.Initialize(tilemap);
        SpawnPiece();
    }

    public void SpawnPiece() {
        this.queue.ClearQueueTiles();
        TetrominoData data = this.queue.pieceQueue.Peek().data;
        // Spawn I pieces one position lower
        Vector3Int spawn = this.spawnPos;
        if (data.tetromino == Tetromino.I) {
            spawn.y--;
        }
        this.activePiece.Initialize(this, spawn, data, this.level);

        if (!IsValidPosition(this.activePiece, spawn)) {
            GameOver();
            return;
        }
        Set(this.activePiece);
        this.queue.UpdateQueue();
    }

    private void GameOver() {
        this.BGM.Stop();
        this.gameOverAudio.Play();
        Destroy(this.activePiece);
        // Placeholder
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex-1));
    }

    IEnumerator LoadScene(int sceneIndex) {
        gameOverAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(2);
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneIndex);
    }

    public void Set(Piece piece) {
        for (int i=0; i<piece.cells.Length; i++) {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void Clear(Piece piece) {
        for (int i=0; i<piece.cells.Length; i++) {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position) {
        for (int i=0; i<piece.cells.Length; i++) {
            Vector3Int tilePosition = piece.cells[i] + position;

            if (this.tilemap.HasTile(tilePosition)) {
                return false;
            } else if (!this.Bounds.Contains((Vector2Int)tilePosition)) {
                return false;
            }
        }
        return true;
    }

    public void ClearLines() {
        RectInt bounds = this.Bounds;
        int row = bounds.yMin;
        int linesCleared = 0;
        while (row < bounds.yMax) {
            if (IsLineFull(row)) {
                linesCleared++;
                this.totalLinesCleared++;
                LineClear(row);
            } else {
                row++;
            }
        }
        if (linesCleared > 0) {
            this.effectSource.PlayOneShot(this.clearClip);
        }
        if (this.BGM.clip != this.nextBGM && this.level >= this.nextBGMLevel) {
            this.BGM.clip = this.nextBGM;
            this.BGM.Play();
        }
        this.level = this.totalLinesCleared/10;
        CalculateScore(linesCleared);
        UpdateText();
    }

    public bool IsLineFull(int row) {
        RectInt bounds = this.Bounds;
        for (int col=bounds.xMin; col<bounds.xMax; col++) {
            Vector3Int pos = new Vector3Int(col, row, 0);
            if (!this.tilemap.HasTile(pos)) {
                return false;
            }
        }
        return true;
    }

    public void LineClear(int row) {
        RectInt bounds = this.Bounds;
        for (int col=bounds.xMin; col<bounds.xMax; col++) {
            Vector3Int pos = new Vector3Int(col, row, 0);
            this.tilemap.SetTile(pos, null);
        }
        while (row < bounds.yMax) {
            for (int col=bounds.xMin; col<bounds.xMax; col++) {
                Vector3Int pos = new Vector3Int(col, row+1, 0);
                TileBase above = this.tilemap.GetTile(pos);
                pos = new Vector3Int(col, row, 0);
                this.tilemap.SetTile(pos, above);
            }
            row++;
        }
    }

    public void SwapPiece() {
        Clear(this.activePiece);
        TetrominoData swappedData = this.hold.SwapPiece(this.activePiece);
        if (swappedData.cells == null) {
            SpawnPiece();
        } else {
            this.activePiece.Initialize(this, this.spawnPos, swappedData, this.level);
            Set(this.activePiece);
        }
    }

    public void UpdateScore(int addedScore) {
        this.score += addedScore;
        this.scoreText.text = this.score.ToString();
    }

    // TODO: implement support for T-Spin scoring, scoring for soft/hard drops
    private void CalculateScore(int linesCleared) {
        Data.ClearType clearType = Data.LinesToClearType[linesCleared];
        if (clearType == Data.ClearType.NONE) {
            this.comboCount = 0;
            return;
        }
        int addedScore = Data.Scores[clearType]*(level+1);
        if (Data.DifficultClears.Contains(clearType) && Data.DifficultClears.Contains(this.lastClear)) {
            addedScore = addedScore*3/2;
        }
        addedScore += 50*this.comboCount*(level+1);
        this.comboCount++;
        this.lastClear = clearType;
        UpdateScore(addedScore);
        if (this.comboCount > 1) {
            this.comboSource.pitch = 1;
            float currentPitch = this.comboSource.pitch;
            this.comboSource.pitch = currentPitch*Mathf.Pow(1.05946f, (this.comboCount-2));
            this.comboSource.Play();
        }
    }

    private void UpdateText() {
        this.linesClearedText.text = this.totalLinesCleared.ToString();
        this.levelText.text = this.level.ToString();
    }

}
