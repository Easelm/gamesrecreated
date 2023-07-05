using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CastleSceneUI : MonoBehaviour
{
    public Button PortalBtn;

    // Start is called before the first frame update
    void Start()
    {
        PortalBtn.onClick.AddListener(() => SceneManager.LoadScene(3));
    }
}
