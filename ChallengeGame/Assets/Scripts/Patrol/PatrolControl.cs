using System;
using UnityEngine;

public class PatrolControl : MonoBehaviour
{
    public Paths[] paths;
    

    public Transform GetPoint(int indexEnemy)
    {
        Paths path = paths[indexEnemy];
        Transform transformPoint = path.points[path.indexPoint];
        path.indexPoint++;
        if (path.indexPoint >= path.points.Length)
            path.indexPoint = 0;

        return transformPoint;
    }
    
}


[Serializable]
public class Paths
{
    public Transform[] points;
    public int indexPoint;
}
