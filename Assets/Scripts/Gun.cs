using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Functionality")]
    [SerializeField] float firerate = .1f;
    [SerializeField] float range = 100f;
    [SerializeField] LayerMask mask;
    [Header("Feedback")]
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] AudioClip fireSound;
    [SerializeField] GameObject bloodSpray;

    Camera cam;
    AudioSource src;
    float nextFire = 0f;

    public bool canFire = true;

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
        if (!canFire)
            return;

        muzzleFlash.Play();
        src.pitch = Random.Range(.9f, 1.1f);
        src.PlayOneShot(fireSound);

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range, mask))
        {
            DeathManager dm = hit.transform.GetComponent<DeathManager>();
            if (dm != null)
            {
                dm.Death();
                Instantiate(bloodSpray, hit.point, Quaternion.Euler(0, 0, 0));
            }
        }
    }
}
