using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Destructible : MonoBehaviour {
    [SerializeField] int armNumber;
    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player") {
            Debug.Log("The player hit arm number: " + armNumber);
            Boss1Controller controller = null;
            Transform currentJoint = this.transform;
            int jointCount = 0;
            while (controller == null) {
                currentJoint = currentJoint.parent;
                controller = currentJoint.GetComponent<Boss1Controller>();
                jointCount++;
            }
            controller.ArmDestroyed(this.transform.parent, armNumber);
        }
    }
}
