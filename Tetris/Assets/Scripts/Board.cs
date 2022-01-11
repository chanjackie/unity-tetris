using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set;}
    public Queue queue { get; private set;}
    public Hold hold { get; private set;}
    public Piece activePiece { get; private set;}
    public TetrominoData[] tetrominos;
    public Vector3Int spawnPos;
    public Vector2Int boardSize = new Vector2Int(10, 20);

    public int pieceQueueSize;

    public RectInt Bounds {
        get {
            Vector2Int position = new Vector2Int(-this.boardSize.x/2, -this.boardSize.y/2);
            return new RectInt(position, this.boardSize);
        }
    }

    private void Awake() {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();
        this.queue = GetComponentInChildren<Queue>();
        this.hold = GetComponentInChildren<Hold>();
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
        this.activePiece.Initialize(this, this.spawnPos, data);

        if (!IsValidPosition(this.activePiece, this.spawnPos)) {
            GameOver();
        }

        Set(this.activePiece);
        this.queue.UpdateQueue();
    }

    private void GameOver() {
        this.tilemap.ClearAllTiles();
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
        while (row < bounds.yMax) {
            if (IsLineFull(row)) {
                LineClear(row);
            } else {
                row++;
            }
        }
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
            this.activePiece.Initialize(this, this.spawnPos, swappedData);
            Set(this.activePiece);
        }
    }

}
