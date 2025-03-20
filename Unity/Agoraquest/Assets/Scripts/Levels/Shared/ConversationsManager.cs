using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using Models;
using Unity.AI.Navigation;
using UnityEngine;

namespace Levels.Shared
{
    public class ConversationsManager : MonoBehaviour
    {
        private readonly List<Conversation> _conversations = new();
        public NavMeshSurface walkingSurface;

        public Conversation AssignToConversation(NpcController controller)
        {
            // Check if the NPC is already in a conversation and remove them
            foreach (var conversation in
                     _conversations.Where(conversation => conversation.Members.Contains(controller)))
            {
                conversation.Members.Remove(controller);
                break;
            }

            // Find groups with less than 3 members
            var availableConversations = _conversations.Where(conversation => !conversation.IsFull).ToList();

            if (availableConversations.Count > 0)
            {
                // Join a random available conversation
                var randomConversation = availableConversations[Random.Range(0, availableConversations.Count)];
                randomConversation.AddToConversation(controller);
                return randomConversation;
            }

            // Create a new conversation
            var randomPos = GetRandomPoint();
            _conversations.Add(new Conversation(randomPos)
            {
                Messages = ConversationsData.GetRandomConversation(),
                Members = new List<NpcController> { controller }
            });
            return _conversations.Last();
        }

        public Vector3 GetRandomPoint()
        {
            // Generate a random point within the bounds
            var randomX = Random.Range(-6f, 6f);
            // var randomZ = Random.Range(10f, 5f); //10f - (walkingSurface?.size.z ?? 5f));
            var randomZ = Random.Range(1.5f, 1.5f - (walkingSurface?.size.z ?? 3f));

            Debug.Log("z---"+randomZ);
            return new Vector3(randomX, transform.position.y, randomZ);
        }


        public void HasArrived(NpcController npcController)
        {
            var conversation =
                _conversations.FirstOrDefault(conversation => conversation.Members.Contains(npcController));
            if (conversation == null) return;

            conversation.ActiveMembers.Add(npcController);

            // start the conversation where there are 2 members
            if (conversation.ActiveMembers.Count == 2) StartCoroutine(StartConversation(conversation));
        }

        private IEnumerator StartConversation(Conversation conversation)
        {
            foreach (var message in conversation.Messages.Where(msg => !msg.IsDone))
            {
                var randomMember = conversation.ActiveMembers[Random.Range(0, conversation.ActiveMembers.Count)];
                // randomMember.Speak(message.Message);
                //Debug.Log(message.Message);

                conversation.IsTalking = true;
                yield return StartCoroutine(randomMember.Speak(message.Message));
                message.IsDone = true;
                conversation.IsTalking = false;

                // Add a 1-second delay between messages
                yield return new WaitForSeconds(2f);
            }

            // End the conversation after all messages are spoken
            EndConversation(conversation);

            // var message = conversation.Messages.FirstOrDefault(msg => !msg.IsDone)?.Message;
            // if (message != null)
            // {
            //     conversation.Members[Random.Range(0, conversation.Members.Count)].Speak(message);
            //     conversation.Messages.First(msg => !msg.IsDone).IsDone = true;
            // }
            // else
            // {
            //     // end conversation
            //     EndConversation(conversation);
            // }
        }

        private void EndConversation(Conversation conversation)
        {
            _conversations.Remove(conversation);
            ReassignCurrentConversationMembers(conversation);
        }

        private void ReassignCurrentConversationMembers(Conversation conversation)
        {
            // Create a temporary list to hold all NPCs in the current conversation
            var curMembers = new List<NpcController>(conversation.Members);

            // Clear the members of the current conversation
            conversation.Members.Clear();

            // Reassign each NPC to a new conversation
            foreach (var npc in curMembers) AssignToConversation(npc);
        }
    }
}