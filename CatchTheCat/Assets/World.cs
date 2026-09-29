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
    [SerializeField] float xOffset;
    [SerializeField] float yOffset;
    [SerializeField] GameObject pointPF;
    List<Point> _points;
    
    public Point GetPointAt(Vector2 coordinates)
    {
        foreach(var point in _points)
        {
            if (point.Coordinates == coordinates)
                return point;
        }

        return null;
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
                SpawnPoint(new Vector2((float)i, (float)j), 0);
            }
        }
    }

    void SpawnPoint(Vector2 coordinates, int priority)
    {
        Vector3 spawnPos = Vector3.zero;

        float horizontalOffset = 2 * Mathf.Sqrt(3) * 2;
        horizontalOffset *= coordinates.x;
        float verticalOffset = (3 / 4) * (3 / 2);
        verticalOffset *= coordinates.y;

        spawnPos = new Vector3(horizontalOffset, verticalOffset, 0);

        GameObject newPoint = GameObject.Instantiate(pointPF, spawnPos, Quaternion.identity);
        newPoint.transform.parent = this.transform;
        newPoint.name = "Point" + new Vector2Int((int)coordinates.x, (int)coordinates.y);


        Point thePoint = newPoint.GetComponent<Point>();
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
        
    }


}
