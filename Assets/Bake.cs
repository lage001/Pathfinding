using System.Collections;
using System.Collections.Generic;
using NavMeshPlus.Components;
using UnityEngine;

public class Bake : MonoBehaviour
{
    public NavMeshSurface navMesh;
    void Start()
    {
        navMesh.BuildNavMesh();
    }
}
