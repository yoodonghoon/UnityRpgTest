using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class DoTweenAlpha : MonoBehaviour
{
    public TextMeshProUGUI Text;

    void Start()
    {
        StartCoroutine(Fade(true));
    }

    public void FadeIn() //���̵� �� ���
    {
        StartCoroutine(Fade(true));
    }

    public void FadeOut() //���̵� �ƿ� ���
    {
        StartCoroutine(Fade(false));
    }

    private IEnumerator Fade(bool isFadeIn)
    {
        if (isFadeIn)
        {
            Text.alpha = 1;
            Tween tween = Text.DOFade(0f, 1f);
            yield return tween.WaitForCompletion();
            StartCoroutine(Fade(false));
        }
        else
        {
            Text.alpha = 0;
            Text.gameObject.SetActive(true);
            Tween tween = Text.DOFade(1f, 1f);
            yield return tween.WaitForCompletion();
            StartCoroutine(Fade(true));
        }
    }
}
