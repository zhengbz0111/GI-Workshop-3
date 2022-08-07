// COMP30019 - Graphics and Interaction
// (c) University of Melbourne, 2022

using UnityEngine;

public class Triangle : SceneEntity
{
    [SerializeField] private Vector3 v1, v2, v3;

    public override RaycastHit? Intersect(Ray ray)
    {
        // By default we use the Unity engine for ray-entity collisions.
        // See the parent 'SceneEntity' class definition for details.
        // Task: Replace with your own intersection computations.
        return base.Intersect(ray);
    }
    
    public Vector3[] Vertices()
    {
        return new[] { this.v1, this.v2, this.v3 };
    }
}
