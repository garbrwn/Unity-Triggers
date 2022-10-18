using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class condition_otherTriggerActivated : TriggerCondition
{
    public TriggerController otherTrigger;

    public override bool CheckCondition()
    {
        return otherTrigger.TriggerActivated;
    }
}
