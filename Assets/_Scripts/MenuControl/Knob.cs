using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Knob : MonoBehaviour
{
    SwipeLevels _swipeLevels;

    private void Start()
    {
        _swipeLevels = GameObject.FindGameObjectWithTag("Scroll View content").GetComponent<SwipeLevels>();
    }

    public void OnKnobClicked(Button btn)
    {
        _swipeLevels.OnKnobClicked(btn);
    }

    public void OnPlayClicked()
    {
        SceneManager.LoadScene((int)_swipeLevels._currenLevel);
    }
}
