using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour {

    private CompletePlayerController player;

    private void Start()
    {
        player = GetComponentInParent<CompletePlayerController>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Projectiles"))
        {
            Destroy(col.gameObject);
            player.ShieldBlocked();
            Destroy(gameObject);
        }
    }
}
