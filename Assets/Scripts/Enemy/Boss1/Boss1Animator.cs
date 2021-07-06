using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DitzelGames.FastIK;

public class Boss1Animator : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Transform[] armEnds;
    [SerializeField] private float armHeight;
    [SerializeField] private float armRadius;

    private Transform armTargetAndPoleContainer;
    private List<Arm> arms;
    private struct Arm {
        public Vector3 homeDir;
        public Transform ikEnd;
        public Transform ikTarget;
        public Transform ikPole;
        public FastIKFabric ikComponent;
    }

    void Start() {
        armTargetAndPoleContainer = new GameObject("Arm Target And Pole Container").transform;
        armTargetAndPoleContainer.SetParent(this.transform);
        arms = new List<Arm>();
        for (int i = 0; i < armEnds.Length; i++) {
            Arm newArm = new Arm();
            newArm.ikEnd = armEnds[i];
            newArm.homeDir = new Vector3(newArm.ikEnd.position.x, 0, newArm.ikEnd.position.z).normalized;
            newArm.ikComponent = newArm.ikEnd.gameObject.AddComponent<FastIKFabric>();
            newArm.ikTarget = newArm.ikComponent.Target;
            newArm.ikTarget.SetParent(armTargetAndPoleContainer);
            newArm.ikPole = new GameObject("IK Pole " + i).transform;
            newArm.ikPole.SetParent(armTargetAndPoleContainer);
            newArm.ikComponent.Pole = newArm.ikPole;
            newArm.ikPole.position = this.transform.position + Vector3.up * 50f;
            newArm.ikComponent.ChainLength = 3;
            arms.Add(newArm);
        }
    }

    public void UpdateArms(Vector3 playerPos) {
        foreach(Arm arm in arms) {
            arm.ikTarget.position = arm.homeDir * armRadius + Vector3.up * armHeight;
        }
    }

}
