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

    //Dialogue
    public GameObject dialogueContainer;
    public TextMeshProUGUI calledName;
    public TextMeshProUGUI sentenceBox;
    public Coroutine currentSentence;
    public Image dialogueChar;

    private Queue<Sentence> sentences;


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
        sentences = new Queue<Sentence>();

        SetHealth();
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

    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("name " + dialogue.calledName);
        calledName.text = dialogue.calledName;
        sentences.Clear();
        animator.SetBool("isOpen", true);
        foreach (Sentence sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        Sentence sentence = sentences.Dequeue();

        if (currentSentence != null)
        {
            StopCoroutine(currentSentence);
        }

        currentSentence = StartCoroutine(TypeSentence(sentence));

    }

    public void EndDialogue()
    {
        //dialogueContainer.SetActive(false);
        animator.SetBool("isOpen", false);
    }

    private IEnumerator TypeSentence(Sentence sentence)
    {
        sentenceBox.text = "";
        if (sentence.charImage != null)
        {
            dialogueChar.enabled = true;
            dialogueChar.sprite = sentence.charImage;
        }
        else
        {
            dialogueChar.enabled = false;
        }
        foreach (char letter in sentence.text.ToCharArray())
        {
            sentenceBox.text += letter;
            yield return 0;
        }

        currentSentence = null;
    }
}
