using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.AI_System.Goals;

namespace Assets.Scripts.AI_System
{
    public interface IEvaluator<T> where T : class
    {
        IGoal<T> GetState (T agent);
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

        public IGoal<AI> GetState (AI agent)
        {
            return new Goal_Search ();
        }
    }

    public class Evaluator_GuardFriendlyFlag : IEvaluator<AI>
    {
        public float CalculateDesirability (AI agent)
        {
            float desirability = 0.0f;
            // Tweaker decided based on pre-tweaked value adjusting.
            const float tweaker = 0.45f;
            Vector3 lastKnownFlagPosition = WorldManager.Instance.GetLastKnownFlagPosition (agent.Data.FriendlyFlagName);

            // If the flag is not within our base, then there's no point guarding it.
            if (WorldManager.Instance.IsTeamFlagHome (agent) == false)
                return desirability = 0.0f;

            // If we're currently holding a flag, then it's not a good idea to guard.
            if (agent.Data.HasEnemyFlag)
                return desirability = 0.0f;

            float score = GlobalEvaluators.Evaluator_CurrentScore (agent);
            float numberOfFriendlies = GlobalEvaluators.Evaluator_NumberOfFriendliesAtPosition (agent, lastKnownFlagPosition);

            desirability = tweaker * ( ( 1 - score ) * (1 - numberOfFriendlies) );

            return Mathf.Clamp (desirability, 0.0f, 1.0f);
        }

        public IGoal<AI> GetState (AI agent)
        {
            return new Goal_GuardArea (WorldManager.Instance.GetLastKnownFlagPosition (agent.Data.FriendlyFlagName));
        }
    }

    public class Evaluator_GuardEnemyFlag : IEvaluator<AI>
    {
        public float CalculateDesirability (AI agent)
        {
            float desirability = 0.0f;
            // Tweaker decided based on pre-tweaked value adjusting.
            const float tweaker = 0.45f;
            Vector3 lastKnownFlagPosition = WorldManager.Instance.GetLastKnownFlagPosition (agent.Data.EnemyFlagName);

            // If the flag is not within our base, then there's no point guarding it.
            if (WorldManager.Instance.IsEnemyFlagCaptured (agent) == false)
                return desirability = 0.0f;

            // If we're currently holding a flag, then it's not a good idea to guard.
            if (agent.Data.HasFriendlyFlag)
                return desirability = 0.0f;

            float score = GlobalEvaluators.Evaluator_CurrentScore (agent);
            float numberOfFriendlies = GlobalEvaluators.Evaluator_NumberOfFriendliesAtPosition (agent, lastKnownFlagPosition);

            desirability = tweaker * ( ( 1 - score ) * ( 1 - numberOfFriendlies ) );

            return Mathf.Clamp (desirability, 0.0f, 1.0f);
        }

        public IGoal<AI> GetState (AI agent)
        {
            return new Goal_GuardArea (WorldManager.Instance.GetLastKnownFlagPosition (agent.Data.EnemyFlagName));
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

            float health = GlobalEvaluators.Evaluator_Health (agent);
            float distanceToTarget = GlobalEvaluators.Evaluator_DistanceToObject (agent.gameObject, target);
            float strength = GlobalEvaluators.Evaluator_Strength (agent);

            desirability = tweaker * ( ( health * ( 1 - distanceToTarget ) ) * strength );

            Log.Desirability ("AttackEnemy", desirability, agent);

            return Mathf.Clamp (desirability, 0.0f, 1.0f);
        }

        public IGoal<AI> GetState (AI agent)
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

            float health = GlobalEvaluators.Evaluator_Health (agent);
            float distanceToPowerup = GlobalEvaluators.Evaluator_DistanceToObject (agent.gameObject, WorldManager.Instance.PowerupSpawner);
            float score = GlobalEvaluators.Evaluator_CurrentScore (agent);

            desirability = tweaker * ( (( ( 1 - distanceToPowerup ) ) ) );

            //Debug.Log ("GET POWERUP DESIRIABILITY: " + desirability);

            return Mathf.Clamp (desirability, 0.0f, 1.0f);
        }

        public IGoal<AI> GetState (AI agent)
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

            float health = GlobalEvaluators.Evaluator_Health (agent);
            float numberofEnemiesInSight = GlobalEvaluators.Evaluator_NumberOfEnemies (agent);
            float distanceOfClosestEnemy = GlobalEvaluators.Evaluator_DistanceToObject (agent.gameObject, agent.Targeting.SelectTarget ());

            desirability = tweaker * (( 1 - distanceOfClosestEnemy ) + numberofEnemiesInSight );

            return Mathf.Clamp (desirability, 0.0f, 1.0f);
        }

        public IGoal<AI> GetState (AI agent)
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

            // Get the distance from here to the health item.
            float distance = GlobalEvaluators.Evaluator_DistanceToObject (agent.gameObject, healthSpawner);
            // Get the health using a sigmoid logistic curve.
            float health = UtilityCurves.Logistic.Evaluate (GlobalEvaluators.Evaluator_Health (agent));

            // If the distance is greater than 1 then return 0 as this goal isn't desirable.
            if (distance >= 1)
                return 0.0f;

            // Tweaker decided based on pre-tweaked value adjusting.
            const float tweaker = 1.2f;

            // Calculate the desirablity of getting the health item and clamp it.
            float desirability = tweaker * (( 1 - health ) / distance);

            //Log.Desirability ("Heal", desirability, agent);

            return Mathf.Clamp (desirability, 0.0f, 1.0f);
        }

        public IGoal<AI> GetState (AI agent)
        {
            return new CompState_Heal ();
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

            float numberOFEnemies = GlobalEvaluators.Evaluator_NumberOfEnemies (agent);

            desirability = tweaker * ( ( ( 1 - numberOFEnemies ) ) );

            return Mathf.Clamp (desirability, 0.0f, 1.0f);
        }

        public IGoal<AI> GetState (AI agent)
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

            float numberOFEnemies = GlobalEvaluators.Evaluator_NumberOfEnemies (agent);

            desirability = tweaker * ( ( ( 1 - numberOFEnemies ) ) );

            return Mathf.Clamp (desirability, 0.0f, 1.0f);
        }

        public IGoal<AI> GetState (AI agent)
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

            Vector3 enemyFlagPosition = WorldManager.Instance.GetLastKnownFlagPosition (agent.Data.EnemyFlagName);
            float health = GlobalEvaluators.Evaluator_Health (agent);
            float distanceToFlag = GlobalEvaluators.Evaluator_DistanceToPosition (agent.gameObject, enemyFlagPosition);

            desirability = tweaker * ( health * ( 1 - distanceToFlag ) );

            return Mathf.Clamp (desirability, 0.0f, 1.0f);
        }

        public IGoal<AI> GetState (AI agent)
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

            Vector3 friendlyFlagPosition = WorldManager.Instance.GetLastKnownFlagPosition (agent.Data.FriendlyFlagName);
            float health = GlobalEvaluators.Evaluator_Health (agent);
            float distanceToFlag = GlobalEvaluators.Evaluator_DistanceToPosition (agent.gameObject, friendlyFlagPosition);
            float flagDistanceFromBase = GlobalEvaluators.Evaluator_DistanceToPosition (agent.Data.FriendlyBase, friendlyFlagPosition);

            desirability = tweaker * ( ( health + ( 1 - distanceToFlag ) ) * flagDistanceFromBase );

            return Mathf.Clamp (desirability, 0.0f, 1.0f);
        }

        public IGoal<AI> GetState (AI agent)
        {
            string flagName = agent.Data.FriendlyFlagName;
            return new CompGoal_GetFlag (flagName);
        }
    }
}
