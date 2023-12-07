using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
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
    public float intensity = 1f; // Intensity of the wobble effect
    private Dictionary<Transform, Vector3> initialPositions = new Dictionary<Transform, Vector3>();
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

    IEnumerator Wobble()
    {
        // Store the initial positions of the GameObjects
        foreach (Transform go in _liquid_drops)
        {
            initialPositions.Add(go, go.transform.position);
        }
        foreach (Transform go in _liquid_drops)
        {
            // Move the GameObject in a random direction
            go.transform.position += new Vector3(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity), 0);
        }

        // Wait for a bit
        yield return new WaitForSeconds(0.25f);

        foreach (Transform go in _liquid_drops)
        {
            // Move the GameObject back to its initial position
            go.transform.position = Vector3.Lerp(go.transform.position, initialPositions[go], 0.5f);
        }

        // Wait for a bit
        yield return new WaitForSeconds(0.25f);
    
}

    void AddToConnectionList(GameObject g)
    {
        GetLiquidBehaviour(g.GetComponent<LiquidBehaviour>());
        g.layer = default;
        connectedObjects.Add(g);
        //StartCoroutine(Wobble());   
    }
    [Header("particle effects")]
    public GameObject[] particleEffects;
        GameObject particleEffect;

    Vector3 GetCenterPosition(List<GameObject> transforms)
    {
        if (transforms.Count == 0)
        {
            return Vector3.zero;
        }

        Vector3 sum = Vector3.zero;
        foreach (GameObject t in transforms)
        {
            sum += t.transform.position;
        }
        return sum / transforms.Count;
    }

        [Header("Assemble liquids")]
        [SerializeField] float assembleTime=.25f;
        [SerializeField] float assembleSpeed=5f;
    // public Transform targetPos;
    public Target Coffee;
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

        StartCoroutine(AssembleLiquids());
        IEnumerator AssembleLiquids()
        {
            foreach (var g in connectedObjects)
            {
                g.GetComponent<Collider2D>().enabled = false;
                g.GetComponent<Rigidbody2D>().isKinematic = true;
            }
            Vector3 targetPos = GetCenterPosition(connectedObjects);
            float _assembleTime = assembleTime;
            while (_assembleTime > 0)
            {
                _assembleTime -= Time.deltaTime;
                foreach (GameObject g in connectedObjects)
                {
                    g.transform.position = Vector3.MoveTowards(g.transform.position, targetPos, assembleSpeed * Time.deltaTime);
                    yield return null;
                }
            }

            Coffee.ReceiveFluids(connectedObjects);

            foreach (var g in connectedObjects)
            {
                if (particleEffect == null) particleEffect = particleEffects[0];

                Instantiate(particleEffect, g.transform.position, Quaternion.identity);

                Destroy(g);
            }
            connectedObjects.Clear();
            _liquid_drops.Clear();
        }

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
        _liquid_drops.Add(lq.transform);
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
