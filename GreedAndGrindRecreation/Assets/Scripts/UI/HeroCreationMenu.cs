using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HeroCreationMenu : MonoBehaviour
{
    public Button KnightCreationBtn;

    void Start()
    {
        KnightCreationBtn.onClick.AddListener(() => { GameManager.Instance.characterChoice = GameManager.CharacterChoice.KNIGHT; SceneManager.LoadScene(2); });
    }
}
