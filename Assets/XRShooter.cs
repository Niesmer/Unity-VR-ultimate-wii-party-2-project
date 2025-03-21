using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XRShooter : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletSpawn;
    public TMP_Text AmmoCounter;
    public int bulletCount = 30;
    public float bulletSpeed = 20f;
    public AudioClip audioTir;
    public AudioClip audioNoBullet;
    private AudioSource thisGun;
    private int originalBulletCount;

    void Start()
    {
        AmmoCounter.text = bulletCount.ToString();
        thisGun = bulletSpawn.gameObject.AddComponent<AudioSource>();
        originalBulletCount = bulletCount;
    }
    public void Shoot()
    {
        if (bulletCount <= 0)
        {
            thisGun.clip = audioNoBullet;
            thisGun.Play();
            return;
        }

        thisGun.clip = audioTir;
        thisGun.Play();
        GameObject spawnedBullet = Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
        Rigidbody rb = spawnedBullet.GetComponent<Rigidbody>();
        rb.linearVelocity = spawnedBullet.transform.up * bulletSpeed;
        bulletCount -= 1;
        AmmoCounter.text = bulletCount.ToString();
    }

    public void Reload()
    {
        bulletCount = originalBulletCount;
        AmmoCounter.text = bulletCount.ToString();
    }
}
