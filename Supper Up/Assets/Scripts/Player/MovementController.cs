using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("Wall Climbing Setting")]
    public float heightValue = 1f;
    public float frontValue = 0.3f;
    public Vector3 boxHalfExtents = Vector3.zero;
    public LayerMask wallLayer;
    public float ClimbSpeed = 1f;

    private void Update()
    {
        Climbing();
    }
    public void Climbing()
    {
        Vector3 origin = transform.position + Vector3.up * heightValue + Vector3.forward * frontValue;
        Collider[] target = Physics.OverlapBox(origin, boxHalfExtents, Quaternion.identity, wallLayer);

        float height;
        if (target.Length >= 1)
        {
            Debug.Log($"{target[0].transform.position.y}, {target[0].transform.localScale.y / 2}");
            height = target[0].transform.position.y + target[0].transform.localScale.y / 2 + 0.5f;
            transform.position = new Vector3(transform.position.x, height, transform.position.z);
        }
    }
    private void OnDrawGizmos()
    {
        //플레이어 벽체크용
        Vector3 origin = transform.position + Vector3.up * heightValue + Vector3.forward * frontValue;
        Gizmos.DrawWireCube(origin, boxHalfExtents * 2f);
    }
}
