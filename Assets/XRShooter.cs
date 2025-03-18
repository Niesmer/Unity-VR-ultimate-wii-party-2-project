using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Shooter : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletSpawn;
    public int bulletCount = 30;
    public float bulletSpeed = 20f;

    public void Shoot()
    {
        if (bulletCount <= 0) {
            return;
        }
        print("Pan !");
        GameObject spawnedBullet = Instantiate(bullet, bulletSpawn.position, bullet.transform.rotation);
        Rigidbody rb = spawnedBullet.GetComponent<Rigidbody>();
        rb.linearVelocity = bulletSpawn.forward * -bulletSpeed;
        bulletCount -= 1;
    }
}
