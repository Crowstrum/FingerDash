using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RotationalEnemy : MonoBehaviour
{
		public GameObject projectilePrefab;
		public Transform shootingTransforms;
		public GameObject _obj;
		private GameObject _refObj;
		private Quaternion _rotation;
		private const float _rotationDegree = 90f;
		private const float _timeUntilRotation = 1f;
		private Transform _thisTransform;
		private float timerTime;
		private delegate void reState ();
		private bool moveDir;
		//refrence for delegate
		private reState stateMethod;
		private Vector3 moveThisWay;
		private Grid _gridRef;
	
	
	
		// Use this for initialization
		void Start ()
		{

				_gridRef = GameObject.Find ("Grid").GetComponent<Grid> ();
				moveDir = false;
				_thisTransform = transform;
				timerTime = _timeUntilRotation;
				stateMethod = new reState (Spawn);
		
		}
	
		void FixedUpdate ()
		{
				stateMethod ();
		}
	
		void Spawn ()
		{
				_refObj = (GameObject)Instantiate (_obj, _thisTransform.position, Quaternion.identity);
				_refObj.transform.parent = transform;
				
				shootingTransforms = _refObj.transform.GetChild (0);
				
				_rotation = Quaternion.Euler (0, 0, 45f) * _refObj.transform.rotation;
				stateMethod = new reState (Move);
				stateMethod ();
		}
	
		void Move ()
		{
		
				if (!moveDir) {
			
						moveThisWay = RandomMovement (Random.Range (0, 4));
						moveDir = true;
			
				}
				if (!Timer ()) {
						Debug.Log ("in move");
						_refObj.transform.position = Vector3.Lerp (_refObj.transform.position, moveThisWay + _refObj.transform.position, 3 * Time.deltaTime);
						_gridRef.ApplyDirectedForce (.05f * moveThisWay, _refObj.transform.position, .7f);
				} else {
						moveDir = false;
						stateMethod = new reState (Rotate);
						stateMethod ();
				}
		}
	
		void Rotate ()
		{		
				if (!Timer ()) {
						_refObj.transform.rotation = Quaternion.Lerp (_refObj.transform.rotation, _rotation, 5f * Time.deltaTime);
				} else {
						stateMethod = new reState (Shoot);
				}
		}
		void ExitRotate ()
		{
				if (!Timer ()) {
						_refObj.transform.rotation = Quaternion.Lerp (_refObj.transform.rotation, Quaternion.identity, 5f * Time.deltaTime);
				} else {
						stateMethod = new reState (Move);
				}
		}
		void Shoot ()
		{
				Quaternion projRot = Quaternion.identity;
				float angle = 0;
				for (int i = 0; i < 10; i++) {
						
						Debug.Log (angle);
						projRot *= Quaternion.Euler (0, 0, angle);
						var temp = (GameObject)Instantiate (projectilePrefab, shootingTransforms.position, projRot);
						temp.GetComponent<BasicProjectile> ().SetMovementType (new SinMovement ());
						angle += 45f;
				}
				stateMethod = new reState (ExitRotate);
		
		}
	
		void DeSpawn ()
		{
				Debug.Log ("in two");
		}
	
		bool Timer ()
		{
				timerTime -= Time.deltaTime;
				if (timerTime <= 0) {
						timerTime = _timeUntilRotation;
						return true;
				}
				return false;
		}
	
		Vector3 RandomMovement (int randNum)
		{
				switch (randNum) {
				case 0:
						return Vector3.up;
				case 1:
						return Vector3.up;
				case 2:
						return Vector3.left;
				case 3:
						return Vector3.right;
				}
				return Vector3.up;
		}
	
	
	
}
