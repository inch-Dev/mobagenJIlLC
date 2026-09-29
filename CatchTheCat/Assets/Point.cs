using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PointType
{
    NULL = -1,
    BLANK,
    WALL,
    NUM_TYPES
}

public class Point : MonoBehaviour
{
    public Vector2 Coordinates;
    public PointType Type;
    public void SetPointType(PointType type)
    {
        Type = type;
        //Set colors later
    }
    public int Priority; //Lower Priority better

    SpriteRenderer _spriteRenderer;
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Frontiered()
    {

    }

    public void Reached()
    {

    }
}
