using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Deck : MonoBehaviour
{
    public Sprite[] faces;
    public GameObject dealer;
    public GameObject player;
    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage;

    public int[] values = new int[52];
    int cardIndex = 0;

    

    private void Awake()
    {    
        InitCardValues();        

    }

    private void Start()
    {
        ShuffleCards();
        StartGame();        
    }

    private void InitCardValues()
    {
        /*TODO:
         * Asignar un valor a cada una de las 52 cartas del atributo "values".
         * En principio, la posición de cada valor se deberá corresponder con la posición de faces. 
         * Por ejemplo, si en faces[1] hay un 2 de corazones, en values[1] debería haber un 2.
         */
        int nuevoValor = 1;
        for(int i = 0; i < values.Length; i++)
        {
            if (i % 13 == 0)
            {
                nuevoValor = 1;
            }
            values[i] = nuevoValor;
            nuevoValor = nuevoValor+1;
        }
    }

    private void ShuffleCards()
    {
        /*TODO:
         * Barajar las cartas aleatoriamente.
         * El método Random.Range(0,n), devuelve un valor entre 0 y n-1
         * Si lo necesitas, puedes definir nuevos arrays.
         */
        cardIndex = UnityEngine.Random.Range(0, 52);
    }

    void StartGame()
    {
        for (int i = 0; i < 2; i++)
        {
            PushPlayer();
            PushDealer();
            /*TODO:
             * Si alguno de los dos obtiene Blackjack, termina el juego y mostramos mensaje
             */
            CardHand playerCardHand = player.GetComponent<CardHand>();
            CardHand dealerCardHand = dealer.GetComponent<CardHand>();

            Debug.Log(playerCardHand.points);
            Debug.Log(dealerCardHand.points);
            if (playerCardHand.points > 21)
            {
                Debug.Log("GAME OVER. THE PLAYER LOST");
                finalMessage.text = "GAME OVER. YOU LOST...";
                hitButton.interactable = false;
                stickButton.interactable = false;
            }
            else if (dealerCardHand.points > 21)
            {
                Debug.Log("GAME OVER. THE DEALER LOST");
                finalMessage.text = "CONGRATULATIONS, YOU WON!";
                hitButton.interactable = false;
                stickButton.interactable = false;
            }
            else if (playerCardHand.points == 21)
            {
                Debug.Log("GAME OVER. THE DEALER LOST");
                finalMessage.text = "CONGRATULATIONS, YOU WON!";
                hitButton.interactable = false;
                stickButton.interactable = false;
            }
            else if(dealerCardHand.points == 21)
            {
                Debug.Log("GAME OVER. THE PLAYER LOST");
                finalMessage.text = "GAME OVER. YOU LOST...";
                hitButton.interactable = false;
                stickButton.interactable = false;
            }
        }
    }

    private void CalculateProbabilities()
    {
        /*TODO:
         * Calcular las probabilidades de:
         * - Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
         * - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta
         * - Probabilidad de que el jugador obtenga más de 21 si pide una carta          
         */

        double probAbove21 = 0.00;
        double between17and21 = 0.00;
        double dealerScoreIsHigher = 0.00;

        int currentHandValue = player.GetComponent<CardHand>().points;
        int playerHandCardNum = player.GetComponent<CardHand>().cards.Count;
        int dealerHandCardNum = dealer.GetComponent<CardHand>().cards.Count;
        int[] avalibleCardValues = new int[52 - (playerHandCardNum + dealerHandCardNum)];
        int[] fullDeckValues = new int[52];
        for(int i=0; i<52; i++)
        {
            fullDeckValues[i] = values[i];
        }
        
        //Cards already in game
        int[] alreadyInGameCards = new int[playerHandCardNum + dealerHandCardNum];
        
        for (int i = 0; i < playerHandCardNum; i++)
        {
            alreadyInGameCards[i] = player.GetComponent<CardHand>().cards[i].GetComponent<CardModel>().value;
        }
        int aux = 0;
        for (int i = playerHandCardNum; i < alreadyInGameCards.Length; i++)
        {
            alreadyInGameCards[i] = dealer.GetComponent<CardHand>().cards[aux].GetComponent<CardModel>().value;
            aux = aux + 1;
        }
        aux = 0;

        //ordenar alreadyInGameCards.values
        int menor;

        for (int i = 0; i < alreadyInGameCards.Length; i++)
        {
            menor = alreadyInGameCards[0];

            if (alreadyInGameCards[i] < menor)
            {
                menor = alreadyInGameCards[i];
            }
            else
            {
                if (alreadyInGameCards[i] > menor)
                {
                    menor = menor;
                }
            }
        }

        for (int i = 0; i >= playerHandCardNum + dealerHandCardNum; i++)
        {
            for (int j = 0; j < 52; j++)
            {
                if (fullDeckValues[j] == alreadyInGameCards[i])
                {
                    fullDeckValues[j] = -1;
                    alreadyInGameCards[i] = -2;
                }
            }
            
        }

        //Re-calculate cards already in game

        for (int i = 0; i < playerHandCardNum; i++)
        {
            alreadyInGameCards[i] = player.GetComponent<CardHand>().cards[i].GetComponent<CardModel>().value;
        }
        aux = 0;
        for (int i = playerHandCardNum; i < alreadyInGameCards.Length; i++)
        {
            alreadyInGameCards[i] = dealer.GetComponent<CardHand>().cards[aux].GetComponent<CardModel>().value;
            aux = aux + 1;
        }
        aux = 0;


        for (int i = 0; i < fullDeckValues.Length; i++)
        {
            menor = fullDeckValues[0];

            if (fullDeckValues[i] < menor)
            {
                menor = fullDeckValues[i];
            }
            else
            {
                if (fullDeckValues[i] > menor)
                {
                    menor = menor;
                }
            }
        }

        int counter0 = 0;
        for (int i = alreadyInGameCards.Length; i < 52; i++)
        {
                avalibleCardValues[counter0] = fullDeckValues[i];
                counter0 = counter0 + 1;
        }

        
        int pointsUntil21 = 21 - currentHandValue;
        int numOfAvalibleCardsForPointsUnder21 = 0;

        for(int i=0; i<avalibleCardValues.Length; i++)
        {
            if (avalibleCardValues[i] <= pointsUntil21)
            {
                numOfAvalibleCardsForPointsUnder21 = numOfAvalibleCardsForPointsUnder21 + 1;
            }
        }

        int numOfViableCards = avalibleCardValues.Length - numOfAvalibleCardsForPointsUnder21;

        double n1 = numOfViableCards;
        double n2 = avalibleCardValues.Length;

        probAbove21 = (n1/n2)*100;

        //cardValuesNeededForBt17a21

        int[] cardValuesNeededForBt17a21 = new int[5];

        int counter1 = 0;
        for(int i=17; i<22; i++)
        {
            cardValuesNeededForBt17a21[counter1] = i - currentHandValue;
            counter1 = counter1 + 1;
        }
        counter1 = cardValuesNeededForBt17a21.Length;

        int[] avalibleCardsViableForBt17a21 = new int[52];
        int numOfavalibleCardsViableForBt17a21 = 0;

        for(int i=0; i < cardValuesNeededForBt17a21.Length; i++)
        {
            for (int j = 0; j < avalibleCardValues.Length; j++)
            {
                if (avalibleCardValues[j] == cardValuesNeededForBt17a21[i] && cardValuesNeededForBt17a21[i]>0)
                {
                    avalibleCardsViableForBt17a21[numOfavalibleCardsViableForBt17a21] = avalibleCardValues[j];
                    numOfavalibleCardsViableForBt17a21 = numOfavalibleCardsViableForBt17a21 + 1;
                }
            }
        }
        
        counter1 = 0;

        double n3 = numOfavalibleCardsViableForBt17a21+1;
        double n4 = avalibleCardValues.Length;

        between17and21 = (n3 / n4) * 100;

        //dealer score is higher

        int valueOfCardShownByDealer =  5; //dealer.GetComponent<CardHand>().cards[1].GetComponent<CardModel>().value;

        int minimumCardValueToExceedPlayer = (currentHandValue + 1) - valueOfCardShownByDealer;

        if (minimumCardValueToExceedPlayer < 0)
        {
            minimumCardValueToExceedPlayer = 0;
        }

        int numOfAvalibleCardsViableForExceedingPlayer = 0;

        for (int i = 0; i < avalibleCardValues.Length; i++)
        {
            if (minimumCardValueToExceedPlayer <= avalibleCardValues[i])
            {
                numOfAvalibleCardsViableForExceedingPlayer = numOfAvalibleCardsViableForExceedingPlayer + 1;
            }

        }


        if (numOfAvalibleCardsViableForExceedingPlayer > 1)
        {
            double n5 = numOfAvalibleCardsViableForExceedingPlayer;
            double n6 = avalibleCardValues.Length;

            dealerScoreIsHigher = (n5 / n6) * 100;
        }

        else
        {
            dealerScoreIsHigher = 0.000000;
        }

        Debug.Log("avalibleCardValues.Length: " + avalibleCardValues.Length);
        Debug.Log("fullDeckValues.Length: " + fullDeckValues.Length);
        Debug.Log("alreadyInGameCards.Length: " + alreadyInGameCards.Length);
        Debug.Log("numOfViableCards: " + numOfViableCards);
        Debug.Log("avalibleCardValues.Length: " + avalibleCardValues.Length);
        Debug.Log("probabilidad: "+probAbove21);

        probMessage.text = "Dealer has higher score: " + Math.Round(dealerScoreIsHigher, 2) + "%" + "\n" + "Score between 17 and 21 if Hit: " + Math.Round(between17and21, 2) + "%" + "\n" + "Score higher than 21 if Hit: " + Math.Round(probAbove21, 2) + "%";

    }

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(faces[cardIndex],values[cardIndex]);
        if (cardIndex < 51)
        {
            cardIndex++;
        }             
    }

    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]/*,cardCopy*/);
        if (cardIndex < 51)
        {
            cardIndex++;
        }
        CalculateProbabilities();
    }       

    public void Hit()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
        CardHand dealerCardHand = dealer.GetComponent<CardHand>();
        dealerCardHand.cards[0].GetComponent<CardModel>().ToggleFace(true);
        //Repartimos carta al jugador
        PushPlayer();

        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */
        CardHand playerCardHand = player.GetComponent<CardHand>();
        if (playerCardHand.points>21)
        {
            Debug.Log("GAME OVER. THE PLAYER LOST");
            finalMessage.text = "GAME OVER. YOU LOST...";
            hitButton.interactable = false;
            stickButton.interactable = false;
        }
        else if(playerCardHand.points < dealerCardHand.points)
        {
            Debug.Log("GAME OVER. THE PLAYER LOST");
            finalMessage.text = "GAME OVER. YOU LOST...";
            hitButton.interactable = false;
            stickButton.interactable = false;
        }
    }

    public void Stand()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
        CardHand dealerCardHand = dealer.GetComponent<CardHand>();
        dealerCardHand.cards[0].GetComponent<CardModel>().ToggleFace(true);
        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */
        CardHand playerCardHand = player.GetComponent<CardHand>();
        if (dealerCardHand.points < 16)
        {
            PushDealer();
            if (dealerCardHand.points > 21)
            {
                Debug.Log("GAME OVER. THE DEALER LOST");
                finalMessage.text = "CONGRATULATIONS, YOU WON!";
                hitButton.interactable = false;
                stickButton.interactable = false;
            }
            else if (playerCardHand.points < dealerCardHand.points)
            {
                Debug.Log("GAME OVER. THE PLAYER LOST");
                finalMessage.text = "GAME OVER. YOU LOST...";
                hitButton.interactable = false;
                stickButton.interactable = false;
            }
            else if (playerCardHand.points > dealerCardHand.points)
            {
                Debug.Log("GAME OVER. THE DEALER LOST");
                finalMessage.text = "CONGRATULATIONS, YOU WON!";
                hitButton.interactable = false;
                stickButton.interactable = false;
            }
        }
        else if (dealerCardHand.points > 16)
        {
            if(playerCardHand.points < dealerCardHand.points)
            {
                Debug.Log("GAME OVER. THE PLAYER LOST");
                finalMessage.text = "GAME OVER. YOU LOST...";
                hitButton.interactable = false;
                stickButton.interactable = false;
            }
            else if (playerCardHand.points > dealerCardHand.points)
            {
                Debug.Log("GAME OVER. THE DEALER LOST");
                finalMessage.text = "CONGRATULATIONS, YOU WON!";
                hitButton.interactable = false;
                stickButton.interactable = false;
            }
        }
    }

    public void PlayAgain()
    {
        hitButton.interactable = true;
        stickButton.interactable = true;
        finalMessage.text = " ";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();          
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }
    
}
