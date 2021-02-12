using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {
    public GameObject explosion;
    public PowerUp[] loots;
    public GameObject playerExplosion;
    public GameObject shieldExplosion;
    private GameController gameController;
    public int scoreValue;
    public float lootProbability;

    void SpawnLoot()
    {
        if (loots.Length == 0) return;
        PowerUp loot = Instantiate(loots[Random.Range(0,loots.Length)], transform.position, Quaternion.identity);
        loot.SetVel(-8.0f);
    }

    private void Start()
    {
        gameController = GameController.FindController();
        if (gameController==null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boundary") || other.CompareTag("Enemy") || other.CompareTag("PowerUp"))
            return;
        else if (other.CompareTag("Shield"))
        {
            Destroy(gameObject);
            other.gameObject.SetActive(false);
            Instantiate(shieldExplosion, transform.position, transform.rotation);
            return;
        }
        else if (other.CompareTag("Player"))
        {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            gameController.GameOver();
        }
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }
        if (lootProbability > 0.0f) //chance to spawn loot?
        {
            if (Random.value < lootProbability) SpawnLoot();
        }
        gameController.AddScore(scoreValue);
        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}
