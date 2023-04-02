using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoDrop : MonoBehaviour
{
    [SerializeField] int rotationSpeed;
    [SerializeField] AudioClip AmmoDropClip;
    [SerializeField] Rigidbody2D rb;
    private float rotationZ;
    public int hp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(AmmoDropClip, transform.position, 1f);
            collision.GetComponent<Shooting>().AddAmmo();
            collision.GetComponent<Player>().AddHP(hp);
            Destroy(gameObject);
        }
    }

    void Update()
    {
        rotationZ += Time.deltaTime * rotationSpeed;
        rb.rotation = rotationZ;
    }
}
