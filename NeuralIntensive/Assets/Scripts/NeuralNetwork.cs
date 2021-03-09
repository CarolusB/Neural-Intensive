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

    float value;
    public void FeedForward(float[] inputs)
    {
        neurons[0] = inputs;
        for (x = 1; x < layers.Length; x++)
        {
            for (y = 0; y < layers[x]; y++)
            {
                value = 0;

                for (z = 0; z < layers[x-1]; z++)
                {
                    value += neurons[x - 1][z] * axons[x - 1][y][z];
                }

                neurons[x][y] = (float) Math.Tanh(value);
            }
        }
    }

    float randNum;
    public void Mutate(float probability)
    {
        for (x = 0; x < axons.Length; x++)
        {
            for (y = 0; y < axons[x].Length; y++)
            {
                for (z = 0; z < axons[x][y].Length; z++)
                {
                    randNum = UnityEngine.Random.Range(0f, 100f);

                    if(randNum < 0.06f * probability)
                    {
                        axons[x][y][z] = UnityEngine.Random.Range(-1f, 1f);
                    }
                    else if(randNum < 0.07f * probability)
                    {
                        axons[x][y][z] *= -1f;
                    }
                    else if(randNum < 0.5f * probability)
                    {
                        axons[x][y][z] = 0.1f*UnityEngine.Random.Range(-1f, 1f);
                    }
                    else if(randNum < 0.75f * probability)
                    {
                        axons[x][y][z] *= UnityEngine.Random.Range(0f, 1f) + 1f;
                    }
                    else if(randNum < 1 * probability)
                    {
                        axons[x][y][z] *= UnityEngine.Random.Range(0f, 1f);
                    }
                }
            }
        }
    }
}
