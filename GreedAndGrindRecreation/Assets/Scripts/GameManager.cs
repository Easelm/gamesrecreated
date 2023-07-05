using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum CharacterChoice
    {
        NONE,
        KNIGHT,
        ARCHER,
        MAGE
    }

    public class Enemy
    {
        public int Health;
        public int MaxHealth;
        public int Damage;
        public int Defense;

        public Enemy(int hp, int maxHealth, int dmg, int defense)
        {
            Health = hp;
            MaxHealth = maxHealth;
            Damage = dmg;
            Defense = defense;
        }
    }

    public CharacterChoice characterChoice;

    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.Find("GameManager").GetComponent<GameManager>();
            return instance;
        }
    }

    public SaveLoad saveLoad;

    public TextMeshProUGUI LevelTxt;
    public TextMeshProUGUI GoldTxt;
    public TextMeshProUGUI EssenceTxt;
    public TextMeshProUGUI StepTxt;
    public TextMeshProUGUI StageTxt;

    private RectTransform PlayerHealthRect;
    private RectTransform EnemyHealthRect;

    public Image PlayerHealthImg;
    public Image EnemyHealthImg;

    public Enemy enemy;
    private PlayerData playerData;

    private float attackTimer;
    private readonly float attackTimerInterval = 1.6f;

    private bool isPlayLoaded;
    

    void Awake()
    {
        DontDestroyOnLoad(this);
        isPlayLoaded = false;
        attackTimer = 0;
        characterChoice = CharacterChoice.NONE;
    }

    void Start()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == "Play")
        {
            isPlayLoaded = true;
            GameObject fightPanel = GameObject.FindGameObjectWithTag("FightPanel");
            Transform fightPanelTransform = fightPanel.transform;
            PlayerHealthImg = fightPanelTransform.GetChild(0).GetChild(0).GetComponent<Image>();
            PlayerHealthRect = PlayerHealthImg.GetComponent<RectTransform>();
            EnemyHealthImg = fightPanelTransform.GetChild(1).GetChild(0).GetComponent<Image>();
            EnemyHealthRect = EnemyHealthImg.GetComponent<RectTransform>();
            LevelTxt = fightPanelTransform.GetChild(2).GetComponent<TextMeshProUGUI>();
            GoldTxt = fightPanelTransform.GetChild(3).GetComponent<TextMeshProUGUI>();
            EssenceTxt = fightPanelTransform.GetChild(4).GetComponent<TextMeshProUGUI>();
            StepTxt = fightPanelTransform.GetChild(5).GetComponent<TextMeshProUGUI>();
            StageTxt = fightPanelTransform.GetChild(6).GetComponent<TextMeshProUGUI>();
            enemy = new Enemy(100, 100, 3, 2);
            EnemyHealthRect.sizeDelta = new Vector2(GetPercentValue(0, enemy.MaxHealth, enemy.Health, enemy.MaxHealth, 2).x, 32);
            SpawnEnemy();
            SaveLoad saveData = saveLoad.Load();
            if (saveData != null)
            {
                saveLoad = saveData;
                playerData = saveLoad.playerData;
            }
            else
            {
                playerData = new();
                playerData.MaxHealth = 100;
                playerData.Health = playerData.MaxHealth;
                playerData.Damage = 15;
                playerData.Resistance = 2;
                playerData.Defense = 6;
                playerData.Gold = 0;
                playerData.Essence = 0;
                playerData.Step = 1;
                playerData.Stage = 1;
                playerData.Exp = 0;

                PlayerHealthRect.sizeDelta = new Vector2(GetPercentValue(0, playerData.MaxHealth, playerData.Health, playerData.MaxHealth, 2).x, 32);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float elapsed = Time.deltaTime;
        if (!isPlayLoaded)
            return;

        if (attackTimer >= attackTimerInterval)
        {
            enemy.Health -= playerData.Damage;
            EnemyHealthRect.sizeDelta = new Vector2(GetPercentValue(0, enemy.Health, enemy.Health, enemy.MaxHealth, 2).x, 32);
            if (enemy.Health <= 0)
            {
                enemy.MaxHealth = enemy.MaxHealth + 10;
                enemy.Health = enemy.MaxHealth;
                enemy.Damage++;
                playerData.Gold++;
                playerData.Essence++;
                playerData.Step++;
                playerData.Exp++;
                if (playerData.Exp == 5)
                {
                    playerData.Level++;
                    playerData.Exp = 0;
                }
                GoldTxt.text = string.Format("Gold: {0}", playerData.Gold);
                EssenceTxt.text = string.Format("Essence: {0}", playerData.Essence);
                LevelTxt.text = string.Format("Level: {0} {1}/5", playerData.Level, playerData.Exp);
                if (playerData.Step == 3)
                {
                    playerData.Stage++;
                    playerData.Step = 1;
                }
                StageTxt.text = string.Format("Steps: {0}", playerData.Stage);
                StepTxt.text = string.Format("Steps: {0}", playerData.Step);
            }

            playerData.Health -= enemy.Damage;
            PlayerHealthRect.sizeDelta = new Vector2(GetPercentValue(0, playerData.Health, playerData.Health, playerData.MaxHealth, 2).x, 32);
            if (playerData.Health <= 0)
            {
                playerData = null;
                isPlayLoaded = false;
                SceneManager.LoadScene(2);
            }

            saveLoad.playerData = playerData;
            saveLoad.Save();
            attackTimer = 0;
        }
        else
            attackTimer += elapsed;
    }

    private Vector2 GetPercentValue(float minA, float maxA, float valA, float valB, float y)
    {
        return new Vector2(Mathf.Lerp(minA, maxA, (valA / valB)), y);
    }

    private GameObject SpawnEnemy()
    {
        return Instantiate(Resources.Load<GameObject>("Enemies/Bandits/Bandits - Pixel Art/Demo/HeavyBandit"), new Vector3(4, -3.5f, 1), Quaternion.identity);
    }
}
