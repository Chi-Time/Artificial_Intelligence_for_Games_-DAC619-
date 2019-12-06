﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.AI_System.Goals;

namespace Assets.Scripts.AI_System
{
    /// <summary>Interface defining common functionality amongst evaluators.</summary>
    /// <typeparam name="T"></typeparam>
    public interface IEvaluator<T> where T : class
    {
        IGoal<T> GetGoal (T agent);
        float CalculateDesirability (T agent);
    }

    public class Evaluator_Search : IEvaluator<AI>
    {
        public float CalculateDesirability (AI agent)
        {
            // Return a fixed value as this goal is only in case there's nothing else more pressing to do right now.
            float desirability = 0.05f;
            return desirability;
        }

        public IGoal<AI> GetGoal (AI agent)
        {
            return new Goal_Search ();
        }
    }

    public class Evaluator_AttackEnemy : IEvaluator<AI>
    {
        public float CalculateDesirability (AI agent)
        {
            float desirability = 0.0f;
            // Tweaker decided based on pre-tweaked value adjusting.
            const float tweaker = 1.0f;

            GameObject target = agent.Targeting.SelectTarget ();

            // No enemies are in sight so this goal isn't desirable.
            if (target == null)
                return desirability = 0.0f;

            // Grab our variables.
            float health = DataEvaluators.Health (agent);
            float distanceToTarget = DataEvaluators.DistanceToObject (agent.gameObject, target);
            float strength = DataEvaluators.Strength (agent);

            // Apply the equation outlined in the Report and clamp the end result.
            desirability = tweaker * ( ( health * ( 1 - distanceToTarget ) ) * strength );

            return Mathf.Clamp (desirability, 0.0f, 1.0f);
        }

        public IGoal<AI> GetGoal (AI agent)
        {
            return new Goal_AttackEnemy ();
        }
    }

    public class Evaluator_GetPowerup : IEvaluator<AI>
    {
        public float CalculateDesirability (AI agent)
        {
            float desirability = 0.0f;
            // Tweaker decided based on pre-tweaked value adjusting.
            const float tweaker = 0.5f;
            float viewRange = agent.Data.ViewRange;

            // Already have the item? No need to collect it.
            if (agent.Inventory.HasItem (Names.PowerUp))
                return 0.0f;

            // If we're close enough to see the object.
            if (Vector3.Distance (agent.transform.position, WorldManager.Instance.PowerupSpawner.transform.position) <= viewRange)
            {
                // Can't see it? This isn't a desirable move.
                if (agent.Senses.GetObjectInViewByName (Names.PowerUp) == false)
                    return 0.0f;
            }

            // Grab our variables.
            float health = DataEvaluators.Health (agent);
            float distanceToPowerup = DataEvaluators.DistanceToObject (agent.gameObject, WorldManager.Instance.PowerupSpawner);

            // Apply the equation outlined in the Report and clamp the end result.
            desirability = tweaker * ( (( ( 1 - distanceToPowerup ) ) ) );

            return Mathf.Clamp (desirability, 0.0f, 1.0f);
        }

        public IGoal<AI> GetGoal (AI agent)
        {
            GameObject itemLocation = WorldManager.Instance.PowerupSpawner;
            return new Goal_GetItem (itemLocation, Names.PowerUp);
        }
    }

    public class Evaluator_UsePowerup : IEvaluator<AI>
    {
        public float CalculateDesirability (AI agent)
        {
            float desirability = 0.0f;
            // Tweaker decided based on pre-tweaked value adjusting.
            const float tweaker = 0.2f;

            // Don't have the item? Then this is undesirable.
            if (agent.Inventory.HasItem (Names.PowerUp) == false)
                return desirability = 0.0f;

            // Grab our variables.
            float health = DataEvaluators.Health (agent);
            float numberofEnemiesInSight = DataEvaluators.NumberOfEnemiesInSight (agent);
            float distanceOfClosestEnemy = DataEvaluators.DistanceToObject (agent.gameObject, agent.Targeting.SelectTarget ());

            // Apply the equation outlined in the Report and clamp the end result.
            desirability = tweaker * (( 1 - distanceOfClosestEnemy ) + numberofEnemiesInSight );

            return Mathf.Clamp (desirability, 0.0f, 1.0f);
        }

        public IGoal<AI> GetGoal (AI agent)
        {
            return new Goal_UseItem (Names.PowerUp);
        }
    }

    public class Evaluator_Heal : IEvaluator<AI>
    {
        public float CalculateDesirability (AI agent)
        {
            float viewRange = agent.Data.ViewRange;
            // Find and get the health item.
            var healthSpawner = WorldManager.Instance.HealthKitSpawner;

            // If the item doesn't exist then return 0 as this goal isn't desirable.
            if (healthSpawner == null)
                return 0.0f;

            // If we're close enough to see the object.
            if (Vector3.Distance (agent.transform.position, WorldManager.Instance.PowerupSpawner.transform.position) <= viewRange)
            {
                // Can't see it? This isn't a desirable move.
                if (agent.Senses.GetObjectInViewByName (Names.HealthKit) == false)
                    return 0.0f;
            }

            // Grab our variables.
            float distance = DataEvaluators.DistanceToObject (agent.gameObject, healthSpawner);
            // Get the health using a sigmoid logistic curve to help influence it and give it characteristic.
            float health = UtilityCurves.Logistic.Evaluate (DataEvaluators.Health (agent));

            // If the distance is greater than 1 then return 0 as this goal isn't desirable.
            if (distance >= 1)
                return 0.0f;

            // Tweaker decided based on pre-tweaked value adjusting.
            const float tweaker = 1.2f;

            // Calculate the desirablity of getting the health item and clamp it.
            float desirability = tweaker * (( 1 - health ) / distance);

            return Mathf.Clamp (desirability, 0.0f, 1.0f);
        }

        public IGoal<AI> GetGoal (AI agent)
        {
            return new CompGoal_Heal ();
        }
    }

    public class Evaluator_ScoreEnemyFlag : IEvaluator<AI>
    {
        public float CalculateDesirability (AI agent)
        {
            float desirability = 1.0f;

            // Tweaker decided based on pre-tweaked value adjusting.
            const float tweaker = 0.9f;

            if (agent.Data.HasEnemyFlag == false)
                return desirability = 0.0f;

            // Grab our variables.
            float numberOFEnemies = DataEvaluators.NumberOfEnemiesInSight (agent);

            // Apply the equation outlined in the Report and clamp the end result.
            desirability = tweaker * ( ( ( 1 - numberOFEnemies ) ) );

            return Mathf.Clamp (desirability, 0.0f, 1.0f);
        }

        public IGoal<AI> GetGoal (AI agent)
        {
            return new CompGoal_ScoreFlag (agent.Inventory.GetItem (agent.Data.EnemyFlagName));
        }
    }

    public class Evaluator_ScoreFriendlyFlag : IEvaluator<AI>
    {
        public float CalculateDesirability (AI agent)
        {
            float desirability = 1.0f;

            // Tweaker decided based on pre-tweaked value adjusting.
            const float tweaker = 0.9f;

            if (agent.Data.HasFriendlyFlag == false)
                return desirability = 0.0f;

            // Grab our variables.
            float numberOFEnemies = DataEvaluators.NumberOfEnemiesInSight (agent);

            // Apply the equation outlined in the Report and clamp the end result.
            desirability = tweaker * ( ( ( 1 - numberOFEnemies ) ) );

            return Mathf.Clamp (desirability, 0.0f, 1.0f);
        }

        public IGoal<AI> GetGoal (AI agent)
        {
            return new CompGoal_ScoreFlag (agent.Inventory.GetItem (agent.Data.FriendlyFlagName));
        }
    }

    public class Evaluator_GetEnemyFlag : IEvaluator<AI>
    {
        public float CalculateDesirability (AI agent)
        {
            float desirability = 0.0f;
            // Tweaker decided based on pre-tweaked value adjusting.
            const float tweaker = 1.1f;

            // If we already have their flag, this isn't a good move.
            if (WorldManager.Instance.TeamHasEnemyFlag (agent))
            {
                return desirability = 0.0f;
            }

            // If the flag is already captured then don't bother.
            if (WorldManager.Instance.IsEnemyFlagCaptured (agent))
                return desirability = 0.0f;

            // If we're already carrying our flag there's no point getting their's.
            if (agent.Data.HasFriendlyFlag)
                return desirability = 0.0f;

            // Grab our variables.
            Vector3 enemyFlagPosition = WorldManager.Instance.GetLastKnownFlagPosition (agent.Data.EnemyFlagName);
            float health = DataEvaluators.Health (agent);
            float distanceToFlag = DataEvaluators.DistanceToPosition (agent.gameObject, enemyFlagPosition);

            // Apply the equation outlined in the Report and clamp the end result.
            desirability = tweaker * ( health * ( 1 - distanceToFlag ) );

            return Mathf.Clamp (desirability, 0.0f, 1.0f);
        }

        public IGoal<AI> GetGoal (AI agent)
        {
            string flagName = agent.Data.EnemyFlagName;
            return new CompGoal_GetFlag (flagName);
        }
    }

    public class Evaluator_RetrieveLostFlag : IEvaluator<AI>
    {
        public float CalculateDesirability (AI agent)
        {
            float desirability = 0.0f;
            // Tweaker decided based on pre-tweaked value adjusting.
            const float tweaker = 0.38f;

            // If we already have our flag, this isn't a good move.
            if (WorldManager.Instance.TeamHasFriendlyFlag (agent))
                return desirability = 0.0f;

            // If our flag is already in our base then don't bother.
            if (WorldManager.Instance.IsTeamFlagHome (agent))
                return desirability = 0.0f;

            // If we're already carrying their flag there's no point getting ours.
            if (agent.Data.HasEnemyFlag)
                return desirability = 0.0f;

            // Grab our variables.
            Vector3 friendlyFlagPosition = WorldManager.Instance.GetLastKnownFlagPosition (agent.Data.FriendlyFlagName);
            float health = DataEvaluators.Health (agent);
            float distanceToFlag = DataEvaluators.DistanceToPosition (agent.gameObject, friendlyFlagPosition);
            float flagDistanceFromBase = DataEvaluators.DistanceToPosition (agent.Data.FriendlyBase, friendlyFlagPosition);

            // Apply the equation outlined in the Report and clamp the end result.
            desirability = tweaker * ( ( health + ( 1 - distanceToFlag ) ) * flagDistanceFromBase );

            return Mathf.Clamp (desirability, 0.0f, 1.0f);
        }

        public IGoal<AI> GetGoal (AI agent)
        {
            string flagName = agent.Data.FriendlyFlagName;
            return new CompGoal_GetFlag (flagName);
        }
    }
}
