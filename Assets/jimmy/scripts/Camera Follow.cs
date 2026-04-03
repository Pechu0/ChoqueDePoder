using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 5f;
    public Vector3 offset;

   void LateUpdate()
{
    transform.position = new Vector3(
        player.position.x,
        player.position.y,
        -10f
    );
}
}
