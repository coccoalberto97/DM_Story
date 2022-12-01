using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class Hud : MonoBehaviour
{
    public static Hud instance;

    public Animator animator;

    public GameObject bossHealthBar;
    public GameObject playerHealthBar;
    public Slider playerHealthBarSlider;
    public Slider bossHealthBarSlider;
    public TextMeshProUGUI playerHealth;

    public Slider playerExpBarSlider;
    public TextMeshProUGUI playerExp;

    //Dialogue
    public GameObject dialogueContainer;
    public CharacterImageController characterImageController;
    public TextMeshProUGUI calledName;
    public TextMeshProUGUI sentenceBox;
    public Coroutine currentSentenceCoroutine;
    public Sentence currentSentence;

    private Queue<Sentence> sentences;

    public AudioClip alert;

    private bool currentSentenceEnded = true;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        bossHealthBarSlider = bossHealthBar.GetComponent<Slider>();
        GameEvents.instance.OnPlayerEntersBoosArea += OnPlayerEntersBoosArea;
        GameEvents.instance.OnBossDeath += OnBossDeath;
        GameEvents.instance.OnBossHit += OnBossHit;
        GameEvents.instance.OnPlayerModHealth += OnPlayerModHealth;
        GameEvents.instance.OnPlayerModExp += OnPlayerModExp;
        sentences = new Queue<Sentence>();

        SetHealth();
        SetExp();
    }

    private void Update()
    {
        if (currentSentence == null) {
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            DisplayNextSentence();
        }
    }

    private void OnPlayerEntersBoosArea(string bossTag, int maxHealth)
    {
        bossHealthBar.SetActive(true);
        bossHealthBarSlider.maxValue = maxHealth;
        bossHealthBarSlider.value = maxHealth;
        bossHealthBarSlider.enabled = true;
    }

    private void OnBossDeath(string bossTag)
    {
        bossHealthBar.SetActive(false);
    }

    private void OnBossHit(int maxHealth)
    {
        bossHealthBarSlider.value = maxHealth;
    }

    private void OnPlayerModHealth()
    {
        this.SetHealth();
    }

    private void SetHealth()
    {
        Player player = Player.instance;
        playerHealth.text = player.getHealth().ToString();
        playerHealthBarSlider.maxValue = player.maxHealth;
        playerHealthBarSlider.value = player.getHealth();
    }

    private void OnPlayerModExp()
    {
        SetExp();
    }

    private void SetExp()
    {
        Player player = Player.instance;
        playerExp.text = player.GetExp().ToString();
        playerExpBarSlider.maxValue = player.maxExp;
        playerExpBarSlider.value = player.GetExp();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        /*Debug.Log("name " + dialogue.calledName);
        calledName.text = dialogue.calledName;*/
        Player.instance.SetInputEnabled(false);
        sentences.Clear();
        animator.SetBool("isOpen", true);
        characterImageController.InitChars(dialogue.characters);
        foreach (Sentence sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        //controllo se il dialogo corrente è terminato
        if (!currentSentenceEnded)
        {
            //se non è terminato completo il testo
            string text = currentSentence.text;
            if (currentSentence.charStop > -1)
            {
                text = text.Substring(0, currentSentence.charStop + 1);
                GameEvents.instance.StopAudio();
                GameEvents.instance.PlaySFXAudioClip(this.alert);
            }

            sentenceBox.text = text;
            if (currentSentenceCoroutine != null)
            {
                StopCoroutine(currentSentenceCoroutine);
                currentSentenceCoroutine = null;
            }
            currentSentenceEnded = true;
            return;
        }


        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        Sentence sentence = sentences.Dequeue();

        if (currentSentenceCoroutine != null)
        {
            StopCoroutine(currentSentenceCoroutine);
        }

        currentSentenceCoroutine = StartCoroutine(TypeSentence(sentence));

    }

    public void EndDialogue()
    {
        characterImageController.HideChars();
        animator.SetBool("isOpen", false);
        Player.instance.SetInputEnabled(true);
    }

    private IEnumerator TypeSentence(Sentence sentence)
    {
        currentSentenceEnded = false;
        currentSentence = sentence;
        if (sentence.ost != null)
        {
            GameEvents.instance.PlayOSTAudioClip(sentence.ost);
        }

        sentenceBox.text = "";
        characterImageController.EnableChar(sentence.charName);
        int charIndex = 0;
        foreach (char letter in sentence.text.ToCharArray())
        {
            if (sentence.charStop > -1)
            {
                if (sentence.charStop == charIndex)
                {
                    GameEvents.instance.StopAudio();
                    GameEvents.instance.PlaySFXAudioClip(this.alert);
                    break;
                }
            }
            sentenceBox.text += letter;
            charIndex++;
            yield return new WaitForSeconds(0.03f);
        }
        currentSentenceEnded = true;
        currentSentenceCoroutine = null;
    }
}
