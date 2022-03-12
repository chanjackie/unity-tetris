using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NetworkBoard : MonoBehaviour
{
    public Tilemap tilemap { get; private set;}

    private void Awake() {
        this.tilemap = GetComponentInChildren<Tilemap>();
    }

    public void SetTilemap(Tilemap tilemap) {
        this.tilemap = tilemap;
    }
}
