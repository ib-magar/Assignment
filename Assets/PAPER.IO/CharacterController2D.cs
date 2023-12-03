using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of character movement

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
    }

    void Update()
    {
        Vector2 movement = Vector2.zero; // Initialize movement as zero vector

        // Get input from WASD keys
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        // Normalize the movement vector to ensure consistent speed in all directions
        movement.Normalize();

        // Move the character based on the input
        rb.velocity = movement * moveSpeed;

        // Rotate the character's up direction towards the movement direction
        if (movement != Vector2.zero)
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        }
    }
}
