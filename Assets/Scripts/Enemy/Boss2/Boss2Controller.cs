using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss2Controller : MonoBehaviour {

    public static Boss2Controller instance; //Creating a singleton
    
    public Transform playerTransform; //Player transform

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
        forceField.SetActive(true);
        count = 1200;
        ChaseUpdate();
    }

    void Update() {
        if (playerTransform != null) {
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
    }

    void IdleUpdate() {
        body.velocity = Vector3.zero;
        anim.SetTrigger("Idle");

        if (playerTransform != null) {
            state = State.Chase;
        }
    }

    void ChaseUpdate() {
        forceField.SetActive(true);
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
        yield return new WaitForSeconds(1.5f);
        
        switch (Random.Range(1, 4)) {
            case 1:
                Rigidbody newMissile1 = Instantiate(missile, missilePoints[0].transform.position, transform.rotation) as Rigidbody;
                Debug.Log("Case 1 happened");
                break;
            case 2:
                Rigidbody newMissile2 = Instantiate(missile, missilePoints[0].transform.position, transform.rotation) as Rigidbody;
                Rigidbody newMissile3 = Instantiate(missile, missilePoints[1].transform.position, transform.rotation) as Rigidbody;
                Debug.Log("Case 2 happened");
                break;
            case 3:
                Rigidbody newMissile4 = Instantiate(missile, missilePoints[0].transform.position, transform.rotation) as Rigidbody;
                Rigidbody newMissile5 = Instantiate(missile, missilePoints[1].transform.position, transform.rotation) as Rigidbody;
                Rigidbody newMissile6 = Instantiate(missile, missilePoints[2].transform.position, transform.rotation) as Rigidbody;
                Debug.Log("Case 3 happened");
                break;
            case 4:
                Rigidbody newMissile7 = Instantiate(missile, missilePoints[0].transform.position, transform.rotation) as Rigidbody;
                Rigidbody newMissile8 = Instantiate(missile, missilePoints[1].transform.position, transform.rotation) as Rigidbody;
                Rigidbody newMissile9 = Instantiate(missile, missilePoints[2].transform.position, transform.rotation) as Rigidbody;
                Rigidbody newMissile10 = Instantiate(missile, missilePoints[3].transform.position, transform.rotation) as Rigidbody;
                Debug.Log("Case 4 happened");
                break;
            default:
            Debug.Log("Case default happened");
                break;
        }
        
        yield return new WaitForSeconds(2f);
        shotHomingMissile = false;
        count = 1200;
        state = State.Chase;
    }
}
