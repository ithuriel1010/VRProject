using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flockUnit : MonoBehaviour
{
    [SerializeField] private float FOVAngle;
    [SerializeField] private float smoothDump;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private Vector3[] directionToCheck;

    private flock assignedFlock;
    private List<flockUnit> cohesionNeighbours = new List<flockUnit>();
    private List<flockUnit> aligmentNeighbours = new List<flockUnit>();
    private List<flockUnit> avoidanceNeighbours = new List<flockUnit>();


    private Vector3 currentVelocity;
    private Vector3 currentObstacleAvoidanceVector;
    private float speed;

    public Transform myTransform { get; set; }

    private void Awake()
    {
        myTransform = transform;
    }


    public void AssingFlock(flock flock)
    {
        assignedFlock = flock;

    }


    public void InitializeSpeed(float speed)
    {
        this.speed = speed;
    }

    public void MoveUnit()
    {
        FindNeighbours();
        CalculateSpeed();
        var cohesionVector = CalculateCohesionVector() * assignedFlock.CohesionWeight;
        var avoidanceVector = CalculateAvoidanceVector() * assignedFlock.AvoidanceWeight;
        var aligmentVector = CalculateAligmentVector() * assignedFlock.AligmentWeight;
        var boundsVector = CalculateBoundsVector() * assignedFlock.BoundsWeight;
        var obstacleVector = CalculateObstacleVector() * assignedFlock.BoundsWeight;
        var moveVector = cohesionVector + avoidanceVector + aligmentVector + boundsVector + obstacleVector;
        moveVector = Vector3.SmoothDamp(myTransform.forward, moveVector, ref currentVelocity, smoothDump);
        moveVector = moveVector.normalized * speed;
        myTransform.forward = moveVector;
        myTransform.position += moveVector * Time.deltaTime;
    }

    private Vector3 CalculateObstacleVector()
    {
        var obstacleVector = Vector3.zero;
        RaycastHit hit;
        if(Physics.Raycast(myTransform.position, myTransform.forward, out hit, assignedFlock.obstacleDistance, obstacleMask))
        {
            obstacleVector = FindBestDirectionToAvoid();
        }
        else
        {
            currentObstacleAvoidanceVector = Vector3.zero;
        }
        return obstacleVector;
    }

    private Vector3 CalculateBoundsVector()
    {
        var centerOffset = assignedFlock.transform.position - myTransform.position;
        bool isFarCenter = (centerOffset.magnitude / assignedFlock.boundsDistance >= 0.9f);
        if (isFarCenter)
            return centerOffset.normalized;
        else
            return Vector3.zero;
    }

    private Vector3 CalculateCohesionVector()
    {
        var cohesionVector = Vector3.zero;
        if (cohesionNeighbours.Count == 0)
            return cohesionVector;
        int neighboursInFOV = 0;
        for (int i = 0; i < cohesionNeighbours.Count; i++)
        {
            if (IsInFOV(cohesionNeighbours[i].myTransform.position))
            {
                neighboursInFOV++;
                cohesionVector += cohesionNeighbours[i].myTransform.position;

            }

        }
        cohesionVector /= neighboursInFOV;
        cohesionVector -= myTransform.position;
        cohesionVector = Vector3.Normalize(cohesionVector);
        return cohesionVector;

    }


    private Vector3 CalculateAligmentVector()
    {
        var aligmentVector = myTransform.forward;
        if (aligmentNeighbours.Count == 0)
            return aligmentVector;
        int neighboursInFOV = 0;
        for (int i = 0; i < aligmentNeighbours.Count; i++)
        {
            if(IsInFOV(aligmentNeighbours[i].myTransform.position))
            {
                neighboursInFOV++;
                aligmentVector += aligmentNeighbours[i].myTransform.forward;
            }
        }
        aligmentVector /= neighboursInFOV;
        aligmentVector = Vector3.Normalize(aligmentVector);
        return aligmentVector;
    }

    private Vector3 CalculateAvoidanceVector()
    {
        var avoidanceVector = Vector3.zero;
        if (aligmentNeighbours.Count == 0)
            return avoidanceVector;
        int neighboursInFOV = 0;
        for (int i = 0; i < aligmentNeighbours.Count; i++)
        {
            if (IsInFOV(aligmentNeighbours[i].myTransform.position))
            {
                neighboursInFOV++;
                avoidanceVector += (myTransform.position - avoidanceNeighbours[i].myTransform.position);
            }
        }
        avoidanceVector /= neighboursInFOV;
        avoidanceVector = Vector3.Normalize(avoidanceVector);
        return avoidanceVector;
    }

    private void CalculateSpeed()
    {
        if (cohesionNeighbours.Count != 0)
        {
            speed = 0;
            for (int i = 0; i < cohesionNeighbours.Count; i++)
            {
                speed += cohesionNeighbours[i].speed;
            }
            speed /= cohesionNeighbours.Count;
            speed = Mathf.Clamp(speed, assignedFlock.minSpeed, assignedFlock.maxSpeed);
        }
    }

    private void FindNeighbours()
    {
        cohesionNeighbours.Clear();
        avoidanceNeighbours.Clear();
        aligmentNeighbours.Clear();
        var allUnits = assignedFlock.allUnits;
        for (int i = 0; i<allUnits.Length; i++)
        {
            var currentUnit = allUnits[i];
            if (currentUnit != this)
            {
                float currentNeighbourDistanceSqr = Vector3.SqrMagnitude(currentUnit.transform.position - myTransform.position);
                if(currentNeighbourDistanceSqr <= assignedFlock.cohesionDistance * assignedFlock.cohesionDistance)
                {
                    cohesionNeighbours.Add(currentUnit);
                }
                if(currentNeighbourDistanceSqr <= assignedFlock.aligmentDistance * assignedFlock.aligmentDistance)
                {
                    aligmentNeighbours.Add(currentUnit);
                }
                if(currentNeighbourDistanceSqr <= assignedFlock.avoidanceDistance * assignedFlock.avoidanceDistance)
                {
                    avoidanceNeighbours.Add(currentUnit);

                }
            }
        }


    }



    private bool IsInFOV(Vector3 position)
    {
        return Vector3.Angle(myTransform.forward, position - myTransform.position) <= FOVAngle;
    }


    private Vector3 FindBestDirectionToAvoid()
    {
        if (currentObstacleAvoidanceVector != Vector3.zero)
        {
            RaycastHit hit;
            if (Physics.Raycast(myTransform.position, myTransform.forward, out hit, assignedFlock.obstacleDistance, obstacleMask))
            {
                return currentObstacleAvoidanceVector;
            }
        }
        float maxDistance = int.MinValue;
        var direction = Vector3.zero;
        for (int i = 0; i < directionToCheck.Length; i++)
        {
            RaycastHit hit;
            var currentDirection = myTransform.TransformDirection(directionToCheck[i].normalized);
            if(Physics.Raycast(myTransform.position, currentDirection, out hit, assignedFlock.obstacleDistance, obstacleMask))
            {
                float currentDistance = (hit.point - myTransform.position).magnitude;
                if (currentDistance > maxDistance)
                {
                    maxDistance = currentDistance;
                    direction = currentDirection;
                }
            }
            else
            {
                direction = currentDirection;
                currentObstacleAvoidanceVector = currentDirection.normalized;
                return direction;
            }
           

        }
        return direction.normalized;
    }
}
