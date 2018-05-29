using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBox : MonoBehaviour
{

    EventManager em;
    Animator thisAnim;

    bool firstTime = true;
    private void OnEnable()
    {
        thisAnim = GetComponent<Animator>();
        em = Toolbox.RegisterComponent<EventManager>();
        em.LevelFadeStart += levelFadeStart;

        if (firstTime)
            firstTime = false;
        else
            thisAnim.SetTrigger("MusicFadeIn");
    }
    private void OnDisable()
    {
        em.LevelFadeStart -= levelFadeStart;
    }
    void levelFadeStart()
    {
        thisAnim.SetTrigger("MusicFadeOut");
    }
}
