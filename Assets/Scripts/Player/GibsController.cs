using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GibsController : MonoBehaviour
{
    public delegate void GibSoundCallback(Vector3 point);
    public GibSoundCallback soundCallback;
    void OnCollisionEnter (Collision collision) {
        soundCallback(collision.contacts[0].point);
    }
}
