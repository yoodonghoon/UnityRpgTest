using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class UIManager : SingletonCommon<UIManager>
{
    Dictionary<string, UIWindow> UI_dic = new ();
    private Stack<UIWindow> openPopups = new Stack<UIWindow>();

    public GameObject OpenUI(string ui, bool isable = true)
    {
        if (UI_dic.ContainsKey(ui))
        {
            UISet(ui, isable);
            return UI_dic[ui].gameObject;
        }
        else
        {
            var obj =Instantiate(Resources.Load(ui)) as GameObject;
            UI_dic.Add(ui, obj.GetComponent<UIWindow>());
            openPopups.Push(obj.GetComponent<UIWindow>());
            return obj;
        }
    }

    public void ClosePopup(string ui)
    {
       if(UI_dic.ContainsKey(ui))
        {
            UISet(ui, false);
        }
    }

    public void UISet(string ui, bool value)
    {
        UI_dic[ui].gameObject.SetActive(value);

        if (value)
            openPopups.Push(UI_dic[ui]);
        else
            openPopups.Pop();
    }

    private void Start()
    {
        var click = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.Escape));
        click.Subscribe( x => CloseLastOpenedPopup());
    }

    //¸¶Áö¸·¿¡ ¿ÀÇÂµÈ ÆË¾÷ ´Ý±â
    public void CloseLastOpenedPopup()
    {
        if (openPopups.Count > 0)
        {
            ClosePopup(openPopups.Peek().GetName());
        }
    }

    //¸ðµç ÆË¾÷ ´Ý±â
    public void CloseAllOpenPopups()
    {
        while (openPopups.Count > 0)
        {
            ClosePopup(openPopups.Peek().GetName());
        }
    }
}