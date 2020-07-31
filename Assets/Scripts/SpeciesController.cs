using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeciesController : MonoBehaviour
{
    private EnvironmentController environmentController;
    private MeshCollider _environmentCollider;
    private BoxCollider _speciesCollider;
    private Rigidbody _rb;
    private bool _canMove;

    private Transform NEdge;
    private Transform SEdge;
    private Transform EEdge;
    private Transform WEdge;

    public float speed = 20;
    public float generationTime;
    [Range(1,10)]
    public float fertilityRate = 2.2f;
    public float rotationEpsilon = 45;

    private Transform speciesHolder;

    private float currentTimeUntilBirth;

    // Start is called before the first frame update
    void Start()
    {
        currentTimeUntilBirth = 0;
        _canMove = false;
        _speciesCollider = GetComponent<BoxCollider>();
        _rb = GetComponent<Rigidbody>();

        Vector3 randomRotation = transform.eulerAngles;
        randomRotation.y = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(randomRotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (_rb.velocity.y == 0)
        {
            _canMove = true;
        }

        if (_canMove)
        {
            float step = speed * Time.deltaTime; // calculate distance to move
            Vector3 currentPosition = this.transform.position;
            Vector3 nextPosition = currentPosition + this.transform.forward;

            this.transform.position = Vector3.Lerp(currentPosition, nextPosition, step);
            currentTimeUntilBirth += Time.deltaTime;

            if (currentTimeUntilBirth >= generationTime)
            {
                environmentController.AddObjectReadyToDie(this.gameObject);
                StartCoroutine(GiveBirth());
                currentTimeUntilBirth -= generationTime;
            }
        }
    }

    private void FixedUpdate()
    {
        RaycastHit hit;

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
    }

    IEnumerator GiveBirth()
    {
        float weightResult = Random.value;

        float probablity = fertilityRate - Mathf.Floor(fertilityRate);

        int birthAmount;

        if (weightResult <= probablity)
        {
            birthAmount = (int) Mathf.Floor(fertilityRate);
        }

        else
        {
            birthAmount = (int)Mathf.Ceil(fertilityRate);
        }

        for (int i = 0; i < birthAmount; i++)
        {
            if (environmentController.speciesHolder.childCount < environmentController.carryingCapacity)
            {
                float _xDim = _environmentCollider.bounds.size.x;
                float _zDim = _environmentCollider.bounds.size.z;

                float randomX = Random.Range(-_xDim / 2, _xDim / 2);
                float randomZ = Random.Range(-_zDim / 2, _zDim / 2);

                Vector3 randomPosition = new Vector3(randomX, _environmentCollider.transform.position.y + 5, randomZ);

                GameObject spawn = Instantiate(this.gameObject, randomPosition, Quaternion.identity, speciesHolder);

                spawn.GetComponent<SpeciesController>().SetEnvironment(_environmentCollider, speciesHolder, environmentController, NEdge, SEdge, EEdge, WEdge);

                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    public void SetEnvironment(MeshCollider meshCollider, Transform speciesHolder, EnvironmentController controller, Transform NEdge, Transform SEdge, Transform EEdge, Transform WEdge)
    {
        this.environmentController = controller;
        _environmentCollider = meshCollider;
        this.speciesHolder = speciesHolder;
        this.NEdge = NEdge;
        this.SEdge = SEdge;
        this.WEdge = WEdge;
        this.EEdge = EEdge;

    }
}
