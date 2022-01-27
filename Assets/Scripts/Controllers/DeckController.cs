using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DeckController : MonoBehaviour
{
    public Sprite[] heartSprites, clubSprites, diamondSprites, spadeSprites;
    public GameObject cardPrefab;
    public int shoeSize = 3;
    public float muPercent, sigmaPercent;
    private Stack<int> deckInts;
    // Start is called before the first frame update
    void Start(){
        ShuffleDeck();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void ChangeCard(CardModel card, int cardNum){
        int cardIndex = cardNum % 13; 
        if (cardNum < 13){//0 -> 12
            card.SetCard(heartSprites[cardIndex], cardIndex);
        }else if(cardNum < 26){//13 -> 25
            card.SetCard(clubSprites[cardIndex], cardIndex);
        }else if(cardNum < 39){//26 -> 38
            card.SetCard(diamondSprites[cardIndex], cardIndex);
        }else{//39 -> 51
            card.SetCard(spadeSprites[cardIndex], cardIndex);
        }
    }

    private void ShuffleDeck(){
        List<int> tempList = new List<int>();
        for(int i = 0; i < shoeSize; i++){
            for(int k = 0; k < 52; k++){
                tempList.Add(k);
            }
        }
        Mathy.Shuffle(tempList);

        //inserts a "card" in the deck that triggers a shuffle
        //ex muPercent = .75, sigmaPercent = .06 
        //most shuffles will happen 3/4 of the way into the deck
        //99.7% of shuffles will be between 0.57 and 0.93 of the way into the deck
        float percentPenetration = Mathf.Clamp(muPercent + (sigmaPercent * Mathy.NextGaussianFloat()), 0.5f, 0.95f);
        int shuffleIndex = (int)((shoeSize * 52) * percentPenetration);
        tempList.Insert(shuffleIndex, -1);//flag at that position

        deckInts = new Stack<int>(tempList);
    }

    public CardModel GetNextCard(bool shouldConceal){
        GameObject go = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity);
        CardModel newCard = go.GetComponent<CardModel>(); 
        int cardNum = deckInts.Pop();
        if(cardNum == -1){
            ShuffleDeck();
            cardNum = deckInts.Pop();
        }
        ChangeCard(newCard, cardNum);
        newCard.SetShouldBeConcealed(shouldConceal);
        return newCard;
    }
}
