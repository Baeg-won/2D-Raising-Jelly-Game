using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int jelatin;
    public int gold;

    public int max_jelatin;
    public int max_gold;

    public int[] jelly_goldlist;
    public bool isSell;
    public bool isLive;

    public RuntimeAnimatorController[] level_ac;

    public Text jelatin_text;
    public Text gold_text;

    public Image jelly_panel;
    public Image plant_panel;
    public Image option_panel;

    Animator jelly_anim;
    Animator plant_anim;

    bool isJellyClick;
    bool isPlantClick;
    bool isOption;

    void Awake()
    {
        jelly_anim = jelly_panel.GetComponent<Animator>();
        plant_anim = plant_panel.GetComponent<Animator>();

        isLive = true;
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel")) {
            if (isJellyClick) ClickJellyBtn();
            else if (isPlantClick) ClickPlantBtn();
            else Option();
        }
    }

    void LateUpdate()
    {
        jelatin_text.text = string.Format("{0:n0}", Mathf.SmoothStep(float.Parse(jelatin_text.text), jelatin, 0.5f));
        gold_text.text = string.Format("{0:n0}", Mathf.SmoothStep(float.Parse(gold_text.text), gold, 0.5f));
    }

    public void ChangeAc(Animator anim, int level)
    {
        anim.runtimeAnimatorController = level_ac[level - 1];
    }

    public void GetJelatin(int id, int level)
    {
        jelatin += (id + 1) * level;

        if (jelatin > max_jelatin)
            jelatin = max_jelatin;
    }

    public void GetGold(int id, int level)
    {
        gold += jelly_goldlist[id] * level;

        if (gold > max_gold)
            gold = max_gold;
    }

    public void CheckSell()
    {
        isSell = isSell == false;
    }

    public void ClickJellyBtn()
    {
        if (isPlantClick) {
            plant_anim.SetTrigger("doHide");
            isPlantClick = false;
            isLive = true;
        }   

        if (isJellyClick)
            jelly_anim.SetTrigger("doHide");
        else
            jelly_anim.SetTrigger("doShow");

        isJellyClick = !isJellyClick;
        isLive = !isLive;
    }

    public void ClickPlantBtn()
    {
        if (isJellyClick) {
            jelly_anim.SetTrigger("doHide");
            isJellyClick = false;
            isLive = true;
        }

        if (isPlantClick)
            plant_anim.SetTrigger("doHide");
        else
            plant_anim.SetTrigger("doShow");

        isPlantClick = !isPlantClick;
        isLive = !isLive;
    }

    void Option()
    {
        isOption = !isOption;
        isLive = !isLive;

        option_panel.gameObject.SetActive(isOption);
        Time.timeScale = isOption == true ? 0 : 1;
    }
}
