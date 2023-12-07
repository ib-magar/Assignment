using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public int TargetCount;
    public FluidColor targetColor;

    private float maxCoffeeLevel;
    private void Start()
    {
        coffeeLevel.fillAmount = 0f;
        maxCoffeeLevel = TargetCount;
        UpdateTargetText();

       // GamecompleteEvent.AddListener(GameComplete);
    }
    public void ReceiveFluids(List<GameObject> fluids)
    {
        int drops = fluids.Count;
        FluidColor _color = fluids[0].GetComponent<Selectable>().color;
        if (_color == targetColor)
        {
            TargetCount -= drops;
            TargetCount = Mathf.Clamp(TargetCount, 0,(int) maxCoffeeLevel);

            UpdateCoffeeLevel();
            UpdateTargetText();
        }
    }
    public TMP_Text targetCountText;
    void UpdateTargetText()
    {
        targetCountText.text= "*"+TargetCount.ToString();
    }
    [Header("Coffee")]
    public Image coffeeLevel;
    public  UnityEvent GamecompleteEvent;
    void UpdateCoffeeLevel()
    {
        float targetlevel = Mathf.Clamp01((maxCoffeeLevel-TargetCount)/maxCoffeeLevel);
        StartCoroutine(levelUp());
        IEnumerator levelUp()
        {
            float timer = .1f;
            while(coffeeLevel.fillAmount<=targetlevel)
            {
                coffeeLevel.fillAmount = Mathf.Lerp(coffeeLevel.fillAmount, targetlevel, Time.deltaTime * 5f); ;
                yield return null;
            }
        }

        if(TargetCount<=0)
        {
            Debug.Log("LevelComplete");
            GamecompleteEvent.Invoke();
         }
    }
    //win condition

    public GameObject completeMark;
     public void GameComplete()
    {
        completeMark.SetActive(true);
    }
   
}
