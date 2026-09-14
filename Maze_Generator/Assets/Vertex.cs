using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Numerics;

public class Vertex : MonoBehaviour
{
    Vector2Int index;
    public void SetIndex(Vector2Int newIndex){ index = newIndex; }
    [SerializeField] List<Sprite> stateSprites = new List<Sprite>();

    Vector<bool> walls;
    //Add wall bools to walls

    public bool GetNorthWall()
    {
        return walls[0];
    }
    public void SetNorthWall(bool northWall){ }

    public bool GetSouthWall()
    {
        return true;
    }
    public void SetSouthWall(bool southWall){ }

    public bool GetEastWall()
    {
        return walls[1];

    }

    public bool GetWestWall()
    {
        return true;
    }
    public void SetWestWall(bool westWall){ }

    public void RandomizeState()
    {
        
    }
}
