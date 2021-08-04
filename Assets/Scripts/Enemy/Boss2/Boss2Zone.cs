using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Zone : MonoBehaviour {
    
    void OnTriggerEnter(Collider c) {
        if (c.gameObject.CompareTag("Player")) {
            Boss2Controller.instance.InitalizeFindPlayer();
        }
    }

    void OnTriggerExit(Collider c) {
        if (c.gameObject.CompareTag("Player")) {
            Boss2Controller.instance.playerTransform = null;
        }
    }
}
