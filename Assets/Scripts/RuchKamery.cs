using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuchKamery : MonoBehaviour
{
    public GameObject Player;

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 nowaPozycjaKamery = new Vector3(Player.transform.position.x,Player.transform.position.y, transform.position.z);
        transform.position = nowaPozycjaKamery;
    }
}
