using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class RoundsSurvive : MonoBehaviour {

    public Text roundTxt;

    void OnEnable()
    {
        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        roundTxt.text = "0";
        int round = 0;
        yield return new WaitForSeconds(.7f);
        while (round < PlayerStats.Rounds)
        {
            round++;
            roundTxt.text = round.ToString();

            yield return new WaitForSeconds(.05f);
        }

    }
}
