using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetJoint : MonoBehaviour
{

    private TargetJoint2D targetJoint;

    void Start()
    {
        targetJoint = GetComponent<TargetJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetJoint.target = new Vector2(worldPosition.x, worldPosition.y);
    }
}
