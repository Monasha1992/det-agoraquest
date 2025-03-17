using UnityEngine;
using System.Collections.Generic;
using Levels.Shared;

public class NpcManager : MonoBehaviour
{
    public static NpcManager Instance;
    public List<NpcController> activeNpcs = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void RegisterNpc(NpcController npc)
    {
        if (!activeNpcs.Contains(npc))
            activeNpcs.Add(npc);
    }

    public void UnregisterNpc(NpcController npc)
    {
        if (activeNpcs.Contains(npc))
            activeNpcs.Remove(npc);
    }

    public List<NpcController> GetNearbyNpcs(Vector3 position, float range)
    {
        List<NpcController> nearbyNpcs = new List<NpcController>();

        foreach (NpcController npc in activeNpcs)
        {
            if (Vector3.Distance(position, npc.transform.position) <= range)
                nearbyNpcs.Add(npc);
        }

        return nearbyNpcs;
    }
}