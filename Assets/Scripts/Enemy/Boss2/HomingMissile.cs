using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour {
    

    public Transform playerTransform; //Player Transform

    public GameObject missileExplosion;
    public GameObject missileSmokeEffect;

    //Public Variables
    public float homingForce;
    public float rotationForce;   
    public float launchForce;
    public bool shouldFollow = false;

    Rigidbody body;

    void Start() {
        body = GetComponent<Rigidbody>();
        playerTransform = GameObject.Find("Sphere").transform;
        StartCoroutine(WaitBeforeHoming());
    }

    void FixedUpdate() {
        if (shouldFollow) {
            if (playerTransform != null) {
                Vector3 dir = playerTransform.position - body.position;
                dir.Normalize();
                Vector3 rotationAmount = Vector3.Cross(transform.forward, dir);
                body.angularVelocity = rotationAmount * rotationForce;
                body.velocity = transform.forward * homingForce;
                GameObject newSmokeEffect = Instantiate(missileSmokeEffect, transform.position, Quaternion.identity);
                Destroy(newSmokeEffect, 2.5f);
            }
        }
    }

    IEnumerator WaitBeforeHoming() {
        body.AddForce(Vector3.up * launchForce, ForceMode.Impulse);
        yield return new WaitForSeconds(2f);
        shouldFollow = true;
    }

    void OnCollisionEnter(Collision c) {
        Instantiate(missileExplosion, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}
