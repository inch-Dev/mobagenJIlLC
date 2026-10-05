using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    bool isActive = false;
    public void SetActive(int active)
    {
        isActive = active == 1 ? true : false;
    }
    float stepInterval = .5f;
    float stepElapsed = 0;

    List<Point> _frontier = new List<Point>();
    Point _frontierHead;
    List<Point> _reached = new List<Point>();
    Point _start;
    Point _goal;
    Dictionary<Point, Point> _cameFromMap = new Dictionary<Point, Point>();
    Dictionary<Point, float> _costSoFar = new Dictionary<Point, float>();
    List<Point> _path = new List<Point>();

    public List<Point> GetNeighbors(Point point)
    {


        //Use cube coordinates
        List<Point> neighbors = new List<Point>();

        List<Vector2> evenDirections = new List<Vector2>();
        evenDirections.Add(new Vector2(1, 0));
        evenDirections.Add(new Vector2(-1, 0));
        evenDirections.Add(new Vector2(1, 1));
        evenDirections.Add(new Vector2(0, 1));
        evenDirections.Add(new Vector2(1, -1));
        evenDirections.Add(new Vector2(0, -1));

        List<Vector2> oddDirections = new List<Vector2>();
        oddDirections.Add(new Vector2(1, 0));
        oddDirections.Add(new Vector2(-1, 0));
        oddDirections.Add(new Vector2(0, 1));
        oddDirections.Add(new Vector2(-1, 1));
        oddDirections.Add(new Vector2(0, -1));
        oddDirections.Add(new Vector2(-1, -1));


        List<Vector2> neighborDirections = ((int)point.Coordinates.y & 1) == 0 ? evenDirections : oddDirections;

        //Debug.Log($"Neighor Directions is even:{neighborDirections == evenDirections}");

        for(int i = 0; i < neighborDirections.Count; i++)
        {
            if(World.instance.GetPointAt(point.Coordinates + neighborDirections[i]))
                neighbors.Add(World.instance.GetPointAt(point.Coordinates + neighborDirections[i]));
        }

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
        if (isActive)
        {
            stepElapsed += Time.deltaTime;
            if (stepElapsed >= stepInterval)
            {
                stepElapsed = 0;
                Step();
            }
        }
    }

    public void BootStrap()
    {
        _start = World.instance.GetPointAt(new Vector2(0, 0));
        _goal = World.instance.GetPointAt(new Vector2(9, 9));
        Debug.Log($"Goal:{_goal}");
        _frontier.Add(World.instance.GetPointAt(Vector2.zero));
        _cameFromMap.Add(World.instance.GetPointAt(Vector2.zero), World.instance.GetPointAt(Vector2.zero));
        _costSoFar.Add(World.instance.GetPointAt(Vector2.zero), World.instance.GetPointAt(Vector2.zero).Priority);
    }

    void Search()
    {
        while(_frontier.Count > 0 )
        {
        
            Step();
        }

    }

    void Step()
    {

        if (_frontier.Count <= 0)
            return;


		if (_frontierHead != null && CheckForGoal(_frontierHead))
		{
			Debug.Log($"Found goal at {_frontierHead.Coordinates}");
            _frontier.Clear();
            CreatePath();


			return;
		}
		Debug.Log("Searching...");

        foreach(var point in _frontier)
        {
            point.Frontiered();
        }

        
        _frontierHead = GetLowestPriority(_frontier);
        _frontierHead.FrontierHead();


        
        _frontier.Remove(_frontierHead);
        {
            _frontierHead.Reached();
        }



        List<Point> neighbors = GetNeighbors(_frontierHead);
        foreach (var neighbor in neighbors)
        {
            if (neighbor.Type == PointType.WALL)
            {
                //Debug.Log("Found wall");
                continue;
            }
            float newCost = _costSoFar[_frontierHead] + World.instance.GetCostOf(_frontierHead, neighbor);
            neighbor.Neighbored();
            //Debug.Log($"Found neighbor of {_frontierHead}:{neighbor}");
            //New Cost
            
            if (!_costSoFar.ContainsKey(neighbor) || newCost < _costSoFar[neighbor])
            {
                _costSoFar[neighbor] = newCost;
                neighbor.Priority = newCost + World.instance.GetCostOf(_goal, neighbor); //Need heuristic
                _frontier.Add(neighbor);
                _cameFromMap[neighbor] = _frontierHead;
            }
            else
            {
                neighbor.Frontiered();
            }
            
        }

    }

    void CreatePath()
    {
        Point currentPoint = _goal;

        while(currentPoint != _start)
        {
            currentPoint.Pathed();
            _path.Add(currentPoint);
            currentPoint = _cameFromMap[currentPoint];
        }
        _path.Add(_start);
        _start.Pathed();
    }

    bool CheckForGoal(Point point)
    {
        return point.Coordinates == _goal.Coordinates;
    }
}
