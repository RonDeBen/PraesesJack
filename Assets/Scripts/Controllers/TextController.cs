using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextController : MonoBehaviour
{
    public TextMeshProUGUI balanceTxt, betTxt, betStandTxt;
    public TextMeshPro outcomeTxt;
    public GameObject outcomeObj;

    public void SetBalanceText(int balance){
        balanceTxt.text = "Balance: $" + balance.ToString();
    }

    public void SetBetText(int bet){
        betTxt.text = "Bet: $" + bet.ToString();
    }

    public void SetBetText(string bet){
        betTxt.text = "Bet: $" + bet;
    }

    public void SetBetStandText(string newText){
        betStandTxt.text = newText;
    }

    public void SetOutcomeText(string newText){
        outcomeTxt.text = newText;
    }

    public void ConcatOutcomeText(string newText){
        if(outcomeTxt.text != ""){
            outcomeTxt.text += " & ";
        }
        outcomeTxt.text = outcomeTxt.text + newText;
    }

    public void SetOutcomeTextConstraints(Vector3 pos, float width){
        outcomeObj.transform.position = pos;
        outcomeObj.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 10f);
    }
}
