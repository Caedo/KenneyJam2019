using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPointArrowEntity : MonoBehaviour
{
    public ShipEntity Entity;
    public ShipRaceController RaceManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Entity == null || RaceManager == null)
        {
            return;
        }

        var nextControlPointPosition = RaceManager.nextControlPoint.transform.position;
        var vectorToTarget = nextControlPointPosition - Entity.transform.position;
        var angle = Mathf.Atan2(vectorToTarget.x, vectorToTarget.z) * Mathf.Rad2Deg;
        var deltaAngle = Mathf.DeltaAngle(angle, transform.eulerAngles.y);

        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, deltaAngle);
    }
}
