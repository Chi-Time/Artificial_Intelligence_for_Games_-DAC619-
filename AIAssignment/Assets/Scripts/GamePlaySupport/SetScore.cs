using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.AI_System;

/// <summary>
/// Updates the score when the enemy flag is dropped inside the friendly base
/// The score is updated every second
/// </summary>
public class SetScore : MonoBehaviour
{
    public int Score;
    public GameObject EnemyFlag;

    private bool _enemyFlagInBase;
    private const float ScoreTickDuration = 1.0f;

    /// <summary>
    /// Collision with base trigger
    /// </summary>
    /// <param name="other">the collidee</param>
    void OnTriggerEnter(Collider other)
    {
        if (this.gameObject.name.Equals (Names.BlueBase) && other.gameObject.name.Equals (Names.BlueFlag))
        {
            WorldManager.Instance.IsBlueFlagInHome = true;
        }
        else if (this.gameObject.name.Equals (Names.RedBase) && other.gameObject.name.Equals (Names.RedFlag))
        {
            WorldManager.Instance.IsRedFlagInHome = true;
        }

        // React to the enemy flag
        if (other.gameObject.name.Equals (EnemyFlag.name))
        {
            _enemyFlagInBase = true;
            StartCoroutine (UpdateScore ());

            if (this.gameObject.name.Equals (Names.BlueBase))
                WorldManager.Instance.IsRedFlagCaptured = true;
            else if (this.gameObject.name.Equals (Names.RedBase))
                WorldManager.Instance.IsBlueFlagCaptured = true;
        }
    }

    /// <summary>
    /// The object has left the base
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        // React to the enemy flag
        if (other.gameObject.name.Equals(EnemyFlag.name))
        {
            _enemyFlagInBase = false;
        }

        if (this.gameObject.name.Equals (Names.BlueBase) && other.gameObject.name.Equals (Names.BlueFlag))
        {
            WorldManager.Instance.IsBlueFlagInHome = false;
        }
        else if (this.gameObject.name.Equals (Names.RedBase) && other.gameObject.name.Equals (Names.RedFlag))
        {
            WorldManager.Instance.IsRedFlagInHome = false;
        }
    }

    /// <summary>
    /// This actually updates the score every second while the flag is in the base
    /// There is no upper limit to the score
    /// </summary>
    /// <returns>Enmuerator for Coroutine</returns>
    IEnumerator UpdateScore()
    {
        // The score updates as long as the flag is in the base
        while(_enemyFlagInBase)
        {
            yield return new WaitForSeconds(ScoreTickDuration);
            Score++;
        }
    }
}
