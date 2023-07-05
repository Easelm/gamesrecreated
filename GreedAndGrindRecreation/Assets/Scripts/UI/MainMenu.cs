using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button NewGameBtn;

    // Start is called before the first frame update
    void Start()
    {
        NewGameBtn.onClick.AddListener(() => SceneManager.LoadScene(1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
