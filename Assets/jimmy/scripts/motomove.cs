using UnityEngine;

public class GiroRueda : MonoBehaviour
{
    public float velocidad = 300f;

    void Update()
    {
        transform.Rotate(0f, 0f, -velocidad * Time.deltaTime);
    }
}