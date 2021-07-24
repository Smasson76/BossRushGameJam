using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Controller : MonoBehaviour {
    [SerializeField] private Boss1Animator animator;
    [SerializeField] private float stateChangeDistance;
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
        if (Vector3.Distance(UtilityFunctions.VectorTo2D(this.transform.position), UtilityFunctions.VectorTo2D(player.position)) > stateChangeDistance) {
            state = State.facingOut;
        } else if (player.transform.position.y > this.transform.position.y) {
            state = State.facingUp;
        } else {
            state = State.facingDown;
        }
        animator.UpdateArms(player.position, state);
    }

    public void ArmDestroyed(Transform transform, int armNumber) {
        Debug.Log("Arm " + armNumber + " Destroyed");
        animator.DestroyArm(transform, armNumber);
    }

    public void AllArmsDestroyed() {
        Debug.Log("All arms destroyed");
    }
}
