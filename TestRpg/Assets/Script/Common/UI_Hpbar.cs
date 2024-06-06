using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Hpbar : MonoBehaviour, IListener
{
    [SerializeField]
    private Image Hpbar;

    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.HP, this);
    }

    public void OnEvent(EVENT_TYPE type, object Param = null)
    {
        if(type == EVENT_TYPE.HP)
        {
            float fill = (float)Param;
            Hpbar.fillAmount = fill * 0.01f;
        }
    }
}
