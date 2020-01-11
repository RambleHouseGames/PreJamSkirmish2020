using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Inst;

    [SerializeField]
    private List<CivilianType> bountyOptions;

    [SerializeField]
    private float bountyTimeLimit;

    public CivilianType activeBounty;
    public float bountyTimer = 0f;
    public int coins;


    void Awake()
    {
        if (GameDataManager.Inst == null)
            Inst = this;
        int rand = Random.Range(0, bountyOptions.Count);
        activeBounty = bountyOptions[rand];
        bountyTimer = bountyTimeLimit;
    }

    private void Start()
    {
        SignalManager.Inst.AddListenner(Signal.CIVILIAN_TURNED_IN, onCivilianTurnedIn);
    }

    void Update()
    {
        bountyTimer -= Time.deltaTime;
        if (bountyTimer < 0)
        {
            changeBounty();
            bountyTimer = bountyTimeLimit;
        }
    }

    private void changeBounty()
    {
        int rand = Random.Range(0, bountyOptions.Count);
        activeBounty = bountyOptions[rand];
    }

    private void onCivilianTurnedIn(System.Object args)
    {
        Civilian turnedInCivilian = ((CivilianTurnedInArgs)args).TurnedInCivilian;
        if(turnedInCivilian.civilianType == activeBounty)
        {
            rewardBounty();
        }
        else
        {
            chargeFine();
        }
    }

    private void rewardBounty()
    {
        coins += 1000000;
    }

    private void chargeFine()
    {
        coins -= 500000;
    }
}

