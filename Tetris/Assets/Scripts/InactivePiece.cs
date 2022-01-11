using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InactivePiece : MonoBehaviour
{
    public TetrominoData data { get; private set;}
    public Vector3Int[] cells { get; private set;}

    public void Initialize(TetrominoData data) {
        this.data = data;

        if (this.cells == null) {
            this.cells = new Vector3Int[data.cells.Length];
        }

        for (int i=0; i<data.cells.Length; i++) {
            this.cells[i] = (Vector3Int)data.cells[i];
        }
    }
}
