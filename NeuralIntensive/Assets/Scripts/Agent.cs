using System;
using UnityEngine;

public class Agent : MonoBehaviour, IComparable<Agent>
{
    public NeuralNetwork net;
    public CarController carController;

    public float fitness;
    public float distanceTraveled;

    public Rigidbody rb;

    float[] inputs = new float[8];

    public Transform nextCheckpoint;
    public float nextCheckpointDist;
    public void ResetAgent()
    {
        fitness = 0;
        distanceTraveled = 0;

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        inputs = new float[net.layers[0]];

        carController.Reset();

        nextCheckpoint = CheckpointManager.instance.firstCheckpoint;
        nextCheckpointDist = (transform.position - nextCheckpoint.position).magnitude;
    }

    private void FixedUpdate()
    {
        InputUpdate();
        OutputUpdate();
        FitnessUpdate();
    }

    private void InputUpdate()
    {
        inputs[0] = RaySensor(transform.position + Vector3.up * 0.2f, transform.forward, 4);
        inputs[1] = RaySensor(transform.position + Vector3.up * 0.2f, transform.right, 1.5f);
        inputs[2] = RaySensor(transform.position + Vector3.up * 0.2f, -transform.right, 1.5f);
        inputs[3] = RaySensor(transform.position + Vector3.up * 0.2f, transform.forward + transform.forward, 2);
        inputs[4] = RaySensor(transform.position + Vector3.up * 0.2f, transform.forward - transform.right, 2);

        inputs[5] = (float) Math.Tanh(rb.velocity.magnitude * 0.05f);
        inputs[6] = (float) Math.Tanh(rb.angularVelocity.y * 0.1f);
        
        inputs[7] = 1;

    }

    void OutputUpdate()
    {
        net.FeedForward(inputs);

        carController.horizontalInput = net.neurons[net.layers.Length - 1][0];
        carController.verticalInput = net.neurons[net.layers.Length - 1][1];
    }

    float currentDistance;
    void FitnessUpdate()
    {
        currentDistance = distanceTraveled + (nextCheckpointDist - (transform.position - nextCheckpoint.position).magnitude);

        if (fitness < currentDistance)
        {
            fitness = currentDistance;
        }
    }

    RaycastHit hit;
    float range = 4;
    public LayerMask layerMask;
    float hitDegree;
    float RaySensor(Vector3 pos, Vector3 direction, float length)
    {
        hitDegree = 0;
        if(Physics.Raycast(pos, direction, out hit, length * range, layerMask))
        {
            hitDegree = (range * length - hit.distance) / (range * length);
            Debug.DrawRay(pos, direction * hit.distance, Color.Lerp(Color.red, Color.green, hitDegree));
            return hitDegree;
        }
        else
        {
            Debug.DrawRay(pos, direction * length, Color.red);
            return 0;
        }
    }

    public void CheckpointReached(Transform checkpoint)
    {
        distanceTraveled += nextCheckpointDist;
        nextCheckpoint = checkpoint;
        nextCheckpointDist = (transform.position - checkpoint.position).magnitude;
    }

    public Renderer render;
    public Material firstMaterial;
    public Material mutatedMaterial;
    public Material defaultMaterial;
    public void SetFirstMaterial()
    {
        render.material = firstMaterial;
    }

    public void SetMutatedMaterial()
    {
        render.material = mutatedMaterial;
    }

    public void SetDefaultMaterial()
    {
        render.material = defaultMaterial;
    }

    public int CompareTo(Agent other)
    {
        if(fitness < other.fitness)
        {
            return 1;
        }
        
        if(fitness> other.fitness)
        {
            return -1;
        }

        return 0;
    }
}
