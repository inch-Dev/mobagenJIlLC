using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class World : MonoBehaviour
{
    [HideInInspector] public static World instance;
    [SerializeField] int xSize;
    [SerializeField] int ySize;
    [SerializeField] GameObject pointPF;
    List<Point> _points = new List<Point>();
    
    public Point GetPointAt(Vector2 coordinates)
    {
        //Debug.Log($"Getting point at {coordinates}");
        foreach(var point in _points)
        {
            if (point.Coordinates == coordinates)
                return point;
        }

        return null;
    }

    public float GetCostOf(Point firstPoint, Point secondPoint)
    {
        return Mathf.Abs(secondPoint.Coordinates.x - firstPoint.Coordinates.x) + Mathf.Abs(secondPoint.Coordinates.y - firstPoint.Coordinates.y);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;

        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateGrid()
    {
        for(int i = 0; i < xSize; i++)
        {
            for(int j = 0; j < ySize; j++)
            {
                SpawnPoint(new Vector2(i,j), 0); //Plug in distance
            }
        }
    }

    void SpawnPoint(Vector2 coordinates, int priority)
    {
        Vector3 spawnPos = Vector3.zero;

        bool shouldOffset = false;
        int column = (int)coordinates.x;
        int row = (int)coordinates.y;
        float width;
        float height;
        float xPos;
        float yPos;
        float horizontalDistance;
        float verticalDistance;
        float offset;
        float size = .65f;

        shouldOffset = (row % 2) == 0;
        width = Mathf.Sqrt(3) * size;
        height = 2f * size;

        horizontalDistance = width;
        verticalDistance = height * (3f/4f);

        offset = shouldOffset ? width / 2f : 0;

        xPos = (column * (horizontalDistance)) + offset;
        yPos = (row * verticalDistance);


        spawnPos = new Vector3(xPos, yPos, 0);

        GameObject newPoint = GameObject.Instantiate(pointPF, spawnPos, Quaternion.identity);
        newPoint.transform.parent = this.transform;
        newPoint.name = "Point" + new Vector2Int((int)coordinates.x, (int)coordinates.y);


        Point thePoint = newPoint.GetComponent<Point>();
        _points.Add(thePoint);
        thePoint.Coordinates = coordinates;


        int randomIndex = Random.Range(0, (int)PointType.NUM_TYPES);

        switch (randomIndex)
        {
            case 0:
                thePoint.SetPointType(PointType.BLANK);
                break;
            case 1:
                thePoint.SetPointType(PointType.WALL);
                break;
        }

        thePoint.Priority = GetCostOf(GetPointAt(Vector2.zero), thePoint);
        
    }


}
