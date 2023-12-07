using Unity.VisualScripting;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField] bool canShoot=false;

    private void Start()
    {
        //currentCutLayer = default;
    }
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = new Vector2(
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y
        );

        transform.up = direction;


        if(Input.GetMouseButtonDown(0)&&canShoot)
        {
            Shoot(spawnPos.transform.up);
        }
    }
    public Bullet bullet;
    public Transform spawnPos;
    public LayerMask currentCutLayer;
    void Shoot(Vector2 dir)
    {
           // canShoot = false;
        Bullet b = Instantiate(bullet, spawnPos.position, Quaternion.identity);
        //b.initialize(dir);
        b.initialize(dir, currentCutLayer);
        b.layerMask = currentCutLayer.value;
        Debug.Log("sent layermas:" + currentCutLayer.value);
        
    }
}
