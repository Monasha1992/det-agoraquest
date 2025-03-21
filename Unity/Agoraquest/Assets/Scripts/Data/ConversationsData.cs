using System;
using System.Collections.Generic;
using Models;

namespace Data
{
    public static class ConversationsData
    {
        public static List<ConversationMessage> GetRandomConversation()
        {
            var conversations = GetAllConversations();
            var randomIndex = new Random().Next(0, conversations.Count);
            return conversations[randomIndex];
        }

        private static List<List<ConversationMessage>> GetAllConversations()
        {
            return new List<List<ConversationMessage>>()
            {
                new()
                {
                    new ConversationMessage { Message = "Hey, how's it going?" },
                    new ConversationMessage { Message = "I'm doing great! Just finished a long day at work." },
                    new ConversationMessage { Message = "Same here, work was exhausting today." },
                    new ConversationMessage { Message = "Did you all hear about the new cafe that opened downtown?" },
                    new ConversationMessage { Message = "Yes! I heard their coffee is amazing." },
                    new ConversationMessage { Message = "We should check it out sometime this weekend." },
                    new ConversationMessage { Message = "Sounds like a plan! Saturday morning?" },
                    new ConversationMessage { Message = "Perfect. Maybe we can grab breakfast there too." },
                    new ConversationMessage { Message = "I hope they have good pastries." },
                    new ConversationMessage { Message = "They do! I saw their menu online." },
                    new ConversationMessage { Message = "By the way, did you guys watch the game last night?" },
                    new ConversationMessage { Message = "Yes! That last-minute goal was incredible." },
                    new ConversationMessage { Message = "I missed it! Who won?" },
                    new ConversationMessage { Message = "Our team won by one goal. It was intense." },
                    new ConversationMessage { Message = "That’s amazing! I need to watch the highlights." },
                    new ConversationMessage { Message = "We should go watch the next match together." },
                    new ConversationMessage { Message = "That would be fun! Let’s plan it." },
                    new ConversationMessage { Message = "Where should we watch it?" },
                    new ConversationMessage { Message = "Maybe at Mike’s place? He has a big TV." },
                    new ConversationMessage { Message = "Yeah, I’m fine with that!" },
                    new ConversationMessage { Message = "Awesome! Let’s meet at 7 PM before the game." },
                    new ConversationMessage { Message = "Are we bringing snacks?" },
                    new ConversationMessage { Message = "Of course! I’ll bring chips." },
                    new ConversationMessage { Message = "I’ll bring some drinks." },
                    new ConversationMessage { Message = "I’ll grab some popcorn." },
                    new ConversationMessage { Message = "This is going to be great!" },
                    new ConversationMessage { Message = "By the way, how’s your new job going, Emily?" },
                    new ConversationMessage { Message = "It’s been really good! The team is nice." },
                    new ConversationMessage { Message = "That’s great to hear! What do you work on?" },
                    new ConversationMessage { Message = "Mostly marketing strategies and social media campaigns." },
                    new ConversationMessage { Message = "That sounds interesting!" },
                    new ConversationMessage { Message = "It is! A lot to learn but very rewarding." },
                    new ConversationMessage { Message = "We should catch up more often like this." },
                    new ConversationMessage { Message = "Yes, it’s always fun to hang out and talk." },
                    new ConversationMessage { Message = "Agreed! We should make it a weekly thing." },
                    new ConversationMessage { Message = "Yeah, a Saturday coffee meetup sounds perfect." },
                    new ConversationMessage { Message = "Okay, let’s make it official!" },
                    new ConversationMessage { Message = "See you all at the cafe on Saturday then." },
                    new ConversationMessage { Message = "Looking forward to it!" },
                    new ConversationMessage { Message = "Same here!" },
                    new ConversationMessage { Message = "Alright, I need to head home now." },
                    new ConversationMessage { Message = "Yeah, me too. It’s getting late." },
                    new ConversationMessage { Message = "Alright, good night everyone!" },
                    new ConversationMessage { Message = "Good night!" },
                    new ConversationMessage { Message = "See you all soon!" },
                },
                new()
                {
                    new ConversationMessage { Message = "Alright team, are we ready for the expedition?" },
                    new ConversationMessage { Message = "Almost, just need to check our supplies." },
                    new ConversationMessage { Message = "We should make sure we have enough food and water." },
                    new ConversationMessage { Message = "And don’t forget the torches!" },
                    new ConversationMessage { Message = "Good point. The caves will be dark." },
                    new ConversationMessage { Message = "How long do we expect to be out there?" },
                    new ConversationMessage { Message = "Maybe three or four days, depending on the terrain." },
                    new ConversationMessage { Message = "We need to pack accordingly then." },
                    new ConversationMessage { Message = "I’ve got the maps ready." },
                    new ConversationMessage { Message = "Great! Are we following the old trail?" },
                    new ConversationMessage
                        { Message = "Yes, but we might need to take a detour if the path is blocked." },
                    new ConversationMessage { Message = "We should have a backup route planned." },
                    new ConversationMessage { Message = "Agreed. Also, do we have enough rope?" },
                    new ConversationMessage { Message = "Yes, I packed extra in case we need to climb." },
                    new ConversationMessage { Message = "What about weapons? Just in case?" },
                    new ConversationMessage { Message = "I’ve got my bow, and Sam has his sword." },
                    new ConversationMessage { Message = "I’ll bring a dagger too, for emergencies." },
                    new ConversationMessage { Message = "We should move in pairs to be safe." },
                    new ConversationMessage { Message = "That’s a good idea." },
                    new ConversationMessage { Message = "Who’s taking the lead?" },
                    new ConversationMessage { Message = "I can go first since I know the path well." },
                    new ConversationMessage { Message = "Okay, I’ll take the rear." },
                    new ConversationMessage { Message = "Let’s try to avoid unnecessary fights." },
                    new ConversationMessage { Message = "Yeah, our goal is exploration, not combat." },
                    new ConversationMessage { Message = "But we should still be prepared for anything." },
                    new ConversationMessage { Message = "Absolutely. We don’t know what’s out there." },
                    new ConversationMessage { Message = "We should get some rest before heading out." },
                    new ConversationMessage { Message = "Agreed, we need to be well-rested for the journey." },
                    new ConversationMessage { Message = "What time are we leaving?" },
                    new ConversationMessage { Message = "At sunrise, to make the most of daylight." },
                    new ConversationMessage { Message = "Then let’s meet at the town gate at dawn." },
                    new ConversationMessage { Message = "Alright, see you all in the morning." },
                    new ConversationMessage { Message = "This is going to be an adventure to remember." },
                },
                
            };
        }
    }
}