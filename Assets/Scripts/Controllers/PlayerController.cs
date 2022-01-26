using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private List<HandModel> hands = new List<HandModel>();
    public Vector3 playerStartPos, offset, doubleDownOffset, handOffset;
    public HouseController houseCont; 
    public BettingController betCont;
    public TextController textCont;
    public SplitModel splitter;
    public Button clearBtn, betStandBtn;
    public GameObject doubleDownObj;
    public List<GameObject> hitMeObjs;
    private List<Vector3> handPositions = new List<Vector3>();
    private RaycastHit2D hit;
    private Vector2 touchPosWorld2D;
    private bool placedABet = false;
    private bool isDoublingDown = false;
    private int currentHandIndex = -1;
    private Vector3 offscreen = new Vector3(100f, 100f, 0f);

    void Start(){
        HandModel newHand = new HandModel();
        hands.Add(newHand);
        handPositions.Add(playerStartPos);
    }

    void FixedUpdate(){
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) {
            touchPosWorld2D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);
            if (hit.collider != null) {
                if(hit.collider.gameObject.tag == "DoubleDown"){
                    HitMe(0, true);
                }
                if(hit.collider.gameObject.tag == "HitMe"){
                    HitMeModel hmm = hit.collider.gameObject.GetComponent<HitMeModel>();
                    HitMe(hmm.handIndex, false);
                }
                if(hit.collider.gameObject.tag == "Insurance"){
                    textCont.SetOutcomeText("Insure up to half your bet");
                    SetCanBetButton(false);
                    betCont.SetBetButtons(true);
                    betCont.SetIsInsuring(true);
                }
                if(hit.collider.gameObject.tag == "Split"){
                    Split();
                }
            }
        }
    }

    public void Split(){
        //makes two separate hands
        currentHandIndex = 0;
        int oldIndex = splitter.handIndex;
        List<CardModel> cardPair = hands[oldIndex].GetCopyOfPair();

        hands[oldIndex].Clear();
        ReceiveCard(cardPair[0], oldIndex, false);

        HandModel newHand = new HandModel();
        hands.Add(newHand);
        ReceiveCard(cardPair[1], (handPositions.Count - 1), false);

        splitter.gameObject.transform.position = offscreen;

        //if two aces, hit both hands immediately
        if((cardPair[0].value == -1) && (cardPair[0].value == cardPair[1].value)){
            houseCont.DealToPlayer(0, false);
            houseCont.DealToPlayer(1, false);
            DetermineWinner(false);
        }
    }

    public void DetermineWinner(bool isDoubleDown){
        textCont.SetOutcomeText("");
        foreach(HandModel hand in hands){
            if(hand.LowestValue() > 21){//checks for a bust, if split
                betCont.LoseBet(false, isDoubleDown);
                textCont.ConcatOutcomeText("Player Busted");
            }else if(hand.HighestValue() == houseCont.HouseValue()){
                textCont.ConcatOutcomeText("It's a tie");
            }else if(hand.HighestValue() > houseCont.HouseValue()){
                textCont.ConcatOutcomeText("Player Wins");
                betCont.WinBet(false, isDoubleDown);
            }else{
                betCont.LoseBet(false, isDoubleDown);
                textCont.ConcatOutcomeText("Player Loses");
            }
        }
    }

    public void HitMe(int handIndex, bool isDoubleDown){
        houseCont.DealToPlayer(handIndex, isDoubleDown);
        CheckHand(handIndex, isDoubleDown);
    }

    public void OnBetStandButtonPressed(){
        if(placedABet){//is the stand button
            Stand(false);
        }else{//is the place bet button
            currentHandIndex = -1;
            placedABet = true;
            betCont.SetBetButtons(false);
            clearBtn.interactable = false;
            textCont.SetOutcomeText("");
            textCont.SetBetStandText("STAND");
            ClearHands();
            houseCont.Deal();
        }
    }
    
    public void Stand(bool isDoubleDown){
        if ((currentHandIndex != -1) && (currentHandIndex < 1)) {
            GoToNextHand();
        }else{
            hitMeObjs[0].transform.position = offscreen;
            hitMeObjs[1].transform.position = offscreen;
            houseCont.DealersTurn(isDoubleDown);
            clearBtn.interactable = true;
            placedABet = false;
            textCont.SetBetStandText("PLACE BET");
        }
    }

    public void ReceiveCard(CardModel newCard, int handIndex, bool isDoubleDown){
        hands[handIndex].Add(newCard, handPositions[handIndex], offset, isDoubleDown); 
        if(!isDoubleDown && CanHit(handIndex)){
            hitMeObjs[handIndex].transform.position = handPositions[handIndex] + (hands[handIndex].Count() * offset);
        }else{
            hitMeObjs[handIndex].transform.position = offscreen;
            if(!HasABust(handIndex) && isDoubleDown){
                Stand(isDoubleDown);
            }
        }
    }

    public void ClearHands(){
        foreach(HandModel hand in hands){
            hand.Clear();
        }
        hands.Clear();
        HandModel newHand = new HandModel();
        hands.Add(newHand);
        handPositions = new List<Vector3>{playerStartPos};
    }

    public void CheckHand(int handIndex, bool isDoubleDown){
        if(HasABust(handIndex)){
            Busted(isDoubleDown);
        }
        if(HasABlackJack(handIndex)){
            GotABlackJack(isDoubleDown);
        }
        CheckForDoubleDown(handIndex);
        if(hands[handIndex].HasAPair() && hands.Count < 2){
            splitter.SetSprite(hands[handIndex].GetCardSprite(1));
            splitter.handIndex = handIndex;
            Vector3 newHandStartPos = handPositions[handIndex] + handOffset;
            splitter.gameObject.transform.position = newHandStartPos;
            handPositions.Add(newHandStartPos);
        }else{
            splitter.gameObject.transform.position = offscreen; 
        }
    }

    public bool HasABust(int handIndex){
        return hands[handIndex].LowestValue() > 21;
    }

    public bool HasABlackJack(int handIndex){
        return hands[handIndex].HighestValue() == 21;
    }

    public void CheckForDoubleDown(int handIndex){
        if(CanDoubleDown()){  
            doubleDownObj.transform.position = handPositions[handIndex] + doubleDownOffset;
        }else{
            doubleDownObj.transform.position = offscreen;
        }
    }

    public bool CanDoubleDown(){
        bool doesntHaveBlackjack = !HasABlackJack(0);
        bool isTheTwoStartingCards = ((hands[0].Count() == 2) && (currentHandIndex == -1));
        bool hasCorrectTotal = hands[0].HasValueToDoubleDown();
        bool canAffordToDoubleDown = betCont.CanAffordToDoubleDown();
        return (doesntHaveBlackjack && isTheTwoStartingCards && hasCorrectTotal && canAffordToDoubleDown);
    }

    public void GotABlackJack(bool isDoubleDown){
        if(currentHandIndex == -1){//must use DetermineWinner() on splits
            FinishedBeforeHouse();
            houseCont.CheckForStandOff();
        }
    }

    public void Busted(bool isDoubleDown){
        if(currentHandIndex == -1){//must use DetermineWinner() on splits
            FinishedBeforeHouse();
            betCont.LoseBet(false, isDoubleDown);
            textCont.SetOutcomeText("Player Busted");
        }
    }

    public void FinishedBeforeHouse(){
        if(HasAnotherHandToPlay()){
            GoToNextHand();
        }else{
            clearBtn.interactable = true;
            placedABet = false;
            textCont.SetBetStandText("PLACE BET");
            if (betCont.isInsuring) {
                houseCont.CheckInsuranceBet();
            }
        }
    }

    public void GoToNextHand(){
        currentHandIndex++;
        hitMeObjs[0].transform.position = offscreen;
        hitMeObjs[currentHandIndex].transform.position = handPositions[currentHandIndex] + (hands[currentHandIndex].Count() * offset);
    }

    private bool CanHit(int handIndex){
        bool hasntBust = (hands[handIndex].LowestValue() < 21);
        bool isOnCurrentHand = ((currentHandIndex == -1) || (handIndex == currentHandIndex));
        return (hasntBust && isOnCurrentHand);
    }

    public void SetCanBetButton(bool truth){
        betStandBtn.interactable = truth;
    }

    public bool CanClearBet(){
        return !placedABet;
    }

    public bool HasAnotherHandToPlay(){
        return ((currentHandIndex != -1) && (currentHandIndex < 1));
    }
}
