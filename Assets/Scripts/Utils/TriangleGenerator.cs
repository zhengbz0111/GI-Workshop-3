// COMP30019 - Graphics and Interaction
// (c) University of Melbourne, 2022

using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class TriangleGenerator : MonoBehaviour
{
    [SerializeField] private Triangle triangle;
    
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
            name = "Triangle"
        };
        
        mesh.SetVertices(this.triangle.Vertices());
        mesh.SetTriangles(new[] { 0, 1, 2 }, 0);

        return mesh;
    }
}
