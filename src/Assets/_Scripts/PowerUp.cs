using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LootTypes { FIRERATE, SCATTERGUN, SHIELD };

public class PowerUp : MonoBehaviour {
    private Rigidbody rb;
    private GameController gameController;
    public float spinRate;
    public string pickupCaption;
    public Color colour;
    public int score;
    
    private PlayerController playerController;
    public BlinkText lootText;

    public LootTypes lootType;
    public void SetVel(float speed)
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0.0f, 0.0f, speed);
        rb.angularVelocity = new Vector3(0.0f, spinRate, 0.0f);

    }
    /*
    void AddBonus()
    {
        switch (lootType)
        {
            case LootTypes.FIRERATE:

                break;
            case LootTypes.SCATTERGUN:
                break;
            case LootTypes.SHIELD:
                break;
        }
    }*/

    private void Start()
    {
        playerController = PlayerController.FindPlayer();
        gameController = GameController.FindController();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //AddBonus();
            playerController.SetBonus(lootType);
            BlinkText textObject = Instantiate(lootText, transform.position, Quaternion.Euler(90f,0.0f,0.0f));
            textObject.SetText(pickupCaption, colour, 18);
            gameController.AddScore(score);
            Destroy(gameObject);
        }
    }

}
