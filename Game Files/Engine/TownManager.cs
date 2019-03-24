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

namespace Engine
{
    public static class TownManager
    {
        private static readonly List<Town> town_list = new List<Town>()
        {
            new NeartonClass(), new SouthfordClass(), new OvershireCityClass(), new PrincipaliaClass(), new SardoothClass(),
            new TriptonClass(), new FallvilleClass(),
            new ValiceClass(), new ValenfallClass(),
            new ParceonClass(),
            new RymnOutpostClass(), new FortSigilClass(), new MardovianCavernsClass(), new MtFalenkarthClass(), new CoranOutpostClass(),
            new DewfrostClass(), new ClayroostClass(), new RavenstoneClass(), new AmbercreekClass(), new CapwildClass(),
            new SimphetClass(), new WhistumnClass(), new HatchnukClass(),
            new CesuraClass(), new TrintooliClass(), new FoqwhitteClass(), new DonkohrinClass(),
            new SanguionClass(), new LantonumClass(),
            new NewEkanmarClass()
        };

        public static List<Town> GetTownList()
        {
            return town_list;
        }

        public static Town FindTownWithID(string town_id)
        {
            return GetTownList().Single(x => x.TownID == town_id);
        }

        public static bool SearchForTowns(bool enter_town)
        {
            List<string> town_list = TileManager.FindTileWithID(CInfo.CurrentTile).TownList;

            if (town_list.Count == 0)
            {
                return false;
            }

            else
            {
                if (enter_town)
                {
                    foreach (string town_id in town_list)
                    {
                        Town town = FindTownWithID(town_id);
                        CMethods.PrintDivider();

                        while (true)
                        {
                            string yes_no = CMethods.SingleCharInput($"The town of {town.TownName} is nearby. Give it a visit? ").ToLower();

                            if (yes_no.IsYesString())
                            {
                                town.EnterTown();
                                break;
                            }

                            else if (yes_no.IsNoString())
                            {
                                CMethods.PrintDivider();
                                break;
                            }
                        }
                    }
                }

                return true;
            }
        }
    }

    /* =========================== *
     *          TOWN TYPES         *
     * =========================== */
    public abstract class Town
    {
        public string TownName { get; set; }
        public string Description { get; set; }
        public List<string> People { get; set; }
        public System.Media.SoundPlayer TownMusic { get; set; }
        public System.Media.SoundPlayer OtherMusic { get; set; }
        public string TownID { get; set; }

        public List<string> Houses { get; set; }

        public void EnterTown()
        {
            CInfo.Gamestate = CEnums.GameState.town;
            CInfo.RespawnTile = CInfo.CurrentTile;

            TownMusic.PlayLooping();

            CMethods.PrintDivider();
            Console.WriteLine($"Welcome to {TownName}!");
            CMethods.PressAnyKeyToContinue();
            CMethods.PrintDivider();

            MainMenu();

            SoundManager.PlayCellMusic();
        }

        public abstract void MainMenu();

        protected void SpeakToNPCs()
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

        protected void ChooseHouse()
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

        public override void MainMenu()
        {
            while (true)
            {
                Console.WriteLine($"You are in {TownName}. What do you want to do?");
                Console.WriteLine("      [1] Look Around");
                Console.WriteLine("      [2] Talk to People");
                Console.WriteLine("      [3] View Party Info");
                Console.WriteLine("      [4] View Inventory");
                Console.WriteLine("      [5] View Locations");

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

                        break;
                    }

                    else if (choice == "5")
                    {
                        CMethods.PrintDivider();
                        BuildingsMenu();
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

        protected void BuildingsMenu()
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

        protected void VisitInn()
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

        protected void GeneralStoreBuyOrSell()
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

        protected void GeneralStoreBuyChooseItem()
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

        protected void GeneralStoreBuyYesOrNo()
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

        protected void GeneralStoreSellChooseCategory()
        {
            /*
            while (true):
                Console.WriteLine("Sellable Categories:
          [1] Armor
          [2] Consumables
          [3] Weapons
          [4] Accessories
          [5] Tools
          [6] Misc. Items")
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
        public override void MainMenu()
        {
            CInfo.Gamestate = CEnums.GameState.town;
            CInfo.RespawnTile = CInfo.CurrentTile;
            CMethods.PrintDivider();
            Console.WriteLine($"Welcome to {TownName}!");
            CMethods.PrintDivider();

            while (true)
            {
                Console.WriteLine($"You are in {TownName}. What do you want to do?");
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

                        break;
                    }

                    else if (choice == "5" && Houses.Count > 0)
                    {
                        CMethods.PrintDivider();
                        ChooseHouse();
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

    /* =========================== *
     *       OVERSHIRE TOWNS       *
     * =========================== */
    public sealed class NeartonClass : MarketTown
    {
        public NeartonClass()
        {
            TownName = "Nearton";
            Description = @"Nearton is a small village in in the Inner Forest. It is in this very town
where numerous brave adventurers have begun their journey. Nearton is your 
standard run-of-the-mill village: it has a general store, an inn, and a few 
small houses.";
            TownID = "town_nearton";

            TownMusic = SoundManager.town_main_cheery;
            OtherMusic = SoundManager.town_other_cheery;

            People = new List<string>();
            Houses = new List<string>();

            GenStock = new List<string>()
            {
                "s_potion",
                "s_elixir",
                "light_armor",
                "fancy_robes",
                "festive_clothes",
                "grass_amulet",
                "dark_amulet",
                "shovel_tool"
            };
        }
    }
    
    public sealed class SouthfordClass : MarketTown
    {
        public SouthfordClass()
        {
            TownName = "Southford";
            Description = @"Southford is a fair-size town in the Southeast of the Inner Forest. The 
inhabitants of this town are known for being quite wise, and may provide you 
with helpful advice.";
            TownID = "town_southford";

            TownMusic = SoundManager.town_main_cheery;
            OtherMusic = SoundManager.town_other_cheery;

            People = new List<string>();
            Houses = new List<string>();

            GenStock = new List<string>()
            {

            };
        }
    }
    
    public sealed class OvershireCityClass : MarketTown
    {
        public OvershireCityClass()
        {
            TownName = "Overshire City";
            Description = @"Overshire City is a city just outside the Inner Forest. Overshire is the 
capital of The Province of Overshire, and therefore the capital of the entire
Kingdom of Harconia. As such, the city is very densely populated. The city is
separated into three sectors: the upper-class inner portion consisting of a 
castle surrounded by reinforced stone walls, a lower-class outer portion
comprised of smalls buildings and huts, and a middle-class section situated in
between. As an outsider, you are forbidden to enter the upper two, but are
welcome to do as you wish in the lower.";
            TownID = "overshire_city";

            TownMusic = SoundManager.town_main_cheery;
            OtherMusic = SoundManager.town_other_cheery;

            People = new List<string>();
            Houses = new List<string>();

            GenStock = new List<string>()
            {

            };
        }
    }
    
    public sealed class PrincipaliaClass : MarketTown
    {
        public PrincipaliaClass()
        {
            TownName = "Principalia";
            Description = @"Hundreds of years ago, King Pyravia II ordered the expansion of this town
from a small village with merely a dozen cottages to a sprawling city, lively
and full of culture. Pyravia II was an interesting man with strange
superstitions. He personally believed that the Kingdom's capital, Overshire City,
had been cursed, and that the third month of every year was when the curse was
at its strongest. Principalia was intended to be his home during that month,
and since then it's been a tradition that every third month of the year the
current King or Queen leaves Overshire to live here. Of course, when the King
is here, the castle here is just as heavily guarded as the one back in
Overshire, so one shouldn't expect to pay him a visit.";
            TownID = "town_principalia";

            TownMusic = SoundManager.town_main_cheery;
            OtherMusic = SoundManager.town_other_cheery;

            People = new List<string>();
            Houses = new List<string>();

            GenStock = new List<string>()
            {

            };
        }
    }
    
    public sealed class SardoothClass : MarketTown
    {
        public SardoothClass()
        {
            TownName = "Sardooth";
            Description = @"Sardooth is a ghost town, without a single permanent inhabitant. This town
was hit the hardest by the latest wave of monsters, causing it to turn from
the bustling hub of commerce and culture to a barren wasteland within just 
six months. Everyone who lived here was either killed or driven out by the 
monsters, and the King's troops were powerless to stop it. The only thing of
note is 'The Undershire', a massive cemetery to the northeast, which is 
rumored to be even more dangerous than here.";
            TownID = "town_sardooth";

            TownMusic = SoundManager.town_main_cheery;
            OtherMusic = SoundManager.town_other_cheery;

            People = new List<string>();
            Houses = new List<string>();

            GenStock = new List<string>()
            {

            };
        }
    }

    /* =========================== *
     *        DOWNPOUR TOWNS       *
     * =========================== */
    public sealed class TriptonClass : MarketTown
    {
        public TriptonClass()
        {
            TownName = "Tripton";
            Description = @"When the town of Tripton was being built, the people working on the
project failed to notice that another town, Fallville, just so happened to be
located mere meters away from the new town's borders. Merchants in Tripton
became very successful, as their superior bartering tactics allowed them to
easily steal business from Fallvillian merchants. This has led to a bitter,
and sometimes violent, rivalry between the two towns, particularly between the
village leaders.";
            TownID = "town_tripton";

            TownMusic = SoundManager.town_main_cheery;
            OtherMusic = SoundManager.town_other_cheery;

            People = new List<string>();
            Houses = new List<string>();

            GenStock = new List<string>()
            {

            };
        }
    }
    
    public sealed class FallvilleClass : MarketTown
    {
        public FallvilleClass()
        {
            TownName = "Fallville";
            Description = @"When the town of Tripton was being built, the people working on the
project failed to notice that another town, Fallville, just so happened to be
located mere meters away from the new town's borders. Merchants in Tripton
became very successful, as their superior bartering tactics allowed them to
easily steal business from Fallvillian merchants. This has led to a bitter,
and sometimes violent, rivalry between the two towns, particularly between the
village leaders.";
            TownID = "town_fallville";

            TownMusic = SoundManager.town_main_cheery;
            OtherMusic = SoundManager.town_other_cheery;

            People = new List<string>();
            Houses = new List<string>();

            GenStock = new List<string>()
            {

            };
        }
    }

    /* =========================== *
     *          FLUTE TOWNS        *
     * =========================== */
    public sealed class ValiceClass : MarketTown
    {
        public ValiceClass()
        {
            TownName = "Valice";
            Description = @"Valice is a massive town in the Province of Overshire. Despite its immense 
size, comparable to that of Overshire City, Valice has little to offer. Back 
during the Harconian Gem Rush, when thousands of tons of gems and ore were 
discovered to be lying beneath the surface of Valice, the town grew 
tremendously in both size and wealth. This wealth did not last, as the gems 
quickly became rarer and rarer and are now nowhere to be seen. This, 
unfortunately, means that Valice is both one of the biggest towns in Overshire,
and also one of the poorest.";
            TownID = "town_valice";

            TownMusic = SoundManager.town_main_cheery;
            OtherMusic = SoundManager.town_other_cheery;

            People = new List<string>();
            Houses = new List<string>();

            GenStock = new List<string>()
            {

            };
        }
    }
    
    public sealed class ValenfallClass : MarketTown
    {
        public ValenfallClass()
        {
            TownName = "Valenfall";
            Description = @"Valenfall is an ancient city, belived to have been created by the forefathers
of modern Harconia. The city used to be situated on a large, floating island
known as the Aether, before it came crashing down. Towns located below the
Aether were forced to evacuate to save themselves from the impending impact.
Strangely, all of the Aether, including Valenfall, was devoid of any life.
Citizens of the now-destroyed towns decided to take over the empty town of
Valenfall which managed to survive falling to Harconia. It is unknonwn how
the Aether floated in the air or why it stopped.";
            TownID = "town_valenfall";

            TownMusic = SoundManager.town_main_cheery;
            OtherMusic = SoundManager.town_other_cheery;

            People = new List<string>();
            Houses = new List<string>();

            GenStock = new List<string>()
            {

            };
        }
    }

    /* =========================== *
     *        DELTORA TOWNS        *
     * =========================== */


    /* =========================== *
     *        PARRIWEY TOWNS       *
     * =========================== */
    public sealed class ParceonClass : MarketTown
    {
        public ParceonClass()
        {
            TownName = "Parceon";
            Description = @"Parceon is a highly populated town renown for it's rich magical background.
Parceon is home to the famous Sorcerers' Guild, a group of unbelievably
skilled and wise mages who work together to establish and enforce magical law.
The head of the guild, Azura, lives in a large tower in the southwest side of
the town.";
            TownID = "town_parceon";

            TownMusic = SoundManager.town_main_cheery;
            OtherMusic = SoundManager.town_other_cheery;

            People = new List<string>();
            Houses = new List<string>();

            GenStock = new List<string>()
            {

            };
        }
    }

    /* =========================== *
     *        CHIN'TOR TOWNS       *
     * =========================== */
    public sealed class RymnOutpostClass : MarketTown
    {
        public RymnOutpostClass()
        {
            TownName = "Rymn Outpost";
            Description = @"Rymn Outpost is one of the several small villages established
after the Thexian Incursion. All of the residents of this town are soldiers or
family members of soldiers, with the exception a few merchants. Rymn Outpost
is named after Rymnes, the Divinic gods of defense.";
            TownID = "town_rymn_outpost";

            TownMusic = SoundManager.town_main_cheery;
            OtherMusic = SoundManager.town_other_cheery;

            People = new List<string>();
            Houses = new List<string>();

            GenStock = new List<string>()
            {

            };
        }
    }
    
    public sealed class FortSigilClass : MarketTown
    {
        public FortSigilClass()
        {
            TownName = "Fort Sigil";
            Description = @"Fort Sigil small village in the Barrier Forest. As the name suggests, the
town was built around an old fort, named Fort Sigil. Originally comprised of
just a few tents meant to house soldiers, many of these soldiers eventually
put down their arms and settled. Despite it's rich backstory and pleasant
scenery, Fort Sigil doesn't get many visitors. Perhaps there's a reason why...";
            TownID = "town_fort_sigil";

            TownMusic = SoundManager.town_main_cheery;
            OtherMusic = SoundManager.town_other_cheery;

            People = new List<string>();
            Houses = new List<string>();

            GenStock = new List<string>()
            {

            };
        }
    }
    
    public sealed class MardovianCavernsClass : MarketTown
    {
        public MardovianCavernsClass()
        {
            TownName = "Mardovian Caverns";
            Description = "";
            TownID = "town_mardoviancaverns";

            TownMusic = SoundManager.town_main_cheery;
            OtherMusic = SoundManager.town_other_cheery;

            People = new List<string>();
            Houses = new List<string>();

            GenStock = new List<string>()
            {

            };
        }
    }
    
    public sealed class MtFalenkarthClass : MarketTown
    {
        public MtFalenkarthClass()
        {
            TownName = "Mt. Falenkarth";
            Description = "";
            TownID = "town_mtfalenkarth";

            TownMusic = SoundManager.town_main_cheery;
            OtherMusic = SoundManager.town_other_cheery;

            People = new List<string>();
            Houses = new List<string>();

            GenStock = new List<string>()
            {

            };
        }
    }
    
    public sealed class CoranOutpostClass : MarketTown
    {
        public CoranOutpostClass ()
        {
            TownName = "Coran Outpost";
            Description = "";
            TownID = "town_coran_outpost";

            TownMusic = SoundManager.town_main_cheery;
            OtherMusic = SoundManager.town_other_cheery;

            People = new List<string>();
            Houses = new List<string>();

            GenStock = new List<string>()
            {

            };
        }
    }

    /* =========================== *
     *       CAMBERLITE TOWNS      *
     * =========================== */
    public sealed class DewfrostClass : MarketTown
    {
        public DewfrostClass()
        {
            TownName = "Dewfrost";
            Description = "";
            TownID = "town_dewfrost";

            TownMusic = SoundManager.town_main_cheery;
            OtherMusic = SoundManager.town_other_cheery;

            People = new List<string>();
            Houses = new List<string>();

            GenStock = new List<string>()
            {

            };
        }
    }
    
    public sealed class ClayroostClass : MarketTown
    {
        public ClayroostClass()
        {
            TownName = "Clayroost";
            Description = "";
            TownID = "town_clayroost";

            TownMusic = SoundManager.town_main_cheery;
            OtherMusic = SoundManager.town_other_cheery;

            People = new List<string>();
            Houses = new List<string>();

            GenStock = new List<string>()
            {

            };
        }
    }
    
    public sealed class RavenstoneClass : MarketTown
    {
        public RavenstoneClass()
        {
            TownName = "Ravenstone";
            Description = @"Ravenstone is a natural sanctuary, home to dozens upon dozens of different 
flora and fauna. Naturally, the majority population of Ravenstone consists of
Druids and other nature-magicians. Ravenstone is also the home of the Druids'
section of the Sorcerers' Guild. Vegetation grows on almost every building and 
statue in the town. When the population of the town is calculated, animals are 
counted as people. More than 35% of the population are various species of 
animals.";
            TownID = "town_ravenstone";

            TownMusic = SoundManager.town_main_cheery;
            OtherMusic = SoundManager.town_other_cheery;

            People = new List<string>();
            Houses = new List<string>();

            GenStock = new List<string>()
            {

            };
        }
    }
    
    public sealed class AmbercreekClass : MarketTown
    {
        public AmbercreekClass()
        {
            TownName = "Ambercreek";
            Description = @"Ambercreek is a large mining town located in the Chin'tor. The Chin'toric
embassy can be found in the middle of this town surrounded by large stone walls
and a few guard-towers. Sugulat, the Lord of Chin'tor, can often be found mining
on the outskirts of town. A very troubled-looking old man is in the southwest 
portion of the town near a few smaller houses.";
            TownID = "town_ambercreek";

            TownMusic = SoundManager.town_main_cheery;
            OtherMusic = SoundManager.town_other_cheery;

            People = new List<string>();
            Houses = new List<string>();

            GenStock = new List<string>()
            {

            };
        }
    }
    
    public sealed class CapwildClass : MarketTown
    {
        public CapwildClass()
        {
            TownName = "Capwild";
            Description = @"Capwild is a medium sized town situated in the Terrius Mt. Range.
Capwild is a supplier of grains and herbs for the entire region, and makes
extensive use of terrace farming to make up for the lack of arable land.
Further investigation reveals that water mages have created self-sustaining
irrigation systems as well, further enhancing Capwild's farming capabilities.";
            TownID = "town_capwild";

            TownMusic = SoundManager.town_main_cheery;
            OtherMusic = SoundManager.town_other_cheery;

            People = new List<string>();
            Houses = new List<string>();

            GenStock = new List<string>()
            {

            };
        }
    }

    /* =========================== *
     *        WHITLOCK TOWNS       *
     * =========================== */
    public sealed class SimphetClass : MarketTown
    {
        public SimphetClass()
        {
            TownName = "Simphet";
            Description = "";
            TownID = "town_simphet";

            TownMusic = SoundManager.town_main_cheery;
            OtherMusic = SoundManager.town_other_cheery;

            People = new List<string>();
            Houses = new List<string>();

            GenStock = new List<string>()
            {

            };
        }
    }
    
    public sealed class WhistumnClass : MarketTown
    {
        public WhistumnClass()
        {
            TownName = "Whistumn";
            Description = @"Whistumn ancient city situated on the border
between the Arcadian Desert and the Barrier Forest. The inhabitants of this town
are known for their skepticism and reasoning. Many of them are scientists and are
skilled mathematicians and engineers. This town has an ongoing rivalry with
the town of Parceon because of their magical background, but this appears
to be mostly one-sided. A saddened-looking woman and her husband are sitting
on the steps of the general store.";
            TownID = "town_whistumn";

            TownMusic = SoundManager.town_main_cheery;
            OtherMusic = SoundManager.town_other_cheery;

            People = new List<string>();
            Houses = new List<string>();

            GenStock = new List<string>()
            {

            };
        }
    }
    
    public sealed class HatchnukClass : MarketTown
    {
        public HatchnukClass()
        {
            TownName = "Hatchnuk";
            Description = @"Hatchnuk is the only remaining town in Harconia that still has cases of 
'The Blight of Hatchnuk', a plague-like disease that killed hundreds of thousands of
people during the 10th and 11th centuries. Something about the strand that 
infects Hatchnuk seems to make it completely incurable, as the disease has been 
running rampant for the past four centuries. The economy of Hatchnuk has 
entirely collapsed, as the risk of spreading disease is far too great for people
to be walking out in the open doing business together. As a result, there are no
buildings that you are able to enter, and no people to talk to. The only people 
who are around to speak to are the guards, but their plague-doctor-esque
apparel and stern looks make it clear that they are not in the mood for 
chit-chat.";
            TownID = "town_hatchnuk";

            TownMusic = SoundManager.town_main_cheery;
            OtherMusic = SoundManager.town_other_cheery;

            People = new List<string>();
            Houses = new List<string>();

            GenStock = new List<string>()
            {

            };
        }
    }

    /* =========================== *
     *         KOHRIN TOWNS        *
     * =========================== */
    public sealed class CesuraClass : MarketTown
    {
        public CesuraClass()
        {
            TownName = "Cesura";
            Description = "";
            TownID = "town_cesura";

            TownMusic = SoundManager.town_main_cheery;
            OtherMusic = SoundManager.town_other_cheery;

            People = new List<string>();
            Houses = new List<string>();

            GenStock = new List<string>()
            {

            };
        }
    }
    
    public sealed class TrintooliClass : MarketTown
    {
        public TrintooliClass()
        {
            TownName = "Trintooli";
            Description = "";
            TownID = "town_trintooli";

            TownMusic = SoundManager.town_main_cheery;
            OtherMusic = SoundManager.town_other_cheery;

            People = new List<string>();
            Houses = new List<string>();

            GenStock = new List<string>()
            {

            };
        }
    }
    
    public sealed class FoqwhitteClass : MarketTown
    {
        public FoqwhitteClass()
        {
            TownName = "Foqwhitte";
            Description = @"Nearton is a small village in in the Inner Forest.It is in this very town
where numerous brave adventurers have begun their journey. Nearton is just
your standard run-of - the - mill village: it has a general store, an inn, and
a few small houses. An old man is standing near one of the houses, and
appears to be very troubled about something.";
            TownID = "town_foqwhitte";

            TownMusic = SoundManager.town_main_cheery;
            OtherMusic = SoundManager.town_other_cheery;

            People = new List<string>();
            Houses = new List<string>();

            GenStock = new List<string>()
            {

            };
        }
    }
    
    public sealed class DonkohrinClass : MarketTown
    {
        public DonkohrinClass()
        {
            TownName = "Don'kohrin";
            Description = "";
            TownID = "town_donkohrin";

            TownMusic = SoundManager.town_main_cheery;
            OtherMusic = SoundManager.town_other_cheery;

            People = new List<string>();
            Houses = new List<string>();

            GenStock = new List<string>()
            {

            };
        }
    }

    /* =========================== *
     *        PELAMORA TOWNS       *
     * =========================== */
    public sealed class SanguionClass : MarketTown
    {
        public SanguionClass()
        {
            TownName = "Sanguion";
            Description = @"Sanguion is a safe-haven for vampires. Vampires are feared throughout
Harconia, so this fairly unknown town is the only place they can go without
being persecuted. The vampires in this town are peaceful, and actually refuse
to drink the blood of intelligent lifeforms. Beware, though, as not all
vampires are as friendly as the ones who inhabit Sanguion.";
            TownID = "town_sanguion";

            TownMusic = SoundManager.town_main_cheery;
            OtherMusic = SoundManager.town_other_cheery;

            People = new List<string>();
            Houses = new List<string>();

            GenStock = new List<string>()
            {

            };
        }
    }
    
    public sealed class LantonumClass : MarketTown
    {
        public LantonumClass()
        {
            TownName = "Lantonum";
            Description = @"Lantonum is a small town that has the best forge in all of Harconia.
Nearly 2/3s of all citizens of this town are experienced blacksmiths, and 90%
of all ores and minerals mined in Pelamora are brought here. It is one of the 
wealthiest cities in Pelamora due to its Mythril, Magestite, and Necrite bar 
exports.";
            TownID = "town_lantonum";

            TownMusic = SoundManager.town_main_cheery;
            OtherMusic = SoundManager.town_other_cheery;

            People = new List<string>();
            Houses = new List<string>();

            GenStock = new List<string>()
            {

            };
        }
    }

    /* =========================== *
     *        CELEMIA TOWNS        *
     * =========================== */
    public sealed class NewEkanmarClass : MarketTown
    {
        public NewEkanmarClass()
        {
            TownName = "New Ekanmar";
            Description = @"New Ekanmar is the capital of Celemia, one of the Harconian provinces. Prior
to the Harconian Revolution, this town was the location of a large portion of
Flyscoria's troops in Harconia. The Harconians drove much of them out, but
a large number of them defected to the Harconian side and stayed. After the
war, the citizens gave up their weapons and became a peaceful town. The vast
majority of the inhabitants of this town are, naturally, Flyscors. It seems
that the Flyscorian Royal Family is visiting here - perhaps you can talk with
them for a bit.";
            TownID = "town_new_ekanmar";

            TownMusic = SoundManager.town_main_cheery;
            OtherMusic = SoundManager.town_other_cheery;

            People = new List<string>();
            Houses = new List<string>();

            GenStock = new List<string>()
            {

            };
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
