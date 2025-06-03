using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBullet : MonoBehaviour
{
    
    private Rigidbody rb;
    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack(float force, Vector3 dir)
    {
        rb.AddForce(dir * force, ForceMode.Impulse);
    }
}
