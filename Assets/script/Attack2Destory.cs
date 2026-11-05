using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack2Destory : MonoBehaviour
{
    public GameObject Obj;
    public float DesTime = 0.0f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Destroy(Obj, DesTime);
        }
    }
}
