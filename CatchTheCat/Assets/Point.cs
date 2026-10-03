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
    public float Priority; //Lower Priority better //Distance from starting point

   [SerializeField] SpriteRenderer _fillSpriteRenderer;
   [SerializeField] SpriteRenderer _borderSpriteRenderer;

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void Neighbored()
    {
        _borderSpriteRenderer.color = Color.green;
    }

    public void Frontiered()
    {
        _borderSpriteRenderer.color = Color.blue;
    }

    public void FrontierHead()
    {
        _borderSpriteRenderer.color = Color.blue;
        _fillSpriteRenderer.color = Color.blue;
    }

    public void Reached()
    {
        _fillSpriteRenderer.color = Color.yellow;
		_borderSpriteRenderer.color = Color.black;
	}
}
