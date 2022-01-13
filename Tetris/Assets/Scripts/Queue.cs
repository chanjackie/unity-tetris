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
    public int queueDisplayCount;

    public void Initialize(Tilemap tilemap, TetrominoData[] data) {
        this.tilemap = tilemap;
        this.tetrominos = data;
        this.pieceQueue = new Queue<InactivePiece>();
        this.queueObject = new GameObject("Queue");
        // Generate two bags initially
        GenerateBag();
        GenerateBag();
    }

    public void GenerateBag() {
        // print("----GENERATING NEW BAG----");
        List<int> bag = new List<int>();
        for (int i=0; i<this.tetrominos.Length; i++) {
            bag.Add(i);
        }
        bag = Shuffle(bag);
        string debugString = "";
        foreach (int n in bag) {
            debugString += n + " ";
            InactivePiece piece = this.queueObject.AddComponent<InactivePiece>();
            piece.Initialize(this.tetrominos[n]);
            this.pieceQueue.Enqueue(piece);
        }
        // print(debugString);
    }

    // Fisher-Yates Shuffle
    public static List<int> Shuffle(List<int> list) {
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = UnityEngine.Random.Range(0, n);
            int value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }

    public void UpdateQueue() {
        Destroy(pieceQueue.Dequeue());
        InactivePiece[] arr = this.pieceQueue.ToArray();
        for (int i=0; i<this.queueDisplayCount; i++) {
            Vector3Int pos = this.queuePos;
            pos.y -= i*3;
            Set(arr[i], pos);
        }
        if (this.pieceQueue.Count <= 7) {
            GenerateBag();
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
        for (int i=0; i<this.queueDisplayCount; i++) {
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
