using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.Instance._menuController.SetBool("isHideMainMenu", true);
        GameManager.Instance._menuController.SetBool("isShowProgressMenu", true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
