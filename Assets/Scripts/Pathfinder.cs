using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{

    Dictionary<Vector2Int, Waypoint> grid = new Dictionary<Vector2Int, Waypoint>();

    [SerializeField] Waypoint startWaypoint, endWaypoint;

    Queue<Waypoint> queue = new Queue<Waypoint>();
    bool isRunning = true;
    Waypoint searchCenter;

    List<Waypoint> path = new List<Waypoint>();

    Vector2Int[] directions =
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };

    public List<Waypoint> GetPath()
    {
        if (path.Count == 0)
        {
            CalculatePath();
        }
            return path;
    }

    private void CalculatePath()
    {
        LoadBlocks();
        //ColorStartAndEnd();
        BreadthFirstSearch();
        CreatePath();
    }

    private void LoadBlocks()
    {
        var waypoints = FindObjectsOfType<Waypoint>();

        foreach (Waypoint waypoint in waypoints)
        {
            var gridPos = waypoint.GetGridPos();

            //overlapping blocks?
            if (grid.ContainsKey(gridPos))
            {
                Debug.LogWarning("Overlapping block " + waypoint);
            }
            else
            {
                //add to Dictionary
                grid.Add(gridPos, waypoint);

            }
        }

    }

   /* private void ColorStartAndEnd()
    {
        startWaypoint.SetTopColor(Color.green);
        endWaypoint.SetTopColor(Color.red);
    }*/

    private void ExploreNeighbours()
    {
        if (!isRunning) { return; }
        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighbourCoordinates = searchCenter.GetGridPos() + direction;
            if(grid.ContainsKey(neighbourCoordinates))
            {
                QueueNewNeighbours(neighbourCoordinates);
            }
            
        }
    }

    private void BreadthFirstSearch()
    {
        queue.Enqueue(startWaypoint);

        while (queue.Count > 0 && isRunning)
        {
            searchCenter = queue.Dequeue();
            searchCenter.isExplored = true;
            HaltIfEndFound();
            ExploreNeighbours();
        }
        print("Finished");
    }

    private void HaltIfEndFound()
    {
        if (searchCenter == endWaypoint)
        {
            print("End node, stopping");
            isRunning = false;
        }
    }

    private void QueueNewNeighbours(Vector2Int neighbourCoordinates)
    {
        Waypoint neighbour = grid[neighbourCoordinates];
        if (neighbour.isExplored || queue.Contains(neighbour))
        {
            //do nothing
        }
        else { 
            //neighbour.SetTopColor(Color.grey);
            queue.Enqueue(neighbour);
            neighbour.exploredFrom = searchCenter;
        }
    }

    private void CreatePath()
    {
        SetAsPath(endWaypoint);
        
        Waypoint previous = endWaypoint.exploredFrom;
        while(previous != startWaypoint)
        {
            // add intermediate waypoints

            SetAsPath(previous);
            previous = previous.exploredFrom;
        }
        //add start waypoint
        SetAsPath(startWaypoint);
        //reverse the list
        path.Reverse();
    }

    private void SetAsPath(Waypoint waypoint)
    {
        path.Add(waypoint);
        waypoint.isPlaceable = false;
    }
}