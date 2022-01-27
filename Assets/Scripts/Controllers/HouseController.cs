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
    private HandModel houseHand = new HandModel();
    private Vector3 offscreen = new Vector3(100f, 100f, 0f);

    public void Deal(){
        deckCont.ShuffleIfFlagWasDrawn();
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

    public void DealersTurn(bool isDoubleDown){
        houseHand.FlipHole();
        houseHand.CalculateValues();
        while(houseHand.HighestValue() < 17){
            DealToHouse(false);
            houseHand.CalculateValues();
        }
        playerCont.DetermineWinner(isDoubleDown);
        betCont.PayOutInsurance((HouseValue() == 21));
        insuranceObj.transform.position = offscreen;
    }

    public void CheckInsuranceBet(){
        houseHand.FlipHole();
        houseHand.CalculateValues();
        betCont.PayOutInsurance(HouseValue() == 21);
        insuranceObj.transform.position = offscreen;
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

    public void SetStartPosition(Vector3 pos) {
        houseStartPos = pos;
    }
}
