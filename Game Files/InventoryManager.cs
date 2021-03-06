﻿/* This file is part of Peasant's Ascension.
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
    public static class InventoryManager
    {
        private readonly static Dictionary<CEnums.InvCategory, List<string>> inventory = new Dictionary<CEnums.InvCategory, List<string>>()
        {
            { CEnums.InvCategory.quest, new List<string>() { } },
            { CEnums.InvCategory.consumables, new List<string>() { "s_potion", "s_elixir" } },
            { CEnums.InvCategory.weapons, new List<string>() { } },
            { CEnums.InvCategory.armor, new List<string>() { } },
            { CEnums.InvCategory.tools, new List<string>() { } },
            { CEnums.InvCategory.accessories, new List<string>() { } },
            { CEnums.InvCategory.misc, new List<string>() { } }
        };

        private static Dictionary<string, Dictionary<CEnums.EquipmentType, string>> equipment = new Dictionary<string, Dictionary<CEnums.EquipmentType, string>>()
        {
             {
                "_player", new Dictionary<CEnums.EquipmentType, string>()
                 {
                     { CEnums.EquipmentType.weapon, "weapon_fists" },
                     { CEnums.EquipmentType.armor, "no_armor" },
                     { CEnums.EquipmentType.elem_accessory, "no_elem_access" },
                     { CEnums.EquipmentType.ammunition, "no_ammunition" }
                 }
             },

             {
                "_solou", new Dictionary<CEnums.EquipmentType, string>()
                 {
                     { CEnums.EquipmentType.weapon, "weapon_fists" },
                     { CEnums.EquipmentType.armor, "no_armor" },
                     { CEnums.EquipmentType.elem_accessory, "no_elem_access" },
                     { CEnums.EquipmentType.ammunition, "no_ammunition" }
                 }
             },

             {
                "_chili", new Dictionary<CEnums.EquipmentType, string>()
                 {
                     { CEnums.EquipmentType.weapon, "weapon_fists" },
                     { CEnums.EquipmentType.armor, "no_armor" },
                     { CEnums.EquipmentType.elem_accessory, "no_elem_access" },
                     { CEnums.EquipmentType.ammunition, "no_ammunition" }
                 }
             },

             {
                "_chyme", new Dictionary<CEnums.EquipmentType, string>()
                 {
                     { CEnums.EquipmentType.weapon, "weapon_fists" },
                     { CEnums.EquipmentType.armor, "no_armor" },
                     { CEnums.EquipmentType.elem_accessory, "no_elem_access" },
                     { CEnums.EquipmentType.ammunition, "no_ammunition" }
                 }
             },

             {
                "_storm", new Dictionary<CEnums.EquipmentType, string>()
                 {
                     { CEnums.EquipmentType.weapon, "weapon_fists" },
                     { CEnums.EquipmentType.armor, "no_armor" },
                     { CEnums.EquipmentType.elem_accessory, "no_elem_access" },
                     { CEnums.EquipmentType.ammunition, "no_ammunition" }
                 }
             },

             {
                "_tavlosk", new Dictionary<CEnums.EquipmentType, string>()
                 {
                     { CEnums.EquipmentType.weapon, "weapon_fists" },
                     { CEnums.EquipmentType.armor, "no_armor" },
                     { CEnums.EquipmentType.elem_accessory, "no_elem_access" },
                     { CEnums.EquipmentType.ammunition, "no_ammunition" }
                 }
             },

             {
                "_adorine", new Dictionary<CEnums.EquipmentType, string>()
                 {
                     { CEnums.EquipmentType.weapon, "weapon_fists" },
                     { CEnums.EquipmentType.armor, "no_armor" },
                     { CEnums.EquipmentType.elem_accessory, "no_elem_access" },
                     { CEnums.EquipmentType.ammunition, "no_ammunition" }
                 }
             },

             {
                "_kaltoh", new Dictionary<CEnums.EquipmentType, string>()
                 {
                     { CEnums.EquipmentType.weapon, "weapon_fists" },
                     { CEnums.EquipmentType.armor, "no_armor" },
                     { CEnums.EquipmentType.elem_accessory, "no_elem_access" },
                     { CEnums.EquipmentType.ammunition, "no_ammunition" }
                 }
             },
        };

        private readonly static Dictionary<CEnums.EquipmentType, string> default_equip_map = new Dictionary<CEnums.EquipmentType, string>()
        {
            { CEnums.EquipmentType.weapon, "weapon_fists" },
            { CEnums.EquipmentType.armor, "no_armor" },
            { CEnums.EquipmentType.elem_accessory, "no_elem_access" },
            { CEnums.EquipmentType.ammunition, "no_ammunition" }
        };

        /* =========================== *
         *      COLLECTION GETTERS     *
         * =========================== */
        #region
        public static Dictionary<CEnums.InvCategory, List<string>> GetInventoryRaw()
        {
            return inventory;
        }

        public static Dictionary<CEnums.InvCategory, List<Item>> GetInventoryItems()
        {
            // We have to convert the inventory from a list of ItemIDs into a list of Items.
            // Storing only the ItemIDs instead of the full items makes it much simpler to
            // serialize and deserialize the inventory when saving.
            Dictionary<CEnums.InvCategory, List<Item>> new_inventory = new Dictionary<CEnums.InvCategory, List<Item>>();

            foreach (KeyValuePair<CEnums.InvCategory, List<string>> kvp in inventory)
            {
                new_inventory[kvp.Key] = kvp.Value.Select(ItemManager.FindItemWithID).ToList();
            }

            return new_inventory;
        }

        public static Dictionary<string, Dictionary<CEnums.EquipmentType, string>> GetEquipmentRaw()
        {
            return equipment;
        }

        public static Dictionary<string, Dictionary<CEnums.EquipmentType, Equipment>> GetEquipmentItems()
        {
            // We have to convert the inventory from a list of ItemIDs into a list of Items.
            // Storing only the ItemIDs instead of the full items makes it much simpler to
            // serialize and deserialize the inventory when saving.
            Dictionary<string, Dictionary<CEnums.EquipmentType, Equipment>> new_equipment = new Dictionary<string, Dictionary<CEnums.EquipmentType, Equipment>>();

            foreach (KeyValuePair<string, Dictionary<CEnums.EquipmentType, string>> kvp in equipment)
            {
                new_equipment[kvp.Key] = new Dictionary<CEnums.EquipmentType, Equipment>();

                foreach (KeyValuePair<CEnums.EquipmentType, string> kvp2 in kvp.Value)
                {
                    new_equipment[kvp.Key][kvp2.Key] = ItemManager.FindItemWithID(kvp2.Value) as Equipment;
                }
            }

            return new_equipment;
        }
        #endregion

        /* =========================== *
         *      COLLECTION SETTERS     *
         * =========================== */
        #region
        public static void AddItemToInventory(string item_id)
        {
            // Adds the item_id to the inventory
            CEnums.InvCategory item_cat = ItemManager.FindItemWithID(item_id).Category;
            inventory[item_cat].Add(item_id);
        }

        public static void RemoveItemFromInventory(string item_id)
        {
            // Removes the item_id from the inventory
            CEnums.InvCategory item_cat = ItemManager.FindItemWithID(item_id).Category;
            inventory[item_cat].Remove(item_id);
        }

        public static void EquipItem(PlayableCharacter equipper, string item_id)
        {
            if (!(ItemManager.FindItemWithID(item_id) is Equipment))
            {
                ExceptionLogger.LogException($"Tried to equip {item_id}, which is not an equipment", new InvalidOperationException());
                return;
            }

            // Equips the item_id to equipper
            CEnums.EquipmentType equip_type = (ItemManager.FindItemWithID(item_id) as Equipment).EquipType;

            if (GetEquipmentItems()[equipper.PlayerID][equip_type].ItemID != default_equip_map[equip_type])
            {
                AddItemToInventory(GetEquipmentItems()[equipper.PlayerID][equip_type].ItemID);
            }

            equipment[equipper.PlayerID][equip_type] = item_id;
            RemoveItemFromInventory(item_id);
        }

        public static void UnequipItem(PlayableCharacter unequipper, string item_id)
        {
            if (!(ItemManager.FindItemWithID(item_id) is Equipment))
            {
                ExceptionLogger.LogException($"Tried to unequip {item_id}, which is not an equipment", new InvalidOperationException());
                return;
            }

            // Unequips the item_id from the unequipper
            CEnums.EquipmentType equip_type = (ItemManager.FindItemWithID(item_id) as Equipment).EquipType;
            equipment[unequipper.PlayerID][equip_type] = default_equip_map[equip_type];
            AddItemToInventory(item_id);
        }

        public static void UpdateInventoryFromSave(Dictionary<CEnums.InvCategory, List<string>> saved_inventory)
        {
            // We manually add each item to the inventory just in case
            // one of the items changed categories (e.g. from a misc item to a quest item)
            foreach (List<string> category in inventory.Values)
            {
                category.Clear();
            }

            foreach (string item_id in saved_inventory.Values.SelectMany(x => x))
            {
                AddItemToInventory(item_id);
            }
        }

        public static void UpdateEquipmentFromSave(Dictionary<string, Dictionary<CEnums.EquipmentType, string>> saved_equipment)
        {
            equipment = saved_equipment;
        }
        #endregion

        /* =========================== *
         *      INVENTORY SYSTEM       *
         * =========================== */
        #region
        public static void PickInventoryCategory()
        {
            CMethods.PrintDivider();
            while (true)
            {
                Console.WriteLine("Your Inventory: ");
                Console.WriteLine("      [1] Armor");
                Console.WriteLine("      [2] Weapons");
                Console.WriteLine("      [3] Accessories");
                Console.WriteLine("      [4] Consumables");
                Console.WriteLine("      [5] Tools");
                Console.WriteLine("      [6] Quest Items");
                Console.WriteLine("      [7] Miscellaneous");

                while (true)
                {
                    string chosen = CMethods.SingleCharInput("Input [#] (or type 'exit'): ").ToLower();
                    CEnums.InvCategory category;

                    if (chosen.IsExitString())
                    {
                        CMethods.PrintDivider();
                        return;
                    }

                    else if (chosen == "1")
                    {
                        category = CEnums.InvCategory.armor;
                    }

                    else if (chosen == "2")
                    {
                        category = CEnums.InvCategory.weapons;
                    }

                    else if (chosen == "3")
                    {
                        category = CEnums.InvCategory.accessories;
                    }

                    else if (chosen == "4")
                    {
                        category = CEnums.InvCategory.consumables;
                    }

                    else if (chosen == "5")
                    {
                        category = CEnums.InvCategory.tools;
                    }

                    else if (chosen == "6")
                    {
                        category = CEnums.InvCategory.quest;
                    }

                    else if (chosen == "7")
                    {
                        category = CEnums.InvCategory.misc;
                    }

                    else
                    {
                        continue;
                    }

                    if (GetInventoryItems()[category].Count > 0)
                    {
                        PickInventoryItem(category, false);
                        break;
                    }

                    else
                    {
                        CMethods.PrintDivider();
                        Console.WriteLine($"Your party has no {category.EnumToString()}");
                        CMethods.PressAnyKeyToContinue();
                        CMethods.PrintDivider();
                        break;
                    }
                }
            }
        }

        public static void PickInventoryItem(CEnums.InvCategory category, bool selling)
        {
            // Select an object to interact with in your inventory
            // If "selling == True" that means that items are being sold, and not used.

            while (true)
            {
                CMethods.PrintDivider();
                List<string> item_ids = DisplayInventory(category, selling);

                while (true)
                {
                    string chosen = CMethods.FlexibleInput("Input [#] (or type 'exit'): ", item_ids.Count).ToLower();

                    try
                    {
                        chosen = item_ids[int.Parse(chosen) - 1];
                    }

                    catch (Exception ex) when (ex is FormatException || ex is ArgumentOutOfRangeException)
                    {
                        if (chosen.IsExitString())
                        {
                            CMethods.PrintDivider();
                            return;
                        }

                        continue;
                    }

                    // If you're selling items at a general store, you have to call a different function
                    if (selling)
                    {
                        SellItem(chosen);

                        if (GetInventoryItems()[category].All(x => x.IsImportant))
                        {
                            return;
                        }
                    }

                    else
                    {
                        PickInventoryAction(chosen);

                        if (GetInventoryRaw()[category].Count == 0)
                        {
                            CMethods.PrintDivider();
                            return;
                        }
                    }

                    break;
                }
            }
        }

        public static List<string> DisplayInventory(CEnums.InvCategory category, bool selling)
        {
            List<string> id_inventory = GetInventoryItems()[category].Select(x => x.ItemID).ToList();
            List<Tuple<string, string, int>> quantity_inv = new List<Tuple<string, string, int>>();

            foreach (string item_id in id_inventory.Distinct())
            {
                string item_name = ItemManager.FindItemWithID(item_id).ItemName;
                int item_count = id_inventory.Count(x => x == item_id);
                if (!selling || !ItemManager.FindItemWithID(item_id).IsImportant)
                {
                    quantity_inv.Add(new Tuple<string, string, int>(item_name, item_id, item_count));
                }
            }

            if (selling)
            {
                int padding;
                try
                {
                    padding = quantity_inv.Max(x => $"{x.Item1} x {x.Item3}".Length);
                } 

                catch (ArgumentException)
                {
                    padding = 1;
                }

                Console.WriteLine($"{category.EnumToString()}: ");

                int highest_charisma = UnitManager.GetActivePCUs().Max(x => x.Attributes[CEnums.PlayerAttribute.charisma]);

                int counter = 0;
                foreach (Tuple<string, string, int> item in quantity_inv)
                {
                    Item true_item = ItemManager.FindItemWithID(item.Item2);

                    int sell_value = true_item.Value/5;
                    int modified_value = Math.Max((int)(sell_value*(1 + (0.01*highest_charisma))), sell_value*2);
                    int full_padding = padding - ($"{item.Item1} x {item.Item3}".Length);
                    string dashes = new string('-', full_padding);

                    Console.WriteLine($"      [{counter + 1}] {item.Item1} x {item.Item3} {dashes}--> {modified_value} GP each");
                    counter++;
                }
            }

            else
            {
                Console.WriteLine($"{category.EnumToString()}: ");

                int counter = 0;
                foreach (Tuple<string, string, int> item in quantity_inv)
                {
                    Console.WriteLine($"      [{counter + 1}] {item.Item1} x {item.Item3}");
                    counter++;
                }
            }

            return quantity_inv.Select(x => x.Item2).ToList();
        }

        public static void PickInventoryAction(string item_id)
        {
            Item this_item = ItemManager.FindItemWithID(item_id);
            CMethods.PrintDivider();

            // Loop while the item is in the inventory
            while (true)
            {
                string action;
                if (this_item is Equipment)
                {
                    // You equip weapons/armor/accessories
                    action = "Equip";
                }

                else
                {
                    // You use other items
                    action = "Use";
                }

                Console.WriteLine($"What should your party do with the {this_item.ItemName}? ");
                Console.WriteLine($"      [1] {action}");
                Console.WriteLine("      [2] Read Description");
                Console.WriteLine("      [3] Drop");

                while (true)
                {
                    string chosen = CMethods.SingleCharInput("Input [#] (or type 'exit'): ").ToLower();


                    if (chosen.IsExitString())
                    {
                        return;
                    }

                    else if (chosen == "1")
                    {
                        // Items of these classes require a target to be used, so we have to acquire a target first
                        if (this_item is Equipment equipment)
                        {
                            if (ItemManager.EquipmentTargetMenu(UnitManager.player, equipment))
                            {
                                CMethods.PrintDivider();
                                equipment.UseItem(UnitManager.player);
                            }
                        }

                        else if (this_item is Consumable consumable)
                        {
                            if (ItemManager.ConsumableTargetMenu(UnitManager.player, consumable))
                            {
                                CMethods.PrintDivider();
                                consumable.UseItem(UnitManager.player);
                            }
                        }

                        // Other items can just be used normally
                        else
                        {
                            CMethods.PrintDivider();
                            this_item.UseItem(UnitManager.player);
                        }

                        return;
                    }

                    else if (chosen == "2")
                    {
                        // Display the item description
                        CMethods.PrintDivider();
                        Console.WriteLine($"Description for '{this_item.ItemName}': \n");
                        Console.WriteLine(this_item.Description);
                        CMethods.PressAnyKeyToContinue();
                        CMethods.PrintDivider();

                        break;
                    }

                    else if (chosen == "3")
                    {
                        CMethods.PrintDivider();

                        // You can't throw away important/essential items, such as tools and quest items.
                        // This is to prevent the game from becoming unwinnable.
                        if (this_item.IsImportant)
                        {
                            Console.WriteLine("Essential items cannot be thrown away.");
                            CMethods.PressAnyKeyToContinue();
                            return;
                        }

                        else
                        {
                            while (true)
                            {
                                string yes_or_no = CMethods.SingleCharInput($"Throw away the {this_item.ItemName}? ").ToLower();

                                if (yes_or_no.IsYesString())
                                {
                                    RemoveItemFromInventory(this_item.ItemID);
                                    Console.WriteLine($"You toss the {this_item.ItemName} aside and continue on your journey.");
                                    CMethods.PressAnyKeyToContinue();

                                    return;
                                }

                                else if (yes_or_no.IsNoString())
                                {
                                    Console.WriteLine($"You decide to keep the {this_item.ItemName}.");
                                    CMethods.PressAnyKeyToContinue();

                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void SellItem(string item_id)
        {
            // Trade player-owned objects for money (GP)
            Item item = ItemManager.FindItemWithID(item_id);
            CMethods.PrintDivider();

            foreach (string line in CMethods.SplitBy79(item.Description))
            {
                Console.WriteLine(line);
            }

            CMethods.PrintDivider();

            int highest_charisma = UnitManager.GetActivePCUs().Max(x => x.Attributes[CEnums.PlayerAttribute.charisma]);
            int modified_value = Math.Max((int)(item.Value * (1 + (0.01 * highest_charisma))), item.Value * 2);

            while (true)
            {
                string yes_no = CMethods.SingleCharInput($"Sell the {item.ItemName} for {modified_value} GP? ").ToLower();

                if (yes_no.IsYesString())
                {
                    RemoveItemFromInventory(item.ItemID);
                    CInfo.GP += modified_value;
                    Console.WriteLine($"The shopkeeper takes the {item.ItemName} and gives you {modified_value} GP.");
                    CMethods.PressAnyKeyToContinue();

                    return;
                }

                else if (yes_no.IsNoString())
                {
                    return;
                }
            }
        }

        public static void PickEquipmentItem()
        {
            TargetMapping t_map = new TargetMapping(true, true, false, true);
            if (!UnitManager.player.PlayerChooseTarget("Choose party member to view equipment for: ", t_map))
            {
                return;
            }

            CMethods.PrintDivider();

            while (true)
            {
                PlayableCharacter equipper = UnitManager.player.CurrentTarget as PlayableCharacter;
                Dictionary <CEnums.EquipmentType, Equipment> player_equipment = GetEquipmentItems()[equipper.PlayerID];

                Console.WriteLine($"{equipper.UnitName}'s Equipped Items:");
                Console.WriteLine($"      [1] Weapon------> {player_equipment[CEnums.EquipmentType.weapon].ItemName}");
                Console.WriteLine($"      [2] Armor ------> {player_equipment[CEnums.EquipmentType.armor].ItemName}");
                Console.WriteLine($"      [3] Accessory --> {player_equipment[CEnums.EquipmentType.elem_accessory].ItemName}");

                if (equipper.PClass == CEnums.CharacterClass.ranger)
                {
                    Console.WriteLine($"      [4] Ammunition -> {player_equipment[CEnums.EquipmentType.ammunition].ItemName}");
                }

                while (true)
                {
                    string selected = CMethods.SingleCharInput("Input [#] (or type 'back'): ").ToLower();
                    Equipment item;

                    if (selected.IsExitString())
                    {
                        CMethods.PrintDivider();
                        return;
                    }

                    else if (selected == "1")
                    {
                        item = player_equipment[CEnums.EquipmentType.weapon];
                    }

                    else if (selected == "2")
                    {
                        item = player_equipment[CEnums.EquipmentType.armor];
                    }

                    else if (selected == "3")
                    {
                        item = player_equipment[CEnums.EquipmentType.elem_accessory];
                    }

                    else if (selected == "4" && equipper.PClass == CEnums.CharacterClass.ranger)
                    {
                        item = player_equipment[CEnums.EquipmentType.ammunition];
                    }

                    else
                    {
                        continue;
                    }

                    if (item.ItemID == default_equip_map[item.EquipType])
                    {
                        CMethods.PrintDivider();
                        Console.WriteLine($"{equipper.UnitName} doesn't have anything equipped in that slot.");
                        CMethods.PressAnyKeyToContinue();
                        CMethods.PrintDivider();

                        break;
                    }

                    Console.WriteLine($"{item.ItemID}, {default_equip_map[item.EquipType]}");

                    CMethods.PrintDivider();
                    PickEquipmentAction(item, equipper);
                    CMethods.PrintDivider();

                    break;
                }
            }
        }

        public static void PickEquipmentAction(Equipment item, PlayableCharacter equipper)
        {
            while (true)
            {
                Console.WriteLine($"What should {equipper.UnitName} do with their {item.ItemName}?");
                Console.WriteLine("      [1] Unequip");
                Console.WriteLine("      [2] Read Description");

                while (true)
                {
                    string action = CMethods.SingleCharInput("Input [#] (or type 'back'): ").ToLower();

                    if (action.IsExitString())
                    {
                        return;
                    }

                    else if (action == "1")
                    {
                        UnequipItem(equipper, item.ItemID);

                        CMethods.PrintDivider();
                        Console.WriteLine($"{equipper.UnitName} unequips the {item.ItemName}.");
                        CMethods.PressAnyKeyToContinue();

                        return;
                    }

                    else if (action == "2")
                    {
                        CMethods.PrintDivider();
                        Console.WriteLine(item.Description);
                        CMethods.PressAnyKeyToContinue();
                        CMethods.PrintDivider();

                        break;
                    }
                }
            }
        }

        public static void ViewQuests()
        {
            /*
            print('-'*save_load.divider_size)
            while True:
                fizz = True
                choice = main.s_input('View [f]inished or [a]ctive quests? | Input [Letter] (or type "back"): ').ToLower()

                print('-'*save_load.divider_size)
                if choice.startswith('f'):  // Finished Quests
                    dia_ = [x for x in dialogue.all_dialogue if isinstance(x, dialogue.Quest) and x.finished]

                else if choice.startswith('a'):
                            dia_ = [x for x in dialogue.all_dialogue if isinstance(x, dialogue.Quest) and not x.finished and x.started]

                else if choice in ['e', 'x', 'exit', 'b', 'back']:
                            return

                        else:
                            continue

                        if dia_:
                            while fizz:
                                if choice.startswith("f"):
                                    print("Finished:")

                                else:
                                    print("Active:")

                                for num, x in enumerate(dia_) :
                                    print(f'      [{num + 1}] {x.name}')

                                while True:
                                    quest = main.s_input('Input [#] (or type "back"): ').ToLower()

                                    try:
                                        quest = dia_[int(quest) - 1]

                                    except(IndexError, ValueError):
                                        if quest in ['e', 'x', 'exit', 'b', 'back']:
                                            fizz = False  // Break the loop twice
                                            break

                                        continue

                                    print('-'*save_load.divider_size)
                                    print(f"QUEST NAME: {quest.name}")
                                    print(f"GIVEN BY: {quest.q_giver}")

                                    for x in main.chop_by_79(quest.dialogue):
                                        print(x)

                                    main.s_input("\nPress enter/return ")
                                    print('-'*save_load.divider_size)

                                    break

                            print('-'*save_load.divider_size)

                        else:
                            print(f'Your party has no {"active" if choice.startswith("a") else "finished"} quests!')
                            main.s_input('\nPress enter/return ')
                            print('-'*save_load.divider_size) */
        }
        #endregion
    }
}
