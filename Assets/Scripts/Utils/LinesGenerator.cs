// COMP30019 - Graphics and Interaction
// (c) University of Melbourne, 2022

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class LinesGenerator : MonoBehaviour
{
    [SerializeField] private float defaultRayDistance = 100f;
    
    private List<(Vector3, Vector3, Color)> _lines;
    private bool _dirty;
    private MeshFilter _meshFilter;
    
    private void Awake()
    {
        this._lines = new List<(Vector3, Vector3, Color)>();
        this._meshFilter = GetComponent<MeshFilter>();
    }

    private void Update()
    {
        // The dirty flag ensures we only re-generate the mesh if it changes.
        // Otherwise, regenerating a mesh each frame is expensive!
        if (this._dirty)
        {
            this._meshFilter.mesh = GenerateMesh();
            this._dirty = false;
        }
    }
    
    public void Ray(Ray ray, Color? color = null, float? distance = null)
    {
        this._lines.Add((
            ray.origin, 
            ray.GetPoint(distance ?? defaultRayDistance), 
            color ?? Color.white));
        RegenerateNextFrame(); 
    }
    
    public void Line(Vector3 a, Vector3 b, Color color)
    {
        this._lines.Add((a, b, color));
        RegenerateNextFrame();
    }

    private Mesh GenerateMesh()
    {
        // Procedurally generate a mesh that is comprised of "lines".
        // A special mesh topology is used to do this (MeshTopology.Lines).

        var mesh = new Mesh
        {
            name = "LineMesh"
        };

        var vertices = new List<Vector3>();
        var colors = new List<Color>();
        var lines = new List<int>();

        this._lines.ForEach(debugLine =>
        {
            var (a, b, color) = debugLine;
            
            vertices.AddRange(new[] { a, b });
            colors.AddRange(Enumerable.Repeat(color, 2));
            lines.AddRange(Enumerable.Range(vertices.Count - 2, 2));
        });

        mesh.SetVertices(vertices);
        mesh.SetColors(colors);
        mesh.SetIndices(lines, MeshTopology.Lines, 0);

        return mesh;
    }

    private void RegenerateNextFrame()
    {
        this._dirty = true;
    }
}
