using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoDrop : MonoBehaviour
{
    [SerializeField] int rotationSpeed;
    [SerializeField] AudioClip AmmoDropClip;
    [SerializeField] TMP_Text infoText;
    [SerializeField] Rigidbody2D rb;
    private float rotationZ;

    void Start()
    {
        infoText.text = "AMMO";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(AmmoDropClip, transform.position, 1f);
            collision.GetComponent<Shooting>().AddAmmo();
            Destroy(gameObject);
        }
    }

    void Update()
    {
        rotationZ += Time.deltaTime * rotationSpeed;
        rb.rotation = rotationZ;
    }
}
