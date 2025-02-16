﻿ using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 10f; // скорость врага

    protected Rigidbody rb;

    protected Vector3 TargetPosition;

    protected Vector3 StartPoint;
    protected Vector3 EndPoint;

    public float Distance;

    protected enum EMovement
    {
        Idle,
        StartToEnd,
        EndToStart,
    }

    protected EMovement CurrentMovement  = EMovement.Idle;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    public void Update()
    {
        Distance = (TargetPosition - transform.position).magnitude;

        if (CurrentMovement != EMovement.Idle)
        {
            if ((TargetPosition - transform.position).magnitude < 0.5f)
            {
                SwitchTargets();
            }

            rb.velocity = (TargetPosition - transform.position).normalized * moveSpeed;
            transform.LookAt(TargetPosition);
        }
    }

    public virtual void SwitchTargets()
    {
        switch (CurrentMovement)
        {
            case EMovement.StartToEnd:
                TargetPosition = StartPoint;
                CurrentMovement = EMovement.EndToStart;
                break;
            case EMovement.EndToStart:
                TargetPosition = EndPoint;
                CurrentMovement = EMovement.StartToEnd;
                break;
        }
    }

    public void Stop()
    {
        rb.velocity = Vector3.zero;
        CurrentMovement = EMovement.Idle;
    }

    public virtual void SetPatrollingPoint(Vector3 newPoint)
    {
        EndPoint = newPoint;
        StartPoint = transform.position;
        SetTargetPosition(EndPoint);

        CurrentMovement = EMovement.StartToEnd;
    }

    public void SetTargetPosition(Vector3 newPositioin)
    {
        TargetPosition = newPositioin;
    }

    public void Resume()
    {
        CurrentMovement = EMovement.StartToEnd;
    }
}
