using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Numerics;

public enum SearchState
{
    NULL = -1,
    UNVISITED,
    OPEN,
    CLOSED
}
    

public class Vertex : MonoBehaviour
{
    Vector2Int index;
    public Vector2Int GetIndex() {  return index; }
    public void SetIndex(Vector2Int newIndex){ index = newIndex; }
    SearchState searchState = SearchState.UNVISITED;
    public SearchState GetSearchState() { return searchState; }
    public void SetSearchState(SearchState state) 
    { 
        searchState = state;

        switch (searchState)
        {
            case SearchState.OPEN:
				spriteRenderer.color = Color.green;
				break;
			case SearchState.CLOSED:
				spriteRenderer.color = Color.cyan;
				break;
        }

    }
    [SerializeField] List<Sprite> stateSprites = new List<Sprite>();
    SpriteRenderer spriteRenderer;
    public SpriteRenderer GetSpriteRenderer() { return spriteRenderer; }

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

    public void RandomizeWalls()
    {
        
    }
	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		searchState = SearchState.UNVISITED;
	}

}
