using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSheepController : MonoBehaviour
{
    public float speed = 20;
    public float rotationEpsilon = 45;
    public float maxEnergy = 50;
    public float energyConsumptionRate = 5;
    public float energyConsumptionTime = 2;

    private Rigidbody _rb;
    private bool _canMove;
    private float currentEnergy;
    private float currentEnergyConsumptionTime;
    private bool eaten;

    public float generationTime;
    [Range(1, 10)]
    public float fertilityRate = 2.2f;
    private float currentTimeUntilBirth;
    private BasicWolfSheepEnvironment environment;

    // Start is called before the first frame update
    void Start()
    {
        environment = transform.parent.GetComponent<BasicWolfSheepEnvironment>();
        eaten = false;
        _rb = GetComponent<Rigidbody>();
        currentEnergy = maxEnergy;
        currentEnergyConsumptionTime = 0;
        environment.IncreaseSpeciesAmount();
    }

    // Update is called once per frame
    void Update()
    {
        if (eaten)
        {
            environment.DecrementSpeciesAmount();
            Destroy(gameObject);
        }

        if (_rb.velocity.y == 0)
        {
            _canMove = true;
        }

        if (_rb.velocity.y != 0)
        {
            _canMove = false;
        }

        if (_canMove)
        {
            float step = speed * Time.deltaTime; // calculate distance to move
            Vector3 currentPosition = this.transform.position;
            Vector3 nextPosition = currentPosition + this.transform.forward;

            this.transform.position = Vector3.Lerp(currentPosition, nextPosition, step);
        }

        currentEnergyConsumptionTime += Time.deltaTime;
        currentTimeUntilBirth += Time.deltaTime;

        if (currentEnergyConsumptionTime >= energyConsumptionTime)
        {
            currentEnergy -= energyConsumptionRate;
            currentEnergyConsumptionTime = 0;
        }

        if (currentTimeUntilBirth >= generationTime)
        {
            StartCoroutine(GiveBirth());
            currentTimeUntilBirth -= generationTime;
        }

        if (currentEnergy <= 0)
        {
            environment.DecrementSpeciesAmount();
            Destroy(gameObject);
        }

    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        RaycastHit grassHit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            if (hit.distance <= 5)
            {
                Vector3 newRotation = transform.eulerAngles;

                float turnedAroundY = Random.Range(-newRotation.y - rotationEpsilon, -newRotation.y + rotationEpsilon);

                newRotation.y = turnedAroundY;

                transform.rotation = Quaternion.Euler(newRotation);
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out grassHit, 10f))
        {
            
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * grassHit.distance, Color.yellow);
            if (grassHit.distance <= 10)
            {
                GameObject grass = grassHit.collider.gameObject;
                BasicGrassController grassController = grass.GetComponent<BasicGrassController>();
                if (grassController == null)
                {
                    return;
                }
                float energyGain = grassController.EnergyCheck();
                
                // To prevent overeating
                if (energyGain + currentEnergy <maxEnergy)
                {
                    currentEnergy += grassController.Eaten();
                }
               
            }
        }

        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 1000, Color.white);
        }
    }

    public float Eaten()
    {
        eaten = true;

        return currentEnergy;
        
    }

    IEnumerator GiveBirth()
    {
        float weightResult = Random.value;

        float probablity = fertilityRate - Mathf.Floor(fertilityRate);

        int birthAmount;

        if (weightResult <= probablity)
        {
            birthAmount = (int)Mathf.Floor(fertilityRate);
        }

        else
        {
            birthAmount = (int)Mathf.Ceil(fertilityRate);
        }

        for (int i = 0; i < birthAmount; i++)
        {

            if (!environment.IsFull())
            {

                float randomX = Random.Range(transform.position.x - 10, transform.position.x + 10);
                float randomZ = Random.Range(transform.position.z - 10, transform.position.z + 10);
                Vector3 randomPosition = new Vector3(randomX, transform.position.y, randomZ);

                GameObject spawn = Instantiate(this.gameObject, randomPosition, Quaternion.identity, transform.parent);

                yield return new WaitForSeconds(0.5f);
            }

            else
            {
                yield return null;
            }
            
        }
    }
}
