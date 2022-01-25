using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextController : MonoBehaviour
{
    public TextMeshProUGUI balanceTxt, betTxt, betStandTxt;
    public TextMeshPro outcomeTxt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBalanceText(int balance){
        balanceTxt.text = "Balance: $" + balance.ToString();
    }

    public void SetBetText(int bet){
        betTxt.text = "Bet: $" + bet.ToString();
    }

    public void SetBetStandText(string newText){
        betStandTxt.text = newText;
    }

    public void SetOutcomeText(string newText){
        outcomeTxt.text = newText;
    }
}
