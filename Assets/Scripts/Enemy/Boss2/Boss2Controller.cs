using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss2Controller : MonoBehaviour {

    public static Boss2Controller instance; //Creating a singleton
    
    [SerializeField] Transform playerTransform;

    Vector3 destination;
    Vector3 lastPosition;

    NavMeshAgent agent;

    public enum State {
		Idle,
        Chase,
        Shoot,
        Death
	}

    public State state = State.Idle;

    Animator anim;
    Rigidbody body;

    void Awake() {
        if (instance == null) instance = this;
    }

    void Start() {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        destination = agent.destination;
    }

    public void InitalizeFindPlayer() {
        playerTransform = GameObject.FindWithTag("Player").transform;
        IdleUpdate();
    }

    void Update() {
        switch (state) {
            case State.Idle:
                IdleUpdate();
                break;
            case State.Chase:
                ChaseUpdate();
                break;
            case State.Shoot:
                break;
            case State.Death:
                break;
            default:
                break;
        }
    }

    void IdleUpdate() {
        body.velocity = Vector3.zero;
        anim.SetTrigger("Idle");

        if (playerTransform != null) {
            state = State.Chase;
        }
    }

    void ChaseUpdate() {
        //if (Vector3.Distance(destination, playerTransform.position) > 0.001f) {
            //destination = playerTransform.position;
            //agent.destination = destination;
        //}

       if (playerTransform.position != lastPosition) {
           agent.SetDestination(playerTransform.position);
           lastPosition = playerTransform.position;
       }
    }
}
