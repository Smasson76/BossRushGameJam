using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour {
    [Header("References")]
    [SerializeField] private PlayerController playerController;
    void OnCollisionEnter(Collision collision) {
        playerController.HitSomething(collision);
    }
}
