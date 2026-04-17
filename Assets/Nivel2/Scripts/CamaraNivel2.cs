using UnityEngine;

public class CamaraNivel2 : MonoBehaviour
{
    [Header("Seguimiento")]
    public Transform player;
    public float smoothSpeed = 5f;
    public Vector3 offset;

    [Header("Limites normales")]
    public float minX = 0f;
    public float maxX = 100f;
    public float posicionY = 0f;

    [Header("Limites zona jefe (se activan con BloquearParaJefe)")]
    public float minXJefe = 50f;
    public float maxXJefe = 60f;

    private bool bloqueadoJefe = false;

    void LateUpdate()
    {
        if (player == null) return;

        float limiteMinX = bloqueadoJefe ? minXJefe : minX;
        float limiteMaxX = bloqueadoJefe ? maxXJefe : maxX;

        float objetivoX = player.position.x + offset.x;
        float suavizadoX = Mathf.Lerp(transform.position.x, objetivoX, smoothSpeed * Time.deltaTime);
        suavizadoX = Mathf.Clamp(suavizadoX, limiteMinX, limiteMaxX);

        transform.position = new Vector3(suavizadoX, posicionY + offset.y, -10f);
    }

    // Llamar desde TriggerJefe al activar al jefe
    public void BloquearParaJefe()
    {
        bloqueadoJefe = true;
    }

    public void DesbloquearCamara()
    {
        bloqueadoJefe = false;
    }
}
