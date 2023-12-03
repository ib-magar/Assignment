using UnityEngine;

public class SpriteCreator : MonoBehaviour
{


    public PolygonCollider2D _collider;
    private void Start()
    {
        //CreateSpriteFromColliderData(_collider);
       // CreateSprite(_collider);    
    }
    public Material material;
    public LayerMask layerMask;
    public void CreateSprite(PolygonCollider2D polygonCollider)
    {
        int verticesCount = polygonCollider.points.Length;

        Vector3[] vertices = new Vector3[verticesCount];
        Vector2[] uv = new Vector2[verticesCount];
        //int[] triangles = new int[verticesCount];       //triangles array

        for (int i = 0; i < verticesCount; i++)
        {
            // Since PolygonCollider2D points are Vector2, we convert them to Vector3
            vertices[i] = polygonCollider.points[i];
        }

        int[] triangles = new int[(verticesCount - 2) * 3];   //triangles [0,1,2,0,2,3...]

        //vertices[0] = Vector3.zero;         //being the positions are relative to the player
        for (int i = 0; i < verticesCount - 1; i++)
        {
           // vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]) + transform.forward * _edgeCutOutThickness;        //for cutout pupose of the mask
            if (i < verticesCount - 2)
            {

                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }



        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;


         GameObject spriteObject = new GameObject("GeneratedSprite");
        //GameObject spriteObject = polygonCollider.gameObject;
        PolygonCollider2D newPolygonCollider= spriteObject.AddComponent<PolygonCollider2D>();
        newPolygonCollider.points= polygonCollider.points;
        spriteObject.layer = 13;
        MeshFilter meshfilter = spriteObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer= spriteObject.AddComponent<MeshRenderer>();
        meshRenderer.material=material;
        meshfilter.mesh = mesh;
        
        Destroy(polygonCollider.gameObject);
    }
    public void CreateSpriteFromColliderData(PolygonCollider2D polygonCollider)
    {
        if (polygonCollider != null)
        {
            print("sprite creation begun");

            // Get the points from the PolygonCollider2D
            Vector2[] points = polygonCollider.GetPath(0);

            // Create a texture and set its pixel colors
            Texture2D texture = new Texture2D(512, 512); // Change the size as needed
            Color[] colors = new Color[texture.width * texture.height];

            // Set all pixels to transparent initially
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = Color.clear;
            }

            // Create a sprite by drawing lines between collider points
            for (int i = 0; i < points.Length - 1; i++)
            {
                Vector2 startPoint = points[i] * 100f; // Scale up the points as needed
                Vector2 endPoint = points[i + 1] * 100f; // Scale up the points as needed

                DrawLine(texture, colors, (int)startPoint.x, (int)startPoint.y, (int)endPoint.x, (int)endPoint.y, Color.white);
            }

            // Apply the colors to the texture
            texture.SetPixels(colors);
            texture.Apply();

            // Create a new GameObject with SpriteRenderer
            GameObject spriteObject = new GameObject("GeneratedSprite");
            SpriteRenderer spriteRenderer = spriteObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = 10;
            // Create a sprite from the generated texture
            spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

            // Set the position of the sprite object
            spriteObject.transform.position = transform.position;
        }
        else
        {
            Debug.LogError("PolygonCollider2D reference is missing!");
        }
    }

    // Bresenham's line algorithm to draw lines between points
    void DrawLine(Texture2D texture, Color[] colors, int x0, int y0, int x1, int y1, Color color)
    {
        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int sx = (x0 < x1) ? 1 : -1;
        int sy = (y0 < y1) ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            if (x0 >= 0 && x0 < texture.width && y0 >= 0 && y0 < texture.height)
            {
                int index = y0 * texture.width + x0;
                colors[index] = color;
            }

            if (x0 == x1 && y0 == y1) break;
            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }
    }

}
