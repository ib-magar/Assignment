using System.Collections;
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

    public void initialize(Vector2 dir)
    {
        velocity = dir * velocityMag;
    }

    private void Update()
    {
        rb.velocity = velocity;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(cutter==null) cutter = GameObject.FindObjectOfType<LinecastCutterBehaviour>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int objLayerMask = 1 << collision.gameObject.layer;
        if ((layerMask.value & objLayerMask) != 0)
        {
            mouseStart = transform.position;
        }
        else
        {
            if(collision.CompareTag("right")|| collision.CompareTag("left"))
            {
                velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
            }
            else if(collision.CompareTag("up"))
            {
                velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
            }
            else if(collision.CompareTag("down"))
            {
                Destroy(gameObject);
            }
          /*  Debug.Log("colliding with wall   "+tag);
            switch(tag)
            {
                case "left": velocity = new Vector2(-rb.velocity.x, rb.velocity.y); break;
                case "right": velocity = new Vector2(-rb.velocity.x, rb.velocity.y); break;
                case "up": velocity = new Vector2(rb.velocity.x, -rb.velocity.y); break;
                case "down":velocity = new Vector2(rb.velocity.x, -rb.velocity.y); break;
                    default: break;

            }*/

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        mouseEnd = transform.position;
        cutter.LinecastCut(mouseStart, mouseEnd, layerMask.value);
    }
}
