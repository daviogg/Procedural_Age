using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

   
    public float interpolationVelocity = 10f;
    public float minDistance;
    public float followDistance;

    public GameObject target;
    public Vector3 offset;

    private float interpolation;
    Vector3 targetPos;
    // Use this for initialization
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        targetPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target)
        {
            Vector3 posNoZ = transform.position;
            posNoZ.z = target.transform.position.z;

            Vector3 targetDirection = (target.transform.position - posNoZ);

            interpolation = targetDirection.magnitude * interpolationVelocity;

            targetPos = transform.position + (targetDirection.normalized * interpolation * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, targetPos + offset, 0.25f);

        }
    }



}
