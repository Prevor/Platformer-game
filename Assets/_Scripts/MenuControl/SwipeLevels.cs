using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwipeLevels : MonoBehaviour
{
    [SerializeField][Range(1, 16)] int transitionSpeed = 5;
    [SerializeField][Range(0, 1)] float neighbourReductionPercentage = 0.5f;
    [SerializeField] bool scrollWhenReleased = true;
    [SerializeField][Range(0.05f, 1)] float scrollStopSpeed = 0.1f;
    [SerializeField] Sprite[] knobSprites;
    [SerializeField] GameObject knob;
    [SerializeField] Scrollbar scrollbar;
    [SerializeField] Transform knobContainer;

    [SerializeField] GameObject btnPlay;

    Vector2 neighbourScale;
    Vector2 mainScale;
    public float _currenLevel = 1;
    float _currentScrollbarValue = 0;
    float[] attractionPoints;
    float subdivisionDistance;
    float attractionPoint;
    float childCount;
    bool knobClicked = false;

    private void Start()
    {
        scrollbar.value = _currenLevel - 1;

        attractionPoints = new float[transform.childCount];
        childCount = attractionPoints.Length;

        subdivisionDistance = 1f / (childCount - 1f);

        for (int i = 0; i < childCount; i++)
        {
            attractionPoints[i] = subdivisionDistance * i;
            Instantiate(knob, knobContainer);
        }
        foreach (Transform child in transform)
        {
            child.localScale = new Vector2(neighbourReductionPercentage, neighbourReductionPercentage);
        }
        if (childCount > 0)
        {
            knobContainer.GetChild(0).GetComponent<Image>().sprite = knobSprites[0];
            transform.GetChild(0).localScale = Vector2.one;
        }
    }

    void Update()
    {
        if (!knobClicked && (Input.GetMouseButton(0) || (scrollWhenReleased && GetScrollSpeed() > scrollStopSpeed)))
        {
            _currentScrollbarValue = scrollbar.value;
            FindAttractionPoint();
            UpdateUI();
        }
        else if (IsBeingScaled())
        {
            scrollbar.value = Mathf.Lerp(scrollbar.value, attractionPoint, transitionSpeed * Time.deltaTime);
            UpdateUI();
        }
        else
        {
            knobClicked = false;
        }

    }

    private void FindAttractionPoint()
    {
        if (_currentScrollbarValue <= 0)
        {
            attractionPoint = 0;
        }
        else
        {
            for (int i = 0; i < childCount; i++)
            {
                if (_currentScrollbarValue < attractionPoints[i] + (subdivisionDistance / 2) && _currentScrollbarValue > attractionPoints[i] - (subdivisionDistance / 2))
                {
                    attractionPoint = attractionPoints[i];
                    break;
                }
                if (i == childCount - 1)
                    attractionPoint = attractionPoints[i];
            }
        }
    }
    private void UpdateUI()
    {
        for (int i = 0; i < attractionPoints.Length; i++)
        {
            if (attractionPoints[i] == attractionPoint)
            {
                knobContainer.GetChild(i).GetComponent<Image>().sprite = knobSprites[0];
                mainScale = Vector2.Lerp(transform.GetChild(i).localScale, Vector2.one, 2 * transitionSpeed * Time.deltaTime);
                transform.GetChild(i).localScale = mainScale;

                if (transform.GetChild(i).childCount == 0)
                {
                    Instantiate(btnPlay, transform.GetChild(i));

                }
            }
            else
            {
                knobContainer.GetChild(i).GetComponent<Image>().sprite = knobSprites[1];
                neighbourScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(neighbourReductionPercentage, neighbourReductionPercentage), 2 * transitionSpeed * Time.deltaTime);
                transform.GetChild(i).localScale = neighbourScale;
                
                if (transform.GetChild(i).childCount == 1)
                {
                    Destroy(transform.GetChild(i).GetChild(0).gameObject);
                }
            }
        }
    }

    private bool IsBeingScaled()
    {
        return Mathf.Abs(scrollbar.value - attractionPoint) > 0.01f || mainScale.x < 0.99f || neighbourScale.x > neighbourReductionPercentage + 0.01f;
    }

    private float GetScrollSpeed()
    {
        return Mathf.Abs(_currentScrollbarValue - scrollbar.value) / Time.deltaTime;
    }

    public void OnKnobClicked(Button btn)
    {
        knobClicked = true;
        Transform parent = btn.transform.parent.transform;
        Transform pressedButton = btn.transform;

        int i = 0;
        foreach (Transform child in parent)
        {
            if (child == pressedButton)
            {
                attractionPoint = attractionPoints[i];
                break;
            }
            i++;
        }
    }
}
