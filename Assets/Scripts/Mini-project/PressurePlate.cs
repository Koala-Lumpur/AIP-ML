using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public Material active;
    public Material inactive;
    public MeshRenderer rend;
    public GameObject goal;
    private MeshRenderer goalRend;
    public MainAgent mainAgent;
    public bool blockInTrigger = false;
    private int numberOfTriggeringObjects = 0;
    public bool goalReached;
    
    private void Start()
    {
        goalRend = goal.GetComponent<MeshRenderer>();
    }

    //When another collider enters the local trigger
    private void OnTriggerEnter(Collider other)
    {
        //Increase number of objects in trigger
        numberOfTriggeringObjects++;
        //If the colliding object has tag "block" and block was not in trigger previously
        if (other.tag == "block" && !blockInTrigger)
        {
            //Grant reward to agent and update boolean to true
            mainAgent.AddReward(1f);
            blockInTrigger = true;
        }
        //We then change materials on goal and button to green and activate the goal
        rend.material = active;
        goalRend.material = active;
        goal.GetComponent<Goal>().active = true;
    }

    //When collider exits trigger
    private void OnTriggerExit(Collider other)
    {
        numberOfTriggeringObjects--;
        //If the block is moved out of trigger
        if (other.tag == "block" && blockInTrigger)
        {
            //Remove the reward, to encourage the agent to keep the block on the button
            mainAgent.AddReward(-1f);
            blockInTrigger = false;
        }
            
        //If not objects are on trigger
        if (numberOfTriggeringObjects <= 0)
        {
            //Change materials to red
            rend.material = inactive;
            goalRend.material = inactive;
            goal.GetComponent<Goal>().active = false;
        }
    }

    //Simple reset function
    public void Reset()
    {
        blockInTrigger = false;
    }
}
