using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {
    private Rigidbody rb;
    private AudioSource weaponSound;
    public float speed;
    public Boundary boundary;
    public float tilt;
    public GameObject shield;
    public AudioClip shieldSound;
    public AudioClip pickupSound;

    public GameObject shot;
    public Transform shotSpawn;

    private float nextFire;

    public float fireRate;
    private float fireDelta;
    private int scatter = 0;
    public float spread = 5.0f;

    public static PlayerController FindPlayer()
    {
        GameObject playerControllerObject = GameObject.FindWithTag("Player");
        if (playerControllerObject != null)
        {
            return playerControllerObject.GetComponent<PlayerController>();
        }
        else return null;
    }

    public void SetBonus(LootTypes lootType)
    {
        //reset to initial stat first before setting new bonus
        switch (lootType )
        {
            case LootTypes.FIRERATE:
                scatter = 0;
                weaponSound.PlayOneShot(pickupSound, 0.2f);
                fireDelta = fireRate * 0.5f;
                break;
            case LootTypes.SCATTERGUN:
                fireDelta = fireRate;
                weaponSound.PlayOneShot(pickupSound, 0.2f);
                scatter = 2;
                break;
            case LootTypes.SHIELD:
                weaponSound.PlayOneShot(shieldSound, 0.6f);
                shield.SetActive(true);
                break;
        }

    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        weaponSound = GetComponent<AudioSource>();
        fireDelta = fireRate;
    }
    void FixedUpdate () {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.velocity = movement * speed;

        rb.position = new Vector3
        (
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
        );

        shield.transform.rotation = Quaternion.identity;
        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
        

    }
    private void Update()
    {
        if ((Input.GetButton("Fire1") || Input.GetKey(KeyCode.Space)) && Time.time > nextFire)      //Input.GetButton("Fire1") || 
        {
            weaponSound.Play();
            nextFire = Time.time + fireDelta;
            GameObject newProjectile = Instantiate(shot, shotSpawn.position, shotSpawn.rotation);            // create code here that animates the newProjectile            nextFire = nextFire - myTime;
            if (scatter > 0)
            {
                for (int i=0; i < scatter; i++)
                {
                    Quaternion spreadAngle = Quaternion.Euler(0.0f, spread * (i + 1), 0.0f);
                    Instantiate(shot, shotSpawn.position, spreadAngle); //shotSpawn.rotation * 
                    Instantiate(shot, shotSpawn.position, Quaternion.Inverse( spreadAngle));
                }
            }
        }
        
    }
}
