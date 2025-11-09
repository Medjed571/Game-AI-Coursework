using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// script that is called for steering behaviours that require the location of another agent in the scene.
/// </summary>

public class Targetting : MonoBehaviour
{
    public Vector3 ClosestEnemy;

    private List<Vector3> enemyCoordList = new List<Vector3>();

    private void Update()
    {
        
    }
}
