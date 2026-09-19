using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using JetBrains.Annotations;

public class Maze : MonoBehaviour, IStateable
{
    public void HandleState(State state)
    {
        switch (state)
        {
            case State.PAUSED:
                canStep = false;
                break;
            case State.PLAY:
                canStep = true;
                break;
        }
    }

    int[] randomNumbers = { 72, 99, 56, 34, 43, 62, 31, 4, 70, 22, 6, 65, 96, 71, 29, 9, 98, 41, 90, 7, 30, 3, 97, 49, 63, 88, 47, 82, 91, 54, 74, 2, 86, 14, 58, 35, 89, 11, 10, 60, 28, 21, 52, 50, 55, 69, 76, 94, 23, 66, 15, 57, 44, 18, 67, 5, 24, 33, 77, 53, 51, 59, 20, 42, 80, 61, 1, 0, 38, 64, 45, 92, 46, 79, 93, 95, 37, 40, 83, 13, 12, 78, 75, 73, 84, 81, 8, 32, 27, 19, 87, 85, 16, 25, 17, 68, 26, 39, 48, 36 };
    int randomIndex = 0;
    int GetRandomNumber()
    {
        if (randomIndex >= randomNumbers.Length)
            randomIndex = 0;

        int randomNumber = randomNumbers[randomIndex];
        randomIndex++;

        return randomNumber;
        
    }
    
    [SerializeField] int sideSize;
    [SerializeField] TextMeshProUGUI sideSizeTF;
    [SerializeField] Slider sideSizeSlider;
    public void SetSideSize()
    {
        sideSize = (int)sideSizeSlider.value;
        sideSizeTF.text = sideSize.ToString();
        Reset();
    }


    [SerializeField] float offset;
    [SerializeField] GameObject vertexPF;
    List<Vertex> mazeVertices = new List<Vertex>();
    Vertex GetVertexAt(UnityEngine.Vector2 index)
    {
        foreach(Vertex vertex in mazeVertices)
        {
            if (vertex.GetIndex() == index)
                return vertex;
        }
        return null;
    }

    bool GetRightWall(UnityEngine.Vector2 index)
    {
        foreach(var vertex in mazeVertices)
        {
            if (vertex.GetIndex() == index)
                return vertex.GetRightWall();
        }
        return false;
    }

    bool GetUpWall(UnityEngine.Vector2 index)
    {
        foreach (var vertex in mazeVertices)
        {
            if (vertex.GetIndex() == index)
                return vertex.GetUpWall();
        }
        return false;
    }

    List<Vertex> searchVertices = new List<Vertex>();
    Vertex searchHead;

    bool canStep;

    List<Vertex> GetNeighbors(Vertex vertex)
    {
        List<Vertex> neighbors = new List<Vertex>();

        //Search in directions
        List<Vector2> directions = new List<Vector2>();
        directions.Add(Vector2.up);
        directions.Add(Vector2.right);
        directions.Add(Vector2.down);
        directions.Add(Vector2.left);
        
        //Search in clockwise order
        for(int i = 0; i < directions.Count; i++)
        {
            if (GetVertexAt(vertex.GetIndex() + directions[i]) != null)
            {
                if (GetVertexAt(vertex.GetIndex() + directions[i]).GetSearchState() == SearchState.UNVISITED)
                {
                    neighbors.Add(GetVertexAt(vertex.GetIndex() + directions[i]));
                    Debug.Log($"Viable neighbor for {vertex.gameObject}:{GetVertexAt(vertex.GetIndex() + directions[i]).gameObject}");
                }
			}
                
        }


        return neighbors;
    }

    Vector2 GetDirectionOfNeighbor(Vertex origin, Vertex neighbor)
    {
        Vector2 direction = Vector2.zero;

        float xDirection = neighbor.GetIndex().x - origin.GetIndex().x;
        float yDirection = neighbor.GetIndex().y - origin.GetIndex().y;

        direction = new Vector2(xDirection, yDirection);

        return direction;
    }
    float stepTimeSeconds = .5f;
    [SerializeField] TextMeshProUGUI stepTimeSecondsTF;
    [SerializeField] Slider stepTimeSecondsSlider;
    public void SetStepTimeSeconds()
    {
        stepTimeSeconds = stepTimeSecondsSlider.value;
        stepTimeSecondsTF.text = stepTimeSeconds.ToString("F2");
    }
    float nextStepInSeconds;
    [SerializeField] TextMeshProUGUI nextStepInSecondsTF;
    void SetNextStepInSeconds(float value)
    {
        nextStepInSeconds = value;
        nextStepInSecondsTF.text = "Next Step In:" + nextStepInSeconds.ToString("F2");
    }

    float totalSeconds;
    [SerializeField] TextMeshProUGUI totalSecondsTF;
    void SetTotalSeconds(float value)
    {
        totalSeconds = value;
        totalSecondsTF.text = totalSeconds.ToString("F2");
    }


    float deltaTime;
    [SerializeField] TextMeshProUGUI deltaTimeTF;
    void SetDeltaTime(float value)
    {
        deltaTime  = value;
        deltaTimeTF.text = deltaTime.ToString("F2") + "ms";
    }
    float averageDeltaTime;
    int updatesRan = 0;

    [SerializeField] TextMeshProUGUI averageDeltaTimeTF;
    void SetAverageDeltaTime(float value)
    {
        averageDeltaTime = value;
        averageDeltaTimeTF.text = "AVG:" + averageDeltaTime.ToString("F2") + "ms";
    }
    float fps;
    [SerializeField] TextMeshProUGUI fpsTF;
    void SetFPS(float value)
    {
        fps = value;
        fpsTF.text = fps.ToString("F2") + "FPS";
    }
   
    private void Start()
    {
        GenerateMaze();
        searchHead = GetVertexAt(new Vector2Int(0, 0));
        searchHead.SetSearchState(SearchState.OPEN);
        searchVertices.Add(GetVertexAt(new Vector2Int(0, 0)));
    }

    private void Update()
    {
        updatesRan++;
        if (canStep)
        {
            SetNextStepInSeconds(nextStepInSeconds + Time.deltaTime);
            if (nextStepInSeconds > stepTimeSeconds)
            {
                SetNextStepInSeconds(0);
                Step();
            }
        }
        SetDeltaTime(Time.deltaTime);
        SetAverageDeltaTime((averageDeltaTime + Time.deltaTime) / (float)updatesRan);
        SetFPS(1/Time.deltaTime);
    }



    void GenerateMaze()
    {
        for(int i = 0; i < sideSize; i++)
        {
            for(int j = 0; j < sideSize; j++)
            {
                GenerateVertex(new Vector2(i * offset, j * offset));
            }
        }
	}

    void GenerateVertex(Vector2 index)
    {
        GameObject newVertexObject = GameObject.Instantiate(vertexPF, new Vector3(index.x, index.y, 0f), Quaternion.identity);
		newVertexObject.name = "Vertex" + new Vector2((index.x / offset), (index.y / offset)).ToString();

		newVertexObject.transform.parent = this.transform;

        Vertex newVertex = newVertexObject.GetComponent<Vertex>();
        newVertex.SetIndex(new Vector2((index.x / offset), (index.y / offset)));

        mazeVertices.Add(newVertex);
    }

    public void Step()
    {

        //If not initialized
        if(!searchHead || searchVertices.Count <= 0)
        {
            return;
        }




        foreach(Vertex vertex in searchVertices)
        {
            vertex.SetColor(Color.green);
        }

        searchHead.SetColor(Color.blue);
		searchHead.SetSearchState(SearchState.VISITED);

		//Move through stack

		//Get neighbors
		List<Vertex> neighbors = GetNeighbors(searchHead);

        //Random select search
        if(neighbors.Count > 1)
        {
            int randomIndex = GetRandomNumber();
            int chosenNeighborIndex = randomIndex % neighbors.Count;

            
            if(GetDirectionOfNeighbor(searchHead, neighbors[chosenNeighborIndex]) == Vector2.up)
            {
                if (searchHead.GetUpWall())
                    searchHead.SetUpWall(false);
            }

            if (GetDirectionOfNeighbor(searchHead, neighbors[chosenNeighborIndex]) == Vector2.right)
            {
                if (searchHead.GetRightWall())
                    searchHead.SetRightWall(false);
            }

            if (GetDirectionOfNeighbor(searchHead, neighbors[chosenNeighborIndex]) == Vector2.down)
            {
                if (neighbors[chosenNeighborIndex].GetUpWall())
                    neighbors[chosenNeighborIndex].SetUpWall(false);
            }

            if (GetDirectionOfNeighbor(searchHead, neighbors[chosenNeighborIndex]) == Vector2.left)
            {
                if (neighbors[chosenNeighborIndex].GetRightWall())
                    neighbors[chosenNeighborIndex].SetRightWall(false);
            }

            searchVertices.Add(neighbors[chosenNeighborIndex]);
			searchHead = neighbors[chosenNeighborIndex];

            
		}

        //One neighbor
        else if(neighbors.Count > 0) 
        {
            if (GetDirectionOfNeighbor(searchHead, neighbors[0]) == Vector2.up)
            {
                if (searchHead.GetUpWall())
                    searchHead.SetUpWall(false);
            }

            if (GetDirectionOfNeighbor(searchHead, neighbors[0]) == Vector2.right)
            {
                if (searchHead.GetRightWall())
                    searchHead.SetRightWall(false);
            }

            if (GetDirectionOfNeighbor(searchHead, neighbors[0]) == Vector2.down)
            {
                if (neighbors[0].GetUpWall())
                    neighbors[0].SetUpWall(false);
            }

            if (GetDirectionOfNeighbor(searchHead, neighbors[0]) == Vector2.left)
            {
                if (neighbors[0].GetRightWall())
                    neighbors[0].SetRightWall(false);
            }
            searchVertices.Add(neighbors[0]);
            searchHead = neighbors[0];
		}

        else
        {
            Debug.Log($"Can't Find Neighbors!!! Turning around from {searchHead}...");
            searchHead.SetColor(Color.cyan);
			//searchHead.SetSearchState(SearchState.VISITED);
			searchVertices.Remove(searchHead);
            if (searchVertices.Count != 0)
            {
                searchHead = searchVertices[searchVertices.Count - 1];
            }
        }
	}

    public void Reset()
    {
        mazeVertices.Clear();

        searchVertices.Clear();

        GenerateMaze();
    }
}
