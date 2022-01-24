using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DeckController : MonoBehaviour
{
    public Sprite[] heartSprites, clubSprites, diamondSprites, spadeSprites;
    public GameObject cardPrefab;
    public int shoeSize = 3;
    private int index = 10;
    private Stack<int> deckInts;
    public float muPercent, sigmaPercent;
    
    // Start is called before the first frame update
    void Start(){
        ShuffleDeck();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void ChangeCard(CardModel card, int cardNum){
        index  = (index + 1) % 52;
        int cardIndex = index % 13; 
        if (index < 13){//0 -> 12
            card.SetCard(heartSprites[cardIndex], NumToVal(cardIndex));
        }else if(index < 26){//13 -> 25
            card.SetCard(clubSprites[cardIndex], NumToVal(cardIndex));
        }else if(index < 39){//26 -> 38
            card.SetCard(diamondSprites[cardIndex], NumToVal(cardIndex));
        }else{//39 -> 51
            card.SetCard(spadeSprites[cardIndex], NumToVal(cardIndex));
        }
    }

    private void ShuffleDeck(){
        List<int> tempList = new List<int>();
        for(int i = 0; i < shoeSize; i++){
            for(int k = 0; k < 52; k++){
                tempList.Add(k);
            }
        }
        //inserts a "card" in the deck that triggers a shuffle
        //ex muPercent = .75, sigmaPercent = .06 
        //most shuffles will happen 3/4 of the way into the deck
        //99.7% of shuffles will be between 0.57 and 0.93 of the way into the deck
        float percentPenetration = Mathf.Clamp(muPercent + (sigmaPercent * Mathy.NextGaussianFloat()), 0.5f, 0.95f);
        int shuffleIndex = (int)((shoeSize * 52) * percentPenetration);
        tempList.Insert(shuffleIndex, -1);//flag at that position

        var rand = new System.Random();
        deckInts = new Stack<int>(tempList.OrderBy(item => rand.Next()));
    }

    private int NumToVal(int num){
        if(num < 9){//0 -> 8 are 2 -> 10
            return num + 2;
        }else if(num < 12){//a face card
            return 10;
        }
        return -1;//ace is a flag
    }

    public CardModel GetNextCard(bool shouldConceal){
        GameObject go = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity);
        CardModel newCard = go.GetComponent<CardModel>(); 
        int cardNum = deckInts.Pop();
        if(cardNum == -1){
            Debug.Log("shuffle");
            ShuffleDeck();
            cardNum = deckInts.Pop();
        }
        ChangeCard(newCard, cardNum);
        newCard.SetShouldBeConcealed(shouldConceal);
        return newCard;
    }
}
