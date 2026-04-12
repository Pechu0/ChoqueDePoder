using UnityEngine;

public class FlipFinal : MonoBehaviour 
{
    private float escalaXOriginal;

    void Start() {
        // Guardamos el tamaño que le pusiste en el editor (ej: 2.5)
        escalaXOriginal = Mathf.Abs(transform.localScale.x);
    }

    void Update() {
        float h = Input.GetAxisRaw("Horizontal");

        if (h > 0) {
            transform.localScale = new Vector3(escalaXOriginal, transform.localScale.y, transform.localScale.z);
        }
        else if (h < 0) {
            transform.localScale = new Vector3(-escalaXOriginal, transform.localScale.y, transform.localScale.z);
        }
    }
}