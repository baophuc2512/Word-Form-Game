using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TakeQuestion : MonoBehaviour
{
    public DataQuestion data;
    public TextMeshProUGUI textQuestion;
    public TextMeshProUGUI timeText;
    public GameObject buttonAnswer;
    public GameObject textAnswer;
    public TextMeshProUGUI correctText;
    public TextMeshProUGUI answeredText;
    public TMP_InputField inputText;
    public int currentQuestion = 0;
    private string myAnswer = "none";
    private bool isReady = true;
    private int answeredQuestion;
    public int currentPhase;
    public float timeForGame;
    public List<bool> checkQuestion;

    public int modeCombat = 1; // mode 1: attack, mode 2: heal
    public Image buttonDealDamage;
    public Image buttonHeal;

    public int healthPlayer = 5;
    public int healthEnemy = 5;
    public TextMeshProUGUI healthPlayerText;
    public TextMeshProUGUI healthEnemyText;

    public GameObject panelEndGame;
    public bool isEndGame;
    public TextMeshProUGUI endText;
    public TextMeshProUGUI youHaveAnswered;

    private void Start()
    {
        currentPhase = 1;
        timeForGame = data.eachQuestion[currentQuestion].time;
        for (int tmp = 0; tmp < data.eachQuestion.Count; tmp++)
        {
            checkQuestion.Insert(tmp, false);
        }
        StartCoroutine(takeTime());
    }

    private void Update()
    {
        textQuestion.text = "Question: " + data.eachQuestion[currentQuestion].textQuestion + " " + data.eachQuestion[currentQuestion].wordToTransform;
        answeredText.text = "Answered: " + answeredQuestion + " / " + checkQuestion.Count;
        healthPlayerText.text = "" + healthPlayer;
        healthEnemyText.text = healthEnemy + "";

        if (myAnswer.ToUpper() == data.eachQuestion[currentQuestion].typeQuestion.ToUpper() && myAnswer != "none" && currentPhase == 1)
        {
            StartCoroutine(showResult("YOUR ANSWER IS CORRECT !!", new Color(51, 255, 0)));
            currentPhase = 2;
        }
        if (myAnswer.ToUpper() != data.eachQuestion[currentQuestion].typeQuestion.ToUpper() && myAnswer != "none" && currentPhase == 1)
        {
            StartCoroutine(showResult("YOUR ANSWER IS WRONG !!", new Color(255, 0, 0)));
            currentPhase = 1;
            inputText.text = "";
            //
            currentQuestion = getQuestion();
            timeForGame = data.eachQuestion[currentQuestion].time;
            //
            activeSkill(false, Random.Range(1,3));
        }

        if (myAnswer.ToUpper() == data.eachQuestion[currentQuestion].answer.ToUpper() && myAnswer != "none" && currentPhase == 2)
        {
            StartCoroutine(showResult("YOUR ANSWER IS CORRECT !!", new Color(0, 204, 0)));
            currentPhase = 1;
            inputText.text = "";
            //
            answeredQuestion += 1;
            checkQuestion[currentQuestion] = true;
            //
            currentQuestion = getQuestion();
            timeForGame = data.eachQuestion[currentQuestion].time;
            //
            activeSkill(true, modeCombat);
        }
        if (myAnswer.ToUpper() != data.eachQuestion[currentQuestion].answer.ToUpper() && myAnswer != "none" && currentPhase == 2)
        {
            StartCoroutine(showResult("YOUR ANSWER IS WRONG !!", new Color(255, 0, 0)));
            currentPhase = 1;
            inputText.text = "";
            //
            currentQuestion = getQuestion();
            timeForGame = data.eachQuestion[currentQuestion].time;
            //
            activeSkill(false, Random.Range(1,3));
        }
        
        //

        if (timeForGame < 0f)
        {
            StartCoroutine(showResult("YOU ARE TOO SLOW !!!!", new Color(255, 0, 0)));
            currentPhase = 1;
            inputText.text = "";
            //
            currentQuestion = getQuestion();
            timeForGame = data.eachQuestion[currentQuestion].time;
            //
            activeSkill(false, Random.Range(1,3));
        }

        //

        if (modeCombat == 1)
        {
            buttonDealDamage.color = new Color(255, 0, 0, 1f);
            buttonHeal.color = new Color(0, 255, 0, 0.5f);
        }
        if (modeCombat == 2)
        {
            buttonDealDamage.color = new Color(255, 0, 0, 0.5f);
            buttonHeal.color = new Color(0, 255, 0, 1f);
        }

        // 
        if (answeredQuestion == data.eachQuestion.Count || healthEnemy == 0) 
        {
            showPanelEnd("You Won !!! CONGRATULATION !!!");
        }
        if (healthPlayer == 0)
        {
            showPanelEnd("You Lost !!! TRY HARDER !!!");
        }
    }

    private void FixedUpdate()
    {
        float speed = 500 * Time.fixedDeltaTime;
        if (currentPhase == 1)
        {
            if (buttonAnswer.transform.position.x < 0 + 1024/2)
            buttonAnswer.transform.position = buttonAnswer.transform.position + new Vector3(speed, 0, 0);
            if (textAnswer.transform.position.x < 1000 + 1024/2)
            textAnswer.transform.position = textAnswer.transform.position + new Vector3(speed, 0, 0);
        }
        if (currentPhase == 2)
        {
            if (buttonAnswer.transform.position.x > -1000 + 1024/2)
            buttonAnswer.transform.position = buttonAnswer.transform.position - new Vector3(speed, 0, 0);
            if (textAnswer.transform.position.x > 0 + 1024/2)
            textAnswer.transform.position = textAnswer.transform.position - new Vector3(speed, 0, 0);
        }

        if (isEndGame == true)
        {
            if (panelEndGame.transform.position.y > 0 + 768/2)
            panelEndGame.transform.position -= new Vector3(0, speed, 0);
        }
    }

    public IEnumerator takeTime()
    {
        while (true)
        {
            if (isEndGame == true) break;
            yield return new WaitForSeconds(1f);
            timeForGame -= 1f;
            timeText.text = "Time left: " + Mathf.Round(timeForGame);
        }
    }

    public IEnumerator showResult(string text, Color color)
    {
        myAnswer = "none";
        isReady = false;
        correctText.text = text;
        for (int tmp = 1; tmp <= 10; tmp++)
        {
            correctText.color = new Color(255, 255, 255);
            yield return new WaitForSeconds(0.1f);
            correctText.color = color;
            yield return new WaitForSeconds(0.1f);
        }
        correctText.text = "";
        isReady = true;
    }

    public void answer (string typeAnswer)
    {
        if (currentPhase == 1 && isReady == true)
        {
            myAnswer = typeAnswer;
        }
    }

    public void answerText ()
    {
        if (currentPhase == 2 && isReady == true)
        {
            myAnswer = inputText.text;
        }
    }

    private int getQuestion ()
    {
        int random = Random.Range(0, checkQuestion.Count);
        while (checkQuestion[random] == true)
        {
            if (isEndGame == true) return 0;
            random = Random.Range(0, checkQuestion.Count);
        }
        return random;
    }

    public void changeMode(int typeMode)
    {
        modeCombat = typeMode;
    }

    private void activeSkill(bool isPlayer, int typeSkill)
    {
        if (typeSkill == 1 && isPlayer == true) healthEnemy -= 1;
        if (typeSkill == 1 && isPlayer == false) healthPlayer -= 1;
        if (typeSkill == 2 && isPlayer == true) healthPlayer += 1;
        if (typeSkill == 2 && isPlayer == false) healthEnemy += 1;
    }

    public void showPanelEnd (string text)
    {
        isEndGame = true;
        endText.text = text;
        youHaveAnswered.text = "You have answered: " + answeredQuestion;
    }
}
