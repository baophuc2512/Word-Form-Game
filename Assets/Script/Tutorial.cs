using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject tutorialPanel;

    public void turnOnPanel()
    {
        tutorialPanel.SetActive(true);
    }

    public void turnOffPanel()
    {
        tutorialPanel.SetActive(false);
    }
}
