using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceControlPoint : MonoBehaviour {
    public bool isEndPoint;
    public RaceControlPoint nextPoint;

    [Header("Palcement")]
    public float pointWidth;
    public Transform leftFlag;
    public Transform rightFlag;
    public BoxCollider trigger;

    void OnValidate() {
        leftFlag.localPosition = -Vector3.right * pointWidth / 2f;
        rightFlag.localPosition = Vector3.right * pointWidth / 2f;

        var triggerSize = trigger.size;
        triggerSize.x = pointWidth;
        trigger.size = triggerSize;
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;

        if (nextPoint != null)
            Gizmos.DrawLine(transform.position + Vector3.up * 5f, nextPoint.transform.position+ Vector3.up * 5f);
    }
}