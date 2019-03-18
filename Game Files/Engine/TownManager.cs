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

namespace Engine
{
    public static class TownManager
    {
        private static readonly List<Town> town_list = new List<Town>()
        {
            new NeartonTown()
        };

        public static List<Town> GetTownList()
        {
            return town_list;
        }

        public static Town FindTownWithID(string town_id)
        {
            return GetTownList().Single(x => x.TownID == town_id);
        }

        public static bool SearchForTowns(bool enter = true)
        {
            if (TileManager.FindTileWithID(CInfo.CurrentTile).TownList.Count == 0)
            {
                return false;
            }

            foreach (string town_id in TileManager.FindTileWithID(CInfo.CurrentTile).TownList)
            {
                Town town = FindTownWithID(town_id);
                CMethods.PrintDivider();

                while (true)
                {
                    string yes_no = CMethods.SingleCharInput($"The town of {town.TownName} is nearby. Enter? [Y]es or [N]o: ");

                    if (yes_no.IsYesString())
                    {
                        CInfo.RespawnTile = CInfo.CurrentTile;
                        town.TownChoice();
                        return true;
                    }

                    else if (yes_no.IsNoString())
                    {
                        CMethods.PrintDivider();
                        break;
                    }
                }
            }

            return true;
        }
    }

    /* =========================== *
     *            TOWNS            *
     * =========================== */
    public abstract class Town
    {
        public string TownName { get; set; }
        public string Description { get; set; }
        public List<string> People { get; set; }
        public string TownMusic { get; set; }
        public string OtherMusic { get; set; }
        public string TownID { get; set; }

        public List<string> Houses { get; set; }

        public abstract void TownChoice();

        public void TownSpeakToNPCs()
        {
            /*
            while (true):
                Console.WriteLine('NPCs: ')

                npc_list = [x for x in self.people if any([y.active for y in x.convos[CInfo['current_town']]])]

                for x, character in enumerate(npc_list):
                    Console.WriteLine(f"      [{x + 1}] {character.name}")

                while (true):
                    character = main.s_input('Input [#] (or type "exit"): ').lower()

                    try:
                        character = npc_list[int(character) - 1]

                    except (IndexError, ValueError):
                        if character in ['e', 'x', 'exit', 'b', 'back']:
                            return

                        continue

                    sounds.play_music(self.store_music)

                    Console.WriteLine('-'*save_load.divider_size)

                    character.speak()

                    sounds.play_music(self.town_music)

                    break */
        }

        public void TownChooseHouse()
        {
            /*
            while (true):
                Console.WriteLine('-' * save_load.divider_size)
                Console.WriteLine("Unlocked Houses:")

                for x, y in enumerate([house for house in self.houses]):
                    Console.WriteLine(f"      [{x + 1}] {y.owner}'s House")

                while (true):
                    chosen_house = main.s_input('Input [#] (or type "exit"): ').lower()

                    try:
                        chosen_house = self.houses[int(chosen_house) - 1]

                    except(IndexError, ValueError):
                        if chosen_house in ['e', 'x', 'exit', 'b', 'back']:
                            return

                        continue

                    chosen_house.enter_house()

                    sounds.play_music('Music/Mayhem in the Village.wav')

                    break */
        }
    }

    public abstract class MarketTown : Town
    {
        // MarketTowns have inns and shops, as well as people and houses
        public List<string> GenStock { get; set; }

        public override void TownChoice()
        {
            /*
            Console.WriteLine('-'*save_load.divider_size)
            Console.WriteLine(f'Welcome to {self.name}!')
            Console.WriteLine('-'*save_load.divider_size)

            while (true):
                CInfo['gamestate'] = 'town'
                CInfo['current_town'] = self.town_id
                Console.WriteLine("""What do you wish to do?
            [1] Town Description
            [2] Buildings
            [3] People
            [4] Player Info
            [5] View Inventory""")

                while (true):
                    choice = main.s_input('Input [#] (or type "exit"): ')

                    if choice == '1':
                        Console.WriteLine('-'*save_load.divider_size)

                        for x in main.chop_by_79(self.desc):
                            Console.WriteLine(x)

                        main.s_input('\nPress enter/return ')
                        Console.WriteLine('-'*save_load.divider_size)

                    else if choice == '2':
                        Console.WriteLine('-'*save_load.divider_size)
                        self.inside_town()
                        Console.WriteLine('-'*save_load.divider_size)

                    else if choice == '3':
                        Console.WriteLine('-'*save_load.divider_size)

                        if [x for x in self.people if any([y.active for y in x.convos[CInfo['current_town']]])]:
                            self.speak_to_npcs()

                        else:
                            Console.WriteLine("There doesn't appear to be anyone to talk to.")

                        Console.WriteLine('-'*save_load.divider_size)

                    else if choice == '4':
                        units.player.choose_target("Select Party Member: ", ally=True, enemy=False)
                        Console.WriteLine('-'*save_load.divider_size)
                        units.player.target.player_info()
                        Console.WriteLine('-'*save_load.divider_size)

                    else if choice == '5':
                        Console.WriteLine('-'*save_load.divider_size)
                        items.pick_category()
                        Console.WriteLine('-'*save_load.divider_size)

                    else if choice.lower() in ['e', 'x', 'exit', 'b', 'back']:
                        sounds.play_music(CInfo['music'])

                        Console.WriteLine('-'*save_load.divider_size)
                        return

                    else:
                        continue

                    break */
        }

        public void TownBuildingsMenu()
        {
            /*
            while (true):
                Console.WriteLine('There is a [G]eneral Store, an [I]nn, and some [U]nlocked houses in this town.')
                buildings = ['i', 'g', 'u']

                while (true):
                    selected = main.s_input('Where do you want to go? | Input [L]etter (or type "exit"): ').lower()

                    if any(map(selected.startswith, buildings)):
                        sounds.play_music(self.store_music)

                        if selected.startswith('u'):
                            self.town_houses()

                        if selected.startswith('g'):
                            self.town_gen()

                        if selected.startswith('i'):
                            self.town_inn()

                        Console.WriteLine('-'*save_load.divider_size)

                        sounds.play_music(self.town_music)

                        break

                    else if selected in ['e', 'x', 'exit', 'b', 'back']:
                        return */

        }

        public void TownInn()
        {
            /*
            Console.WriteLine('-'*save_load.divider_size)
            Console.WriteLine('Inn Keeper: "Greetings, Traveler!"')

            while (true):
                choice = main.s_input(f'"Would you like to stay at our inn? It\'s free, y\'know." | Y/N: ').lower()

                if choice.startswith('y'):

                    Console.WriteLine('\n"Goodnight, Traveler."')
                    Console.WriteLine('Sleeping', end='')

                    sys.stdout.flush()

                    main.text_scroll('...', spacing=0.75)
                    Console.WriteLine()

                    for character in [
                        units.player,
                        units.solou,
                        units.chili,
                        units.chyme,
                        units.storm,
                        units.parsto,
                        units.adorine
                    ]:

                        character.hp = copy.copy(character.max_hp)
                        character.mp = copy.copy(character.max_mp)
                        character.status_ail = ['alive']

                    Console.WriteLine("Your party's HP and MP have been fully restored.")
                    Console.WriteLine('Your party has been relieved of all status ailments.')
                    main.s_input("\nPress enter/return ")
                    Console.WriteLine('-'*save_load.divider_size)

                    save_load.save_game()

                    return

                else if choice.startswith('n'):
                    return */
        }

        public void TownGeneralStoreBuyOrSell()
        {
            /*
            # A dictionary containing objects the player can purchase. This list is populated based on the current
            # cell's store_level attribute
            stock = {'All': []}

            store_level = tiles.find_cell_with_tile_id(CInfo['current_tile'].tile_id).store_level - 1

            for category in items.gs_stock:
                stock[category] = []

                for item_group in items.gs_stock[category]:
                    stock[category].append(item_group[store_level])
                    stock['All'].append(item_group[store_level])

            Console.WriteLine('-'*save_load.divider_size)
            Console.WriteLine('Merchant: "Welcome, Traveler!"')

            while (true):
                chosen = main.s_input('Do you want to [b]uy or [s]ell items? | Input letter (or type "exit"): ').lower()

                if chosen.startswith('b'):
                    Console.WriteLine('-'*save_load.divider_size)
                    self.buy_choose_cat(stock)

                else if chosen.startswith('s'):
                    Console.WriteLine('-'*save_load.divider_size)
                    self.sell_choose_cat()

                else if chosen in ['e', 'x', 'exit', 'b', 'back']:
                    return */
        }

        public void TownGeneralStoreBuyChooseItem()
        {
            /*
            highest_charisma = max([pcu.attributes['cha'] for pcu in [units.player,
                                                                      units.solou,
                                                                      units.chili,
                                                                      units.chyme,
                                                                      units.adorine,
                                                                      units.parsto]]) - 1

            while (true):
                padding = len(max([item.name for item in stock[item_cat]], key=len))
                Console.WriteLine(f"{item_cat} (Your party has {CInfo['gp']} GP): ")

                for num, item in enumerate(stock[item_cat]):
                    modified_value = math.ceil(max([item.value/(1 + 0.01*highest_charisma), item.value*0.5]))
                    Console.WriteLine(f"      [{num + 1}] {item.name} {(padding - len(item.name))*'-'}--> {modified_value} GP")

                while (true):
                    chosen = main.s_input('Input [#] (or type "back"): ').lower()

                    try:
                        chosen = stock[item_cat][int(chosen) - 1]

                    except (IndexError, ValueError):
                        if chosen in ['e', 'x', 'exit', 'b', 'back']:
                            Console.WriteLine('-'*save_load.divider_size)
                            return

                        continue

                    Console.WriteLine('-' * save_load.divider_size)
                    Console.WriteLine(f'-{chosen.name.upper()}-')
                    Console.WriteLine(ascii_art.item_sprites[chosen.ascart])
                    Console.WriteLine(f'"{chosen.desc}"')
                    Console.WriteLine('-' * save_load.divider_size)

                    self.buy_yes_or_no(chosen)

                    break */
        }

        public void TownGeneralStoreBuyYesOrNo()
        {
            /*
            highest_charisma = max([pcu.attributes['cha'] for pcu in [units.player,
                                                                      units.solou,
                                                                      units.chili,
                                                                      units.chyme,
                                                                      units.adorine,
                                                                      units.parsto]]) - 1

            modified_value = math.ceil(max([chosen.value/(1 + 0.01*highest_charisma), chosen.value*0.5]))

            while (true):
                y_n = main.s_input(f"Ya want this {chosen.name}? Will cost ya {modified_value} GP. | Y/N: ").lower()

                if y_n.startswith('y'):
                    if CInfo['gp'] >= modified_value:
                        CInfo['gp'] -= modified_value
                        items.add_item(chosen.item_id)

                        Console.WriteLine('-' * save_load.divider_size)
                        Console.WriteLine(f'You purchase the {chosen.name} for {modified_value} GP.')
                        main.s_input("\nPress enter/return ")

                    else:
                        Console.WriteLine(f"Sorry, you don't have enough GP for that!")
                        main.s_input("\nPress enter/return ")

                    Console.WriteLine('-' * save_load.divider_size)

                    return

                else if y_n.startswith('n'):
                    Console.WriteLine('-' * save_load.divider_size)

                    return */
        }

        public void TownGeneralStoreSellChooseCategory()
        {
            /*
            while (true):
                Console.WriteLine("""Sellable Categories:
          [1] Armor
          [2] Consumables
          [3] Weapons
          [4] Accessories
          [5] Tools
          [6] Misc. Items""")
                while (true):
                    cat = main.s_input('Input [#] (or type "back"): ').lower()

                    if cat == '1':
                        cat = 'armor'
                        vis_cat = 'Armor'
                    else if cat == '2':
                        cat = 'consumables'
                        vis_cat = 'Consumables'
                    else if cat == '3':
                        cat = 'weapons'
                        vis_cat = 'Weapons'
                    else if cat == '4':
                        cat = 'access'
                        vis_cat = 'Accessories'
                    else if cat == '5':
                        cat = 'tools'
                        vis_cat = 'Tools'
                    else if cat == '6':
                        cat = 'misc'
                        vis_cat = 'Misc. Items'

                    else if cat in ['e', 'x', 'exit', 'b', 'back']:
                        Console.WriteLine('-' * save_load.divider_size)
                        return

                    else:
                        continue

                    if items.inventory[cat] and any([not i.imp for i in items.inventory[cat]]):
                        items.pick_item(cat, vis_cat, selling=True)
                        Console.WriteLine('-' * save_load.divider_size)

                        break

                    else:
                        Console.WriteLine('-' * save_load.divider_size)
                        Console.WriteLine(f"You don't have any sellable items in the {vis_cat} category.")
                        main.s_input("\nPress enter/return")
                        Console.WriteLine('-' * save_load.divider_size)

                        break */

        }
    }

    public abstract class PeopleTown : Town
    {
        // PeopleTowns only have people in them, like camps and wandering parties
        // They also optionally have houses
        public override void TownChoice()
        {
            CInfo.Gamestate = CEnums.GameState.town;
            CMethods.PrintDivider();
            Console.WriteLine($"Welcome to {TownName}!");
            CMethods.PrintDivider();

            while (true)
            {
                Console.WriteLine("What do you want to do?");
                Console.WriteLine("      [1] Look Around");
                Console.WriteLine("      [2] Talk to People");
                Console.WriteLine("      [3] View Party Info");
                Console.WriteLine("      [4] View Inventory");

                if (Houses.Count > 0)
                {
                    Console.WriteLine("      [5] Enter Houses");
                }

                while (true)
                {
                    string choice = CMethods.SingleCharInput("Input [#] (or type 'exit'): ");

                    if (choice == "1")
                    {
                        CMethods.PrintDivider();
                        Console.WriteLine(Description);
                        CMethods.PressAnyKeyToContinue();
                        CMethods.PrintDivider();

                        break;
                    }

                    else if (choice == "2")
                    {
                        CMethods.PrintDivider();

                        // To-do!!
                        // if [x for x in self.people if any([y.active for y in x.convos[CInfo["current_town"]]])] 
                        // {
                                // self.speak_to_npcs()
                        // }

                        // else 
                        // {
                                Console.WriteLine("There doesn't appear to be anyone to talk to.");
                                CMethods.PressAnyKeyToContinue();
                        // }

                        CMethods.PrintDivider();

                        break;
                    }

                    else if (choice == "3")
                    {
                        CommandManager.PlayerStatsCommand();

                        break;
                    }

                    else if (choice == "4")
                    {
                        CMethods.PrintDivider();
                        InventoryManager.PickInventoryCategory();
                        CMethods.PrintDivider();

                        break;
                    }

                    else if (choice == "5" && Houses.Count > 0)
                    {
                        CMethods.PrintDivider();
                        TownChooseHouse();
                        CMethods.PrintDivider();

                        break;
                    }

                    else if (choice.IsExitString()) 
                    {
                        CMethods.PrintDivider();
                        return;
                    }
                }
            }
        }
    }

    public sealed class NeartonTown : PeopleTown
    {
        public NeartonTown()
        {
            TownName = "Nearton";
            Description = @"Nearton is a small village in in the Inner Forest.It is in this very town
where numerous brave adventurers have begun their journey. Nearton is just
your standard run-of - the - mill village: it has a general store, an inn, and
a few small houses. An old man is standing near one of the houses, and
appears to be very troubled about something.";
            TownID = "town_nearton";

            TownMusic = "Music/Chickens (going peck peck peck).wav";
            OtherMusic = "Music/Mayhem in the Village";

            People = new List<string>();
            Houses = new List<string>();
        }
    }

    /* =========================== *
     *      HOUSES AND CHESTS      *
     * =========================== */
    public class House
    {

    }

    public class Chest
    {

    }
}
