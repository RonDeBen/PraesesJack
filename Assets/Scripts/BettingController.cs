using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BettingController : MonoBehaviour
{
    public TextController textCont;
    public PlayerController playerCont;
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

    public void LoseBet(bool hasInsurance){
        balance -= (hasInsurance) ? (int)(bet / 2) : bet;
        textCont.SetBalanceText(balance);
        if(balance <= bet){
            bet = balance;
            textCont.SetBetText(bet);
        }
        if(bet == 0){
            playerCont.SetCanBetButton(false);
        }
    }

    public void WinBet(bool gotABlackjack){
        balance += (gotABlackjack) ? (int)(bet * 1.5) : bet;
        textCont.SetBalanceText(balance);
    }
}
