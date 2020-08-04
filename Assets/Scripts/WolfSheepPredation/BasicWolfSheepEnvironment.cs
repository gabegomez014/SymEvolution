using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWolfSheepEnvironment : MonoBehaviour
{
    public GameObject grassTile;
    public GameObject wolf;
    public GameObject sheep;
    public int sheepAmount = 10;
    public int wolfAmount = 4;

    private float tileOffset;
    private BoxCollider grassBoxCollider;
    private MeshCollider meshCollider;
    private string mode;

    // Start is called before the first frame update
    void Start()
    {
        mode = "Wolf-Sheep";
        meshCollider = GetComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup()
    {
        
        // Model to base the rest off of
        GameObject exampleTile = Instantiate(grassTile);
        BoxCollider box = exampleTile.GetComponent<BoxCollider>();
        float xTileOffset = box.size.x / 2;
        float zTileOffset = box.size.z / 2;
        float tileXScale = box.size.x;
        float tileZScale = box.size.z;
        exampleTile.SetActive(false);

        float xLocalScale = transform.localScale.x;
        float zLocalScale = transform.localScale.z;
        float xTopRight = transform.localScale.x / 2;
        float zTopLeft = transform.localScale.z / 2;

        // Withinn this we can add code that changes the likelihood of grass spawning on a patch
        for (float i = -xTopRight; i < xTopRight; i += tileXScale)
        {
            for (float j = zTopLeft; j > -zTopLeft; j -= tileZScale)
            {
                GameObject tile = Instantiate(grassTile, transform);
                tile.GetComponent<BasicGrassController>().SetMode(this.mode);

                Vector3 tilePosition = new Vector3(i + xTileOffset, tile.transform.position.y, j - zTileOffset);
                tile.transform.localPosition = tilePosition;
            }
        }

        for (int i = 0; i < sheepAmount; i++)
        {
            GameObject sheepSpawn = Instantiate(sheep, transform);

            sheepSpawn.transform.position = new Vector3(xTopRight / 2, sheepSpawn.transform.position.y, zTopLeft / 2);
        }

        for (int i = 0; i < wolfAmount; i++)
        {
            GameObject wolfSpawn = Instantiate(wolf, transform);

            wolfSpawn.transform.position = new Vector3(-xTopRight / 2, wolfSpawn.transform.position.y, zTopLeft / 2);
        }

    }

    public void SetMode(string mode)
    {
        this.mode = mode;
        // Going to need to give it to the grass tiles if they are already spawned in.
    }
}
