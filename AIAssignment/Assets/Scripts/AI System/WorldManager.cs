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

        public bool IsRedFlagInHome { get; set; }
        public bool IsBlueFlagInHome { get; set; }
        public bool IsRedFlagCaptured { get; set; }
        public bool IsBlueFlagCaptured { get; set; }
        public GameObject RedFlag { get; private set; }
        public GameObject BlueFlag { get; private set; }
        public Vector3 RedFlagLastPosition { get; set; }
        public Vector3 BlueFlagLastPosition { get; set; }
        public GameObject RedBase { get; private set; }
        public GameObject BlueBase { get; private set; }
        public GameObject PowerupSpawner { get; private set; }
        public GameObject HealthKitSpawner { get; private set; }
        public HashSet<AI> RedTeamMembers { get; set; }
        public HashSet<AI> BlueTeamMembers { get; set; }
        public List<GameObject> ImportantLocations { get; private set; }

        private void Awake ()
        {
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
            RedFlag = GameObject.Find (Names.RedFlag);
            BlueFlag = GameObject.Find (Names.BlueFlag);
            RedBase = GameObject.Find (Names.RedBase);
            BlueBase = GameObject.Find (Names.BlueBase);
            PowerupSpawner = GameObject.Find (Names.PowerupSpawner);
            HealthKitSpawner = GameObject.Find (Names.HealthKitSpawner);
            RedTeamMembers = new HashSet<AI> ();
            BlueTeamMembers = new HashSet<AI> ();
            RedFlagLastPosition = RedFlag.transform.position;
            BlueFlagLastPosition = BlueFlag.transform.position;

            ImportantLocations = new List<GameObject> ();
            ImportantLocations.Add (RedBase);
            ImportantLocations.Add (BlueBase);
            ImportantLocations.Add (PowerupSpawner);
            ImportantLocations.Add (HealthKitSpawner);
        }

        public void AddMemberToTeam (AI agent)
        {
            if (agent.Data.CompareTag (Tags.BlueTeam))
                BlueTeamMembers.Add (agent);
            else if (agent.Data.CompareTag (Tags.RedTeam))
                RedTeamMembers.Add (agent);
        }

        public void RemoveMemberFromTeam (AI agent)
        {
            if (agent.Data.CompareTag (Tags.BlueTeam))
                BlueTeamMembers.Remove (agent);
            else if (agent.Data.CompareTag (Tags.RedTeam))
                RedTeamMembers.Remove (agent);
        }

        public Vector3 GetLastKnownFlagPosition (string flagName)
        {
            if (flagName == Names.RedFlag)
                return RedFlagLastPosition;
            else if (flagName == Names.BlueFlag)
                return BlueFlagLastPosition;

            return Vector3.zero;
        }

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

        public GameObject GetImportantLocation ()
        {
            var location = ImportantLocations[UnityEngine.Random.Range (0, ImportantLocations.Count)];

            return location;
        }

        public AI[] GetAllAgents ()
        {
            AI[] agents = null;

            agents.Concat (RedTeamMembers);
            agents.Concat (BlueTeamMembers);

            return agents;
        }

        public AI[] GetTeammates (AI agent)
        {
            AI[] teamMembers = null;

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

            return null;
        }

        public bool TeamHasFriendlyFlag (AI agent)
        {
            var teamMembers = GetTeammates (agent);

            foreach (AI member in teamMembers)
            {
                if (member.Data.HasFriendlyFlag)
                {
                    return true;
                }
            }

            return false;
        }

        public bool TeamHasEnemyFlag (AI agent)
        {
            var teamMembers = GetTeammates (agent);

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
