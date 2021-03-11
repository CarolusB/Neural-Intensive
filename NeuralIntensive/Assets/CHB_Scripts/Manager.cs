using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class Manager : MonoBehaviour
{
    public static Manager instance;

    public int populationSize;
    public float trainingDuration = 30;
    public float mutationRate = 5;

    public Agent agentPrefab;
    public Transform agentGroup;

    Agent agent;

    List<Agent> agents = new List<Agent>();

    public CameraController cameraController;
    public TimeManipulator timeManipulator;
    public Text genCounter;

    #region Var for training time automation
    [SerializeField] bool automatedTrainingMode = false;
    bool doAutomateTraining;
    [Serializable]
    public class ProgressionRequirements
    {
        [Range(0.2f, 1.1f)]
        public float setProportion = 0.3f;
        public int numberOfCheckpoints = 1;
        public float timeAllowed = 5;
        public float wishedMutationRate = 5;
    }

    public ProgressionRequirements[] progSteps;
    public int currentProgStep;
    public int numberAgentsReached;
    #endregion

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        doAutomateTraining = automatedTrainingMode;
        currentProgStep = 0;
        generationCount = 0;
        numberAgentsReached = 0;

        if(doAutomateTraining)
            SetAutoStepTrainingTimeAndMutation(currentProgStep);

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

    float reachedProportion;
    public int generationCount;
    private void NewGeneration()
    {
        if (doAutomateTraining)
        {
            reachedProportion = (float)numberAgentsReached / populationSize;

            Debug.Log("Gen " + generationCount + " -> "+ numberAgentsReached + " agents or " + 
            (float)reachedProportion * 100 + "% have passed " + progSteps[currentProgStep].numberOfCheckpoints + "checkpoint(s)");

            if (reachedProportion >= progSteps[currentProgStep].setProportion)
            {
                if (currentProgStep < progSteps.Length -1)
                {
                    currentProgStep++;
                    SetAutoStepTrainingTimeAndMutation(currentProgStep);
                }
                else
                {
                    trainingDuration = 180;
                    mutationRate = 5;
                }
            }
        }
        generationCount++;
        genCounter.text = generationCount.ToString();
        timeManipulator.timerValue = trainingDuration;

        numberAgentsReached = 0;

        agents.Sort();
        AddOrRemoveAgents();
        Mutate();
        Reset();
        SetMaterials();
    }

    void SetAutoStepTrainingTimeAndMutation(int _progStep)
    {
        trainingDuration = progSteps[_progStep].timeAllowed;
        mutationRate = progSteps[_progStep].wishedMutationRate;

        Debug.Log("[Challenge " + _progStep + "] " + progSteps[_progStep].numberOfCheckpoints +
        " CPs in " + progSteps[_progStep].timeAllowed + "s");
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

        if (doAutomateTraining)
        {
            currentProgStep = 0;
            SetAutoStepTrainingTimeAndMutation(currentProgStep);
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

        if(doAutomateTraining)
            SetAutoStepTrainingTimeAndMutation(currentProgStep);
        
        End();
    }

    void InitNeuralViewer()
    {
        NeuralNetworkViewer.instance.Init(agents[0]);
    }
}
