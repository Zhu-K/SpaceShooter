using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOManeuver : MonoBehaviour {
    public Boundary boundary;
    private Rigidbody rb;
    public Vector2 startWait;
    public Vector2 maneuverTime;
    public Vector2 maneuverWait;
    private float targetManeuver;
    public float dodge;
    public float smooth;
    private float currentSpeed;
    public GameObject shield;

    IEnumerator Evade()
    {
        yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));
        while (true)
        {
            targetManeuver = Random.Range(1,dodge) * -Mathf.Sign(transform.position.x);
            yield return new WaitForSeconds(Random.Range(maneuverTime.x, maneuverTime.y));
            targetManeuver = 0;
            yield return new WaitForSeconds(Random.Range(maneuverWait.x, maneuverWait.y));  
        }
    }
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(Evade());
        currentSpeed = rb.velocity.z;
        //rb.angularVelocity = new Vector3(0f,5f,0f);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        float newManeuver = Mathf.MoveTowards(rb.velocity.x, targetManeuver, Time.deltaTime * smooth);
        rb.velocity = new Vector3(newManeuver, 0, currentSpeed);
        rb.position = new Vector3
        (
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
        );
        rb.rotation = Quaternion.Euler(0f, Time.time * 120, 0f) * Quaternion.Euler(35f, Time.time*10, 0f); //rb.velocity.x * -tilt
        shield.transform.rotation = Quaternion.identity;
    }
}
