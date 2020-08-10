using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWolfController : MonoBehaviour
{
    public float speed = 20;
    public float rotationEpsilon = 45;
    public float maxEnergy = 75;
    public float energyConsumptionRate = 5;
    public float energyConsumptionTime = 2;

    private Rigidbody _rb;
    private bool _canMove;
    private float currentEnergy;
    private float currentEnergyConsumptionTime;

    private Transform sheepTracking;
    private bool hunting;

    public float generationTime;
    [Range(1, 10)]
    public float fertilityRate = 2.2f;
    private float currentTimeUntilBirth;
    private BasicWolfSheepEnvironment environment;

    // Start is called before the first frame update
    void Start()
    {
        environment = transform.parent.GetComponent<BasicWolfSheepEnvironment>();
        hunting = false;
        _rb = GetComponent<Rigidbody>();
        currentEnergy = maxEnergy;
        currentEnergyConsumptionTime = 0;
        environment.IncreaseSpeciesAmount();
    }

    // Update is called once per frame
    void Update()
    {
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
            if (hunting && sheepTracking != null)
            {

                // Determine which direction to rotate towards
                Vector3 targetDirection = sheepTracking.position - transform.position;

                // The step size is equal to speed times frame time.
                float singleStep = (speed / 2) * Time.deltaTime;

                // Rotate the forward vector towards the target direction by one step
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.5f);

                // Draw a ray pointing at our target in
                Debug.DrawRay(transform.position, newDirection, Color.red);

                // Calculate a rotation a step closer to the target and applies rotation to this object
                transform.rotation = Quaternion.LookRotation(newDirection);

                float step = speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, sheepTracking.position, step);
            }

            else
            {
                float step = speed * Time.deltaTime; // calculate distance to move
                Vector3 currentPosition = this.transform.position;
                Vector3 nextPosition = currentPosition + this.transform.forward;

                this.transform.position = Vector3.Lerp(currentPosition, nextPosition, step);
            }

            currentTimeUntilBirth += Time.deltaTime;

            if (currentTimeUntilBirth >= generationTime)
            {
                StartCoroutine(GiveBirth());
                currentTimeUntilBirth -= generationTime;
            }
        }

        currentEnergyConsumptionTime += Time.deltaTime;

        if (currentEnergyConsumptionTime >= energyConsumptionTime)
        {
            currentEnergy -= energyConsumptionRate;
            currentEnergyConsumptionTime = 0;

            if (currentEnergy <= 3 * maxEnergy / 4 && !hunting)
            {
                hunting = true;
            }
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

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 50))
        {
            if (hit.collider.tag == "Sheep" && hunting && hit.distance < 20)
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
                sheepTracking = hit.collider.transform;
            }

            if (hit.collider.tag == "Sheep" && hunting && hit.distance < 5)
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);
                currentEnergy = sheepTracking.GetComponent<BasicSheepController>().Eaten();
                sheepTracking = null;
            }

            if (hit.distance <= 5 && hit.collider.tag == "Walls")
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Vector3 newRotation = transform.eulerAngles;

                if (newRotation.y < 0)
                {
                    newRotation.y += 180;
                }

                else
                {
                    newRotation.y -= 180;
                }

                float randomizedY = Random.Range(newRotation.y - rotationEpsilon, newRotation.y + rotationEpsilon);

                newRotation.y = randomizedY;

                transform.rotation = Quaternion.Euler(newRotation);
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        }
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
    
                float randomX = Random.Range(transform.position.x - 10, transform.position.x + 10);
                float randomZ = Random.Range(transform.position.z - 10, transform.position.z + 10);
                Vector3 randomPosition = new Vector3(randomX, transform.position.y, randomZ);

                GameObject spawn = Instantiate(this.gameObject, randomPosition, Quaternion.identity, transform.parent);

                yield return new WaitForSeconds(0.5f);
            
        }
    }
}
