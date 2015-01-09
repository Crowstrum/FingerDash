using UnityEngine;
using System.Collections;

public class BasicProjectile : MonoBehaviour
{
		private IMovementType _movementType;
		private Grid _gridRef;

		// Use this for initialization
		void Start ()
		{
				_gridRef = GameObject.Find ("Grid").GetComponent<Grid> ();
				Destroy (gameObject, 2f);
		}
	
		// Update is called once per frame
		void Update ()
		{
				_movementType.Move (gameObject);
			
				_gridRef.ApplyExplosiveForce (4f, gameObject.transform.position, 1f);
		}

		public void SetMovementType (IMovementType type)
		{
				_movementType = type;
		}
}
