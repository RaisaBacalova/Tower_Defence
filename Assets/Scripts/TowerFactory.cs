using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFactory : MonoBehaviour {

    [SerializeField] int towerLimit = 5;
    [SerializeField] Tower towerPrefab;
    [SerializeField] Transform towerParentTransform;


    //create an empty queue of towers
    Queue<Tower> towerQueue = new Queue<Tower>();

    public void AddTower(Waypoint baseWaypoint)
    {
        print(towerQueue.Count);
        int numTowers = towerQueue.Count;
        if (numTowers < towerLimit)
        {
            InstantiateNewTower(baseWaypoint);
        } else
        {
            MoveExistingTower(baseWaypoint);
        }
    }

    private void InstantiateNewTower(Waypoint baseWaypoint)
    {
        var newTower = Instantiate(towerPrefab, baseWaypoint.transform.position, Quaternion.identity);
        newTower.transform.parent = towerParentTransform;

        //set the placeable flags
        baseWaypoint.isPlaceable = false;

        //set the baseWaypoints
        newTower.baseWaypoint = baseWaypoint;
        baseWaypoint.isPlaceable = false;

        //put new tower on the queue
        towerQueue.Enqueue(newTower);

    }

    private void MoveExistingTower(Waypoint newbaseWaypoint)
    {
        //take bottom tower off queue
        var oldTower = towerQueue.Dequeue();

        //set the placeable flags
        oldTower.baseWaypoint.isPlaceable = true; //free-up the block(cube)
        newbaseWaypoint.isPlaceable = false;

        //set the baseWaypoints
        oldTower.baseWaypoint = newbaseWaypoint;
        //move the tower
        oldTower.transform.position = newbaseWaypoint.transform.position;

        //put the old tower on the queue
        towerQueue.Enqueue(oldTower);
    }

  
}
