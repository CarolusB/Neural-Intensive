using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform nextCheckpoint;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.GetComponent<Agent>())
        {
            if (other.transform.parent.GetComponent<Agent>().nextCheckpoint == transform)
                other.transform.parent.GetComponent<Agent>().CheckpointReached(nextCheckpoint);
        }
    }
}
