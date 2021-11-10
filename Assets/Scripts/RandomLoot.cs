using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLoot : MonoBehaviour
{
    public List<GameObject> lights;
    public int[] table = 
    {
        90, //coin
        10  //potion
    };

    public int total;
    public int randomNumber;

    private void Start()
    {
        // tally the total weight
        // draw a random number between 0 and total weight (100)

        foreach(var item in table)
        {
            total += item;
        }

        randomNumber = Random.Range(0, total);

        for(int i = 0; i < table.Length; i++)
        {
            if (randomNumber <= table[i])
            {
                lights[i].SetActive(true);
                return;
            }
            else
            {
                randomNumber -= table[i];
            }
        }

    }
}