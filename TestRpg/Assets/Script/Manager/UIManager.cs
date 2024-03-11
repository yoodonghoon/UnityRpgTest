using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonCommon<UIManager>
{
    Dictionary<string, GameObject> UI_dic = new ();

    public GameObject OpenUI(string ui, bool isable = true)
    {
        if (UI_dic.ContainsKey(ui))
        {
            UISet(ui, true);
            return UI_dic[ui];
        }
        else
        {
            var obj =Instantiate(Resources.Load(ui)) as GameObject;
            UI_dic.Add(ui, obj);
            return obj;
        }
    }

    public void UISet(string ui, bool value) => UI_dic[ui].gameObject.SetActive(value);
}