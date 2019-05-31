using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker
{
    public List<NavMeshSurface> surfaces;

    public NavMeshBaker() {
        surfaces = new List<NavMeshSurface>();
    }

    public void buildNavMesh() {
        foreach(NavMeshSurface i in surfaces) {
            i.BuildNavMesh();
        }
    }

    public void addSurface(NavMeshSurface sur) {
        surfaces.Add(sur);
    }
}
