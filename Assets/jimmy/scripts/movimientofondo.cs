using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimientofondo : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float velocidadParallax = 0.2f;

    private Vector3 ultimaPosicionPlayer;
    private Vector2 offset;
    private Material material;

    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
        ultimaPosicionPlayer = player.position;
    }

    private void Update()
    {
        float movimientoX = player.position.x - ultimaPosicionPlayer.x;

        offset.x += movimientoX * velocidadParallax;

        material.mainTextureOffset = offset;

        ultimaPosicionPlayer = player.position;
    }
}