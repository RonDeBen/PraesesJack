using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // private List<CardModel> hand = new List<CardModel>();
    private List<HandModel> hands = new List<HandModel>();
    public Vector3 playerStartPos, offset;
    public HouseController hc; 

    void Start(){
        HandModel newHand = new HandModel();
        hands.Add(newHand);
    }

    // Update is called once per frame
    void Update(){
        if (Input.GetKeyDown("space")) {
            ClearHands();
            hc.Deal();
        }
        if(Input.GetKeyDown("z")){
            HitMe(0);
        }
        if(Input.GetKeyDown("x")){
            Stand();
        }
    }

    public void DetermineWinner(){
        foreach(HandModel hand in hands){
            if(hand.HighestValue() == hc.HouseValue()){
                Debug.Log("It's a tie");
            }else if(hand.HighestValue() > hc.HouseValue()){
                Debug.Log("Player Wins");
            }else{
                Debug.Log("Player Loses");
            }
        }
    }

    public void HitMe(int handIndex){
        hc.DealToPlayer(handIndex);
    }
    
    public void Stand(){
        hc.DealersTurn();
    }

    public void ReceiveCard(CardModel newCard, int handIndex){
        hands[handIndex].Add(newCard, playerStartPos, offset); 
        CheckHand(handIndex);
    }

    public void ClearHands(){
        foreach(HandModel hand in hands){
            hand.Clear();
        }
        hands.Clear();
        HandModel newHand = new HandModel();
        hands.Add(newHand);
    }

    public void CheckHand(int handIndex){
        CheckForBust(handIndex);
        CheckForBlackJack(handIndex);
    }

    public void CheckForBust(int handIndex){
        if(hands[handIndex].LowestValue() > 21){
            Busted();
        }
    }

    public void CheckForBlackJack(int handIndex){
        if(hands[handIndex].HighestValue() == 21){
            GotABlackJack();
        }
    }

    public void GotABlackJack(){
        Debug.Log("You Got a BlackJack!!");
    }

    public void Busted(){
        Debug.Log("You Busted");
    }
}
