using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    [SerializeField]
    private Sprite sprite;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") //if touches hitbox
        {
            Player player = collision.GetComponent<Player>();
            player.SetCurrentCheckpoint(this);
            spriteRenderer.sprite = sprite;
        }
    }
}
