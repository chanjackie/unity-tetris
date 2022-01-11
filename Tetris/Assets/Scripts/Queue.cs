using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Queue : MonoBehaviour
{

    public Tilemap tilemap { get; private set;}
    public TetrominoData[] tetrominos;
    public GameObject queueObject { get; private set;}
    public Queue<InactivePiece> pieceQueue { get; private set;}
    public Vector3Int queuePos;
    public int queueSize;

    public void Initialize(Tilemap tilemap, TetrominoData[] data) {
        this.tilemap = tilemap;
        this.tetrominos = data;
        this.pieceQueue = new Queue<InactivePiece>();
        this.queueObject = new GameObject("Queue");
        for (int i=0; i<this.queueSize; i++) {
            int random = UnityEngine.Random.Range(0, this.tetrominos.Length);
            InactivePiece piece = this.queueObject.AddComponent<InactivePiece>();
            piece.Initialize(this.tetrominos[random]);
            this.pieceQueue.Enqueue(piece);
        }
    }

    public void UpdateQueue() {
        Destroy(pieceQueue.Dequeue());
        int random = UnityEngine.Random.Range(0, this.tetrominos.Length);
        InactivePiece piece = this.queueObject.AddComponent<InactivePiece>();
        piece.Initialize(this.tetrominos[random]);
        this.pieceQueue.Enqueue(piece);
        int i = 0;
        foreach (InactivePiece qp in this.pieceQueue) {
            Vector3Int pos = queuePos;
            pos.y -= i*3;
            i++;
            Set(qp, pos);
        }
    }

    public void Set(InactivePiece piece, Vector3Int position) {
        for (int i=0; i<piece.cells.Length; i++) {
            Vector3Int tilePosition = piece.cells[i] + position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void ClearQueueTiles() {
        InactivePiece[] arr = this.pieceQueue.ToArray();
        for (int i=0; i<this.queueSize; i++) {
            Vector3Int[] cells = arr[i].cells;
            for (int j=0; j<cells.Length; j++) {
                Vector3Int pos = this.queuePos;
                pos.y -= i*3;
                Vector3Int tilePosition = cells[j] + pos;
                this.tilemap.SetTile(tilePosition, null);
            }
        }
    }
}
