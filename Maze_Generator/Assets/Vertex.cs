using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.Experimental.GraphView;

public enum SearchState
{
    NULL = -1,
    UNVISITED,
    OPEN,
    CLOSED,
    VISITED,
}
    

public class Vertex : MonoBehaviour
{
    UnityEngine.Vector2 index;
    public UnityEngine.Vector2 GetIndex() {  return index; }
    public void SetIndex(UnityEngine.Vector2 newIndex){ index = newIndex; }
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

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }

    bool rightWall;
    public bool GetRightWall() { return rightWall; }

    public void SetRightWall(bool wall)
    {
        rightWall = wall;
        SetWalls();
    }
    bool upWall;
    public bool GetUpWall() { return upWall; }
    public void SetUpWall(bool wall)
    {
        upWall = wall;
        SetWalls();
    }
    [SerializeField] List<Sprite> wallSprites = new List<Sprite>();
    void SetWalls()
    {
        if (rightWall && upWall)
        {
            spriteRenderer.sprite = wallSprites[wallSprites.Count - 1];
        }

        else if (rightWall)
        {
            spriteRenderer.sprite = wallSprites[1];
        }

        else if (upWall)
        {
            spriteRenderer.sprite = wallSprites[2];
        }

        else
        {
            spriteRenderer.sprite = wallSprites[0];
        }
    }
    SpriteRenderer spriteRenderer;
    public SpriteRenderer GetSpriteRenderer() { return spriteRenderer; }
	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		searchState = SearchState.UNVISITED;
	}

    private void Start()
    {
        RandomizeWalls();
    }

    public void RandomizeWalls()
    {
        while (!rightWall || !upWall)
        {
            int rightRandom = Random.Range(0, 2);
            int upRandom = Random.Range(0, 2);

            if (rightRandom == 0)
            {
                rightWall = false;
            }

            else
            {
                rightWall = true;
            }

            if (upRandom == 0)
            {
                upWall = false;
            }
            else
            {
                upWall = true;
            }
        }


        SetWalls();
    }

}
