using System;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public Transform ghostPrefab;
    public Transform ghostParent;
    
    private Transform car;
    private TransformSavePoint[,] carGhostsReplays;
    private int ghostsMaxCount = 5;
    private List<Transform> ghosts = new();
    private int framesPerSecForeReplay = 50;
    private int index;
    private int indexLimit;

    private DateTime startRecordTime;
    
    public void StartNewRace(Transform car)
    {
        Transform PreviousCar = this.car;
        StartNewGhostRecord(car);
        
        if (PreviousCar is not null)
        {
            CreateNewGhost();
        }

        if(ghosts.Count >= 0)
            ReplayCarMovement();
    }
    private void StartNewGhostRecord(Transform car)
    {
        if(ghosts.Count >= ghostsMaxCount)
            return;
        
        this.car = car;
        startRecordTime = DateTime.Now;
    }
    private void CreateNewGhost()
    {
        if(ghosts.Count >= ghostsMaxCount)
            return;
        
        Transform newGhost = Instantiate(ghostPrefab, ghostParent);
        ghosts.Add(newGhost);
    }

    public void ReplayCarMovement()
    {
        index = 0;
        ghosts = new List<Transform>();
        for(int i = 0; i < ghostParent.childCount; i++)
        {
            ghosts.Add(ghostParent.GetChild(i));
        }
    }

    private void Awake()
    {
        indexLimit = 300 * framesPerSecForeReplay; //5 minutes = 60 sec
        carGhostsReplays = new TransformSavePoint[ghostsMaxCount, indexLimit];
    }

    private void FixedUpdate()
    {
        if (DateTime.Now > startRecordTime.AddMilliseconds((1000 * index) / framesPerSecForeReplay))// in 1 second is 1000 milliseconds
        {
            index++;
            if (index >= indexLimit)
            {
                enabled = false;
                return;
            }
            ReplayCar();
            RecordCar();
        }
    }

    private void RecordCar()
    {
        if(ghosts.Count >= ghostsMaxCount) return;
        
        TransformSavePoint savePoint = new()
        {
            Position = car.position,
            Rotation = car.eulerAngles
        };
        carGhostsReplays[ghosts.Count, index] = savePoint;
    }

    private void ReplayCar()
    {
        foreach (var ghostCar in ghosts)
        {
            ghostCar.position = carGhostsReplays[ghostCar.GetSiblingIndex(), index].Position;
            ghostCar.rotation = Quaternion.Euler(carGhostsReplays[ghostCar.GetSiblingIndex(), index].Rotation);
        }
    }
}