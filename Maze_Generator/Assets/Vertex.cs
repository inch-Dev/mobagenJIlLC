using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Numerics;

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

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }
    [SerializeField] List<Sprite> stateSprites = new List<Sprite>();
    SpriteRenderer spriteRenderer;
    public SpriteRenderer GetSpriteRenderer() { return spriteRenderer; }
	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		searchState = SearchState.UNVISITED;
	}

}
