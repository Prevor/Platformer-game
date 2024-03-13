using UnityEngine;

public class ProgressMenu : MonoBehaviour
{
    public void BackToMainMenu()
    {
        GameManager.Instance._menuController.SetBool("isShowProgressMenu", false);
        GameManager.Instance._menuController.SetBool("isHideMainMenu", false);
    }

    public void SelectGame()
    {
        GameManager.Instance._menuController.SetBool("isShowProgressMenu", false);
        GameManager.Instance._menuController.SetBool("isShowGameMenu", true);
    }
}
