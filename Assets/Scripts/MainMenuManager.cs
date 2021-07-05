using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuManager : MonoBehaviour {

    public static MainMenuManager instance;

    PlayerInput playerInput;

    void Awake() {
        if (instance == null) instance = this;

        playerInput = this.GetComponent<PlayerInput>();
    }

    public void ChargeStation() {
        Debug.Log("Charge Station called");
        RaycastHit hit;
        Vector2 mousePos = playerInput.GetPointerPos();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit)) {
            Debug.Log("hit something");
            Debug.Log(hit);
            if (hit.transform.gameObject.tag == "ChargeStation") {
                Debug.Log("Charge station hit");
            }
        }
    }
}
