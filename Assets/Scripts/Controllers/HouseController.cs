using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : MonoBehaviour
{
    public DeckController deckCont;
    public PlayerController playerCont;
    public TextController textCont;
    public BettingController betCont;
    public GameObject insuranceObj;
    public Vector3 houseStartPos, offset;
    // private List<CardModel> houseHand = new List<CardModel>();
    private HandModel houseHand = new HandModel();

    public void Deal(){
        houseHand.Clear();
        DealToPlayer(0, false);
        DealToHouse(false);
        DealToPlayer(0, false);
        DealToHouse(true);
        playerCont.CheckHand(0, false);

        if(houseHand.GetCard(0).value == -1){//face up card is an ace
            insuranceObj.transform.position = houseStartPos + (4f * offset);
        }else{
            insuranceObj.transform.position = new Vector3(100f, 100f, 0f);
        }
    }

    public void DealToHouse(bool shouldConceal){
        CardModel newCard = deckCont.GetNextCard(shouldConceal);
        houseHand.Add(newCard, houseStartPos, offset, false);
    }

    public void DealToPlayer(int handIndex, bool isDoubleDown){
        playerCont.ReceiveCard(deckCont.GetNextCard(false), handIndex, isDoubleDown);
    }

    public void DealersTurn(bool isDoubleDown, bool isSplit){
        houseHand.FlipHole();
        houseHand.CalculateValues();
        while(houseHand.HighestValue() < 17){
            DealToHouse(false);
            houseHand.CalculateValues();
        }
        playerCont.DetermineWinner(isDoubleDown);
        betCont.PayOutInsurance(HouseValue() == 21);
        insuranceObj.transform.position = new Vector3(100f, 100f, 0f);
    }

    public void CheckInsuranceBet(){
        houseHand.FlipHole();
        houseHand.CalculateValues();
        betCont.PayOutInsurance(HouseValue() == 21);
        insuranceObj.transform.position = new Vector3(100f, 100f, 0f);
    }

    public int HouseValue(){
        return houseHand.HighestValue();
    }

    public void CheckForStandOff(){
        houseHand.FlipHole();
        if(HouseValue() == 21){
            textCont.SetOutcomeText("It's a Stand-Off");
        }else{
            textCont.SetOutcomeText("You Got a Blackjack!");
            betCont.WinBet(true, false);
        }
    }
}
