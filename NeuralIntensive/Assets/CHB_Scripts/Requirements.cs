using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Requirements : MonoBehaviour, IComparable<Requirements>
{
    #region Variables
    public List<int> listOfInt;
    public int[] arrayOfInt;

    public int[][] jaggedArray2dOfInt;
    public int[][][] jaggedArray3dOfInt;

    public LayerMask layerMask;

    public int fitness;
    #endregion
    
    public int CompareTo(Requirements other)
    {
        if (fitness < other.fitness)
            return 1;

        if(fitness > other.fitness)
            return -1;

        return 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        TestList();
        TestArray();
        TestJaggedArray2D();
        TestJaggedArray3D();
        TestRaycast();
    }

    private void TestRaycast()
    {
        Vector3 origin = Vector3.zero;
        Vector3 direction = Vector3.up;
        float length = 2;

        RaycastHit hit;

        if(Physics.Raycast(origin, direction, out hit, length, layerMask))
        {
            Debug.DrawRay(origin, direction * hit.distance, Color.green);
        }
        else
        {
            Debug.DrawRay(origin, direction * length, Color.red);
        }
    }

    private void TestJaggedArray3D()
    {
        jaggedArray3dOfInt = new int[4][][];

        for (int x = 0; x < jaggedArray3dOfInt.Length; x++)
        {
            jaggedArray3dOfInt[x] = new int[jaggedArray3dOfInt.Length][];

            for (int y = 0; y < jaggedArray3dOfInt[x].Length; y++)
            {
                jaggedArray3dOfInt[x][y] = new int[jaggedArray3dOfInt[x].Length];

                for (int z = 0; z < jaggedArray3dOfInt[x][y].Length; z++)
                {
                    jaggedArray3dOfInt[x][y][z] = 3;
                    Debug.Log("jagg [" + x + "] [" + y + "] ["+ z +"] = " + jaggedArray3dOfInt[x][y][z]);
                }
            }
        }
    }

    private void TestJaggedArray2D()
    {
        int numPrompt = 1;
        jaggedArray2dOfInt = new int[4][];

        //jaggedArray2dOfInt[0] = new int[4];
        //jaggedArray2dOfInt[1] = new int[2];
        //jaggedArray2dOfInt[2] = new int[4];
        //jaggedArray2dOfInt[3] = new int[2];

        Debug.Log( "Jagged array length: "+ jaggedArray2dOfInt.Length);
        for (int x = 0; x < jaggedArray2dOfInt.Length; x++)
        {
            jaggedArray2dOfInt[x] = new int[x + 1];
            Debug.Log("column of jagg n°" + x + " has: " + jaggedArray2dOfInt[x].Length + " columns");

            for (int y = 0; y < jaggedArray2dOfInt[x].Length; y++)
            {
                jaggedArray2dOfInt[x][y] = numPrompt;
                numPrompt++;

                Debug.Log("jagg [" + x + "] [" + y + "] = " + jaggedArray2dOfInt[x][y]);
            }
        }
    }

    private void TestArray()
    {
        arrayOfInt = new int[4];
        arrayOfInt[0] = 3;
        arrayOfInt[1] = 1;
        arrayOfInt[2] = 0;
        arrayOfInt[3] = 2;

        for (int x = 0; x < arrayOfInt.Length; x++)
        {
            Debug.Log(arrayOfInt[x]);
        }
    }

    void TestList()
    {
        listOfInt = new List<int>();
        listOfInt.Add(99);
        listOfInt.Add(95);
        listOfInt.Add(123);

        listOfInt.RemoveAt(1);

        listOfInt = new List<int>();
        listOfInt.Add(4);
        listOfInt.Add(2);
        listOfInt.Add(3);
        listOfInt.Add(1);

        listOfInt.Sort();

        for (int x = 0; x < listOfInt.Count; x++)
        {
            Debug.Log(listOfInt[x]);
        }

        
    }
}
