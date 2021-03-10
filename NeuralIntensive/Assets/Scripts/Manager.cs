using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

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
        CheckpointManager.instance.Init();
        StartCoroutine(InitCoroutine());
    }

    IEnumerator InitCoroutine()
    {
        NewGeneration();
        FocusCar();
        yield return new WaitForSeconds(trainingDuration);

        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        NewGeneration();
        FocusCar();
        yield return new WaitForSeconds(trainingDuration);

        StartCoroutine(Loop());
    }
    private void FocusCar()
    {
        cameraController.target = agents[0].transform;
    }

    private void NewGeneration()
    {
        agents.Sort();
        AddOrRemoveAgents();
    }

    private void AddOrRemoveAgents()
    {
        if(agents.Count != populationSize)
        {
            int dif = populationSize - agents.Count;

            if (dif > 0)
            {
                for (int i = 0; i < dif; i++)
                {
                    AddAgent();
                }
            }
            else
            {
                for (int i = 0; i < dif; i++)
                {
                    RemoveAgent();
                }
            }
        }
    }

    void AddAgent()
    {
        agent = Instantiate(agentPrefab, Vector3.zero, Quaternion.identity, agentGroup);
        agent.net = new NeuralNetwork(agentPrefab.net.layers);

        agents.Add(agent);
    }

    void RemoveAgent()
    {
        Destroy((agents[agents.Count - 1]).transform);
        agents.RemoveAt(agents.Count - 1);
    }

    void Mutate()
    {
        for (int i = agents.Count/2; i < agents.Count; i++)
        {
            agents[i].net.CopyNet(agents[i - agents.Count / 2].net);
            agents[i].net.Mutate(mutationRate);
            agents[i].SetMutatedMaterial();
        }
    }

    private void Reset()
    {
        for (int i = 0; i < agents.Count; i++)
        {
            agents[i].ResetAgent();
        }
    }

    void SetMaterials()
    {
        agents[0].SetFirstMaterial();

        for (int i = 1; i < agents.Count/2; i++)
        {
            agents[i].SetDefaultMaterial();
        }
    }
}
