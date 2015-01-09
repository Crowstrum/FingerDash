using UnityEngine;
using System.Collections;

public class SinMovement :  IMovementType
{
		private const float amplitude = 1f;
		private const float frequency = 1f;

	#region IMovementType implementation

		public void Move (GameObject projectileObject)
		{
				float yMovement = amplitude * (Mathf.Sin (2 * Mathf.PI * frequency * Time.time) - Mathf.Sin (2 * Mathf.PI * frequency * (Time.time - Time.deltaTime)));
				projectileObject.transform.Translate (Time.deltaTime * 5f, yMovement, 0f);
		}

	#endregion



}
