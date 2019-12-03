﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.AI_System;

/// <summary>
/// Handles the AI agents sensory suite, uses OverlapSphereNonAlloc to detect objects in view range,
/// provides various access functions to percieved objects with different constraints e.g. only enemies
/// and access to the list holding all detected objects. Only objects in the 'VisibleToAI' layer are found
/// by the OverlapSphereNonAlloc. A ray cast is used to determine if a wall, defined by the 'Walls' layer,
/// obstruct the view.
/// </summary>
public class Sensing : MonoBehaviour
{
    // The owner of the senses
    private AgentData _agentData;

    private const int MaxObjectsInView = 10;
    // The maximum number of objects which the short term memory can recall.
    private const int MaxObjectsInMemory = 5;
    // The length in seconds that an object will persist in memory for.
    private const float MemoryLength = 7.5f;

    // Masks to limit visibility
    public LayerMask VisibleToAiMask;
    public LayerMask WallsLayer;

    // Keep track of game objects in our visual field
    private readonly Dictionary<String, GameObject> _objectsPercieved = new Dictionary<String, GameObject>();
    public Dictionary<String, GameObject> ObjectsPercieved
    {
        get { return _objectsPercieved; }
    }

    // _overlapResults is returned by the sphere overlap function
    private Collider[] _overlapResults = new Collider[MaxObjectsInView];
    // _objects in view is the list of objects not obstructed (and not ourself)
    private List<GameObject> _objectsInView = new List<GameObject>(MaxObjectsInView);
    // short term memory is the lust of objects not obstructed which we recall seeing.
    private CustomQueue<GameObject> _shortTermMemory = new CustomQueue<GameObject> ();

    // Use this for initialization
    void Awake ()
    {
        _agentData = GetComponentInParent<AgentData> ();
    }

    private void Start ()
    {
        StartCoroutine (UpdateShortTermMemory ());
    }

    // Update short term memory and remove the earliest object we remember from memory.
    private IEnumerator UpdateShortTermMemory ()
    {
        // If there are still things in memory, remove the earliest one.
        if (_shortTermMemory.Count > 0)
            _shortTermMemory.Dequeue ();

        yield return new WaitForSeconds (MemoryLength);

        // Call this again after our memory length has warranted a new removal.
        StartCoroutine (UpdateShortTermMemory ());
    }

    /// <summary>
    /// This updates the objectsPercievecd list by calling OverlapSphereNonAlloc with the mask selecting only
    /// objects the AI should be able to see. this list is filtered further by using a raycast to remove any objects
    /// obstructed by walls, using the WallsLayer layer. This method is called whenever the AI needs information about
    /// objects it can see
    /// </summary>
    private void UpdateViewedObjectsList()
    {
        _objectsInView.Clear();

        // Get objects in view
        int numFound = Physics.OverlapSphereNonAlloc(transform.position, _agentData.ViewRange, _overlapResults, VisibleToAiMask);

        // Add all except ourselves to list of GameObjects in view range
        for (int i = 0; i < numFound; i++)
        {
            if (!_overlapResults[i].gameObject.name.Equals(gameObject.transform.parent.name))
            {
                // Do this to prevent the raycast finding a wall behind the object and therefore treating the object as obstructed
                float objectDistance = Mathf.Min(Vector3.Distance(transform.position, _overlapResults[i].gameObject.transform.position), _agentData.ViewRange);

                // Ensure we are not looking through a wall
                if (!Physics.Raycast(transform.position, _overlapResults[i].gameObject.transform.position - transform.position, objectDistance, WallsLayer))
                {
                    // We can see it
                    _objectsInView.Add(_overlapResults[i].gameObject);

                    //// Loop through the memory and ensure the object isn't already within memory.
                    //foreach (GameObject go in _shortTermMemory)
                    //{
                    //    if (go == _overlapResults[i].gameObject)
                    //    {
                    //        _shortTermMemory.
                    //    }

                    //    // If there is still room in our short term memory then add it.
                    //    if (_shortTermMemory.Count <= MaxObjectsInMemory - 1)
                    //    {
                    //        _shortTermMemory.Enqueue (_overlapResults[i].gameObject);
                    //    }
                    //    // If there isn't then remove an earlier known object and add it.
                    //    else
                    //    {
                    //        _shortTermMemory.Dequeue ();
                    //        _shortTermMemory.Enqueue (_overlapResults[i].gameObject);
                    //    }
                    //}
                }
            }
        }
    }

    /// <summary>
    /// Return a list of all the objects the AI can see
    /// </summary>
    /// <returns>List of GameObjects</returns>
    public List<GameObject> GetObjectsInView()
    {
        UpdateViewedObjectsList();
        return _objectsInView;
    }

    /// <summary>
    /// Returns a list of all the collectable objects in view
    /// </summary>
    /// <returns>List of GameObjects</returns>
    public List<GameObject> GetCollectablesInView()
    {
        UpdateViewedObjectsList();
        return _objectsInView.Where(x => x.CompareTag(Tags.Collectable)).ToList();
    }

    /// <summary>
    /// Returns a list of friendly AI's in view
    /// </summary>
    /// <returns>List of GameObjects</returns>
    public List<GameObject> GetFriendliesInView()
    {
        UpdateViewedObjectsList();
        return _objectsInView.Where(x => x.CompareTag(_agentData.FriendlyTeamTag)).ToList();
    }

    /// <summary>
    /// Returns a list of enemy AI's in view
    /// </summary>
    /// <returns>List of GameObjects</returns>
    public List<GameObject> GetEnemiesInView()
    {
        UpdateViewedObjectsList();
        return _objectsInView.Where(x => x.CompareTag(_agentData.EnemyTeamTag)).ToList();
    }

    /// <summary>
    /// Returns a list of object with a specific tag in view
    /// </summary>
    /// <param name="tagToSelect">The tag to filter the returned list by</param>
    /// <returns>List of GameObjects</returns>
    public List<GameObject> GetObjectsInViewByTag(string tagToSelect)
    {
        UpdateViewedObjectsList();
        return _objectsInView.Where(x => x.CompareTag(tagToSelect)).ToList();
    }

    /// <summary>
    /// Returns an object with a specific name
    /// </summary>
    /// <param name="nameToSelect">The name of the object to return</param>
    /// <returns>GameObject</returns>
    public GameObject GetObjectInViewByName(string nameToSelect)
    {
        UpdateViewedObjectsList();
        return _objectsInView.SingleOrDefault(x=>x.name.Equals(nameToSelect));
    }

    /// <summary>
    /// Check if a GameObject is within the AI agents reach
    /// </summary>
    /// <param name="item">the item to check the distance of</param>
    /// <returns>true if the object is in range, false otherwise</returns>
    public bool IsItemInReach(GameObject item)
    {
        if (item != null)
        {
            var collectablesInView = GetCollectablesInView ();

            if (collectablesInView.Count > 0)
            {
                if (Vector3.Distance (gameObject.transform.parent.position, item.transform.position) < _agentData.PickUpRange)
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Check if we're with attacking range of a specific enemy AI
    /// </summary>
    /// <param name="target">The enemy AI to check the distance of</param>
    /// <returns>true if the enemy is within range, false otherwise</returns>
    public bool IsInAttackRange(GameObject target)
    {
        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.transform.position) < _agentData.AttackRange)
            {
                return true;
            }
        }
        return false;
    }
}