using System.Collections;
using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


// Player control for object using my custom fuzzx.
public class FuzzxPlayerControl : MonoBehaviour {

    private PlatformerBody2D body;

    // Does nothing. Only exists to visualize current speed.
    public float DEBUG_speedDisplay = 0.0f;

    // Use this for initialization
    void Start() {
        body = GetComponent<PlatformerBody2D>();
    }

    public enum JumpRequest {
        Idle,
        Jump,
        Stop,
    };

    public enum AirState {
        OnGround,
        Rising,         // TODO: Add more force the longer you hold.
        // Neutral,
        Falling,
    };

    // public to visualize values during debugging
    public JumpRequest jump = JumpRequest.Idle;
    public AirState air = AirState.OnGround;
    public float moveDirection = 0.0f;

    public float maxSpeed = 5.0f;

    // Update is called once per frame
    void Update() {

        switch (jump) {
            case JumpRequest.Idle:
                if (IsRequestingJump()) {
                    jump = JumpRequest.Jump;
                }
                break;

            case JumpRequest.Jump:
                if (false == IsRequestingJump()) {
                    jump = JumpRequest.Stop;
                }
                break;

            case JumpRequest.Stop:
                // wait here until we are allowed to jump again.
                if (air == AirState.OnGround) {
                    jump = JumpRequest.Idle;
                }
                break;
        } 


        moveDirection = GetRequestedMove();

    }

    // FixedUpdate is called once every x seconds (multiple times per frame)
    void FixedUpdate() {
        if (air == AirState.Rising) {
            if (jump == JumpRequest.Stop) {
                // TODO: Only start falling when moving in direction of gravity?
                //air = AirState.Neutral;
                air = AirState.Falling;
            }
        }
        else {
            if (jump == JumpRequest.Jump) {
                StartJump();
                air = AirState.Rising;
            }
        }

        if (moveDirection == 0.0f) {
        }
        else {
            float velocity = body.velocity.x;
            DEBUG_speedDisplay = velocity;

            float negativeIfChangingDirections = Math.Sign(velocity) * Math.Sign(moveDirection);

            StartMove(moveDirection, Math.Abs(velocity) / maxSpeed * negativeIfChangingDirections);
        }

    }

    void StartJump() {
        body.AccelerateUp(1.0f);
        Debug.Log("started jump", this);
    }

    void StartMove(float move_input, float speedLimiterProgress) {
        body.AccelerateRight(Math.Sign(move_input));
        Debug.Log(string.Format("started move: move_input={0} speedLimiterProgress={1}", move_input, speedLimiterProgress), this);
    }

    bool IsRequestingJump() {
        return CrossPlatformInputManager.GetButtonDown("Jump")
            || CrossPlatformInputManager.GetAxis("Vertical") > 0;
    }

    float GetRequestedMove() {
        float epsilon = 0.1f;
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        if (Math.Abs(h) > epsilon) {
            return h;
        }
        else {
            return 0.0f;
        }
    }

    void OnCollisionEnter2D(Collision2D bam) {
        CheckForGroundTouch(bam);
    }
    void OnCollisionStay2D(Collision2D bam) {
        CheckForGroundTouch(bam);
    }

    void CheckForGroundTouch(Collision2D potentialFloor) {
        // Checking contact points ensures only jumping off supporting
        // (gravity-interfering) surfaces.
        Vector2 avg_normal = new Vector2(0,0);

        /*~ 
          // TODO: make contacts a member variable to prevent alloc
        ContactPoint2D[] contacts = new ContactPoint2D[8];
        int count = potentialFloor.GetContacts(contacts);
        */
        ContactPoint2D[] contacts = potentialFloor.contacts;
        int count = contacts.Length;
        for (int i = 0; i < count; ++i) {
            var c = contacts[i];
            avg_normal += c.normal;
        }
        avg_normal /= count;


        float dot = Vector2.Dot(Physics.gravity, avg_normal);
        bool isOnGround = dot < 0;
        if (isOnGround) {
            air = AirState.OnGround;
        }
        else if (air != AirState.Rising) {
            // Probably walked off a ledge.
            air = AirState.Falling;
        }
    }

}
