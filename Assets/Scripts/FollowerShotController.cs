using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerShotController : ShotController
{
	public float rotationSpeed = 6f;
    public float detectionRadius;
	public LayerMask targetMask;

	private Transform target;

    protected override void Update()
    {        
        if (target != null)
        {
            var rotation = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
		else
		{
			DetectEnemiesInRange();
		}

        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

	private void DetectEnemiesInRange()
	{
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, targetMask);

		if (hitColliders.Length > 0)
			target = hitColliders[0].transform;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, detectionRadius);
	}
}
