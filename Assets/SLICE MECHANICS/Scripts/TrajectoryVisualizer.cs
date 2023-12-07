using UnityEngine;

public class TrajectoryVisualizer : MonoBehaviour
{
    public LineRenderer trajectoryLineRenderer;
    public LayerMask reflectionLayerMask;

    private void Start()
    {
        trajectoryLineRenderer.positionCount = 3;
        trajectoryLineRenderer.SetPosition(0,transform.position);
    }
    void Update()
    {
        // Get mouse position in world coordinates
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Raycast from object's position to mouse position
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (mousePosition - (Vector2)transform.position).normalized, Mathf.Infinity,reflectionLayerMask);

        if (hit.collider != null)
        {
            // Calculate reflection direction
            Vector2 incomingDirection = (mousePosition -(Vector2) transform.position).normalized;
            Vector2 reflectionDirection = Vector2.Reflect(incomingDirection, hit.normal);

            // Visualize trajectory with LineRenderer
            //trajectoryLineRenderer.positionCount = 3;
            trajectoryLineRenderer.SetPosition(1, hit.point);
            trajectoryLineRenderer.SetPosition(2, hit.point + reflectionDirection * 5f); // Adjust length as needed
        }
       /* else
        {
            // No collision detected, disable LineRenderer
           // trajectoryLineRenderer.positionCount = 2;
            trajectoryLineRenderer.SetPosition(1,mousePosition);
            trajectoryLineRenderer.SetPosition(2,mousePosition);

        }*/
    }
}
