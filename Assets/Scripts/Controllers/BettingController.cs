using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BettingController : MonoBehaviour
{
    public TextController textCont;
    public PlayerController playerCont;
    public Button oneBtn, fiveBtn, twentyFiveBtn, oneHundredBtn;
    public int balance = 1000;
    public int bet = 5;
    public int insuranceBet;
    [HideInInspector]
    public bool isInsuring = false;

    void Start(){
        textCont.SetBalanceText(balance);
        textCont.SetBetText(bet);
    }

    public void OnOneButtonPressed(){
        AddToBet(1);
    }

    public void OnFiveButtonPressed(){
        AddToBet(5);
    }

    public void OnTwentyFiveButtonPressed(){
        AddToBet(25);
    }

    public void OnOneHundredButtonPressed(){
        AddToBet(100);
    }

    public void SetIsInsuring(bool truth){
        isInsuring = truth;
    }

    public void AddToBet(int num){
        if(!isInsuring){
            if(CanAffordBet(num + bet)){
                bet += num;
                textCont.SetBetText(bet);
                playerCont.SetCanBetButton(true);
            }
        }else{//is betting for insurance
            if(CanAffordBet(num + bet + insuranceBet) && ((num + insuranceBet) <= (int)(bet / 2))){//up to half
                insuranceBet += num;
                string s = bet + " & " + insuranceBet;
                textCont.SetBetText(s);
                playerCont.SetCanBetButton(true);
            }
        }
    }

    public bool CanAffordBet(int num){
        return (num < balance);
    }

    public bool CanAffordToDoubleDown() {
        return CanAffordBet(bet * 2);
    }

    public void LoseBet(bool hasInsurance, bool isDoubleDown){
        int tempBet = (isDoubleDown) ? bet * 2 : bet;
        balance -= (hasInsurance) ? (int)(tempBet / 2) : tempBet;
        textCont.SetBalanceText(balance);
        if(balance <= tempBet){
            tempBet = balance;
            textCont.SetBetText(tempBet);
        }
        if(tempBet == 0){
            playerCont.SetCanBetButton(false);
        }
        SetBetButtons(true);
    }

    public void WinBet(bool gotABlackjack, bool isDoubleDown){
        int tempBet = (isDoubleDown) ? bet * 2 : bet;
        balance += (gotABlackjack) ? (int)(tempBet * 1.5) : tempBet;
        textCont.SetBalanceText(balance);
        SetBetButtons(true);
    }

    public void SetBetButtons(bool truth){
        oneBtn.interactable = truth;
        fiveBtn.interactable = truth;
        twentyFiveBtn.interactable = truth;
        oneHundredBtn.interactable = truth;
    }

    public void OnClearPressed() {
        bet = 0;
        textCont.SetBetText(bet);
        playerCont.SetCanBetButton(false);
    }

    public void PayOutInsurance(bool houseHitABlackjack){
        if(isInsuring){
            if(houseHitABlackjack){
                balance += insuranceBet * 2;
                textCont.ConcatOutcomeText(" & Won Insurance Bet");
            }else{
                balance -= insuranceBet;
                textCont.ConcatOutcomeText(" & Lost Insurance Bet");
            }
        }
        isInsuring = false;
        insuranceBet = 0;
        textCont.SetBetText(bet);
    }
}
