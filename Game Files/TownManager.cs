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
    public static class TownManager
    {
        private static readonly List<Town> town_list = new List<Town>()
        {
            /* Overshire  */  new NeartonClass(), new SouthfordClass(), new OvershireCityClass(), new PrincipaliaClass(),
            /* Downpour   */  new TriptonClass(), new FallvilleClass(), new SardoothClass(),
            /* Flute      */  new ValiceClass(), new ValenfallClass(), new FortSigilClass(),
            /* Deltora    */  new SanguionClass(), new LantonumClass(),
            /* Parriwey   */  new ParceonClass(), new CesuraClass(),
            /* Chin'tor   */  new AmbercreekClass(), new MardovianCavernsClass(), new MtFalenkarthClass(),
            /* Camberlite */  new DewfrostClass(), new ClayroostClass(), new RavenstoneClass(), new CapwildClass(),
            /* Whitlock   */  new SimphetClass(), new WhistumnClass(), new HatchnukClass(),
            /* Koh'rin    */  new TrintooliClass(), new FoqwhitteClass(), new DonkohrinClass(),
            /* Pelamora   */  new CoranOutpostClass(), new RymnOutpostClass(),
            /* Celemia    */  new ParvocStrongholdClass()
            /* Thex       */  
        };

        public static List<Town> GetTownList()
        {
            return town_list;
        }

        public static Town FindTownWithID(string town_id)
        {
            return GetTownList().Single(x => x.TownID == town_id);
        }

        public static bool SearchForTowns(bool enter_if_found)
        {
            List<string> town_list = TileManager.FindTileWithID(CInfo.CurrentTile).TownList;

            if (town_list.Count == 0)
            {
                return false;
            }

            else
            {
                if (enter_if_found)
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
            Console.WriteLine($"Welcome to {TownName}!\n");

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
        public string TavernName { get; set; }

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
                        GameLoopManager.PlayerStatsCommand();

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
            while (true)
            {
                Console.WriteLine("Where do you want to go?");
                Console.WriteLine("      [1] General Store");
                Console.WriteLine($"      [2] {TavernName}");
                Console.WriteLine("      [3] Houses");

                while (true)
                {
                    string selection = CMethods.SingleCharInput("Input [#] (or type 'exit'): ").ToLower();

                    if (selection == "1")
                    {
                        OtherMusic.PlayLooping();
                        VisitGeneralStore();
                        CMethods.PrintDivider();
                        TownMusic.PlayLooping();

                        break;
                    }

                    else if (selection == "2")
                    {
                        OtherMusic.PlayLooping();
                        VisitInn();
                        CMethods.PrintDivider();
                        TownMusic.PlayLooping();

                        break;
                    }

                    else if (selection == "3")
                    {
                        OtherMusic.PlayLooping();
                        ChooseHouse();
                        CMethods.PrintDivider();
                        TownMusic.PlayLooping();

                        break;
                    }

                    else if (selection.IsExitString())
                    {
                        return;
                    }
                }
            }
        }

        protected void VisitInn()
        {
            CMethods.PrintDivider();
            Console.WriteLine($"Inn Keeper: \"Greetings, Traveler! Welcome to {TavernName}!\"");

            while (true)
            {
                string yes_no = CMethods.SingleCharInput("Inn Keeper: \"Would you like to stay at our inn? It's free, y'know!\" ").ToLower();

                if (yes_no.IsYesString())
                {
                    CMethods.PrintDivider();
                    Console.WriteLine("Inn Keeper: \"Goodnight, Traveler.\"");
                    Console.Write("Sleeping");

                    foreach (char c in "... ")
                    {
                        CMethods.SmartSleep(750);
                        Console.Write(c);
                    }

                    Console.WriteLine();

                    UnitManager.HealAllPCUs(true, true, true, true);

                    CMethods.PrintDivider();
                    Console.WriteLine("Your party's HP and MP have been fully restored.");
                    Console.WriteLine("Your party has been relieved of all status ailments.");
                    CMethods.PressAnyKeyToContinue();

                    CMethods.PrintDivider();
                    SavefileManager.WouldYouLikeToSave();

                    CMethods.PrintDivider();
                    Console.WriteLine("Inn Keeper: \"Thanks for staying, come again soon!\"");
                    CMethods.PressAnyKeyToContinue();

                    return;
                }

                else if (yes_no.IsNoString())
                {
                    return;
                }
            }
        }

        protected void VisitGeneralStore()
        {
            CMethods.PrintDivider();
            Console.WriteLine("Store Clerk: \"Welcome, Traveler!\"");

            while (true)
            {
                string choice = CMethods.SingleCharInput("Are you looking to [b]uy or [s]ell? | Input letter (or type 'exit'): ").ToLower();

                if (choice.StartsWith("b"))
                {
                    CMethods.PrintDivider();
                    GeneralStoreChooseItemToBuy();
                }

                else if (choice.StartsWith("s"))
                {
                    CMethods.PrintDivider();
                    GeneralStoreSellItem();
                }

                else if (choice.IsExitString())
                {
                    return;
                }
            }
        }

        protected void GeneralStoreChooseItemToBuy()
        {
            int highest_charisma = UnitManager.GetAllPCUs().Max(x => x.Attributes[CEnums.PlayerAttribute.charisma]);
            double cost_divisor = Math.Min(1 + (0.005 * highest_charisma), 2);

            while (true)
            {
                List<Item> stock = GenStock.Select(x => ItemManager.FindItemWithID(x)).ToList();
                Console.WriteLine($"\"Here's what we've got\": (You have {CInfo.GP} GP) ");

                int counter = 0;
                foreach (Item item in stock)
                {
                    string padding = new string('-', stock.Max(x => x.ItemName.Length) - item.ItemName.Length);
                    int modified_value = (int)(item.Value / cost_divisor);

                    Console.WriteLine($"      [{counter + 1}] {item.ItemName} {padding}-> {modified_value} GP");
                    counter++;
                }

                while (true)
                {
                    string choice = CMethods.FlexibleInput("Input [#] (or type 'exit'): ", stock.Count).ToLower();
                    Item chosen;
                    int modified_value;

                    try
                    {
                        chosen = stock[int.Parse(choice) - 1];
                        modified_value = (int)(chosen.Value / cost_divisor);
                    }

                    catch (Exception ex) when (ex is FormatException || ex is ArgumentOutOfRangeException)
                    {
                        if (choice.IsExitString())
                        {
                            CMethods.PrintDivider();
                            return;
                        }

                        continue;
                    }

                    CMethods.PrintDivider();
                    Console.WriteLine($"{chosen.ItemName}");
                    Console.WriteLine($"{chosen.Description}");
                    CMethods.PrintDivider();

                    GeneralStoreBuyYesOrNo(chosen, modified_value);
                    break;
                }
            }
        }

        protected void GeneralStoreBuyYesOrNo(Item item, int modified_value)
        {
            while (true)
            {
                string yes_no = CMethods.SingleCharInput($"\"That {item.ItemName} will cost ya {modified_value} GP. Wanna buy it?\" ").ToLower();

                if (yes_no.IsYesString())
                {
                    if (CInfo.GP >= modified_value)
                    {
                        CInfo.GP -= modified_value;
                        InventoryManager.AddItemToInventory(item.ItemID);

                        CMethods.PrintDivider();
                        Console.WriteLine($"You purchase the {item.ItemName} for {modified_value} GP.");
                        CMethods.PressAnyKeyToContinue();
                    }

                    else
                    {
                        Console.WriteLine($"Oi you can't afford this! Get outta here!");
                        CMethods.PressAnyKeyToContinue();
                    }

                    CMethods.PrintDivider();

                    return;
                }

                else if (yes_no.IsNoString())
                {
                    CMethods.PrintDivider();
                    return;
                }
            }
        }

        protected void GeneralStoreSellItem()
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
                        GameLoopManager.PlayerStatsCommand();

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

            Description = 
@"Nearton is a small, humble village in the Overshire Province. It is located 
within the Forest Plains, a small section of the Central Forest that
was chopped down hundreds of years ago to make way for what was to be the next
mega-city. As can be seen, that mega-city never took off, and Nearton turned
out to be as simple and ordinary as can be. This village is known for farming
and not much else. Just like all towns, it has a General Store and an Inn.
Not a bad place to start an adventure.";

            TavernName = "The Traveling Merchant";
            TownID = "town_nearton";

            TownMusic = SoundManager.town_main_cheery;
            OtherMusic = SoundManager.town_other_cheery;

            People = new List<string>();
            Houses = new List<string>();

            GenStock = new List<string>()
            {
                "s_potion",
                "s_elixir",
                "festive_clothes",
                "grass_amulet",
                "shovel_tool",
                "copper_lockpick"
            };
        }
    }
    
    public sealed class SouthfordClass : MarketTown
    {
        public SouthfordClass()
        {
            TownName = "Southford";

            Description = 
@"Southford is a fair-size town within the Forest Plains. Much larger and
busier than Nearton, Southford has become known as a haven for some of Brumia's
most lively individuals: the Bards. While bards are common in every town under
the sun, in Southford there is not a place in sight where one cannot see the 
colorful attire and hear the splendid tales of a bard. One cannot even escape
the music in their own home! This can all be attributed to the rise of the
Bard's Guild, located at The Dancing Jester Inn. An advantage to being in a
town full of bards is that bards tend to be very observant. Speaking to one
will undoubtably grant you helpful information about this world!";

            TavernName = "The Dancing Jester";
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

            Description = 
@"Overshire City is the capital and largest city in the Kingdom of Harconia.
It is named after the Overshire Watchmen who guard its borders. Originally a 
rag-tag group of vigilantes, the Watchmen fought monsters and bandits for 
decades using their unparalled archery skills. They were eventually hired 
full-time by to guard what was to become the capital of the kingdom. 
Eventually, the Watchmen branched out and now run the largest Ranger's Guild
in the world, located in the heart of the city. Being the capital, this is
also where the King or Queen live, except during the third month of every
year where they live in Principalia. Luckily, it is not currently the third
month of the year. Don't expect to be able to meet with the King so easily, 
though.";

            TavernName = "The Wandering Falcon";
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

            Description = 
@"Hundreds of years ago, King Pyravia II the construction of a new city that
eventually came to be known as Principalia. Pyravia II was a man with strange 
superstitions. He believed that the Kingdom's capital, Overshire City, had 
been cursed, and that the third month of every year was when the curse was at 
its strongest. Principalia was intended to be his home during that month, and 
since then it's been a tradition that every third month of the year the current 
King or Queen leaves Overshire City to live here. Unfortunately, it is not 
currently the third month of the year. Not that you would have been able to
meet with the King anyway.";

            TavernName = "The Drunken Moon";
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

    /* =========================== *
     *        DOWNPOUR TOWNS       *
     * =========================== */
    public sealed class TriptonClass : MarketTown
    {
        public TriptonClass()
        {
            TownName = "Tripton";
            Description = 
@"When the town of Tripton was being built, the people working on the
project failed to notice that another town, Fallville, just so happened to be
located mere meters away from the new town's borders. Merchants in Tripton
became very successful, as their superior bartering tactics allowed them to
easily steal business from Fallvillian merchants. This has led to a bitter,
and sometimes violent, rivalry between the two towns, particularly between the
village leaders.";
            TavernName = "The Tainted Tunic";
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
            Description = 
@"When the town of Tripton was being built, the people working on the
project failed to notice that another town, Fallville, just so happened to be
located mere meters away from the new town's borders. Merchants in Tripton
became very successful, as their superior bartering tactics allowed them to
easily steal business from Fallvillian merchants. This has led to a bitter,
and sometimes violent, rivalry between the two towns, particularly between the
village leaders.";
            TavernName = "The Jolly Juggler";
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

    public sealed class SardoothClass : PeopleTown
    {
        public SardoothClass()
        {
            TownName = "Sardooth";
            Description =
@"Sardooth is a ghost town without a single permanent inhabitant. This town
was hit the hardest by the latest wave of monsters, causing it to turn from
the bustling hub of commerce and culture to a barren wasteland within just 
six months. Everyone who lived here was either killed or driven out by the 
monsters, and the King's troops were powerless to stop it. The only thing of
note is 'The Undershire', a massive cemetery to the northeast, which is 
rumored to be even more dangerous than here.";
            TownID = "town_sardooth";

            TownMusic = SoundManager.town_main_moody;
            OtherMusic = SoundManager.town_other_moody;

            People = new List<string>();
            Houses = new List<string>();
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
            Description = 
@"Valice is a massive town in the Province of Overshire. Despite its immense 
size, comparable to that of Overshire City, Valice has little to offer. Back 
during the Harconian Gem Rush, when thousands of tons of gems and ore were 
discovered to be lying beneath the surface of Valice, the town grew 
tremendously in both size and wealth. This wealth did not last, as the gems 
quickly became rarer and rarer and are now nowhere to be seen. This, 
unfortunately, means that Valice is both one of the biggest towns in Overshire,
and also one of the poorest.";
            TavernName = "The Painted Bard";
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
            Description = 
@"Valenfall is an ancient city, belived to have been created by a now extinct 
race of sky beings. The city used to be situated on a large, floating island
known as the Aether, before it came crashing down. Towns located below the
Aether were forced to evacuate to save themselves from the impending impact.
Strangely, all of the Aether, including Valenfall, was devoid of any life.
Citizens of the now-destroyed towns decided to take over the empty town of
Valenfall which managed to survive falling to Harconia. It is unknown how
the Aether floated in the air or why it stopped.";
            TavernName = "The Roudy Knight";
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
            TavernName = "The Cowardly Dagger";
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

    /* =========================== *
     *        DELTORA TOWNS        *
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
            TavernName = "The Spooky Snapdragon";
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
            TavernName = "The Rise and Shine";
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
            TavernName = "The Thirsty Wizard";
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

    public sealed class CesuraClass : MarketTown
    {
        public CesuraClass()
        {
            TownName = "Cesura";
            Description = "";
            TavernName = "The Peaceful Sparrow";
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

    /* =========================== *
     *        CHIN'TOR TOWNS       *
     * =========================== */
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
            TavernName = "The Digging Dwarf";
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
    
    public sealed class MardovianCavernsClass : MarketTown
    {
        public MardovianCavernsClass()
        {
            TownName = "Mardovian Caverns";
            Description = "";
            TavernName = "The Smiling Rapier";
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
            TavernName = "The Golden Watchman";
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

    /* =========================== *
     *       CAMBERLITE TOWNS      *
     * =========================== */
    public sealed class DewfrostClass : MarketTown
    {
        public DewfrostClass()
        {
            TownName = "Dewfrost";
            Description = "";
            TavernName = "The Brave Foal";
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
            TavernName = "The Joyful Goose";
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
            TavernName = "The Healthy Vegetable";
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
            TavernName = "The Loyal Weasel";
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
            TavernName = "The Simple Squire";
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
            TavernName = "The Fearsome Ferret";
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
    
    public sealed class HatchnukClass : PeopleTown
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

            TownMusic = SoundManager.town_main_moody;
            OtherMusic = SoundManager.town_other_moody;

            People = new List<string>();
            Houses = new List<string>();
        }
    }

    /* =========================== *
     *         KOHRIN TOWNS        *
     * =========================== */    
    public sealed class TrintooliClass : MarketTown
    {
        public TrintooliClass()
        {
            TownName = "Trintooli";
            Description = "";
            TavernName = "The Noble Soldier";
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
            TavernName = "The Whimsical Whistle";
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
            TavernName = "The Skillful Squirrel";
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
    public sealed class RymnOutpostClass : MarketTown
    {
        public RymnOutpostClass()
        {
            TownName = "Rymn Outpost";
            Description = @"Rymn Outpost is one of the several small villages established
after the Thexian Incursion. All of the residents of this town are soldiers or
family members of soldiers, with the exception a few merchants. Rymn Outpost
is named after Rymnes, the Divinic gods of defense.";
            TavernName = "The Vanishing Skull";
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

    public sealed class CoranOutpostClass : MarketTown
    {
        public CoranOutpostClass()
        {
            TownName = "Coran Outpost";
            Description = "";
            TavernName = "The Howling Warrior";
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
     *        CELEMIA TOWNS        *
     * =========================== */
    public sealed class ParvocStrongholdClass : MarketTown
    {
        public ParvocStrongholdClass()
        {
            TownName = "The Parvoc Stronghold";
            Description = @"New Ekanmar is the capital of Celemia, one of the Harconian provinces. Prior
to the Harconian Revolution, this town was the location of a large portion of
Flyscoria's troops in Harconia. The Harconians drove much of them out, but
a large number of them defected to the Harconian side and stayed. After the
war, the citizens gave up their weapons and became a peaceful town. The vast
majority of the inhabitants of this town are, naturally, Flyscors. It seems
that the Flyscorian Royal Family is visiting here - perhaps you can talk with
them for a bit.";
            TavernName = "The Last Resort";
            TownID = "town_parvoc_stronghold"; 

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
     *          THEX TOWNS         *
     * =========================== */

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
