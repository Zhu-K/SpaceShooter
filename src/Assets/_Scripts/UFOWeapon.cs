using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOWeapon : MonoBehaviour {
    private AudioSource audioSource;
    public GameObject shot;
    private PlayerController player;
    public float fireRate;
    public Vector2 burstDelay;
    public int burstSize;

    

    IEnumerator Fire()
    {
        Vector3 offsetPos = new Vector3(0f, 0f, -0.8f);
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(burstDelay.x, burstDelay.y));
            for (int i = 0; i < burstSize; i++)
            {
                if (player==null)   //player destroyed, stop firing
                {
                    yield break;
                }
                Quaternion rotToPlayer = Quaternion.LookRotation(player.transform.position-transform.position);
                Instantiate(shot, transform.position + rotToPlayer * offsetPos, rotToPlayer );
                audioSource.Play();
                yield return new WaitForSeconds(fireRate);
            }
        }

    }

    void Start()
    {
        player = PlayerController.FindPlayer();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(Fire());
    }
}
