using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Controller : MonoBehaviour {
    [SerializeField] private Boss1Animator animator;
    private Transform player;
    private State state;
    public enum State {
        facingOut,
        facingDown,
        facingUp,
    }


    void Start() {
        player = this.transform; //Avoid null reference before the player spawns
    }

    void OnEnable() {
        GameManager.OnPlayerSpawn += newPlayer;
    }

    void OnDisable() {
        GameManager.OnPlayerSpawn -= newPlayer;
    }

    void newPlayer(PlayerController newPlayer) {
        player = newPlayer.transform.Find("Sphere");
    }

    void Update() {
        animator.UpdateArms(player.position);
    }

    public void ArmDestroyed(Transform transform, int armNumber) {
        Debug.Log("Arm " + armNumber + " Destroyed");
        animator.DestroyArm(transform, armNumber);
    }

    public void AllArmsDestroyed() {
        Debug.Log("All arms destroyed");
    }
}
