using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWolfSheepEnvironment : MonoBehaviour
{
    public GameObject grassTile;
    public GameObject wolf;
    public GameObject sheep;

    private float tileOffset;
    private BoxCollider grassBoxCollider;
    private MeshCollider meshCollider;

    // Start is called before the first frame update
    void Start()
    {

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

                Vector3 tilePosition = new Vector3(i + xTileOffset, tile.transform.position.y, j - zTileOffset);
                tile.transform.localPosition = tilePosition;
            }
        }
        


        //Vector3 tilePosition = new Vector3(xTopLeft + tileOffset, tile.transform.position.y, zTopLeft - tileOffset);

        //Debug.Log("Tile position is " + tilePosition);

        //tile.transform.localPosition = tilePosition;
    }
}
