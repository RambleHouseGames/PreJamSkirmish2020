using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private Rect titleRect = new Rect(Screen.width * .25f, Screen.height * .1f, Screen.width * .5f, Screen.height * .1f);
    private Rect buttonRect = new Rect(Screen.width * .4f, Screen.height * .8f, Screen.width * .2f, Screen.width * .2f);

    void OnGUI()
    {
        GUI.Label(titleRect, "TITLE");
        if (GUI.Button(buttonRect, "START"))
            SignalManager.Inst.FireSignal(Signal.START_BUTTON_PRESSED, null);
    }
}
