using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Manager : MonoBehaviour
{
    public int populationSize;
    public float trainingDuration = 30;
    public float mutationRate = 5;

    public Agent agentPrefab;
    public Transform agentGroup;

    Agent agent;

    List<Agent> agents = new List<Agent>();

    public CameraController cameraController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator InitCoroutine()
    {
        yield return null;
    }

}
