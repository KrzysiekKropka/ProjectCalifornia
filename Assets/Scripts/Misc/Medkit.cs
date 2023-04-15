using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Medkit : MonoBehaviour
{
    [SerializeField] int rotationSpeed;
    [SerializeField] AudioClip MedkitClip;
    [SerializeField] TMP_Text hpText;
    [SerializeField] Rigidbody2D rb;
    private float rotationZ;
    public int hp;

    void Start()
    {
        hpText.text = hp + " HP";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if(collision.GetComponent<Player>().AddHP(hp) == true) Destroy(gameObject);
        }
    }

    void Update()
    {
        rotationZ += Time.deltaTime * rotationSpeed;
        rb.rotation = rotationZ;
    }
}
