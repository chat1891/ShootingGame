using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    Vector3 velovity;
    Rigidbody myRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Move(Vector3 _velocity)
    {
        velovity = _velocity;
    }

    public void Lookat(Vector3 lookPoint)
    {
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(heightCorrectedPoint);
    }

    /// <summary>
    /// executed at small regular steps, so it never go through a object
    /// in slow frame rate, it is calling fixedUpdate multiple times through a frame, through doing movement increments
    /// </summary>
    void FixedUpdate()
    {
        myRigidbody.MovePosition(myRigidbody.position + velovity * Time.fixedDeltaTime);
        //Time.fixedDeltaTime : time between fixedUpdate
    }
}
