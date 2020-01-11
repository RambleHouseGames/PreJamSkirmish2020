using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    private Rect activeBountyTextRect;
    private Rect coinTextRect;
    private Rect bountyTimerTextRect;

    void Awake()
    {
        float height = Screen.height * .05f;
        float width = Screen.width * .5f;

        activeBountyTextRect = new Rect(0f, 0f, width, height);
        coinTextRect = new Rect(width, 0f, width, height);
        bountyTimerTextRect = new Rect(0f, height, width, height);
    }

    void OnGUI()
    {
        GUI.Label(activeBountyTextRect, "Active Bounty: " + GameDataManager.Inst.activeBounty.ToString());
        GUI.Label(coinTextRect, "Coins: " + GameDataManager.Inst.coins);
        GUI.Label(bountyTimerTextRect, "Remaining Time: " + GameDataManager.Inst.bountyTimer);
    }
}
