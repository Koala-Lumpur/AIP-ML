using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using MLAgents;
using TMPro;
using Random = UnityEngine.Random;

public class TrainingArea : MonoBehaviour
{
    public MainAgent mainAgent;
    public MeshRenderer agentCol;

    public GameObject goal;

    public TextMesh rewardText;

    public GameObject button;
    public MeshRenderer buttonCol;

    public GameObject pushBlock;
    public MeshRenderer pushBlockCol;

    private void Start()
    {
        buttonCol = button.GetComponent<MeshRenderer>();
        pushBlockCol = pushBlock.GetComponent<MeshRenderer>();
        //Reset area on start
        ResetArea();
    }

    private void Update()
    {
        //Show individual agent's rewards on 3D text mesh
        rewardText.text = mainAgent.GetCumulativeReward().ToString("0.00");
    }


    //Resets certain attributes and places agent, goal, button and pushblock
    public void ResetArea()
    {
        button.GetComponent<PressurePlate>().Reset();
        PlaceAgent();
        PlaceGoal();
        PlaceButton();
        PlacePushBlock();
    }

    //Function to return a random position inside a square area, where you define the top left and bottom right of the area
    //Also takes in a y position, which is not randomized
    public static Vector3 RandomPosition(Vector3 topLeft, Vector3 bottomRight, float ypos)
    {
        return new Vector3(Random.Range(topLeft.x, bottomRight.x), ypos, Random.Range(topLeft.z, bottomRight.z));
    }

    private void PlaceAgent()
    {
        //Get agent rigidbody, and set velocity attributes to zero, so it doesn't make slight movement, without the request
        //of the agent
        Rigidbody rb = mainAgent.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        //Generate random position
        mainAgent.transform.position = RandomPosition(transform.position + new Vector3(-12, 0.5f, 12), 
            transform.position + new Vector3(12, 0.5f, -12), 0.5f);
        //Generate random rotation
        mainAgent.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
    }

    //Randomize goal position on border of training area
    private void PlaceGoal()
    {
        
        Rigidbody rb = goal.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        //Rolls which border the goal should be placed on (Left, right, top ot bottom)
        int bound = Random.Range(0, 4);
        //Left
        if (bound == 0)
        {
            goal.transform.position = transform.position + new Vector3(-14.75f, 2.5f, Random.Range(-12.5f, 12.5f));
            goal.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
        //Right
        if (bound == 1)
        {
            goal.transform.position = transform.position + new Vector3(14.75f, 2.5f, Random.Range(-12.5f, 12.5f));
            goal.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
        //Top
        if (bound == 2)
        {
            goal.transform.position = transform.position + new Vector3(Random.Range(-12.5f, 12.5f), 2.5f, 14.75f);
            goal.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        //Bottom
        if (bound == 3)
        {
            goal.transform.position = transform.position + new Vector3(Random.Range(-12.5f, 12.5f), 2.5f, -14.75f);
            goal.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    private void PlaceButton()
    {
        //Generate random position for button
        button.transform.position = RandomPosition(transform.position + new Vector3(-9, 0.1f, 9),
            transform.position + new Vector3(9, 0.1f, -9), 0.1f);
    }

    private void PlacePushBlock()
    {
        Rigidbody rb = pushBlock.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        //Generate random position for push block
        pushBlock.transform.position = RandomPosition(transform.position + new Vector3(-10, 0.9f, 10),
            transform.position + new Vector3(10, 0.9f, -10), 0.9f);
        //If the pushblock intersects with the button or agent, we generate a new position, until it doesn't intersect
        //with any of them
        while (pushBlockCol.bounds.Intersects(buttonCol.bounds) || pushBlockCol.bounds.Intersects(agentCol.bounds) )
        {
            //Debug.Log(gameObject.name + " intersection");
            pushBlock.transform.position = RandomPosition(transform.position + new Vector3(-10, 0.9f, 10),
                transform.position + new Vector3(10, 0.9f, -10), 1f);
        }
    }
}
