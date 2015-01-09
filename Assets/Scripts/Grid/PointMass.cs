using UnityEngine;
using System.Collections;

public class PointMass
{
    public GameObject go;
    public Vector3 Position;
    public Vector3 Velocity;
    public float InverseMass;

    private Vector3 acceleration;
    private float damping = 0.98f;

    public PointMass(Vector3 position, float invMass, GameObject _go)
    {
        go = (GameObject) MonoBehaviour.Instantiate(_go, Position, Quaternion.identity);
       // go.transform.position = Position;
        Position = position;
        InverseMass = invMass;
    }

    public void ApplyForce(Vector3 force)
    {
        acceleration += force * InverseMass;
    }

    public void IncreaseDamping(float factor)
    {
        damping *= factor;
    }

    public GameObject GetPointMassGameObject()
    {
        return go;
    }
    public void Update()
    {
        Velocity += acceleration;
        Position += Velocity;
        if (Position.z < -10f)
        {
            Position = new Vector3(Position.x,Position.y,-10f);
        }
        go.transform.position = Position;
        acceleration = Vector3.zero;
        if ((Velocity.magnitude * Velocity.magnitude) < 0.001f * 0.001f)
            Velocity = Vector3.zero;

        Velocity *= damping;
        damping = 0.98f;
    }
}