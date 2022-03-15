using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [System.Serializable]
    public struct level {
        public Sprite levelSprite;
        public string sceneName;
    }

    [SerializeField]
    private level[] levels = { };
    public Button[] selectors = { };
    public Image[] difficulties = { };
    public Image blank = null; //used as the current image visible
    public float timeForMove = 1;

    private DifficultyToken dT;
    private Image next;
    private int currentDifficulty;
    private Vector3 left, right, center;
    private int currentIndex;
    private void Awake()
    {
        dT = FindObjectOfType<DifficultyToken>();
        timeForMove = Mathf.Abs(timeForMove);
        center = blank.rectTransform.anchoredPosition;
        Vector3 difference = new Vector3(blank.rectTransform.rect.width, 0, 0);
        left = center - difference;
        right = center + difference;
        next = Instantiate(blank.gameObject, blank.transform.parent).GetComponent<Image>();
        Reset();
    }
    private void OnEnable()
    {
        Reset();
    }
    private void Reset()
    {
        StopAllCoroutines();
        currentIndex = 0;
        Plus(0);
        SetSprite(blank);
        blank.rectTransform.anchoredPosition = center;
        next.rectTransform.anchoredPosition = right;
        foreach (Button b in selectors)
        {
            b.gameObject.SetActive(levels.Length > 1);
        }
        SetDifficulty();
    }
    public void SelectNext(bool isLeft = true) {
        int nextNum = isLeft ? -1 : 1;
        Plus(nextNum);
        SetSprite(next);
        foreach (Button b in selectors) {
            b.gameObject.SetActive(false);
        }
        StartCoroutine("ChangePictures", isLeft);
    }
    private IEnumerator ChangePictures(bool isLeft = true) {
        next.rectTransform.anchoredPosition = isLeft ? right : left;
        StartCoroutine(MovePic(blank, isLeft));
        yield return new WaitForSeconds(timeForMove / 5);
        yield return StartCoroutine(MovePic(next, isLeft));
        SetSprite(blank);
        blank.rectTransform.anchoredPosition = center;
        next.rectTransform.anchoredPosition = right;
        if (levels.Length > 1) {
            foreach (Button b in selectors)
            {
                b.gameObject.SetActive(true);
            }
        }
    }
    private IEnumerator MovePic(Image image, bool isLeft = true) {
        Vector3 origin = image.rectTransform.anchoredPosition;
        Vector3 newPosition = origin + (isLeft ? -1 : 1) * (right - center);
        for (float currentTime = 0; currentTime < timeForMove; currentTime += Time.deltaTime) {
            image.rectTransform.anchoredPosition = Vector3.Lerp(origin, newPosition, currentTime / timeForMove);
            yield return null;
        }
    }
    private void SetSprite(Image i) {
        if (currentIndex != -1) {
            i.sprite = levels[currentIndex].levelSprite;
        }
    }
    public void SetDifficulty(int dif = -1) {
        currentDifficulty = dif % difficulties.Length;
        if (currentDifficulty < 0)
        {
            foreach (Image i in difficulties)
            {
                i.color = Color.white;
            }
        }
        else {
            foreach (Image i in difficulties) {
                i.color = Color.gray;
            }
            difficulties[currentDifficulty].color = Color.white;
        }
    }
    public void Confirm() {
        if (currentDifficulty > -1)
        {
            dT.Set(currentDifficulty);
            FindObjectOfType<MenuManager>().SetScene(levels[currentIndex].sceneName);
        }
        else {
            foreach (Image i in difficulties) {
                i.color = Color.gray;
            }
        }
    }
    private void Plus(int amount) {
        int newValue = currentIndex + amount;
        if (levels.Length < 1)
        {
            currentIndex = -1;
        }
        else if (newValue < 0) {
            currentIndex = levels.Length - 1;
        }
        else {
            currentIndex = newValue % levels.Length;
        }
    }
}
