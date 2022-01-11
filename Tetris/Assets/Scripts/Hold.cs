using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Hold : MonoBehaviour
{
    public Tilemap tilemap { get; private set;}
    public InactivePiece piece { get; private set;}
    public GameObject hold { get; private set;}
    public Vector3Int holdPosition;

    public void Initialize(Tilemap tilemap) {
        this.tilemap = tilemap;
        this.hold = new GameObject("Hold");
        this.piece = this.hold.AddComponent<InactivePiece>();
    }

    public void ClearHoldPiece() {
        for (int i=0; i<this.piece.cells.Length; i++) {
            Vector3Int tilePosition = this.piece.cells[i] + holdPosition;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    public void Set() {
        for (int i=0; i<this.piece.cells.Length; i++) {
            Vector3Int tilePosition = this.piece.cells[i] + holdPosition;
            this.tilemap.SetTile(tilePosition, this.piece.data.tile);
        }
    }

    public TetrominoData SwapPiece(Piece swap) {
        if (this.piece.cells != null) {
            ClearHoldPiece();
        }
        TetrominoData pieceData = this.piece.data;
        this.piece.Initialize(swap.data);
        Set();
        return pieceData;
    }
}
