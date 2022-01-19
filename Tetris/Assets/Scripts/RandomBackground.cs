using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomBackground : MonoBehaviour
{
    public Sprite[] backgrounds;
    public Image background { get; private set;}

    void Start()
    {
        this.background = GetComponent<Image>();
        this.background.sprite = backgrounds[Random.Range(0, backgrounds.Length)];
    }
}
