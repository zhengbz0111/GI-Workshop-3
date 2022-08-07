// COMP30019 - Graphics and Interaction
// (c) University of Melbourne, 2022

using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class SphereGenerator : MonoBehaviour
{
    [SerializeField] private Sphere sphere;
    
    private void Awake()
    {
        // Using built-in sphere mesh for convenience.
        // So we just enable the renderer to "generate" it.
        GetComponent<MeshRenderer>().enabled = true;
        
        // Scale/position sphere mesh (rotation doesn't do much).
        this.transform.localPosition = this.sphere.Center;
        this.transform.localScale = Vector3.one * this.sphere.Radius * 2f;

        // Create collider after transform so it inherits it.
        this.gameObject.AddComponent<SphereCollider>();
    }
}
