using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyItself : MonoBehaviour
{
    public void DestroyGameObject()
    {
        Destroy(transform.root.gameObject);
    }
}
