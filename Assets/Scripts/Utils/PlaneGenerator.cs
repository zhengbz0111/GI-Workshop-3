// COMP30019 - Graphics and Interaction
// (c) University of Melbourne, 2022

using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class PlaneGenerator : MonoBehaviour
{
    [SerializeField] private Plane plane;

    private void Awake()
    {
        var mesh = CreateMesh();

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    private Mesh CreateMesh()
    {
        var mesh = new Mesh
        {
            name = "Plane"
        };

        const float infinity = 1000f; // "Infinite" plane

        var rotation = Quaternion.LookRotation(this.plane.Normal);
        var vertices = new[]
        {
            new Vector3(1f, -1f, 0f),
            new Vector3(1f, 1f, 0f),
            new Vector3(-1f, 1f, 0f),
            new Vector3(-1f, -1f, 0f)
        }.Select(v => rotation * (v * infinity) + this.plane.Center);
        
        mesh.SetVertices(vertices.ToList());
        mesh.SetIndices(new[] { 0, 1, 2, 3 }, MeshTopology.Quads, 0);

        return mesh;
    }
}
