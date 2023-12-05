using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Connector : MonoBehaviour
{
    public LayerMask layerToDetect; // Set this layer in the Unity Editor
    public Camera mainCamera; // Assign your main camera here

    private bool isConnecting;
   [SerializeField]  private List<GameObject> connectedObjects = new List<GameObject>();
   [SerializeField]  private LayerMask currentSelectionLayerMask;
    [SerializeField] private FluidHandler fluidHandler;
    public float _min_distance_between_objects=1.5f;

    private List<Transform> _liquid_drops= new List<Transform>();
    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Check for a mouse button (left click) press
        if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
        {
            CheckForObjectUnderCursor();
        }

        if (isConnecting)
        {
            Debug.Log("Connecting");
            Vector2 ray = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero, 100f, currentSelectionLayerMask);

            // Perform the raycast
            if (hit.collider != null)
            {
                if (Vector3.Distance(connectedObjects[connectedObjects.Count-1].transform.position, hit.collider.transform.position)<_min_distance_between_objects)
                AddToConnectionList(hit.collider.gameObject);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isConnecting)
                DestroyTheConnectedObjects();
        }
    }

    void AddToConnectionList(GameObject g)
    {
        GetLiquidBehaviour(g.GetComponent<LiquidBehaviour>());
        g.layer = default;
        connectedObjects.Add(g);
    }
    [Header("particle effects")]
    public GameObject[] particleEffects;
        GameObject particleEffect;

    void DestroyTheConnectedObjects()
    {
       
        // Implement the logic to destroy connected objects
        isConnecting = false;
        switch (currentSelectionLayerMask)
        {
            case 7: particleEffect = particleEffects[0]; break;
            case 8: particleEffect = particleEffects[1]; break;
            case 9: particleEffect = particleEffects[2]; break;
            case 10: particleEffect = particleEffects[3]; break;
            case 11: particleEffect = particleEffects[4]; break;
            default: particleEffect = null; break;
        }

        foreach (var g in connectedObjects)
        {
            if (particleEffect == null) particleEffect = particleEffects[0];
            
                Instantiate(particleEffect, g.transform.position, Quaternion.identity);
            
            Destroy(g);
        }
        connectedObjects.Clear();
        // Reset currentSelectionLayerMask if needed
        // currentSelectionLayerMask = default;
    }

    void CheckForObjectUnderCursor()
    {
        Vector2 ray = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero, Mathf.Infinity, layerToDetect);

        // Perform the raycast                                
        if (hit.collider != null)
        {
            // A collider on the specified layer was hit by the ray
            //   Debug.Log("Object on the correct layer detected: " + hit.collider.gameObject.name, hit.collider.gameObject);
            currentSelectionLayerMask = hit.collider.GetComponent<Selectable>().layerMask;
            fluidHandler.ChangeFluidColor(hit.collider.GetComponent<Selectable>().color);
            GetLiquidBehaviour(hit.collider.GetComponent<LiquidBehaviour>());
            hit.collider.gameObject.layer = default;
            StartConnecting(hit.collider.gameObject);     
        }
    }

    void GetLiquidBehaviour(LiquidBehaviour lq)
    {
        lq?.StartLiquidBehaviour();
    }

    void StartConnecting(GameObject clickedObject)
    {
        connectedObjects.Clear();
        connectedObjects.Add(clickedObject);
        isConnecting = true;
    }

    private void LateUpdate()
    {
        for(int i=0;i<connectedObjects.Count-1;i++)
        {
            Debug.DrawLine(connectedObjects[i].transform.position, connectedObjects[i + 1].transform.position, Color.red);
        }
    }

    private void OnDrawGizmos()
    {
        // Optional: Implement any Gizmo drawing for debugging
    }
}
