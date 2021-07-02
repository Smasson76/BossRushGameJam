using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Controller : MonoBehaviour {

    public GameObject midOfBody;

    public float spinDist = 5f;
    public float spinSpeed = 300f;
    
    public enum State {
        Idle,
        Spin
    }

    public State state = State.Idle;

    Animator anim;
    public Transform player;
    
    void Start() {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update() {
        switch (state) {
            case State.Idle:
                IdleUpdate();
                break;
            case State.Spin:
                StartCoroutine(SpinUpdate());
                break;
            default:
                break;
        }
    }

    void IdleUpdate() {
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist < spinDist) {
            state = State.Spin;
        }
    }

    IEnumerator SpinUpdate() {
        midOfBody.transform.Rotate(0, Time.deltaTime * spinSpeed, 0, Space.Self);
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist > spinDist) state = State.Idle;
        yield return new WaitForSeconds(2f);
    }
}
