﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BaseWeapon : MonoBehaviour
{
    public Transform playerCamera;
    public ParticleSystem muzzleFlash;
    public ParticleSystem impactEffect;
    public int ammoInClips;
    public int maxAmmo;
    public float fireRate;
    [HideInInspector] public int maxAmmoCap;

    [Space]
    
    public float range;
    public float damage;
    public float force;

    public TextMeshProUGUI ammoText;

    public LayerMask raycastLayer;

    protected Animator animator;
    protected AudioSource shotSound;
    
    protected bool isShooting = false;
    protected bool isReloading = false;
    protected bool isOutOfAmmo = false;
    protected bool isOutOfClips = false;
    protected bool canReload = false;
    protected int ammoInWeapon;

    protected void InitWeapon()
    {
        animator = GetComponent<Animator>();
        shotSound = GetComponent<AudioSource>();
        
        ammoInWeapon = ammoInClips;
        maxAmmoCap = maxAmmo;
        muzzleFlash.Stop();
    }
    protected void UpdateAmmo()
    {
        if (maxAmmo == 0)
        {
            isOutOfClips = true;
        }
        else
        {
            isOutOfClips = false;
        }

        if (ammoInWeapon == ammoInClips)
        {
            canReload = false;
        }
        else
        {
            canReload = true;
        }

        if (ammoInWeapon == 0)
        {
            isOutOfAmmo = true;
        }
        else
        {
            isOutOfAmmo = false;
        }

        ammoText.text = ammoInWeapon.ToString() + " / " + maxAmmo.ToString();
    }

    void OnDisable()
    {
        isReloading = false;
    }

    public virtual IEnumerator Reload(float reloadDuration)
    {
        isReloading = true;
        animator.SetBool("isReloading", true);

        int ammoToLoad = ammoInClips - ammoInWeapon;

        yield return new WaitForSeconds(reloadDuration);

        if (maxAmmo > ammoToLoad)
        {
            ammoInWeapon += ammoToLoad;
            maxAmmo -= ammoToLoad;
        }
        else
        {
            ammoInWeapon += maxAmmo;
            maxAmmo = 0;
        }

        animator.SetBool("isReloading", false);
        isReloading = false;
    }

    public virtual void Fire()
    {
        RaycastHit hit;
        ammoInWeapon--;
        muzzleFlash.Play();
        animator.SetBool("isShooting", true);
        shotSound.Play();

        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, range, raycastLayer))
        {
            Debug.DrawRay(playerCamera.position, playerCamera.forward * hit.distance, Color.red);
            string layerHitted = LayerMask.LayerToName(hit.transform.gameObject.layer);
            Vector3 direction = hit.transform.position - playerCamera.position;
            direction.y = 0;

            switch (layerHitted)
            {
                case "Enemy":
                    hit.collider.gameObject.GetComponent<BaseEnemy>().TakeDamage(damage);
                    hit.collider.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    //hit.collider.gameObject.GetComponent<Rigidbody>().AddForce(direction.normalized * force, ForceMode.Impulse);
                    break;
            }
            PlaceImpactEffect(hit);
           
        }
        else
        {
            Debug.DrawRay(playerCamera.position, playerCamera.forward * range, Color.green);
        }
        StartCoroutine(MuzzleFlash(0.3f));
    }

    public IEnumerator MuzzleFlash(float duration)
    {
        muzzleFlash.Play();
        yield return new WaitForSeconds(duration);
        animator.SetBool("isShooting", false);
        muzzleFlash.Stop();
    }

    protected void PlaceImpactEffect(RaycastHit hitPoint)
    {
        GameObject impactGO = Instantiate(impactEffect.gameObject, hitPoint.point, Quaternion.LookRotation(hitPoint.normal));
        Destroy(impactGO, 2f);
    }
}
