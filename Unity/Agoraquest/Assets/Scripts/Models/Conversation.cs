using System.Collections.Generic;
using Levels.Shared;
using UnityEngine;

namespace Models
{
    public class Conversation
    {
        public Vector3 Position { get; set; }
        public List<NpcController> Members { get; set; } = new();
        public List<NpcController> ActiveMembers { get; set; } = new();
        public List<ConversationMessage> Messages { get; set; }
        public bool IsTalking { get; set; } = false;
        private int MaxMembers { get; } = Random.Range(2, 6);
        public bool IsFull => Members.Count == MaxMembers;

        public Conversation(Vector3 position)
        {
            Position = position;
        }

        public void AddToConversation(NpcController npc)
        {
            if (!Members.Contains(npc) && !IsFull) Members.Add(npc);
        }
    }
}