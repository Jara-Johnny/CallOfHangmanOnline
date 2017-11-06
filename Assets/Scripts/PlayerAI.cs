using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : PlayerNetworking
{
    [SerializeField]
    private string[] words;
    [SerializeField]
    private AnimationCurve behaviourDistribution;

    //Char list
    private List<char> vocals;
    private List<char> commonConsonants;
    private List<char> unusualConsonants;

    private float behaviourFactor;
    private float behaviourGap = 4.0f;

    public void SelectWord()
    {
        word = words[Random.Range(0, words.Length)];
    }

    public void DoMove()
    {
        int playableTurn = (int)(GameManagerNetworking.Singleton.turn * 0.5) - 1;

        behaviourFactor = playableTurn + Random.Range(-behaviourGap, behaviourGap) * behaviourFactor;

        if (behaviourFactor >= -behaviourGap && behaviourFactor <= behaviourGap)
            PlayVocal();
        else if (behaviourFactor >= 0 && behaviourFactor <= 2 * behaviourGap)
            PlayCommonConsonant();
        else if (behaviourFactor >= behaviourGap && behaviourFactor <= 3 * behaviourGap)
            PlayUnusualConsonant();
        else
            PlaySign();
    }

    public void PlayVocal()
    {

    }

    public void PlayCommonConsonant()
    {

    }

    public void PlayUnusualConsonant()
    {

    }

    public void PlaySign()
    {

    }
}
