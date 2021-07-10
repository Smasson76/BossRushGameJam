using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Controller : MonoBehaviour {
    [SerializeField] private Boss1Animator animator;
    private Transform player;
    private GameManager gameManager;

    void Awake() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void OnEnable() {
        gameManager.OnPlayerSpawn += newPlayer;
    }

    void OnDisable() {
        gameManager.OnPlayerSpawn -= newPlayer;
    }

    void newPlayer(PlayerController newPlayer) {
        player = newPlayer.transform;
    }
    void Update() {
        animator.UpdateArms(player.position);
    }
}
