using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss2Controller : MonoBehaviour {

    public static Boss2Controller instance; //Creating a singleton
    
    [SerializeField] Transform playerTransform; //Player transform

    //For Nav Mesh
    Vector3 destination;
    Vector3 lastPosition;

    //GameObjects
    NavMeshAgent agent;
    public Rigidbody missile;
    public GameObject forceField;
    public GameObject[] missilePoints;

    //Variables
    public int count = 500;

    bool createNewPath = true;
    public bool shotHomingMissile = false;


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
        forceField.SetActive(true);
        destination = agent.destination;
    }

    public void InitalizeFindPlayer() {
        playerTransform = GameObject.Find("Sphere").transform;
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
                ShootUpdate();
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
        if (createNewPath) StartCoroutine(ChasePlayer());

        if (count > 0) count-=1;
        else state = State.Shoot;
    }

    IEnumerator ChasePlayer() {
        agent.SetDestination(playerTransform.position);
        createNewPath = false;
        yield return new WaitForSeconds(2f);
        createNewPath = true;
    }

    void ShootUpdate() {
        forceField.SetActive(false);

        if (shotHomingMissile == false) { 
            StartCoroutine(SpawnHomingMissile());
        }
    }

    IEnumerator SpawnHomingMissile() {
        shotHomingMissile = true;
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < missilePoints.Length; i++) {
            Rigidbody newMissile = Instantiate(missile, missilePoints[i].transform.position, transform.rotation) as Rigidbody;
        }
    }
}
