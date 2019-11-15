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
using System.Threading;
using System.Windows.Media;

namespace Game
{
    public static class SoundManager
    {
        private const string sound_fx_location = "Data/Sound FX";
        private const string music_location = "Data/Music";

        /* =========================== *
         *        SOUND EFFECTS        *
         * =========================== */
        #region
        // Sword Slash -- Played when you attempt to physically attack an enemy
        public static readonly MediaWrapper sword_slash = new MediaWrapper($"{sound_fx_location}/sword_slash.wav", CEnums.SoundType.soundfx);

        // Magical Attack -- Played when you attempt to use a magical attack
        public static readonly MediaWrapper magic_attack = new MediaWrapper($"{sound_fx_location}/magic_attack.wav", CEnums.SoundType.soundfx);

        // Magic Healing -- Played when you attempt to use a magical healing spell
        public static readonly MediaWrapper magic_healing = new MediaWrapper($"{sound_fx_location}/magic_healing.wav", CEnums.SoundType.soundfx);

        // Enemy-hit -- Played when the enemy is hit by a player attack
        public static readonly MediaWrapper enemy_hit = new MediaWrapper($"{sound_fx_location}/enemy_hit.wav", CEnums.SoundType.soundfx);

        // Foot-steps -- Played when you move on the overworld
        public static readonly MediaWrapper foot_steps = new MediaWrapper($"{sound_fx_location}/foot_steps.wav", CEnums.SoundType.soundfx);

        // Aim Weapon -- Played when attempting to attack with a ranged weapon
        public static readonly MediaWrapper aim_weapon = new MediaWrapper($"{sound_fx_location}/aim_weapon.wav", CEnums.SoundType.soundfx);

        // Attack Miss -- Played when attempting to attack and then failing
        public static readonly MediaWrapper attack_miss = new MediaWrapper($"{sound_fx_location}/attack_miss.wav", CEnums.SoundType.soundfx);

        // Got Item -- Played when you receive an item, GP, or XP
        public static readonly MediaWrapper item_pickup = new MediaWrapper($"{sound_fx_location}/item_pickup.wav", CEnums.SoundType.soundfx);

        // Low Health -- Played when you have low (less than 20%) health remaining
        public static readonly MediaWrapper health_low = new MediaWrapper($"{sound_fx_location}/health_low.wav", CEnums.SoundType.soundfx);

        // Poison Damage -- Played when the player or enemy take poison damage
        public static readonly MediaWrapper poison_damage = new MediaWrapper($"{sound_fx_location}/poison_damage.wav", CEnums.SoundType.soundfx);

        // Use Buff Spell -- Played when the player or enemy use a buff spell
        public static readonly MediaWrapper buff_spell = new MediaWrapper($"{sound_fx_location}/buff_spell.wav", CEnums.SoundType.soundfx);

        // Ally Death -- Played when a member of your party dies
        public static readonly MediaWrapper ally_death = new MediaWrapper($"{sound_fx_location}/ally_death.wav", CEnums.SoundType.soundfx);

        // Enemy Death -- Played when a member of the enemy team dies
        public static readonly MediaWrapper enemy_death = new MediaWrapper($"{sound_fx_location}/enemy_death.wav", CEnums.SoundType.soundfx);

        // Critical Hit -- Played when someone lands a critical hit
        public static readonly MediaWrapper critical_hit = new MediaWrapper($"{sound_fx_location}/critical_hit.wav", CEnums.SoundType.soundfx);

        // Lockpick Break -- Played when failing to pick a lock
        public static readonly MediaWrapper lockpick_break = new MediaWrapper($"{sound_fx_location}/lockpick_break.wav", CEnums.SoundType.soundfx);

        // Lockpicking -- Played when attempting to pick a lock
        public static readonly MediaWrapper lockpicking = new MediaWrapper($"{sound_fx_location}/lockpicking.wav", CEnums.SoundType.soundfx);

        // Unlock Chest -- Played when succeeding to pick a lock
        public static readonly MediaWrapper unlock_chest = new MediaWrapper($"{sound_fx_location}/unlock_chest.wav", CEnums.SoundType.soundfx);

        // Debuff -- Played when the player suffers from a debuff
        public static readonly MediaWrapper debuff = new MediaWrapper($"{sound_fx_location}/debuff.wav", CEnums.SoundType.soundfx);

        // Ability cast -- Used when non-magical abilities are casted
        public static readonly MediaWrapper ability_cast = new MediaWrapper($"{sound_fx_location}/ability_cast.wav", CEnums.SoundType.soundfx);

        // Potion Brew -- Used when brewing potions
        public static readonly MediaWrapper potion_brew = new MediaWrapper($"{sound_fx_location}/potion_brew.wav", CEnums.SoundType.soundfx);

        // Eerie Sound -- No current use
        public static readonly MediaWrapper eerie_sound = new MediaWrapper($"{sound_fx_location}/eerie_sound.wav", CEnums.SoundType.soundfx);

        // Random encounter -- No current use
        public static readonly MediaWrapper random_enc = new MediaWrapper($"{sound_fx_location}/random_encounter.wav", CEnums.SoundType.soundfx);

        // Bard sounds -- Used when playing a bard instrument
        public static readonly Dictionary<string, MediaWrapper> bard_sounds = new Dictionary<string, MediaWrapper>()
        {
            { "snare_drum", new MediaWrapper($"{sound_fx_location}/Bard Sounds/snare_1.wav", CEnums.SoundType.soundfx) },
            { "violin", new MediaWrapper($"{sound_fx_location}/Bard Sounds/violin_1.wav", CEnums.SoundType.soundfx) },
            { "flute", new MediaWrapper($"{sound_fx_location}/Bard Sounds/flute_1.wav", CEnums.SoundType.soundfx) },
            { "trumpet", new MediaWrapper($"{sound_fx_location}/Bard Sounds/trumpet_1.wav", CEnums.SoundType.soundfx) },
            { "kazoo", new MediaWrapper($"{sound_fx_location}/Bard Sounds/kazoo_1.wav", CEnums.SoundType.soundfx) },
            { "bagpipes", new MediaWrapper($"{sound_fx_location}/Bard Sounds/bagpipes_1.wav", CEnums.SoundType.soundfx) },
            { "lyre", new MediaWrapper($"{sound_fx_location}/Bard Sounds/harp_2.wav", CEnums.SoundType.soundfx) }
        };

        private static readonly List<MediaWrapper> sound_list = new List<MediaWrapper>()
        {
            sword_slash,
            magic_attack,
            magic_healing,
            enemy_hit,
            foot_steps,
            aim_weapon,
            attack_miss,
            item_pickup,
            health_low,
            poison_damage,
            buff_spell,
            ally_death,
            enemy_death,
            critical_hit,
            lockpick_break,
            lockpicking,
            unlock_chest,
            debuff,
            potion_brew,
            eerie_sound,
            random_enc,
            bard_sounds["snare_drum"],
            bard_sounds["violin"],
            bard_sounds["flute"],
            bard_sounds["trumpet"],
            bard_sounds["kazoo"],
            bard_sounds["bagpipes"],
            bard_sounds["lyre"]
        };
        #endregion

        /* =========================== *
         *            MUSIC            *
         * =========================== */
        #region
        // Music that plays when you level up
        public static readonly string levelup_music = $"{music_location}/Adventures in Pixels.wav";

        // Music that plays when your party loses a battle
        public static readonly string gameover_music = $"{music_location}/Power-Up.wav";

        // Music that plays when your party wins a battle
        public static readonly string victory_music = $"{music_location}/Rolemusic Python.wav";

        // Music that plays during the credits
        public static readonly string credits_music = $"{music_location}/Credits Music for an 8-Bit RPG.wav";

        // Music that plays on the title screen
        public static readonly string title_music = $"{music_location}/Adam Haynes Prologue.wav";

        // Music that plays inside cheery towns
        public static readonly string town_main_cheery = $"{music_location}/Chickens (going peck peck peck).wav";

        // Music that plays in cheery towns when talking to NPCs OR when sneaking inside a house
        public static readonly string town_other_cheery = $"{music_location}/Mayhem in the Village.wav";

        // Music that plays inside moody towns
        public static readonly string town_main_moody = $"{music_location}/song_14_04.wav";

        // Music that plays in moody towns when talking to NPCs OR when sneaking inside a house
        public static readonly string town_other_moody = $"{music_location}/song21_02.wav";

        // Music that plays during a boss battle
        public static readonly string battle_music_boss = $"{music_location}/Terrible Tarantuloid.wav";

        // Music that plays during a battle with "Animal" type monsters
        public static readonly string battle_music_animal = $"{music_location}/Shingle Tingle.wav";

        // Music that plays during a battle with "Monster" type monsters
        public static readonly string battle_music_monster = $"{music_location}/Ruari 8-bit Battle.wav";

        // Music that plays during a battle with "Humanoid" type monsters
        public static readonly string battle_music_humanoid = $"{music_location}/Adam Haynes Boss Battle.wav";

        // Music that plays during a battle with "Undead" type monsters
        public static readonly string battle_music_undead = $"{music_location}/song_37_03.wav";

        // Music that plays during a battle with "Dungeon" type monsters
        public static readonly string battle_music_dungeon = $"{music_location}/Indescriminate.wav";

        // Music that plays in forested areas
        public static readonly string area_forest_music = $"{music_location}/Through the Forest.wav";

        // Music that plays in haunted areas
        public static readonly string area_haunted_music = $"{music_location}/song17_02.wav";

        // Music that plays inside a dungeon
        public static readonly string area_dungeon_music = $"{music_location}/Eight_Bit_Dungeon_Monster_Stomp.wav";

        // Music that plays inside a castle
        public static readonly string area_castle_music = $"{music_location}/Castle.wav";

        // Music that plays in mountainous areas
        public static readonly string area_mountain_music = $"{music_location}/Mountain.wav";
        #endregion

        // Method that plays the music from the current cell
        public static void PlayCellMusic()
        {
            MusicPlayer.PlaySong(TileManager.FindCellWithTileID(CInfo.CurrentTile).Music, -1);
        }

        public static void CleanupSoundFX()
        {
            foreach (MediaWrapper soundfx in sound_list)
            {
                if (soundfx.Position == soundfx.NaturalDuration)
                {
                    soundfx.Close();
                }
            }
        }
    }

    public class MediaWrapper : MediaPlayer
    {
        public string URI { get; set; }
        public CEnums.SoundType SoundType { get; set; }

        public void SmartPlay()
        {
            Open(new Uri(URI, UriKind.Relative));
            
            if (SoundType == CEnums.SoundType.music)
            {
                Volume = SettingsManager.music_vol;
            }
            
            if (SoundType == CEnums.SoundType.soundfx)
            {
                Volume = SettingsManager.sound_vol;
                SoundManager.CleanupSoundFX();
            }
            
            Play();
        }

        // Using this class, we can assign the MediaPlayer a URI inside the constructor
        public MediaWrapper(string uri, CEnums.SoundType sound_type) : base()
        {
            URI = uri;
            SoundType = sound_type;
        }
    }

    public static class MusicPlayer
    {
        public static bool MusicboxIsPlaying = false;
        public static Thread MusicboxThread;
        public static string MusicboxFolder = "";
        public static CEnums.MusicboxMode MusicboxMode = CEnums.MusicboxMode.AtoZ;

        private static Thread song_thread;
        private static bool stop_song;
        private static double? new_volume = null;

        // Set play_count to -1 for repeat
        public static void PlaySong(string sound_location, int play_count)
        {
            if (MusicboxIsPlaying)
            {
                return;
            }

            StopMusic();
            song_thread = new Thread(_ => SongThread(sound_location, play_count));
            song_thread.Start();
        }

        // Sends a flag to the SongThread to stop playing the current song and exit the thread
        public static void StopMusic()
        {
            stop_song = true;
            while (song_thread?.IsAlive == true) { }
            stop_song = false;
        }

        // Sends a flag to the SongThread to stop playing the current song and exit the thread
        public static void UpdateVolume(double new_vol)
        {
            new_volume = new_vol;
        }

        private static void SongThread(string sound_location, int play_count)
        {
            MediaWrapper the_song = new MediaWrapper(sound_location, CEnums.SoundType.music);

            // This for-loop causes the song to play 'play_count' number of times.
            // Because of the way the comparison is done, if you set play_count to -1 the song will loop infinitely.
            for (int i = 0; i != play_count; i++)
            {
                the_song.SmartPlay();
                while (the_song.Position < the_song.NaturalDuration)
                {
                    // If the stop_song flag is triggered, then stop the current song and exit the thread
                    if (stop_song)
                    {
                        stop_song = false;
                        the_song.Close();
                        return;
                    }

                    // If the new_volume flag is triggered, then update the currently playing song to the new volume
                    if (new_volume != null)
                    {
                        the_song.Volume = (double)new_volume;
                        new_volume = null;
                    }

                    // The SongThread will consume a LOT of processing power if you don't wait a bit between each loop
                    CMethods.SmartSleep(10);
                }
            }
        }
    }
}
