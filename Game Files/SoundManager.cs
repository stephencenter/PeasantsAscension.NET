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
using System.Media;
using System.Threading;
using System.Windows.Media;

namespace Game
{
    public static class SoundManager
    {
        private const string sound_fx_location = "Data/Sound FX";
        private const string music_location = "Data/Music";

        // =========================== //
        //        SOUND EFFECTS        //
        // =========================== //
        // Sword Slash -- Played when you attempt to physically attack an enemy
        public static readonly MediaWrapper sword_slash = new MediaWrapper($"{sound_fx_location}/sword_slash.wav");

        // Magical Attack -- Played when you attempt to use a magical attack
        public static readonly MediaWrapper magic_attack = new MediaWrapper($"{sound_fx_location}/magic_attack.wav");

        // Magic Healing -- Played when you attempt to use a magical healing spell
        public static readonly MediaWrapper magic_healing = new MediaWrapper($"{sound_fx_location}/magic_healing.wav");

        // Enemy-hit -- Played when the enemy is hit by a player attack
        public static readonly MediaWrapper enemy_hit = new MediaWrapper($"{sound_fx_location}/enemy_hit.wav");

        // Foot-steps -- Played when you move on the overworld
        public static readonly MediaWrapper foot_steps = new MediaWrapper($"{sound_fx_location}/foot_steps.wav");

        // Aim Weapon -- Played when attempting to attack with a ranged weapon
        public static readonly MediaWrapper aim_weapon = new MediaWrapper($"{sound_fx_location}/aim_weapon.wav");

        // Attack Miss -- Played when attempting to attack and then failing
        public static readonly MediaWrapper attack_miss = new MediaWrapper($"{sound_fx_location}/attack_miss.wav");

        // Got Item -- Played when you receive an item, GP, or XP
        public static readonly MediaWrapper item_pickup = new MediaWrapper($"{sound_fx_location}/item_pickup.wav");

        // Low Health -- Played when you have low (less than 20%) health remaining
        public static readonly MediaWrapper health_low = new MediaWrapper($"{sound_fx_location}/health_low.wav");

        // Poison Damage -- Played when the player or enemy take poison damage
        public static readonly MediaWrapper poison_damage = new MediaWrapper($"{sound_fx_location}/poison_damage.wav");

        // Use Buff Spell -- Played when the player or enemy use a buff spell
        public static readonly MediaWrapper buff_spell = new MediaWrapper($"{sound_fx_location}/buff_spell.wav");

        // Ally Death -- Played when a member of your party dies
        public static readonly MediaWrapper ally_death = new MediaWrapper($"{sound_fx_location}/ally_death.wav");

        // Enemy Death -- Played when a member of the enemy team dies
        public static readonly MediaWrapper enemy_death = new MediaWrapper($"{sound_fx_location}/enemy_death.wav");

        // Critical Hit -- Played when someone lands a critical hit
        public static readonly MediaWrapper critical_hit = new MediaWrapper($"{sound_fx_location}/critical_hit.wav");

        // Lockpick Break -- Played when failing to pick a lock
        public static readonly MediaWrapper lockpick_break = new MediaWrapper($"{sound_fx_location}/lockpick_break.wav");

        // Lockpicking -- Played when attempting to pick a lock
        public static readonly MediaWrapper lockpicking = new MediaWrapper($"{sound_fx_location}/lockpicking.wav");

        // Unlock Chest -- Played when succeeding to pick a lock
        public static readonly MediaWrapper unlock_chest = new MediaWrapper($"{sound_fx_location}/unlock_chest.wav");

        // Debuff -- Played when the player suffers from a debuff
        public static readonly MediaWrapper debuff = new MediaWrapper($"{sound_fx_location}/debuff.wav");

        // Ability cast -- Used when non-magical abilities are casted
        public static readonly MediaWrapper ability_cast = new MediaWrapper($"{sound_fx_location}/ability_cast.wav");

        // Potion Brew -- Used when brewing potions
        public static readonly MediaWrapper potion_brew = new MediaWrapper($"{sound_fx_location}/potion_brew.wav");

        // Eerie Sound -- No current use
        public static readonly MediaWrapper eerie_sound = new MediaWrapper($"{sound_fx_location}/eerie_sound.wav");

        // Random encounter -- No current use
        public static readonly MediaWrapper random_enc = new MediaWrapper($"{sound_fx_location}/random_encounter.wav");

        public static readonly Dictionary<string, MediaWrapper> bard_sounds = new Dictionary<string, MediaWrapper>()
        {
            { "snare_drum", new MediaWrapper($"{sound_fx_location}/Bard Sounds/snare_1.wav") },
            { "violin", new MediaWrapper($"{sound_fx_location}/Bard Sounds/violin_1.wav") },
            { "flute", new MediaWrapper($"{sound_fx_location}/Bard Sounds/flute_1.wav") },
            { "trumpet", new MediaWrapper($"{sound_fx_location}/Bard Sounds/trumpet_1.wav") },
            { "kazoo", new MediaWrapper($"{sound_fx_location}/Bard Sounds/kazoo_1.wav") },
            { "bagpipes", new MediaWrapper($"{sound_fx_location}/Bard Sounds/bagpipes_1.wav") },
            { "lyre", new MediaWrapper($"{sound_fx_location}/Bard Sounds/harp_2.wav") }
        };

        /* =========================== *
         *        GENERAL MUSIC        *
         * =========================== */
        // Music that plays when you level up
        public static readonly SoundPlayer levelup_music = new SoundPlayer($"{music_location}/Adventures in Pixels.wav");

        // Music that plays when your party loses a battle
        public static readonly SoundPlayer gameover_music = new SoundPlayer($"{music_location}/Power-Up.wav");

        // Music that plays when your party wins a battle
        public static readonly SoundPlayer victory_music = new SoundPlayer($"{music_location}/Rolemusic Python.wav");

        // Music that plays during the credits
        public static readonly SoundPlayer credits_music = new SoundPlayer($"{music_location}/Credits Music for an 8-Bit RPG.wav");

        // Music that plays on the title screen
        public static readonly SoundPlayer title_music = new SoundPlayer($"{music_location}/Adam Haynes Prologue.wav");

        /* =========================== *
         *          TOWN MUSIC         *
         * =========================== */
        // Music that plays inside cheery towns
        public static readonly SoundPlayer town_main_cheery = new SoundPlayer($"{music_location}/Chickens (going peck peck peck).wav");

        // Music that plays in cheery towns when talking to NPCs OR when sneaking inside a house
        public static readonly SoundPlayer town_other_cheery = new SoundPlayer($"{music_location}/Mayhem in the Village.wav");

        // Music that plays inside moody towns
        public static readonly SoundPlayer town_main_moody = new SoundPlayer($"{music_location}/song_14_04.wav");

        // Music that plays in moody towns when talking to NPCs OR when sneaking inside a house
        public static readonly SoundPlayer town_other_moody = new SoundPlayer($"{music_location}/song21_02.wav");

        /* =========================== *
         *         BATTLE MUSIC        *
         * =========================== */
        // Music that plays during a boss battle
        public static readonly SoundPlayer battle_music_boss = new SoundPlayer($"{music_location}/Terrible Tarantuloid.wav");

        // Music that plays during a battle with "Animal" type monsters
        public static readonly SoundPlayer battle_music_animal = new SoundPlayer($"{music_location}/Shingle Tingle.wav");

        // Music that plays during a battle with "Monster" type monsters
        public static readonly SoundPlayer battle_music_monster = new SoundPlayer($"{music_location}/Ruari 8-bit Battle.wav");

        // Music that plays during a battle with "Humanoid" type monsters
        public static readonly SoundPlayer battle_music_humanoid = new SoundPlayer($"{music_location}/Adam Haynes Boss Battle.wav");

        // Music that plays during a battle with "Undead" type monsters
        public static readonly SoundPlayer battle_music_undead = new SoundPlayer($"{music_location}/song_37_03.wav");

        // Music that plays during a battle with "Dungeon" type monsters
        public static readonly SoundPlayer battle_music_dungeon = new SoundPlayer($"{music_location}/Indescriminate.wav");

        /* =========================== *
         *          AREA MUSIC         *
         * =========================== */
        // Music that plays in forested areas
        public static readonly SoundPlayer area_forest_music = new SoundPlayer($"{music_location}/Through the Forest.wav");

        // Music that plays in haunted areas
        public static readonly SoundPlayer area_haunted_music = new SoundPlayer($"{music_location}/song17_02.wav");

        // Music that plays inside a dungeon
        public static readonly SoundPlayer area_dungeon_music = new SoundPlayer($"{music_location}/Eight_Bit_Dungeon_Monster_Stomp.wav");

        // Music that plays inside a castle
        public static readonly SoundPlayer area_castle_music = new SoundPlayer($"{music_location}/Castle.wav");

        // Music that plays in mountainous areas
        public static readonly SoundPlayer area_mountain_music = new SoundPlayer($"{music_location}/Mountain.wav");

        // Method that plays the music from the current cell
        public static void PlayCellMusic()
        {
            TileManager.FindCellWithTileID(CInfo.CurrentTile).Music.PlayLooping();
        }

        public static void StopAllMusic()
        {
            levelup_music.Stop();
            gameover_music.Stop();
            victory_music.Stop();
            credits_music.Stop();
            title_music.Stop();
            town_main_cheery.Stop();
            town_other_cheery.Stop();
            town_main_moody.Stop();
            town_other_moody.Stop();
            battle_music_boss.Stop();
            battle_music_animal.Stop();
            battle_music_monster.Stop();
            battle_music_humanoid.Stop();
            battle_music_undead.Stop();
            battle_music_dungeon.Stop();
            area_forest_music.Stop();
            area_haunted_music.Stop();
            area_dungeon_music.Stop();
            area_castle_music.Stop();
            area_mountain_music.Stop();
        }
    }

    public class MediaWrapper : MediaPlayer
    {
        public string URI { get; set; }

        public void SmartPlay()
        {
            Open(new Uri(URI, UriKind.Relative));
            Play();
        }

        // Using this class, we can assign the MediaPlayer a URI inside the constructor
        public MediaWrapper(string uri) : base()
        {
            URI = uri;
        }
    }

    public class MusicboxSong
    {
        public string SongFile { get; set; }
        public Thread SongThread { get; set; }
        public bool IsStopped { get; set; } = true;

        public void Play()
        {
            if (!IsStopped)
            {
                return;
            }

            SongThread = new Thread(PlayThread);
            SongThread.Start();
        }

        private void PlayThread()
        {
            IsStopped = false;
            SoundPlayer player = new SoundPlayer(SongFile);
            player.PlaySync();
            IsStopped = true;

            SongThread.Join();
        }

        public MusicboxSong(string file)
        {
            SongFile = file;
        }
    }
}
