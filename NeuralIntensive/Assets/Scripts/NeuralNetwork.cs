using System;
using System.Collections.Generic;

[System.Serializable]
public class NeuralNetwork
{
    public int[] layers;
    public float[][] neurons;
    public float[][][] axons;

    public int x, y, z;

    public NeuralNetwork()
    {}

    public NeuralNetwork(int[] _layers)
    {
        layers = new int[_layers.Length];

        for (int x = 0; x < _layers.Length; x++)
        {
            layers[x] = _layers[x];
        }

        InitNeurons();
        InitAxons();
    }

    void InitNeurons()
    {
        neurons = new float[layers.Length][];

        for (int x = 0; x < layers.Length; x++)
        {
            neurons[x] = new float[layers[x]];
        }
    }

    void InitAxons() 
    {
        axons = new float[layers.Length - 1][][];

        for (x = 0; x < layers.Length - 1; x++)
        {
            axons[x] = new float[layers[x + 1]][];

            for (y = 0; y < layers[x + 1]; y++)
            {
                axons[x][y] = new float[layers[x]];

                for (z = 0; z < layers[x]; z++)
                {
                    axons[x][y][z] = UnityEngine.Random.Range(-1f, 1f);
                }
            }
        }
    }

}
