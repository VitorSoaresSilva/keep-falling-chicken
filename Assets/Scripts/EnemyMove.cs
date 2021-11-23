using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMove : MonoBehaviour
{
    
    public Vector3[] directionsToSpawn;
    public Vector3[] waypoints;
    public int currentPoint = -1;
    public ParticleSystem[] particles;
    void Update()
    {
        if (currentPoint == -1) return;
        Vector3 position = transform.position;
        float distance = (position.z - waypoints[currentPoint].z);
        if (distance <= 0.2)
        {
            currentPoint = Mathf.Clamp(currentPoint + 1,0,waypoints.Length-1);
        }
        Vector3 movement = Vector3.MoveTowards(position, waypoints[currentPoint],
            EnemiesManager.instance.currSpeed * Time.deltaTime);
        
        transform.position = movement;

    }

    private void Awake()
    {
        particles = GetComponentsInChildren<ParticleSystem>();
    }

    public Vector3 GetRandomDirection()
    {
        if (directionsToSpawn.Length > 0)
        {
            return directionsToSpawn[Random.Range(0,directionsToSpawn.Length)];            
        }
        return Vector3.zero;
    }

    public void Initialize(Vector3[] positions)
    {
        currentPoint = -1;
        waypoints = new Vector3[positions.Length];
        waypoints = positions;
    }

    public void Spawn()
    {
        if (particles != null)
        {
            foreach (ParticleSystem particle in particles)
            {
                particle.Stop();
                if (particle.isStopped)
                {
                    particle.Play();
                }
            }
        }
        transform.position = waypoints[0];
        currentPoint = 0;
    }
}
