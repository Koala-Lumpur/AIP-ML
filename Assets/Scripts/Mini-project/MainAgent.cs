using System;
using UnityEngine;
using MLAgents;
using MLAgents.Sensors;

public class MainAgent : Agent
{
    public float moveSpeed;

    public float turnSpeed;
    
    private TrainingArea trainingArea;
        new private Rigidbody rb;
        private GameObject goal;
        public GameObject button;
        public GameObject pushBlock;
        private Rigidbody pushBlockRb;
        public bool goalReached = false;
        private float goalRadius = 1.0f;
        

        private void Start()
        {
            //Get the rigidbody component on the pushblock
            pushBlockRb = pushBlock.GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            //Every 5 steps request decision, which is great for physics based simulations
            if (StepCount % 5 == 0)
            {
                RequestDecision();
            }
            else
            {
                RequestAction();
            }
        }
        
        //Override the Initialize function in the Agent class
        public override void Initialize()
    {
        //Call the base class function
        base.Initialize();
        
        //Get components and goal object for later use
        trainingArea = GetComponentInParent<TrainingArea>();
        goal = trainingArea.goal;
        rb = GetComponent<Rigidbody>();
    }
        
    //When action is received from the AI
    public override void OnActionReceived(float[] vectorAction)
    {
        //Determine action of the agent
        float forwardAmount = vectorAction[0];
        float turnAmount = 0f;
        if (vectorAction[1] == 1f)
        {
            turnAmount = -1f;
        }
        else if (vectorAction[1] == 2f)
        {
            turnAmount = 1f;
        }
        
        //Move and rotate agent based on the vectoraction of the agent
        rb.MovePosition(transform.position + transform.forward * forwardAmount * moveSpeed * Time.fixedDeltaTime);
        transform.Rotate(transform.up * turnAmount * turnSpeed * Time.fixedDeltaTime);
        
        //Decrease reward based on the maxStep count
        if(maxStep > 0)
            AddReward(-1f / maxStep);

        //Detect no floor under agent or pushblock, if so penalize agent and restart
        /*
        if ((!Physics.Raycast(rb.position, Vector3.down, 20))
            || (!Physics.Raycast(pushBlockRb.position, Vector3.down, 20)))
        {
            SetReward(-1f);
            EndEpisode();
        } */
        
    }
    
    //Provides heuristic controls, so you can control the agent manually, to test capabilities of the scene
    public override float[] Heuristic()
    {
        float forwardAction = 0f;
        float turnAction = 0f;
        if (Input.GetKey(KeyCode.W))
        {
            forwardAction = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            forwardAction = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            turnAction = 1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            turnAction = 2f;
        }
        
        return new float[] { forwardAction, turnAction };
    }
    
    //When episode begins this is called 
    public override void OnEpisodeBegin()
    {
        goalReached = false;
        trainingArea.ResetArea();
    }

    //When agent reaches the goal it is rewarded and the episode ends, and a new episode begins automatically
    private void ReachGoal()
    {
        AddReward(1f);
        EndEpisode();
    }

    //When agent collides with another object
    private void OnCollisionEnter(Collision collision)
    {
        //If the colliding object has the tag "goal" and it is active.
        if (collision.transform.CompareTag("goal") && goal.GetComponent<Goal>().active)
        {
            //Reset button state and call ReachGoal
            button.GetComponent<PressurePlate>().Reset();
            ReachGoal();
        }
    }
}
