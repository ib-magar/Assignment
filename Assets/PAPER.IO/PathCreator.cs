using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    public LayerMask layerMask;
    [SerializeField] bool isPathing;

    [SerializeField] float readPathInterval;
    private float readpathintervalCounter;
    private void Start()
    {
        isPathing = false;
        readpathintervalCounter = readPathInterval;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        int objLayerMask = 1 << collision.gameObject.layer;
        if ((layerMask.value & objLayerMask) != 0)
        {
            //provide the vector2 data to the polygon Collider script 
            isPathing = true;

            // SendData(transform.position);   //instead we can send the collider data also.
            InitializePolygonCollider(transform.position);
        }
    }

    private void Update()
    {
        if(isPathing)
        {
            readpathintervalCounter-=Time.deltaTime;
            if(readpathintervalCounter<=0f)
            {
                readpathintervalCounter = readPathInterval;
                //Send position to the script
                AddVertexPoint(transform.position);
            }
        }
    }
    public CustomPolygonCollider AreaPrefab;
    private CustomPolygonCollider currentAreaPrefab;
    void InitializePolygonCollider(Vector2 pos)
    {
        currentAreaPrefab = Instantiate(AreaPrefab, transform.position, Quaternion.identity);
        currentAreaPrefab.InitializeCollider(pos);
    }
    void AddVertexPoint(Vector2 pos)
    {
        //send position
        currentAreaPrefab.AddVertex(pos);
    }

    [Header("Sprite Creation")]
    public SpriteCreator _spriteCreator;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int objLayerMask = 1 << collision.gameObject.layer;
        if ((layerMask.value & objLayerMask) != 0 && isPathing)
        {
            isPathing = false;
            readpathintervalCounter = readPathInterval;
            if(currentAreaPrefab != null)
            {
                //_spriteCreator.CreateSpriteFromColliderData(currentAreaPrefab.GetComponent<PolygonCollider2D>());
                _spriteCreator.CreateSprite(currentAreaPrefab.GetComponent<PolygonCollider2D>());
            //provide the vector2 data to the polygon Collider script And for the sprite out of it

            }
        }
    }

}
