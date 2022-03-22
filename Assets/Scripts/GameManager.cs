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

    public RuntimeAnimatorController[] level_ac;

    public Text jelatin_text;
    public Text gold_text;

    void Awake()
    {
        isSell = false;
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
}
