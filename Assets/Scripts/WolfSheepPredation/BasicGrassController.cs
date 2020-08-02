using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGrassController : MonoBehaviour
{
    public float energyAmount = 10;
    public float regenerationTime = 20;

    private string mode;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMode(string mode)
    {
        this.mode = mode;
    }
}
