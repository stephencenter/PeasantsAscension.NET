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
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public static class NPCManager
    {
        private static readonly List<NPC> npc_list = new List<NPC>()
        {
            new NPC("Solou", "Page", true, "nearton_solou", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() { "solou_convo_a", "solou_convo_b", "solou_convo_c", "solou_convo_d", "solou_quest_a", "solou_convo_e" } },
                { 1, new List<string>() { "solou_convo_f" } }
            }),

            new NPC("Philliard", "Scribe", true, "nearton_philliard", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() { "philliard_convo_a" } },
                { 1, new List<string>() { "philliard_convo_b", "philliard_convo_c", "philliard_convo_d", } }
            }),

            new NPC("Joseph", "Mayor of Overshire", true, "overshire_joseph", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() { "joseph_convo_a", "joseph_quest_a", "joseph_convo_b", "joseph_convo_c" } }
            }),

            new NPC("Orius", "Mayor of Valice", true, "valice_orius", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() }
            }),

            new NPC("Azura", "Sorcerer's Guildmaster", true, "parceon_azura", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() { "azura_convo_a", "azura_convo_b", "azura_convo_c" } }
            }),

            new NPC("Raidon", "Village Shaman", true, "ambercreek_raidon", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() { "raidon_convo_a" } }
            }),

            new NPC("Stewson", "Captain of the Guard", true, "sardooth_stewson", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() { "stewson_convo_a", "stewson_convo_b", "stewson_convo_c", "stewson_quest_a" } }
            }),

            new NPC("Seriph", "Blacksmith", true, "fortsigil_seriph", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() { "seriph_convo_a", "seriph_convo_b", "seriph_convo_c" } }
            }),

            new NPC("Rivesh", "Village Elder", true, "fortsigil_rivesh", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() { "rivesh_convo_a", "rivesh_convo_b", "rivesh_convo_c", "rivesh_convo_d", "rivesh_quest_a" } }
            }),

            new NPC("Alfred", "Cobbler", true, "nearton_alfred", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() { "alfred_convo_a", "alfred_convo_b", "alfred_convo_c", "alfred_quest_a", "alfred_convo_d" } }
            }),

            new NPC("Koric", "Village Elder", true, "tripton_koric", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() { "kyle_convo_a", "kyle_convo_b", "kyle_convo_c", "kyle_convo_d" } }
            }),

            new NPC("Keric", "Village Elder", true, "fallville_keric", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() { "krystin_convo_a", "krystin_convo_b", "krystin_convo_c", "krystin_convo_d" } }
            }),

            new NPC("Frederick", "Scholar", true, "fallville_frederick", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() { "frederick_convo_a", "frederick_convo_b", "frederick_convo_c" } }
            }),

            new NPC("Alden", "Sage", true, "tripton_alden", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() { "alden_quest_a", "alden_convo_a", "alden_convo_b", "alden_convo_c" } }
            }),

            new NPC("Polmor", "Scientist", true, "whistumn_polmor", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() { "polmor_convo_a", "polmor_quest_a", "polmor_convo_b" } }
            }),

            new NPC("Serena", "Scientist", true, "whistumn_polmor", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() { "serena_convo_a", "serena_convo_b", "serena_convo_c" } }
            }),

            new NPC("Matthew", "Matt", true, "lantonum_matthew", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() { "matthew_convo_a", "matthew_quest_a", "matthew_convo_b", "matthew_convo_c", "matthew_convo_d", "matthew_convo_e" } }
            }),

            new NPC("Pime", "Vampire Shaman", true, "sanguion_pime", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() { "pime_convo_a", "pime_convo_b", "pime_quest_a", "pime_convo_c" } }
            }),

            new NPC("Wesley", "Peasant", true, "nearton_wesley", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() { "wesley_convo_a" } }
            }),

            new NPC("Saar", "Bard", true, "southford_saar", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() { "saar_convo_a" } }               
            }),

            new NPC("Lazaro", "Oracle", true, "southford_lazaro", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() { "lazaro_convo_a" } }
            }),

            new NPC("Jeffery", "Traveler", true, "overshire_jeffery", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() { "jeffery_convo_a" } }                  
            }),

            new NPC("Harthos", "Lumberjack", true, "overshire_harthos", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() { "harthos_convo_a" } }
            }),

            new NPC("Sondalar", "Goods Peddler", true, "overshire_sondalar", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() { "sondalar_convo_a" } }
            }),
        
            new NPC("Sareka", "Head of the Royal Guard", true, "principalia_sereka", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() { "sakura_convo_a" } }
            }),

            new NPC("Ethos", "Courier", true, "valice_ethos", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() { "ethos_convo_a" } }
            }),

            new NPC("Typhen", "Novice Cleric", true, "valice_typhen", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() { "typhen_convo_a" } }
            }),

            new NPC("Fly", "Duke of Celemia", true, "parvoc_fly", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() { "fly_convo_a" } }
            }),

            new NPC("Stravi", "Duchess of Celemia", true, "parvoc_stravi", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() { "stravi_convo_a" } }
            }),

            new NPC("Caesar", "Royal Servant", true, "parvoc_caesar", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() { "caesar_convo_a" } }
            }),

            new NPC("Pedric", "Druid", true,  "ravenstone_pedric", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() { "strathius_convo_a" } }
            }),

            new NPC("Sugulat", "Duke of Chin'tor", true, "ambercreek_sugulat", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() { "sugulat_convo_a" } }
            }),

            new NPC("Morrison", "Engineer", true, "cesura_morrison", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() { "morrison_convo_a" } }                   
            }),

            new NPC("Ariver", "Vampire", true, "sanguion_ariver", new Dictionary<int, List<string>>()
            {
                { 0, new List<string>() { "ariver_convo_a" }}
            }),

            new NPC("Fitzgerald", "Raving Alcoholic", true, "fitzgerald_valenfall", new Dictionary<int, List<string>>() 
            {
                { 0, new List<string>() { "fitz_convo_a" } }
            })
        };

        public static List<NPC> GetNPCList()
        {
            return npc_list;
        }

        public static NPC FindNPCWithID(string npc_id)
        {
            return npc_list.Single(x => x.NPCID == npc_id);
        }

        public static void UpdateConvoState(string npc_id, int new_state)
        {
            NPC the_npc = FindNPCWithID(npc_id);

            if (the_npc.Conversations.ContainsKey(new_state))
            {
                the_npc.ConvoState = new_state;
                return;
            }

            throw new ArgumentException($"{new_state} is not a valid conversation state for {the_npc.NPCID}.");
        }
    }

    public class NPC
    {
        public string NPCName { get; set; }
        public string Occupation { get; set; }
        public bool Active { get; set; }
        public string NPCID { get; set; }
        public Dictionary<int, List<string>> Conversations { get; set; }

        public int ConvoState { get; set; }

        public void Speak()
        {
            // Print the NPC's dialogue to the player
            Console.WriteLine($"{NPCName}, the {Occupation}: ");

            List<Conversation> convo_list = Conversations[ConvoState].Select(DialogueManager.FindConvoWithID).ToList();

            foreach (Conversation convo in convo_list)
            {
                string name_inserted = convo.Dialogue.Replace(Conversation.TEXT_TO_REPLACE, UnitManager.player.UnitName);
                List<string> sentences = CMethods.SplitBy79(name_inserted);

                if (convo is Quest quest)
                {
                    if (quest.Completed)
                    {
                        if (!quest.TurnedIn)
                        {
                            quest.QuestComplete();
                        }

                        continue;
                    }

                    foreach (string sentence in sentences)
                    {
                        CMethods.PressAnyKeyToContinue(sentence);
                    }

                    if (!quest.Started)
                    {
                        if (quest.ForceAccept)
                        {
                            quest.GiveQuestForced();
                        }

                        else
                        {
                            quest.GiveQuestOption();
                        }
                    }
                }

                else
                {
                    foreach (string sentence in sentences)
                    {
                        CMethods.PressAnyKeyToContinue(sentence);
                    }
                }

                if (convo != convo_list.Last() && !(convo is Quest))
                {
                    Console.WriteLine();
                }

                convo.AfterTalking();
            }

            CMethods.PrintDivider();
        }

        public NPC(string name, string job, bool active, string npc_id, Dictionary<int, List<string>> convos)
        {
            NPCName = name;
            Occupation = job;
            Active = active;
            NPCID = npc_id;
            Conversations = convos;
            ConvoState = 0;
        }
    }
}
