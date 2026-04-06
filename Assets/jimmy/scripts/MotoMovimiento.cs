using UnityEngine;

public class MotoMovimiento : MonoBehaviour
{
    public float velocidad = 5f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float input = 0f;

        if (Input.GetKey(KeyCode.A))
            input = -1f;

        if (Input.GetKey(KeyCode.D))
            input = 1f;

        // Movimiento
        rb.velocity = new Vector2(input * velocidad, rb.velocity.y);

        // Flip basado en dirección
        if (input > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (input < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }
}