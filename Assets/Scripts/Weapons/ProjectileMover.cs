﻿using UnityEngine;
using System.Collections;

public class ProjectileMover : MonoBehaviour
{
    public float speed;
    public float range;
    float timeToDestroy;
    public float currentTime = 0f;
    public Vector3 originPos;

    public void Init(Vector3 _originPos, float _speed, float _range)
    {
        speed = _speed;
        range = _range;
        originPos = _originPos;
        rigidbody.velocity = transform.forward * speed;

    }

    //void Start()
    //{
    //    rigidbody.velocity = transform.forward * speed;
    //    timeToDestroy = range / speed;
    //    //Destroy(gameObject, timeToDestroy);
    //    //Invoke("ReturnToPool", timeToDestroy);
    //    currentTime = 0f;
    //}
    void Update()
    {
        //if(currentTime>=timeToDestroy)
        //{
        //    ReturnToPool();
        //}
        //currentTime += Time.deltaTime;
        if(Vector3.Distance(transform.position,originPos)>range+1)
        {
            ReturnToPool();
        }
    }
    public void ReturnToPool()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        ObjectPool.instance.PoolObject(gameObject);
    }

}
