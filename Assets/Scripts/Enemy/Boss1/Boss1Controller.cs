using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Controller : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Boss1Animator animator;

    [Header("Sheild Movement")]
    [SerializeField] private float stateChangeDistance;

    [Header("Walking Movement")]
    [SerializeField] private float targetHeight;
    [SerializeField] private float springStrength;
    [SerializeField] private float springDamper;
    [SerializeField] private float rotationStrength;
    [SerializeField] private float rotationDamper;
    [SerializeField] private float slowdown;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float walkDirRandomisation;
    [SerializeField] private float obstacleRaycastLength;
    [SerializeField] private float obstacleRaycastHeightDelta;
    private Transform player;
    private State state = State.facingOut;
    private Rigidbody bossRb;
    private float walkDir;
    public enum State {
        facingOut,
        facingDown,
        facingUp,
        walking,
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
        if (state != State.walking) {
            if (Vector3.Distance(UtilityFunctions.VectorTo2D(this.transform.position), UtilityFunctions.VectorTo2D(player.position)) > stateChangeDistance) {
                state = State.facingOut;
            } else if (player.transform.position.y > this.transform.position.y) {
                state = State.facingUp;
            } else {
                state = State.facingDown;
            }
        } else {
            RaycastHit hit;
            int layermask = ~(1 << 7);
            Physics.Raycast(bossRb.position, Vector3.down, out hit, Mathf.Infinity, layermask);
            float height = targetHeight - hit.distance;
            float springForce = (height * springStrength) - (bossRb.velocity.y * springDamper);
            bossRb.AddForce(Vector3.up * springForce);

            Quaternion goal = Quaternion.LookRotation(Vector3.forward);
            Quaternion diff = UtilityFunctions.ShortestRotation(goal, bossRb.rotation);
            Vector3 rotAxis;
            float rotDegrees;
            diff.ToAngleAxis(out rotDegrees, out rotAxis);
            rotAxis.Normalize();
            float rotRadians = rotDegrees * Mathf.Deg2Rad;
            bossRb.AddTorque((rotAxis * (rotRadians * rotationStrength)) - (bossRb.angularVelocity * rotationDamper));
            
            bossRb.velocity = new Vector3(bossRb.velocity.x * slowdown, bossRb.velocity.y, bossRb.velocity.z * slowdown);

            walkDir = GetMoveDir();
            bossRb.AddForce((Quaternion.Euler(0, walkDir, 0) * Vector3.forward) * walkSpeed);
        }
        animator.UpdateArms(player.position, state);
    }

    float GetMoveDir() {
        Vector3 raycastOrigin = bossRb.position + Vector3.up * obstacleRaycastHeightDelta;
        walkDir += Random.Range(-walkDirRandomisation, walkDirRandomisation);
        int dir = -1;
        int testAngle = 0;
        while (testAngle < 180) {
            RaycastHit hit;
            int layermask = ~(1 << 7);
            float angle = walkDir + testAngle * dir;
            if (!Physics.Raycast(raycastOrigin, Quaternion.Euler(0, angle, 0) * Vector3.forward, out hit, obstacleRaycastLength, layermask)) {
                Debug.DrawRay(raycastOrigin, Quaternion.Euler(0, angle, 0) * Vector3.forward * obstacleRaycastLength, Color.blue, 0.1f);
                return angle;
            } else {
                Debug.DrawRay(raycastOrigin, Quaternion.Euler(0, angle, 0) * Vector3.forward * obstacleRaycastLength, Color.red, 0.1f);
                dir = -dir;
                testAngle += 1;
            }
        }
        return walkDir;
    }

    public void ArmDestroyed(Transform transform, int armNumber) {
        Debug.Log("Arm " + armNumber + " Destroyed");
        animator.DestroyArm(transform, armNumber);
    }

    public void AllArmsDestroyed() {
        state = State.walking;
        bossRb = this.gameObject.AddComponent<Rigidbody>();
        bossRb.mass = 500;
        Debug.Log("All arms destroyed");
    }
}
