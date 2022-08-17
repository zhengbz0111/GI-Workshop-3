// COMP30019 - Graphics and Interaction
// (c) University of Melbourne, 2022

using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Scene : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] [Range(1, 179)] private float fieldOfView = 60f;
    
    [SerializeField] private LinesGenerator debug;

    private float _imagePlaneHeight;
    private float _imagePlaneWidth;
    
    private void Start()
    {
        // Figure out how the image is scaled in the world (the image "plane").
        ComputeWorldImageBounds();
        
        // Place the image in the world accordingly, so we can visualise this.
        EmbedImageInWorld();

        // Perform ray tracing to render the image.
        Render();
        
        // Add additional visualisations to help debug things.
        DebugVisualisations();
    }

    private void DebugVisualisations()
    {
        // Here you may use "debug rays" to visualise rays in the scene.

        // Image plane "corner" rays first (frustum edges).
        this.debug.Ray(new Ray(Vector3.zero, NormalizedImageToWorldCoord(0f, 0f)), Color.blue);
        
        // Add more rays to visualise here...
        this.debug.Ray(new Ray(Vector3.zero, NormalizedImageToWorldCoord(1f, 1f)), Color.blue);
        this.debug.Ray(new Ray(Vector3.zero, NormalizedImageToWorldCoord(0f, 1f)), Color.blue);
        this.debug.Ray(new Ray(Vector3.zero, NormalizedImageToWorldCoord(1f, 0f)), Color.blue);


        this.debug.Ray(new Ray(Vector3.zero, NormalizedImageToWorldCoord(0.5f/9f, 0.5f/9f)), Color.white);

        for (float i = 0.5f; i < 9f; i+=1.0f) 
        {
            for(float j = 0.5f; j < 9f; j+=1.0f){
                this.debug.Ray(new Ray(Vector3.zero, NormalizedImageToWorldCoord(i/9f, j/9f)), Color.white);

            }
            
        }
        
    }   

    private void Render()
    {
        // Render the image here...
        for (int i = 0; i < this.image.Width; i+=1) 
        for(int j = 0; j < this.image.Height; j+=1)
        {
            this.image.SetPixel(i,j, Color.black);

            var ray = PixelRay(i, j);
            RaycastHit? nearestHit = null;
            foreach (var sceneEntity in FindObjectsOfType<SceneEntity>())
            {
                var hit = sceneEntity.Intersect(ray);
                // Note: sceneEntity could actually be a Triangle OR Plane OR Sphere.
                // But does it matter for the purposes of this exercise?
                if (hit != null && (hit?.distance < nearestHit?.distance || nearestHit == null))
                {
                this.image.SetPixel(i, j, sceneEntity.Color());
                nearestHit = hit;
                }
                
            }

        }
    }

    private Ray PixelRay(int x, int y)
    {
        var normX = (x + 0.5f) / this.image.Width;
        var normY = (y + 0.5f) / this.image.Height;

        var worldPixelCoord = NormalizedImageToWorldCoord(normX, normY);

        return new Ray(Vector3.zero, worldPixelCoord);
    }

    private Vector3 NormalizedImageToWorldCoord(float x, float y)
    {
        return new Vector3(
            this._imagePlaneWidth * (x - 0.5f),
            this._imagePlaneHeight * (0.5f - y),
            1.0f); // Image plane is 1 unit from camera.
    }

    private void ComputeWorldImageBounds()
    {
        var aspectRatio = (float)this.image.Width / this.image.Height;
        var fovLength = Mathf.Tan(this.fieldOfView / 2f * Mathf.Deg2Rad) * 2f;

        // Note: We are using vertical FOV here.
        this._imagePlaneHeight = fovLength;
        this._imagePlaneWidth = this._imagePlaneHeight * aspectRatio;
    }

    private void EmbedImageInWorld()
    {
        this.image.transform.position = new Vector3(0f, 0f, 1f);
        this.image.transform.localScale = new Vector3(this._imagePlaneWidth, this._imagePlaneHeight, 0f);
    }
}
