#pragma warning disable 0414 // private field assigned but not used.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Camera2DFollow : MonoBehaviour {

	public Transform target;
	public float damping = 1;
	public float lookAheadFactor = 3;
	public float lookAheadReturnSpeed = 0.5f;
	public float lookAheadMoveThreshold = 0.1f;

	public float changeViewInY = 10.0f;

	float offsetZ;
	Vector3 lastTargetPosition;
	Vector3 currentVelocity;
	Vector3 lookAheadPos;

	Vector3 moveCameraUpwards;

	// Use this for initialization
	void Start () {
		lastTargetPosition = target.position;;
		offsetZ = (transform.position - target.position).z;
		transform.parent = null;
	}

	// Update is called once per frame
	void Update () {

		if (target == null) {
			return;
		}

		moveCameraUpwards = new Vector3 (0.0f, changeViewInY, 0.0f);

		float xMoveDelta = (target.position - lastTargetPosition).x;
		bool updateLookAheadTarget = Mathf.Abs (xMoveDelta) > lookAheadMoveThreshold;

		if (updateLookAheadTarget) {
			lookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign (xMoveDelta);
		} else {
			lookAheadPos = Vector3.MoveTowards(lookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
		}

		Vector3 aheadTargetPos = target.position + lookAheadPos + Vector3.forward * offsetZ + moveCameraUpwards;
		Vector3 newPos = Vector3.SmoothDamp (transform.position, aheadTargetPos, ref currentVelocity, damping);

		transform.position = newPos;

		lastTargetPosition = target.position;
	}
}
