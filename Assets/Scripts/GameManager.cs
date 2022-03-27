using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int jelatin;
    public int gold;
    public List<Jelly> jelly_list = new List<Jelly>();
    public List<Data> jelly_data_list = new List<Data>();
    public bool[] jelly_unlock_list;

    public int max_jelatin;
    public int max_gold;

    public bool isSell;
    public bool isLive;

    public Sprite[] jelly_spritelist;
    public string[] jelly_namelist;
    public int[] jelly_jelatinlist;
    public int[] jelly_goldlist;

    public Text page_text;
    public Image unlock_group_jelly_img;
    public Text unlock_group_gold_text;
    public Text unlock_group_name_text;

    public GameObject lock_group;
    public Image lock_group_jelly_img;
    public Text lock_group_jelatin_text;

    public RuntimeAnimatorController[] level_ac;

    public Text jelatin_text;
    public Text gold_text;

    public Image jelly_panel;
    public Image plant_panel;
    public Image option_panel;

    public GameObject prefab;

    public GameObject data_manager_obj;

    DataManager data_manager;

    Animator jelly_anim;
    Animator plant_anim;

    bool isJellyClick;
    bool isPlantClick;
    bool isOption;

    int page;

    void Awake()
    {
        instance = this;

        jelly_anim = jelly_panel.GetComponent<Animator>();
        plant_anim = plant_panel.GetComponent<Animator>();

        isLive = true;

        jelatin_text.text = jelatin.ToString();
        gold_text.text = gold.ToString();
        unlock_group_gold_text.text = jelly_goldlist[0].ToString();
        lock_group_jelatin_text.text = jelly_jelatinlist[0].ToString();

        data_manager = data_manager_obj.GetComponent<DataManager>();

        page = 0;
        jelly_unlock_list = new bool[12];
    }

    void Start()
    {
        Invoke("LoadData", 0.1f);
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
        jelatin_text.text = string.Format("{0:n0}", (int)Mathf.SmoothStep(float.Parse(jelatin_text.text), jelatin, 0.5f));
        gold_text.text = string.Format("{0:n0}", (int)Mathf.SmoothStep(float.Parse(gold_text.text), gold, 0.5f));
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

    public void GetGold(int id, int level, Jelly jelly)
    {
        gold += jelly_goldlist[id] * level;

        if (gold > max_gold)
            gold = max_gold;

        jelly_list.Remove(jelly);
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

    public void PageUp()
    {
        if (page >= 11) return;

        ++page;
        ChangePage();
    }

    public void PageDown()
    {
        if (page <= 0) return;

        --page;
        ChangePage();
    }

    void ChangePage()
    {
        lock_group.gameObject.SetActive(!jelly_unlock_list[page]);

        page_text.text = string.Format("#{0:00}", (page + 1));

        if (lock_group.activeSelf) {
            lock_group_jelly_img.sprite = jelly_spritelist[page];
            lock_group_jelatin_text.text = string.Format("{0:n0}", jelly_jelatinlist[page]);

            lock_group_jelly_img.SetNativeSize();
        }
        else {
            unlock_group_jelly_img.sprite = jelly_spritelist[page];
            unlock_group_name_text.text = jelly_namelist[page];
            unlock_group_gold_text.text = string.Format("{0:n0}", jelly_goldlist[page]);

            unlock_group_jelly_img.SetNativeSize();
        }
    }

    public void Unlock()
    {
        if (jelatin < jelly_jelatinlist[page]) return;

        jelly_unlock_list[page] = true;
        ChangePage();

        jelatin -= jelly_jelatinlist[page];
    }

    public void BuyJelly()
    {
        if (gold < jelly_goldlist[page]) return;

        gold -= jelly_goldlist[page];

        GameObject obj = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        Jelly jelly = obj.GetComponent<Jelly>();
        obj.name = "Jelly " + page;
        jelly.id = page;
        jelly.sprite_renderer.sprite = jelly_spritelist[page];

        jelly_list.Add(jelly);
    }

    void LoadData()
    {
        lock_group.gameObject.SetActive(!jelly_unlock_list[page]);

        for (int i = 0; i < jelly_data_list.Count; ++i)
        {
            GameObject obj = Instantiate(prefab, jelly_data_list[i].pos, Quaternion.identity);
            Jelly jelly = obj.GetComponent<Jelly>();
            jelly.id = jelly_data_list[i].id;
            jelly.level = jelly_data_list[i].level;
            jelly.exp = jelly_data_list[i].exp;
            jelly.sprite_renderer.sprite = jelly_spritelist[jelly.id];
            jelly.anim.runtimeAnimatorController = level_ac[jelly.level - 1];
            obj.name = "Jelly " + jelly.id;

            jelly_list.Add(jelly);
        }
    }

    void OnApplicationQuit()
    {
        data_manager.JsonSave();
    }
}