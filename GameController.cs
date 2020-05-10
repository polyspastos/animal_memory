using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Versioning;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    [SerializeField]
    private Sprite backgroundImage;
    public List<Button> buttons = new List<Button>();
    public Sprite[] pieces;
    public List<Sprite> gamePieces = new List<Sprite>();

    public bool GuessOne, GuessTwo;
    private int countGuesses;
    private int countCorrectGuesses;
    private int gameGuesses;

    private int GuessOneIndex, GuessTwoIndex;

    private string GuessOnePuzzle, GuessTwoPuzzle;

    GameObject thankyou;
    Sprite thankyousprite;

    void Awake()
    {
        pieces = Resources.LoadAll<Sprite>("Sprites");
    }

    void Start()
    {
        GetButtons();
        AddListeners();
        AddGamePuzzles();
        gameGuesses = gamePieces.Count / 2;
        Shuffle(gamePieces);
    }

    void GetButtons()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PieceButton");

        for (int i = 0; i < objects.Length; i++)
        {
            buttons.Add(objects[i].GetComponent<Button>());
            buttons[i].image.sprite = backgroundImage;
        }
    }

    void AddGamePieces()
    {
        int looper = buttons.Count;
        int index = 0;

        for (int i = 0; i < looper; i++)
        {
            if (index == looper / 2)
            {
                index = 0;
            }

            gamePieces.Add(pieces[index]);
            index++;
        }
    }

    void AddListeners()
    {
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => PickAPuzzle());
        }
    }


    public void PickAPuzzle()
    {
        string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        UnityEngine.Debug.Log("You are clicking a button named:  " + name);

        if (!GuessOne)
        {
            GuessOne = true;
            GuessOneIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            GuessOnePuzzle = gamePieces[GuessOneIndex].name;
            buttons[GuessOneIndex].image.sprite = gamePieces[GuessOneIndex];
        }
        else if (!GuessTwo)
        {
            GuessTwo = true;
            GuessTwoIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            GuessTwoPuzzle = gamePieces[GuessTwoIndex].name;
            buttons[GuessTwoIndex].image.sprite = gamePieces[GuessTwoIndex];
            countGuesses++;
            StartCoroutine(CheckIfThePiecesMatch());

            if (GuessOnePuzzle == GuessTwoPuzzle)
            {
                UnityEngine.Debug.Log("The pieces match");
            }
            else
            {
                UnityEngine.Debug.Log("The pieces dont match");
            }
        }
    }

    IEnumerator CheckIfThePiecesMatch()
    {
        yield return new WaitForSeconds(0.6f);
        if (GuessOnePuzzle == GuessTwoPuzzle)
        {
            yield return new WaitForSeconds(0.4f);
            buttons[GuessOneIndex].interactable = false;
            buttons[GuessTwoIndex].interactable = false;
            buttons[GuessOneIndex].image.color = new Color(0, 0, 0, 0);
            buttons[GuessTwoIndex].image.color = new Color(0, 0, 0, 0);
            CheckIfTheGameIsFinished();
        }
        else 
        {
            yield return new WaitForSeconds(.2f);

            buttons[GuessOneIndex].image.sprite = backgroundImage;
            buttons[GuessTwoIndex].image.sprite = backgroundImage;

        }

        yield return new WaitForSeconds(.2f);
        GuessOne = GuessTwo = false;

    }

    void CheckIfTheGameIsFinished()
    {
        countCorrectGuesses++;

        if(countCorrectGuesses == gameGuesses)
        {
            UnityEngine.Debug.Log("Game finished");
            UnityEngine.Debug.Log("It took you " + countGuesses + " guesses to finish the game.");
            
            
            thankyou = GameObject.FindWithTag("koszi");
            SpriteRenderer renderer = thankyou.GetComponent<SpriteRenderer>();
            renderer.sortingOrder = 2;

            StartCoroutine(Quit());
        }
    }

    IEnumerator Quit()
    {
        yield return new WaitForSeconds(2.4f);
        Application.Quit();

    }
    
    void Shuffle(List<Sprite> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Sprite temp = list[i];
            int randomIndex = UnityEngine.Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
