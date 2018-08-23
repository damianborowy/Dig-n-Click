using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleEarnings
{
    public static void IdleReward(int idleTime)
    {
        GameController.Instance.AddMoney(idleTime);
    }
}
