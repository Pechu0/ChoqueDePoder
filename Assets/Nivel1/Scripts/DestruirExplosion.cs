using UnityEngine;

public class DestruirExplosion : MonoBehaviour
{
    [SerializeField] private float duracion = -1f; // -1 = usa la duracion del clip actual

    void Start()
    {
        float tiempo = duracion;

        if (tiempo <= 0f)
        {
            Animator anim = GetComponent<Animator>();
            if (anim != null)
            {
                AnimatorClipInfo[] clips = anim.GetCurrentAnimatorClipInfo(0);
                if (clips.Length > 0)
                    tiempo = clips[0].clip.length;
            }
        }

        if (tiempo <= 0f) tiempo = 1f; // fallback

        Destroy(gameObject, tiempo);
    }
}
