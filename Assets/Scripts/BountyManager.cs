using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BountyManager : MonoBehaviour
{
    private int bountiesCollected = 0;

    private float timeLimit = 240f;
    private float timeRemaining = 240f;

    private Dictionary<PirateType, int> thisBounty;

    void Start()
    {
        SignalManager.Inst.AddListenner(Signal.PIRATE_TURNED_IN, onPirateTurnedIn);
        startBounty();
    }

    private void OnGUI()
    {
        timeRemaining -= Time.deltaTime;
        if (timeRemaining < 0)
            SignalManager.Inst.FireSignal(Signal.GAME_OVER, new GameOverArgs(bountiesCollected));

        float labelWidth = Screen.width / 6f;
        Rect bountyRect = new Rect(0, 0, Screen.width / 6f, Screen.height * .2f);
        Rect redRect = new Rect(labelWidth , 0, Screen.width / 6f, Screen.height * .2f);
        Rect blueRect = new Rect(labelWidth * 2, 0, Screen.width / 6f, Screen.height * .2f);
        Rect greenRect = new Rect(labelWidth * 3, 0, Screen.width / 6f, Screen.height * .2f);
        Rect yellowRect = new Rect(labelWidth * 4, 0, Screen.width / 6f, Screen.height * .2f);
        Rect timeRect = new Rect(labelWidth * 5, 0, Screen.width / 6f, Screen.height * .2f);

        GUI.Label(bountyRect, "Collected Bounties: " + bountiesCollected);
        GUI.Label(redRect, "Red Pirates Remaining: " + thisBounty[PirateType.Red]);
        GUI.Label(greenRect, "Green Pirates Remaining: " + thisBounty[PirateType.Green]);
        GUI.Label(blueRect, "Blue Pirates Remaining: " + thisBounty[PirateType.Blue]);
        GUI.Label(yellowRect, "Yellow Pirates Remaining: " + thisBounty[PirateType.Yellow]);
        GUI.Label(timeRect, "Time Remaining: " + timeRemaining);

    }

    private void onPirateTurnedIn(System.Object args)
    {
        PirateTurnedInArgs pirateTurnedInArgs = (PirateTurnedInArgs)args;
        PirateType turnInType = pirateTurnedInArgs.TurnedInPirate.myType;
        if(thisBounty[turnInType] > 0)
        {
            thisBounty[turnInType] -= 1;
        }
        if (bountyIsComplete())
            collectBounty();
    }

    private void startBounty()
    {
        timeRemaining = timeLimit;
        thisBounty = new Dictionary<PirateType, int>();
        thisBounty.Add(PirateType.Red, bountiesCollected + 1);
        thisBounty.Add(PirateType.Green, bountiesCollected + 1);
        thisBounty.Add(PirateType.Blue, bountiesCollected + 1);
        thisBounty.Add(PirateType.Yellow, bountiesCollected + 1);
    }

    private bool bountyIsComplete()
    {
        return thisBounty[PirateType.Red] <= 0 && thisBounty[PirateType.Green] <= 0 && thisBounty[PirateType.Blue] <= 0 && thisBounty[PirateType.Yellow] <= 0;
    }

    private void collectBounty()
    {
        bountiesCollected++;
        startBounty();
    }
}
