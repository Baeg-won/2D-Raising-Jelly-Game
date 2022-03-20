using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int jelatin;
    public int gold;

    public Text jelatin_text;
    public Text gold_text;

    void LateUpdate()
    {
        jelatin_text.text = string.Format("{0:n0}", Mathf.SmoothStep(float.Parse(jelatin_text.text), jelatin, 0.5f));
        gold_text.text = string.Format("{0:n0}", Mathf.SmoothStep(float.Parse(gold_text.text), gold, 0.5f));
    }
}
