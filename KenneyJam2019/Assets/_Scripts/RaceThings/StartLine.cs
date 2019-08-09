using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLine : MonoBehaviour {
    public float startLineSize;

    public Vector3[] GetStartPositionsForPlayers(int playersCount) {
        var positions = new Vector3[playersCount];

        var startPoint = transform.position + transform.right * startLineSize;
        var endPoint = transform.position - transform.right * startLineSize;

        for (int i = 0; i < playersCount; i++) {
            positions[i] = Vector3.Lerp(startPoint, endPoint, (float) i / (playersCount - 1));
        }

        return positions;
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.magenta;

        var startPoint = transform.position + transform.right * startLineSize;
        var endPoint = transform.position - transform.right * startLineSize;

        Gizmos.DrawLine(startPoint, endPoint);
    }
}