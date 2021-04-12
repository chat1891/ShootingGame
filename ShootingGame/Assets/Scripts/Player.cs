using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(PlayerController))]
[RequireComponent (typeof(GunController))]
public class Player : LivingEntity
{
    public float moveSpeed = 5;
    PlayerController controller;
    Camera viewCamera;
    GunController gunController;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();//this calls start method in LivingEntity
        controller = GetComponent<PlayerController>();
        gunController = GetComponent<GunController>();
        viewCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //---add movement
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        controller.Move(moveVelocity);

        //---Look input
        //give screen position of mouse
        //return a ray from camera through that position to infinity
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        //then need to intersect that ray with ground plane to see where we want our player to be looking
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        //returns true, if ray intersect with plane
        //rayDistance is distance from camera to intersection
        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 pointIntersection = ray.GetPoint(rayDistance);
            //Debug.DrawLine(ray.origin, pointIntersection, Color.red);

            controller.Lookat(pointIntersection);
        }

        // ---weapon input
        //left mouse button is pressed
        if (Input.GetMouseButton(0) || Input.GetKey("space"))
        {
            gunController.shoot();
        }
    }
}
