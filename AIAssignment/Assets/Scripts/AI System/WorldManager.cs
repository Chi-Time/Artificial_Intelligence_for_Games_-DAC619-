using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI_System
{
    class WorldManager : MonoBehaviour
    {
        public static WorldManager Instance { get; private set; }

        /// <summary>Returns true if the red flag is in the red base, false if not.</summary>
        public bool IsRedFlagInHome { get; set; }
        /// <summary>Returns true if the blue flag is in blue base, false if not.</summary>
        public bool IsBlueFlagInHome { get; set; }
        /// <summary>Returns true if the red flag is in blue base, false if not.</summary>
        public bool IsRedFlagCaptured { get; set; }
        /// <summary>Returns true if the blue flag is in red base, false if not.</summary>
        public bool IsBlueFlagCaptured { get; set; }
        /// <summary>Reference to the red team's flag.</summary>
        public GameObject RedFlag { get; private set; }
        /// <summary>Reference to the blue team's flag.</summary>
        public GameObject BlueFlag { get; private set; }
        /// <summary>Reference to the last known position of the red flag.</summary>
        public Vector3 RedFlagLastPosition { get; set; }
        /// <summary>Reference to the last known position of the blue flag.</summary>
        public Vector3 BlueFlagLastPosition { get; set; }
        /// <summary>Refernce to the powerup spawner.</summary>
        public GameObject PowerupSpawner { get; private set; }
        /// <summary>Reference to the health kit spawner.</summary>
        public GameObject HealthKitSpawner { get; private set; }
        /// <summary>Reference to all the members of red team.</summary>
        public HashSet<AI> RedTeamMembers { get; set; }
        /// <summary>Reference to all the members of blue team.</summary>
        public HashSet<AI> BlueTeamMembers { get; set; }
        /// <summary>Reference to the important locations around the map.</summary>
        public List<GameObject> ImportantLocations { get; private set; }

        private void Awake ()
        {
            // Initialise singleton.
            if (Instance == null)
                Instance = this;
            else
                Destroy (this.gameObject);
        }

        private void Start ()
        {
            FindObjects ();
        }

        private void FindObjects ()
        {
            // Find and assign each of the objects from the world.
            RedFlag = GameObject.Find (Names.RedFlag);
            BlueFlag = GameObject.Find (Names.BlueFlag);
            PowerupSpawner = GameObject.Find (Names.PowerupSpawner);
            HealthKitSpawner = GameObject.Find (Names.HealthKitSpawner);
            RedTeamMembers = new HashSet<AI> ();
            BlueTeamMembers = new HashSet<AI> ();
            RedFlagLastPosition = RedFlag.transform.position;
            BlueFlagLastPosition = BlueFlag.transform.position;

            ImportantLocations = new List<GameObject> ();
            ImportantLocations.Add (PowerupSpawner);
            ImportantLocations.Add (HealthKitSpawner);
        }

        /// <summary>Adds member to the relevent team.</summary>
        /// <param name="agent">The agent to add to the team.</param>
        public void AddMemberToTeam (AI agent)
        {
            // Check to see what team the agent belongs to and add him to the respective list.
            if (agent.Data.CompareTag (Tags.BlueTeam))
                BlueTeamMembers.Add (agent);
            else if (agent.Data.CompareTag (Tags.RedTeam))
                RedTeamMembers.Add (agent);
        }

        /// <summary>Removes member from the relevent team.</summary>
        /// <param name="agent">The agent to remove from the team.</param>
        public void RemoveMemberFromTeam (AI agent)
        {
            // Check to see what team the agent belongs to and remove him from the respective list.
            if (agent.Data.CompareTag (Tags.BlueTeam))
                BlueTeamMembers.Remove (agent);
            else if (agent.Data.CompareTag (Tags.RedTeam))
                RedTeamMembers.Remove (agent);
        }

        /// <summary>Get the last known position of the given flag.</summary>
        /// <param name="flagName">The flag in to check for.</param>
        /// <returns>A coordinate of the flag's last known position.</returns>
        public Vector3 GetLastKnownFlagPosition (string flagName)
        {
            if (flagName == Names.RedFlag)
                return RedFlagLastPosition;
            else if (flagName == Names.BlueFlag)
                return BlueFlagLastPosition;

            return Vector3.zero;
        }

        /// <summary>Determines if the agent's teams flag is at there base.</summary>
        /// <param name="agent">The agent to check for.</param>
        /// <returns>True if the friendly flag is at their base, false if not.</returns>
        public bool IsTeamFlagHome (AI agent)
        {
            if (agent.Data.CompareTag (Tags.BlueTeam))
            {
                return IsBlueFlagInHome;
            }
            else if (agent.Data.CompareTag (Tags.RedTeam))
            {
                return IsRedFlagInHome;
            }

            return false;
        }

        /// <summary>Determines if the agent's enemies flag is at there base.</summary>
        /// <param name="agent">The agent to check for.</param>
        /// <returns>True if the enemy flag is at their base, false if not.</returns>
        public bool IsEnemyFlagCaptured (AI agent)
        {
            if (agent.Data.CompareTag (Tags.BlueTeam))
            {
                return IsRedFlagCaptured;
            }
            else if (agent.Data.CompareTag (Tags.RedTeam))
            {
                return IsBlueFlagCaptured;
            }

            return false;
        }

        /// <summary>Get a random important location from the map.</summary>
        /// <returns>A random important location.</returns>
        public GameObject GetImportantLocation ()
        {
            var location = ImportantLocations[UnityEngine.Random.Range (0, ImportantLocations.Count)];

            return location;
        }

        /// <summary>Retrive's everyone on the agent's team including them.</summary>
        /// <param name="agent">The agent target.</param>
        /// <returns>The entire agent's team.</returns>
        public AI[] GetTeammates (AI agent)
        {
            // Setup a default array.
            AI[] teamMembers = null;

            // Base on which team the agent belongs to, return the relevent array of members.
            if (agent.tag == Tags.RedTeam)
            {
                teamMembers = RedTeamMembers.ToArray ();

                return teamMembers.ToArray ();
            }
            else if (agent.tag == Tags.BlueTeam)
            {
                teamMembers = BlueTeamMembers.ToArray ();

                return teamMembers;
            }

            // If the agent is null or belongs to no-one, just return null.
            return null;
        }

        /// <summary>Determines if the agent's team currently has their own flag held.</summary>
        /// <param name="agent">The agent target.</param>
        /// <returns>True if the team is holding the friendlies flag.</returns>
        public bool TeamHasFriendlyFlag (AI agent)
        {
            // Get all the entire team's members.
            var teamMembers = GetTeammates (agent);

            // If any of them are holding flag return true that they have it.
            foreach (AI member in teamMembers)
            {
                if (member.Data.HasFriendlyFlag)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>Determines if the agent's team currently has their enemies flag held.</summary>
        /// <param name="agent">The agent target.</param>
        /// <returns>True if the team is holding the enemies flag.</returns>
        public bool TeamHasEnemyFlag (AI agent)
        {
            // Get all the entire team's members.
            var teamMembers = GetTeammates (agent);

            // If any of them are holding flag return true that they have it.
            foreach (AI member in teamMembers)
            {
                if (member.Inventory.HasItem (agent.Data.EnemyFlagName))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
