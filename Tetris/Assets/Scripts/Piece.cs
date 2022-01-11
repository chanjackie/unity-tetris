using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board board { get; private set;}
    public TetrominoData data { get; private set;}
    public Vector3Int[] cells { get; private set;}
    public Vector3Int position { get; private set;}
    public int rotationIndex { get; private set;}
    public bool swappedOnce = false;
    public bool dropFast { get; private set;}
    public bool leftFast { get; private set;}
    public bool rightFast { get; private set;}

    public float stepDelay = 1f;
    public float lockDelay = 0.25f;
    public float holdDelay = 0.2f;
    public float moveFastDelay = 0.05f;

    private float stepTime;
    private float lockTime;

    private float dropHoldTime;
    private float leftHoldTime;
    private float rightHoldTime;

    private float dropFastTime;
    private float leftFastTime;
    private float rightFastTime;
    public void Initialize(Board board, Vector3Int position, TetrominoData data) {
        this.board = board;
        this.position = position;
        this.data = data;
        this.rotationIndex = 0;
        this.stepTime = Time.time + this.stepDelay;
        this.lockTime = 0f;
        this.dropHoldTime = this.leftHoldTime = this.rightHoldTime = 0f;
        this.dropFastTime = this.leftFastTime = this.rightFastTime = 0f;
        this.dropFast = this.leftFast = this.rightFast = false;

        if (this.cells == null) {
            this.cells = new Vector3Int[data.cells.Length];
        }

        for (int i=0; i<data.cells.Length; i++) {
            this.cells[i] = (Vector3Int)data.cells[i];
        }
    }

    private void Update() {
        this.board.Clear(this);
        this.lockTime += Time.deltaTime;

        if (!swappedOnce && Input.GetKeyDown(KeyCode.LeftShift)) {
            this.swappedOnce = true;
            this.board.SwapPiece();
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
            Rotate(1);
        } else if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.LeftControl)) {
            Rotate(-1);
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
            Move(Vector2Int.left);
        } else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
            Move(Vector2Int.right);
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
            Move(Vector2Int.down);
        }

        CheckHeldKeys();

        if (this.dropFast) {
            this.dropFastTime += Time.deltaTime;
            if (this.dropFastTime >= this.moveFastDelay) {
                Move(Vector2Int.down);
                this.dropFastTime = 0f;
            }
        }

        if (this.leftFast) {
            this.leftFastTime += Time.deltaTime;
            if (this.leftFastTime >= this.moveFastDelay) {
                Move(Vector2Int.left);
                this.leftFastTime = 0f;
            }
        }

        if (this.rightFast) {
            this.rightFastTime += Time.deltaTime;
            if (this.rightFastTime >= this.moveFastDelay) {
                Move(Vector2Int.right);
                this.rightFastTime = 0f;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            HardDrop();
        }

        if (Time.time >= this.stepTime) {
            Step();
        }

        this.board.Set(this);
    }

    private void CheckHeldKeys() {
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            this.dropHoldTime += Time.deltaTime;
            if (this.dropHoldTime >= this.holdDelay) {
                this.dropFast = true;
            }
        } else {
            this.dropHoldTime = 0f;
            this.dropFast = false;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            this.leftHoldTime += Time.deltaTime;
            if (this.leftHoldTime >= this.holdDelay) {
                this.leftFast = true;
            }
        } else {
            this.leftHoldTime = 0f;
            this.leftFast = false;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            this.rightHoldTime += Time.deltaTime;
            if (this.rightHoldTime >= this.holdDelay) {
                this.rightFast = true;
            }
        } else {
            this.rightHoldTime = 0f;
            this.rightFast = false;
        }
    }

    private void Step() {
        this.stepTime = Time.time + this.stepDelay;

        Move(Vector2Int.down);

        if (this.lockTime >= this.lockDelay) {
            Lock();
        }
    }

    private void Lock() {
        this.swappedOnce = false;
        this.board.Set(this);
        this.board.ClearLines();
        this.board.SpawnPiece();
    }

    private void HardDrop() {
        while (Move(Vector2Int.down)) {
            continue;
        }
        Lock();
    }

    private bool Move(Vector2Int translation) {
        Vector3Int newPos = this.position;
        newPos.x += translation.x;
        newPos.y += translation.y;
        bool valid = this.board.IsValidPosition(this, newPos);
        if (valid) {
            this.position = newPos;
            this.lockTime = 0f;
        }
        
        return valid;
    }

    private void Rotate(int direction) {
        int currentRotation = this.rotationIndex;
        this.rotationIndex = Wrap(this.rotationIndex+direction, 0, 4);
        
        ApplyRotationMatrix(direction);

        if (!TestWallKicks(this.rotationIndex, direction)) {
            this.rotationIndex = currentRotation;
            ApplyRotationMatrix(-direction);
        }
    }

    private void ApplyRotationMatrix(int direction) {
        for (int i=0; i<this.cells.Length; i++) {
            Vector3 cell = this.cells[i];
            int x, y;

            switch (this.data.tetromino) {
                case Tetromino.I:
                case Tetromino.O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
                default:
                    x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
            }
            this.cells[i] = new Vector3Int(x, y, 0);
        }
    }

    private bool TestWallKicks(int rotationIndex, int direction) {
        int wallKickIndex = GetWallKickIndex(rotationIndex, direction);

        for (int i=0; i<this.data.wallKicks.GetLength(1); i++) {
            Vector2Int translation = this.data.wallKicks[wallKickIndex, i];
            if (Move(translation)) {
                return true;
            }
        }
        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int direction) {

        int wallKickIndex = rotationIndex*2;
        if (direction<0) {
            wallKickIndex--;
        }
        Wrap(wallKickIndex, 0, this.data.wallKicks.GetLength(0));
        return wallKickIndex;
    }

    private int Wrap(int input, int min, int max) {
        if (input < min) {
            return max - (min - input) % (max - min);
        } else {
            return min + (input - min) % (max - min);
        }
    }
}
