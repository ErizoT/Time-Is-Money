using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public TextMeshProUGUI tmpro;


    void OnEnable()
    {
        tmpro.text = "You Made $" + GlobalData.Instance.PlayerMoney.ToString() + " ....";

    }


    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

}
