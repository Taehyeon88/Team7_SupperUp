using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBullet : MonoBehaviour
{
    
    private Rigidbody rb;
    [SerializeField] private GameObject target;
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

    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //StartCoroutine(DestroyBullet());

        Debug.Log("´ê¾Ò´Ù");
        Instantiate(target, transform.position, Quaternion.identity);
    }
}
