using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class condition_HasKey : TriggerCondition
{
    public GameObject player;

    public override bool CheckCondition()
    {
        //if(player.hasKey....
        return true;
    }
}
