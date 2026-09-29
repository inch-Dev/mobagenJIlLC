using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{

    List<Point> _frontier;
    Point _frontierHead;
    List<Point> _reached;
    Dictionary<Point, Point> _cameFromMap;

    public List<Point> GetNeighbors(Point point)
    {
        List<Point> neighbors = new List<Point>();

        List<Vector2> directions = new List<Vector2>();
        directions.Add(new Vector2(1, 1));
        directions.Add(Vector2.right);
        directions.Add(new Vector2(1, -1));
        directions.Add(new Vector2(-1, -1));
        directions.Add(Vector2.left);
        directions.Add(new Vector2(-1, 1));

        return neighbors;
    }

    public Point GetLowerPriority(Point pointA, Point pointB)
    {
        if (pointA.Priority < pointB.Priority)
            return pointA;
        else if (pointB.Priority < pointA.Priority)
            return pointB;
        else
        {
            int randomIndex = Random.Range(0, 2);
            switch (randomIndex)
            {
                case 0:
                    return pointA;
                case 1:
                    return pointB;
            }
        }

        return null;
    }

    public Point GetLowestPriority(List<Point> points)
    {
        var lowPoint = points[0];

        if (points.Count <= 1)
            return lowPoint;

        for(int i = 1; i < points.Count; i++)
        {
            lowPoint = GetLowerPriority(lowPoint, points[i]);
        }

        return lowPoint;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void BootStrap()
    {
        _frontier.Add(World.instance.GetPointAt(Vector2.zero));
        _cameFromMap.Add(World.instance.GetPointAt(Vector2.zero), World.instance.GetPointAt(Vector2.zero));
        Search();
    }

    void Search()
    {
    //Change to do in steps
        _frontierHead = GetLowestPriority(_frontier);


        while(_frontier.Count > 0)
        {
            foreach(var neighbor in GetNeighbors(_frontierHead))
            {
                if(_reached!.Contains(neighbor))
                {
                    _frontier.Add(neighbor);
                    neighbor.Frontiered();
                    _reached.Add(neighbor);
                    neighbor.Reached();
                    _cameFromMap[neighbor] = _frontierHead;
                }
            }
        }

    }
}
