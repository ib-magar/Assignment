using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class CustomPolygonCollider : MonoBehaviour
{
    public PolygonCollider2D polygonCollider;

    void Awake()
    {
        // Get the PolygonCollider2D component attached to the GameObject
        polygonCollider = GetComponent<PolygonCollider2D>();

        // Initialize the collider with a triangle shape
       
    }

   public void InitializeCollider(Vector2 pos)
    {
        Vector2[] initialVertices = new Vector2[]
       {
           pos,
            pos,
           pos
       };
        polygonCollider.SetPath(0, initialVertices);
    }
   public void InitializeCollider(Vector2 pos1, Vector2 pos2, Vector2 pos3)
    {
        Vector2[] initialVertices = new Vector2[]
       {
           pos1,
            pos2,
           pos3
       };
        polygonCollider.SetPath(0, initialVertices);
    }


    // Function to add a vertex to the collider
    public void AddVertex(Vector2 newVertex)
    {
        // Get the current vertices of the collider
        Vector2[] vertices = polygonCollider.points;

        // Create a new array with one extra slot for the new vertex
        Vector2[] newVertices = new Vector2[vertices.Length + 1];

        // Copy the existing vertices to the new array
        for (int i = 0; i < vertices.Length; i++)
        {
            newVertices[i] = vertices[i];
        }

        // Add the new vertex to the end of the array
        newVertices[newVertices.Length - 1] = newVertex;

        // Update the collider's vertices
        polygonCollider.SetPath(0, newVertices);
    }
}
