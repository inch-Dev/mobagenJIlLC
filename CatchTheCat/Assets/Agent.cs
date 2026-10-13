using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Agent : MonoBehaviour
{
    protected bool wasSearched;
    [SerializeField] protected bool isAgent;
    public void SetIsAgent(bool shouldAgent){ isAgent = shouldAgent; }
    protected List<Point> _frontier = new List<Point>();
    protected Point _frontierHead;
    protected List<Point> _reached = new List<Point>();
    protected Point _start;
    protected Point _goal;
    protected List<Point> _goals = new List<Point>();

    protected Point _position;
    public Point GetPosition(){ return _position; }
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

    public float Heuristic(Point point)
    {
        return Mathf.Min(World.instance.GetXSize() / 2 - Mathf.Abs(point.Coordinates.x), World.instance.GetYSize() / 2 - Mathf.Abs(point.Coordinates.y));
    }

    public virtual void BootStrap()
    {
        _start = World.instance.GetPointAt(new Vector2(0, 0));

        for (int i = 0; i < World.instance.GetXSize(); i++)
        {
            for (int j = 0; j < World.instance.GetYSize(); j++)
            {
                if (i == 0 || j == 0 || i == World.instance.GetXSize() - 1 || j == World.instance.GetYSize() - 1)
                    _goals.Add(World.instance.GetPointAt(new Vector2(i, j)));
            }
        }
        _frontier.Add(World.instance.GetPointAt(Vector2.zero));
        _cameFromMap.Add(World.instance.GetPointAt(Vector2.zero), World.instance.GetPointAt(Vector2.zero));
        _costSoFar.Add(World.instance.GetPointAt(Vector2.zero), World.instance.GetPointAt(Vector2.zero).Priority);
    }

    public virtual void Search()
    {
		if (wasSearched)
		{
			_start = _position;

			_frontier.Clear();
			_frontierHead = null;
			_cameFromMap.Clear();
			_costSoFar.Clear();
			_path.Clear();


			_frontier.Add(_start);
			_cameFromMap.Add(_start, _start);
			_costSoFar.Add(_start, 0);

		}
		while (_frontier.Count > 0)
        {
            Step();
        }
            
    }

    public virtual void Step()
    {
                
        _frontierHead = GetLowestPriority(_frontier);
		_frontier.Remove(_frontierHead);
		_frontierHead.FrontierHead();
        _frontierHead.Reached();


		if (_frontierHead != null && CheckForGoal(_frontierHead))
		{
            Debug.Log($"Found goal at:{_frontierHead}");
            _goal = _frontierHead;
            _frontier.Clear();
            CreatePath();
			return;
		}

        foreach(var point in _frontier)
        {
            point.Frontiered();
        }

        List<Point> neighbors = GetNeighbors(_frontierHead);
        foreach (var neighbor in neighbors)
        {
            if (neighbor.Type == PointType.WALL)
            {
                continue;
            }

            float newCost = _costSoFar[_frontierHead] + World.instance.GetCostOf(_frontierHead, neighbor);
            neighbor.Neighbored();

            if (!_costSoFar.ContainsKey(neighbor) || newCost < _costSoFar[neighbor])
            {
                _costSoFar[neighbor] = newCost;
                neighbor.Priority = newCost + Heuristic(neighbor);
                _frontier.Add(neighbor);
                _cameFromMap[neighbor] = _frontierHead;
            }
            else
            {
                neighbor.Frontiered();
            }
            
        }
		if (_frontier.Count <= 0)
		{
			CreatePath();
			return;
		}

		wasSearched = true;
    }

    public virtual void CreatePath()
    {
        //Debug.Log("Creating path...");
        Point currentPoint = _goal;

        //Debug.Log($"Goal:{currentPoint}. Start:{_start}");

        while(currentPoint != _start)
        {
            if (_cameFromMap.Count <= 0)
                break;

            currentPoint.Pathed();
            
            if (_cameFromMap.ContainsKey(currentPoint))
            {
				_path.Add(currentPoint);
				currentPoint = _cameFromMap[currentPoint];
            }
            else
            {
                break;
            }
        }
        _path.Reverse();
        _start.Pathed();

        Behavior();
    }

    public virtual void Behavior()
    {
        World.instance.ResetPoints();
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
