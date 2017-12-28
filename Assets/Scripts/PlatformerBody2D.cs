using System.Collections;
using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


// A manually-controlled rigidbody for platformers.
public class PlatformerBody2D : MonoBehaviour {

    public Vector2 maxSpeed = new Vector2(5,5);
    public Vector2 timeToMaxSpeed = new Vector2(1,2);
    public float gravityScale = 1;
    public Vector2 dragAccelerationMagnitude = new Vector2(1,1);

    // Public for debugging purposes!
    public Vector2 velocity = new Vector2(0,0);
    public Vector2 maxAccelerationMagnitude;
    public Vector2 queuedAcceleration = new Vector2(0,0);

    // Use this for initialization
    void Start() {
        // todo: calculate based on timeToMaxSpeed.
        maxAccelerationMagnitude = new Vector2(5,5);
    }

    // Update is called once per frame
    void Update() {
    }

    // FixedUpdate is called once every x seconds (multiple times per frame)
    void FixedUpdate() {
        // Only account for gravity here.
        Vector2 gravity = Physics.gravity * gravityScale;
        queuedAcceleration += gravity;

        // v = a * t
        velocity += queuedAcceleration * Time.deltaTime;
        // d = v * t + .5 * a * t^2
        Vector2 delta = velocity * Time.deltaTime + queuedAcceleration * Mathf.Pow(Time.deltaTime, 2.0f);

        //~ transform.Translate(delta);
        Vector2 pos = transform.position;
        Vector2 target = pos;
        target += delta;
        GetComponent<Rigidbody2D>().MovePosition(target);

        // todo: figure out actual velocity based on how we moved.
        Vector2 new_pos = transform.position;
        delta = new_pos - pos;

        // TODO: If velocity is 0 (we're blocked), then we should not
        // accelerate.
        queuedAcceleration = dragAccelerationMagnitude;
        queuedAcceleration.x *= ApplyVelocityBasedDrag(velocity.x, dragAccelerationMagnitude.x);
        queuedAcceleration.y *= ApplyVelocityBasedDrag(velocity.y, dragAccelerationMagnitude.y);
    }

    float ApplyVelocityBasedDrag(float velocity, float drag) {
        if (Mathf.Approximately(velocity, 0.0f)) {
            return 0.0f;
        }
        else {
            return -1.0f * Mathf.Sign(velocity);
        }
    }

    // TODO: I think I want these to take a normalized value for direction?
    // Or at least clamped in -1,1?
    public void AccelerateUp(float direction) {
        queuedAcceleration.y = direction * maxAccelerationMagnitude.y;
    }

    public void AccelerateRight(float direction) {
        queuedAcceleration.x = direction * maxAccelerationMagnitude.x;
    }

}
