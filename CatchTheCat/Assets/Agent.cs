using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    protected bool isActive = false;
    public void SetActive(int active)
    {
        isActive = active == 1 ? true : false;
    }
    float stepInterval = .5f;
    float stepElapsed = 0;

    protected List<Point> _frontier = new List<Point>();
    protected Point _frontierHead;
    protected List<Point> _reached = new List<Point>();
    protected Point _start;
    protected Point _goal;
    protected List<Point> _goals = new List<Point>();
    public Point GetClosestGoal(Point point)
    {
        if (_goals.Count < +0)
            return null;

        Point closestGoal = _goals[0];
        float lowestCost = World.instance.GetCostOf(point, _goals[0]);

        for(int i = 0; i < _goals.Count; i++)
        {
            if (World.instance.GetCostOf(point, _goals[i]) < lowestCost)
            {
                lowestCost = World.instance.GetCostOf(point, _goals[i]);
                closestGoal = _goals[i];
            }
        }

        return closestGoal;
    }
    protected Dictionary<Point, Point> _cameFromMap = new Dictionary<Point, Point>();
    protected Dictionary<Point, float> _costSoFar = new Dictionary<Point, float>();
    protected List<Point> _path = new List<Point>();

    public List<Point> GetNeighbors(Point point)
    {
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

    public virtual void BootStrap()
    {
        _start = World.instance.GetPointAt(new Vector2(0, 0));
        _goals.Add(World.instance.GetPointAt(new Vector2(9, 9)));
        //Debug.Log($"Goal:{_goal}");
        _frontier.Add(World.instance.GetPointAt(Vector2.zero));
        _cameFromMap.Add(World.instance.GetPointAt(Vector2.zero), World.instance.GetPointAt(Vector2.zero));
        _costSoFar.Add(World.instance.GetPointAt(Vector2.zero), World.instance.GetPointAt(Vector2.zero).Priority);
    }

    public virtual void Step()
    {
        Search();

    }

    public virtual void Search()
    {

        if (_frontier.Count <= 0)
            return;


		if (_frontierHead != null && CheckForGoal(_frontierHead))
		{
            _goal = _frontierHead;
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

            if(neighbor == null)
            {
                Debug.Log("The neighbor is null!");
            }
            float newCost = _costSoFar[_frontierHead] + World.instance.GetCostOf(_frontierHead, neighbor);
            neighbor.Neighbored();
            //Debug.Log($"Found neighbor of {_frontierHead}:{neighbor}");
            //New Cost
            
            if (!_costSoFar.ContainsKey(neighbor) || newCost < _costSoFar[neighbor])
            {
                _costSoFar[neighbor] = newCost;
                neighbor.Priority = newCost + World.instance.GetCostOf(GetClosestGoal(neighbor), neighbor); //Need heuristic
                _frontier.Add(neighbor);
                _cameFromMap[neighbor] = _frontierHead;
            }
            else
            {
                neighbor.Frontiered();
            }
            
        }

    }

    public virtual void CreatePath()
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

        MoveAlongPath();
    }

    public virtual void MoveAlongPath()
    {
        //Get next point in path
        //Set as start and construct new path
    }

    public bool CheckForGoal(Point point)
    {
        foreach(var goalPoint in _goals)
        {
            if (point.Coordinates == goalPoint.Coordinates)
                return true;
        }

        return false;

    }
}
