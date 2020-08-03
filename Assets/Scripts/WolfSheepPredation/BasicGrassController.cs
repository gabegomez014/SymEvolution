using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGrassController : MonoBehaviour
{
    public float energyAmount = 10;
    public float regenerationTime = 20;

    public Material dirtMat;
    public Material grassMat;

    private bool readyToBeEaten;
    private float currentRegenTime;
    private MeshRenderer meshRenderer;

    private string mode;
    // Start is called before the first frame update
    void Start()
    {
        readyToBeEaten = true;
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!readyToBeEaten)
        {
            currentRegenTime += Time.deltaTime;
        }

        if (currentRegenTime >= regenerationTime)
        {
            currentRegenTime = 0;
            this.meshRenderer.material = grassMat;
            readyToBeEaten = true;
        }
    }

    public void SetMode(string mode)
    {
        this.mode = mode;
    }

    public float Eaten()
    {
        if (mode == "Wolf-Sheep-Grass")
        {
            currentRegenTime = 0;
            readyToBeEaten = false;
            this.meshRenderer.material = dirtMat;
        }

        return energyAmount;
    }

    public float EnergyCheck()
    {
        return energyAmount;
    }
}
