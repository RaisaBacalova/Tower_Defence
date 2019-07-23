﻿using System;
using UnityEngine;

public class CubeEditor: MonoBehaviour {

    [ExecuteInEditMode]
    //[RequireComponent(typeof(Waypoint))]

    
    Waypoint waypoint;

    private void Awake()
    {
        waypoint = GetComponent<Waypoint>();
    }

    void Update()
    {
        SnapToGrid();
        UpdateLabel();   
    }

    private void SnapToGrid()
    {
        int gridSize = waypoint.GetGridSize();
        
        transform.position = new Vector3(
            waypoint.GetGridPos().x * gridSize, 
            0f,
            waypoint.GetGridPos().y * gridSize
            );
    }

    private void UpdateLabel()
    {
        TextMesh textMesh = GetComponentInChildren<TextMesh>();
        
        string labelText = 
            waypoint.GetGridPos().x + 
            "," + 
            waypoint.GetGridPos().y;

        textMesh.text = labelText;
        gameObject.name = labelText;
    }
}
