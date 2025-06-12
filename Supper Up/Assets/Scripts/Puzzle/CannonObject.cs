using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CannonObject : MonoBehaviour
{
    [SerializeField] private float checkingAngle = 20f;
    [SerializeField] private float maxDistance = 20f;

    [SerializeField] private GameObject head;
    [SerializeField] private float rotateSpeed = 1f;

    [SerializeField] private GameObject bullerPf;
    [SerializeField] private float firePower = 2f;
    [SerializeField] private float intitialPower = 2f;
    [SerializeField] private float fireDuration = 5f;

    private const float Maxheight = 3f;
    private float timer = 0f;

    //내부변수
    private PlayerController player;

    private float time = 0f;

    private GameObject text;
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        text = Instantiate(bullerPf);
    }

    void Update()
    {
        CheckPlayer();

        CheckFirePos(head.transform.forward * firePower, transform.position, head.transform.forward * intitialPower, 3f, text);
    }

    private void CheckPlayer()
    {
        float dis = Vector3.Distance(player.transform.position, transform.position);

        float playerH = player.transform.position.y;
        float cannonH = transform.position.y;

        if (dis > maxDistance || playerH < cannonH - 0.5f || playerH > cannonH + Maxheight) return; //최대거리 및 높이제한

        Vector3 dir = (player.transform.position - transform.position).normalized;
        float dot = Vector3.Dot(dir, transform.forward);
        float angle = Mathf.Rad2Deg * Mathf.Acos(dot);

        if (angle < checkingAngle / 2)
        {
            //Debug.Log(angle);
            //SlerpHead();
            CheckFire();
        }
    }

    private void SlerpHead()
    {
        float height = head.transform.forward.y;
        Vector3 dir = player.transform.position - transform.position;
        dir.y = height;

        Quaternion lookRot = Quaternion.LookRotation(dir, Vector2.up);
        float t = 1f - Mathf.Exp(-rotateSpeed * Time.deltaTime);
        head.transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, t);
    }

    private void CheckFire()
    {
        timer += Time.deltaTime;
        if (timer > fireDuration)
        {
            Fire();
            timer = 0f;
        }
    }

    private void Fire()
    {
        Vector3 firePos = head.transform.position + head.transform.forward;
        //Vector3 dir = (player.transform.position - firePos).normalized;
        GameObject bullet = Instantiate(bullerPf, firePos, Quaternion.identity);
        bullet.GetComponent<CannonBullet>().Attack(firePower, head.transform.forward);
    }

    private void CheckFirePos(Vector3 externalForce, Vector3 startPos, Vector3 initialVelocity, float mass, GameObject bullet)
    {
        time += Time.deltaTime / 1.5f;
        Vector3 gravity = Physics.gravity;
        Vector3 acceleration = externalForce / mass + gravity;

        Vector3 newPos = startPos + initialVelocity * time + 0.5f * acceleration * time * time;

        bullet.transform.position = newPos;
    }



    private Vector3 AngleTodir(float angle)
    {
        float radian = Mathf.Deg2Rad * angle;
        return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
    }

    private void OnDrawGizmos()
    {
        //Vector3 rightdir = AngleTodir(transform.eulerAngles.y + checkingAngle);
        //Vector3 leftdir = AngleTodir(transform.eulerAngles.y - checkingAngle);

        //Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.red);
        //Debug.DrawRay(transform.position, rightdir * maxDistance, Color.red);
        //Debug.DrawRay(transform.position, leftdir * maxDistance, Color.red);

        Debug.DrawRay(head.transform.position, head.transform.forward * 2f, Color.green);
    }
}
