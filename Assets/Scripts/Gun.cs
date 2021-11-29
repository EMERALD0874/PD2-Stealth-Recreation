using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Functionality")]
    [SerializeField] float firerate = .2f;
    [SerializeField] float range = 100f;
    [SerializeField] LayerMask mask;
    [Header("Feedback")]
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] AudioClip fireSound;

    Camera cam;
    AudioSource src;
    float nextFire = 0f;

    private void Start()
    {
        cam = Camera.main;
        src = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFire)
        {
            nextFire = Time.time + firerate;
            Shoot();
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();
        src.PlayOneShot(fireSound);

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range, mask))
        {
            DeathManager dm = hit.transform.GetComponent<DeathManager>();
            if (dm != null)
            {
                dm.Death();
            }
        }
    }
}
