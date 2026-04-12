using UnityEngine;

public class MotoFlip : MonoBehaviour
{
    private float lastXPosition;
    private bool facingRight = true;

    void Start()
    {
        // Inicializamos la posición para comparar en el primer frame
        lastXPosition = transform.position.x;
    }

    void Update()
    {
        float currentX = transform.position.x;

        // Verificamos si se ha movido significativamente para evitar "jittering"
        if (currentX < lastXPosition && facingRight)
        {
            Flip();
        }
        else if (currentX > lastXPosition && !facingRight)
        {
            Flip();
        }

        lastXPosition = currentX;
    }

  void Flip()
{
    facingRight = !facingRight;

    if (facingRight)
    {
        // Rotación normal (mirando a la derecha)
        transform.eulerAngles = new Vector3(0, 0, 0);
    }
    else
    {
        // Rotación de 180 grados (mirando a la izquierda)
        transform.eulerAngles = new Vector3(0, 180, 0);
    }
}
}
