/* This file is part of Peasant's Ascension.
 * 
 * Peasant's Ascension is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * Peasant's Ascension is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with Peasant's Ascension.  If not, see <http://www.gnu.org/licenses/>. */

using System;
using System.Linq;
using System.Collections.Generic;

namespace Game
{
    public static class DialogueManager
    {
        private static readonly List<Conversation> convo_list = new List<Conversation>()
        {

        };

        public static List<Conversation> GetConvoList()
        {
            return convo_list;
        }

        public static Conversation FindConvoWithID(string convo_id)
        {
            return convo_list.Single(x => x.ConvoID == convo_id);
        }
    }

    public abstract class Conversation
    {
        public List<string> Sentences { get; set; }
        public string ConvoID { get; set; }

        public abstract void AfterTalking();

        protected Conversation(string dialogue, string convo_id)
        {
            Sentences = CMethods.SplitBy79(dialogue);
            ConvoID = convo_id;
        }
    }

    public abstract class Quest : Conversation
    {
        public string QuestName { get; set; } // The name of the quest
        public string QuestGiver { get; set; }  // The name of the person who gave you the quest
        public string AcceptMessage { get; set; }  // The text that's displayed if you accept the quest
        public Tuple<int, int> Reward { get; set; } // A Tuple [GP, XP] of your reward for the quest
        public bool Started { get; set; }  // Whether you have accepted the quest yet
        public bool Completed { get; set; }  // Whether you have completed the requirements for finishing the quest
        public bool TurnedIn { get; set; }  // Whether you have accepted the reward for completing the quest yet

        public void GiveQuest()
        {
            CMethods.PrintDivider();
            Console.WriteLine($"{QuestGiver} is offering you the quest '{QuestName}'.");

            while (true)
            {
                string yes_no = CMethods.SingleCharInput("Do you accept this quest? ").ToLower();

                if (yes_no.IsYesString())
                {
                    CMethods.PrintDivider();
                    Console.WriteLine($"{QuestGiver}: \"{AcceptMessage}\"");
                    CMethods.PressAnyKeyToContinue();
                    Started = true;
                    OnStarting();

                    return;
                }

                else if (yes_no.IsNoString())
                {
                    return;
                }
            }
        }

        public abstract void OnStarting();

        public void QuestComplete()
        {
            OnCompletion();

            Console.WriteLine("Quest Complete!");
            Console.WriteLine($"You've received {Reward.Item1} GP and {Reward.Item2} XP for completing this quest.");
            CMethods.PressAnyKeyToContinue();

            CInfo.GP += Reward.Item1;
            
            foreach (PlayableCharacter pcu in UnitManager.GetAllPCUs())
            {
                pcu.CurrentXP += Reward.Item2;
                pcu.PlayerLevelUp();
            }

            TurnedIn = true;
        }

        public abstract void OnCompletion();
        
        protected Quest(string dialogue, string convo_id) : base(dialogue, convo_id)
        {
            Started = false;
            Completed = false;
        }
    }
}
