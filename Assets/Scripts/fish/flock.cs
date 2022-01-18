using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flock : MonoBehaviour
{
    [Header("Spawn Setup")]
    [SerializeField] private flockUnit fishPrefab;
    [SerializeField] private int flockSize;
    [SerializeField] private Vector3 spawnBounds;


    [Header("Detection Distances")]
    [Range(0, 10)]
    [SerializeField] private float _cohesionDistance;
    public float cohesionDistance { get { return _cohesionDistance; } }

    [Range(0, 10)]
    [SerializeField] private float _avoidanceDistance;
    public float avoidanceDistance { get { return _avoidanceDistance; } }


    [Range(0, 10)]
    [SerializeField] private float _aligmentDistance;
    public float aligmentDistance { get { return _aligmentDistance; } }

    [Range(0, 10)]
    [SerializeField] private float _boundsDistance;
    public float boundsDistance { get { return _boundsDistance; } }

    [Range(0, 10)]
    [SerializeField] private float _obstacleDistance;
    public float obstacleDistance { get { return _obstacleDistance; } }

    [Header("Speed Setup")]
    [Range(0, 10)]
    [SerializeField] private float _minSpeed;
    [Range(0, 10)]
    [SerializeField] private float _maxSpeed;


    [Header("Behaviour Weights")]
    [Range(0, 10)]
    [SerializeField] private float _cohesionWeight;
    [Range(0, 10)]
    [SerializeField] private float _avoidanceWeight;
    [Range(0, 10)]
    [SerializeField] private float _aligmentWeight;
    [Range(0, 10)]
    [SerializeField] private float _boundsWeight;
    [Range(0, 10)]
    [SerializeField] private float _obstacleWeight;


    public flockUnit[] allUnits { get; set; }

    public float minSpeed { get => _minSpeed; set => _minSpeed = value; }
    public float maxSpeed { get => _maxSpeed; set => _maxSpeed = value; }
    public float CohesionWeight { get => _cohesionWeight; set => _cohesionWeight = value; }
    public float AvoidanceWeight { get => _avoidanceWeight; set => _avoidanceWeight = value; }
    public float AligmentWeight { get => _aligmentWeight; set => _aligmentWeight = value; }
    public float BoundsWeight { get => _boundsWeight; set => _boundsWeight = value; }
    public float ObstacleWeight { get => _obstacleWeight; set => _obstacleWeight = value; }

    private void Start()
    {
        GenerateUnits();   
    }

    private void Update()
    {
        for (int i = 0; i<allUnits.Length; i++)
        {
            allUnits[i].MoveUnit();
        }
    }

    // Update is called once per frame
    void GenerateUnits()
    {
        allUnits = new flockUnit[flockSize];
        for (int i = 0; i < flockSize; i++)
        {
            var randomVector = UnityEngine.Random.insideUnitSphere;
            randomVector = new Vector3(randomVector.x * spawnBounds.x, randomVector.y * spawnBounds.y, randomVector.z * spawnBounds.z);
            var spawnPosition = transform.position + randomVector;
            var rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
            allUnits[i] = Instantiate(fishPrefab, spawnPosition, rotation);
            allUnits[i].AssingFlock(this);
            allUnits[i].InitializeSpeed(Random.Range(minSpeed, maxSpeed));
        }

    }
}
