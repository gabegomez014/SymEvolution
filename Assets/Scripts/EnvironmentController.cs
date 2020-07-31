using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    // To set the species that can be spawned
    public GameObject[] species;
    public Transform speciesHolder;

    // Setting up the environments carrying capacity
    public int carryingCapacity = 1000;

    [Range(0.0f, 10.0f)]
    public float species1FertilityRate = 2;
    [Range(0.0f, 10.0f)]
    public float species2FertilityRate = 2;

    private MeshCollider _meshCollider;
    private int _speciesSpawnAmount;
    private float _spawnPauser = 0.001f;

    private bool _gamePaused;

    private float _xDim;
    private float _zDim;

    private Transform NEdge;
    private Transform SEdge;
    private Transform EEdge;
    private Transform WEdge;

    private ArrayList readyToDie; // List of game objects ready to die

    private void Start()
    {
        _gamePaused = false;
        _meshCollider = GetComponent<MeshCollider>();
        readyToDie = new ArrayList();

        _xDim = _meshCollider.bounds.size.x;
        _zDim = _meshCollider.bounds.size.z;

        this.NEdge = this.transform.Find("NEdge");
        this.SEdge = this.transform.Find("SEdge");
        this.EEdge = this.transform.Find("EEdge");
        this.WEdge = this.transform.Find("WEdge");
    }

    private void Update()
    {
        if (speciesHolder.childCount >= carryingCapacity)
        {
            for (int i = 0; i < readyToDie.Count; i++)
            {
                GameObject spawn = (GameObject)readyToDie[i];

                readyToDie.RemoveAt(i);
                Destroy(spawn);
            }
        }
    }

    public void Setup()
    {
        if (carryingCapacity % 2 == 1)
        {
            carryingCapacity -= 1;
        }

        _speciesSpawnAmount = carryingCapacity / species.Length;

        for (int i = 0; i < species.Length; i++)
        {
            StartCoroutine(SpawnSpecies(species[i]));
        }
    }

    public void AddObjectReadyToDie(GameObject spawn)
    {
        readyToDie.Add(spawn);
    }

    IEnumerator SpawnSpecies(GameObject species)
    {

        for (int i = 0; i < _speciesSpawnAmount; i++)
        {
            float randomX = Random.Range(-_xDim/2, _xDim/2);
            float randomZ = Random.Range(-_zDim/2, _zDim/2);

            Vector3 randomPosition = new Vector3(randomX, _meshCollider.transform.position.y + 5, randomZ);

            GameObject spawn = Instantiate(species, randomPosition, Quaternion.identity, speciesHolder);

            spawn.GetComponent<SpeciesController>().SetEnvironment(_meshCollider, speciesHolder, this, NEdge, SEdge, EEdge, WEdge);

            yield return new WaitForSeconds(_spawnPauser);
        }
    }

    //IEnumerator SpeciesGeneration(float generationTime)
    //{
    //    while(!_gamePaused)
    //    {
    //        float randomX = Random.Range(-_boxCollider.size.x + 1, _boxCollider.size.x - 1);
    //        float randomZ = Random.Range(-_boxCollider.size.z + 1, _boxCollider.size.z - 1);

    //            Vector3 randomPosition = new Vector3(randomX, _boxCollider.size.y + _boxCollider.center.y, randomZ);

    //          Instantiate(species, randomPosition, Quaternion.identity, speciesHolder);

    //        yield return new WaitForSeconds(generationTime);
    //  }
    //}


}
