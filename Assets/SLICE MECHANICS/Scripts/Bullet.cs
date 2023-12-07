using System.Collections;
using System.Collections.Specialized;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector2 mouseStart;
    Vector2 mouseEnd;
    public LayerMask layerMask;
    public LinecastCutterBehaviour cutter;

    [SerializeField] Vector2 velocity = Vector2.one;
    private Rigidbody2D rb;
    public float velocityMag;

    public int ReflectCount = 5;
    private void Awake()
    {
        Physics2D.queriesStartInColliders = false;
        //layerMask = default;

    }
    public void initialize(Vector2 dir)
    {
        velocity = dir * velocityMag;
    }
      public void initialize(Vector2 dir,LayerMask l)
    {
        velocity = dir * velocityMag;
        layerMask = l;
        Debug.Log("received layermas: " + layerMask.value);
    }

    private void Update()
    {
        rb.velocity = velocity;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (cutter == null) cutter = GameObject.FindObjectOfType<LinecastCutterBehaviour>();
    }

    public LayerMask[] _layers;

    private bool hasEnter = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        /* if (layerMask == default)
         {
             for (int i = 0; i < _layers.Length; i++)
             {
                 if (((1 << collision.gameObject.layer) & _layers[i]) != 0)
                 {
                     // Assign the layer mask value to the variable
                     layerMask = collision.gameObject.layer;
                     break;
                 } 
             }
         }*/
       

        int objLayerMask = 1 << collision.gameObject.layer;
        if ((layerMask.value & objLayerMask) != 0)
        {
            mouseStart = transform.position;
            hasEnter = true;
        }
        else
        {

            // Cast a ray to approximate the contact point
             Vector3 raycastOrigin = transform.position;
            //Vector3 raycastDirection = collision.transform.position - raycastOrigin;
            RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, velocity.normalized, 5);

            if (hit.collider != null )
            {
                // Calculate the reflected velocity based on the approximate contact point
                Vector3 contactPoint = hit.point;
                Vector2 reflectedVelocity = Vector2.Reflect(velocity, hit.normal);
                velocity = reflectedVelocity;

                if (hasEnter)
                {
                    mouseEnd = transform.position;
                    CutShape();
                }

                ReflectCount--;
                if(ReflectCount<=0f)
                    Destroy(gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        int objLayerMask = 1 << collision.gameObject.layer;
        if ((layerMask.value & objLayerMask) != 0 && hasEnter)
        {

            mouseEnd = transform.position;
            CutShape();
        }
    }
    void CutShape()
    {
        cutter.LinecastCut(mouseStart, mouseEnd, layerMask.value);
        hasEnter = false;
        
    }
}
