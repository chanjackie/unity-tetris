using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBackground : MonoBehaviour
{
    public Sprite[] backgrounds;
    public SpriteRenderer render { get; private set;}

    void Start()
    {
        this.render = GetComponent<SpriteRenderer>();
        this.render.sprite = backgrounds[Random.Range(0, backgrounds.Length)];
        
    }
}
