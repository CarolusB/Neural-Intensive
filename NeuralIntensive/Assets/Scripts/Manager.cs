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

    #region Var for training time automation
    [Serializable]
    public class ProgressionRequirements
    {
        [Range(20f, 70f)]
        public float setProportion = 30;
        public int numberOfCheckpoints = 1;
        public float timeAllowed = 5;
    }

    public ProgressionRequirements[] progSteps;
    int currentProgStep;

    List<Agent> agentsReached = new List<Agent>();
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        currentProgStep = 0;
        CheckpointManager.instance.Init();
        StartCoroutine(InitCoroutine());
    }

    IEnumerator InitCoroutine()
    {
        NewGeneration();
        InitNeuralViewer();
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
    
    public void End()
    {
        StopAllCoroutines();
        StartCoroutine(Loop());
    }
    
    private void FocusCar()
    {
        NeuralNetworkViewer.instance.agent = agents[0];
        NeuralNetworkViewer.instance.RefreshAxons();
        
        cameraController.target = agents[0].transform;
    }

    public void Refocus()
    {
        agents.Sort();
        FocusCar();
    }

    private void NewGeneration()
    {
        agents.Sort();
        AddOrRemoveAgents();
        Mutate();
        Reset();
        SetMaterials();
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
        agent.nextCheckpoint = CheckpointManager.instance.firstCheckpoint;

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

    public void ResetNets()
    {
        for (int i = 0; i < agents.Count; i++)
        {
            agents[i].net = new NeuralNetwork(agentPrefab.net.layers);
        }

        End();
    }

    public void Save()
    {
        List<NeuralNetwork> nets = new List<NeuralNetwork>();

        for (int i = 0; i < agents.Count; i++)
        {
            nets.Add(agents[i].net);
        }

        DataManager.instance.Save(nets);
    }

    public void Load()
    {
        Data data = DataManager.instance.Load();

        if(data != null)
        {
            for (int i = 0; i < agents.Count; i++)
            {
                agents[i].net = data.nets[i];
            }
        }

        End();
    }

    void InitNeuralViewer()
    {
        NeuralNetworkViewer.instance.Init(agents[0]);
    }
}
