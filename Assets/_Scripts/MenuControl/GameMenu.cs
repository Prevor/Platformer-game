using Cinemachine;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera rogueCam, barbarianeCam, knightCam, mageCam;

    private int currentCharacterSelection;

    public void OnArrowClick(bool right)
    {
        if (right)
        {
            currentCharacterSelection++;

            if (currentCharacterSelection >= 4)
                currentCharacterSelection = 1;

            if (rogueCam.Priority == 2) // there are a problem here!!
            {
                currentCharacterSelection++;
                OnSelectionChanged(currentCharacterSelection);
            }
            else
            {
                OnSelectionChanged(currentCharacterSelection);
            }
        }
        else
        {
            currentCharacterSelection--;

            if (currentCharacterSelection < 1)
                currentCharacterSelection = 4;

            OnSelectionChanged(currentCharacterSelection);
        }
    }

    private void OnSelectionChanged(int currentCharacterSelection)
    {
        Debug.Log(currentCharacterSelection);

        switch (currentCharacterSelection)
        {
            case 1:
                rogueCam.Priority = 2;
                barbarianeCam.Priority = 0;
                knightCam.Priority = 0;
                mageCam.Priority = 0;
                break;
            case 2:
                rogueCam.Priority = 0;
                barbarianeCam.Priority = 2;
                knightCam.Priority = 0;
                mageCam.Priority = 0;
                break;
            case 3:
                rogueCam.Priority = 0;
                barbarianeCam.Priority = 0;
                knightCam.Priority = 2;
                mageCam.Priority = 0;
                break;
            case 4:
                rogueCam.Priority = 0;
                barbarianeCam.Priority = 0;
                knightCam.Priority = 0;
                mageCam.Priority = 2;
                break;
            default:
                break;
        }
    }

    public void OnCharacterClicked()
    {
        GameManager.Instance._menuController.SetTrigger("ShowCharacherSelection");

        rogueCam.Priority = 2;
    }

    public void OnLevelsClicked()
    {

        GameManager.Instance._menuController.SetTrigger("ShowLevelsSelection");
        SetDefaultCamera();
    }

    public void OnShopClicked()
    {
        GameManager.Instance._menuController.SetTrigger("ShowShopPage");
        SetDefaultCamera();
    }

    public void OnMainMenuClicked()
    {
        GameManager.Instance._menuController.SetBool("isShowGameMenu", false);
        GameManager.Instance._menuController.SetBool("isHideMainMenu", false);
        GameManager.Instance._menuController.SetTrigger("ShowMainMenu");
        SetDefaultCamera();
    }

    private void SetDefaultCamera()
    {
        rogueCam.Priority = 0;
        barbarianeCam.Priority = 0;
        knightCam.Priority = 0;
        mageCam.Priority = 0;
    }
}
