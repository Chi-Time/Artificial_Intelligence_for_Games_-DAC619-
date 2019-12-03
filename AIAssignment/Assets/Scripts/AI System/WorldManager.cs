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

        public GameObject RedFlag { get; private set; }
        public GameObject BlueFlag { get; private set; }
        public GameObject RedBase { get; private set; }
        public GameObject BlueBase { get; private set; }
        public GameObject PowerupSpawner { get; private set; }
        public GameObject HealthKitSpawner { get; private set; }
        public List<AI> RedTeamMembers { get; private set; }
        public List<AI> BlueTeamMembers { get; private set; }

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
            FindTeams ();
        }

        private void FindObjects ()
        {
            RedFlag = GameObject.Find (Names.RedFlag);
            BlueFlag = GameObject.Find (Names.BlueFlag);
            RedBase = GameObject.Find (Names.RedBase);
            BlueBase = GameObject.Find (Names.BlueBase);
            PowerupSpawner = GameObject.Find (Names.PowerupSpawner);
            HealthKitSpawner = GameObject.Find (Names.HealthKitSpawner);
        }

        private void FindTeams ()
        {
            var redTeam = GameObject.FindGameObjectsWithTag (Tags.RedTeam);
            var blueTeam = GameObject.FindGameObjectsWithTag (Tags.BlueTeam);

            foreach (GameObject member in redTeam)
            {
                if (member.GetComponent<AI> () != null)
                    RedTeamMembers.Add (member.GetComponent<AI> ());
            }

            foreach (GameObject member in blueTeam)
            {
                if (member.GetComponent<AI> () != null)
                    BlueTeamMembers.Add (member.GetComponent<AI> ());
            }
        }

        private AI[] GetAllAgents ()
        {
            List<AI> agents = new List<AI> (6);

            agents.Concat (RedTeamMembers);
            agents.Concat (BlueTeamMembers);

            return agents.ToArray ();
        }

        private AI[] GetTeammates (AI agent)
        {
            List<AI> teamMembers = null;

            if (agent.tag == Tags.RedTeam)
            {
                teamMembers = RedTeamMembers;
                teamMembers.Remove (agent);

                return teamMembers.ToArray ();
            }
            else if (agent.tag == Tags.BlueTeam)
            {
                teamMembers = BlueTeamMembers;
                teamMembers.Remove (agent);

                return teamMembers.ToArray ();
            }

            return null;
        }
    }
}
