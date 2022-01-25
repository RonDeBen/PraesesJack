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

    void Start(){
        textCont.SetBalanceText(balance);
        textCont.SetBetText(bet);
    }

    public void OnClearPressed(){
        bet = 0;
        textCont.SetBetText(bet);
        playerCont.SetCanBetButton(false);
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

    public void AddToBet(int num){
        if((num + bet) < balance){
            bet += num;
            textCont.SetBetText(bet);
            playerCont.SetCanBetButton(true);
        }
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

    public bool CanAffordToDoubleDown(){
        return ((bet * 2) < balance);
    }

    public void SetBetButtons(bool truth){
        oneBtn.interactable = truth;
        fiveBtn.interactable = truth;
        twentyFiveBtn.interactable = truth;
        oneHundredBtn.interactable = truth;
    }
}
