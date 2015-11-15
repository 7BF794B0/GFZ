// Game.cs
// The main class of the game.
// It contains all of the gameplay mechanics.
using System;
using System.Collections.Generic;
using System.Linq;
// Project forms.
using GFZ.Forms;

namespace GFZ.Model
{
    class Game
    {
        // List of all resources.
        private readonly IList<int> _lstResource = new List<int>();
        // The number of mines that we captured.
        private readonly int[] _myMine = { 0, 0, 0, 0, 0, 0, 0 };

        // Time.
        // Day.
        private int _day = 1;
        // Week.
        private int _week = 1;

        // Resource.
        public int Wood { get; set; } = 10;
        public int Stone { get; set; } = 10;
        public int Mercury { get; set; } = 5;
        public int Sulfur { get; set; } = 5;
        public int Crystals { get; set; } = 5;
        public int Gems { get; set; } = 5;
        public int Gold { get; set; } = 10000;
        // That is where we are today were it mine?
        public bool IsMine { get; private set; }

        #region getter/setter Время
        public int Day
        {
            get { return _day; }
            set
            {
                if (_day == 7)
                {
                    _day = 1;
                    Week++;
                }
                else
                    _day = value;
            }
        }

        public int Week
        {
            get { return _week; }
            set
            {
                if (_week == 4)
                {
                    _week = 1;
                    Month++;
                }
                else
                    _week = value;
            }
        }

        public int Month { get; set; } = 1;

        public int Turn { get; set; }
        #endregion

        // Battles.
        // 0 - victory over the monster
        // 1 - defeated by a monster
        // 2 - draw
        // 3 - a victory over squad
        // 4 - defeat of the squad.

        // Whether there has been, in general, the fight.
        public bool FightOccurred { get; private set; }
        // Win / Loss / Drawn.
        public int Win { get; set; }
        // Who participated in the battle.
        public string Actor { get; private set; } = string.Empty;
        // How to get experience.
        public int Experience { get; private set; }
        // WE WON?
        public bool GlobalWin { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Game()
        {
            UpdateListAllResource();
        }

        /// <summary>
        /// Reset all fields to the initial.
        /// It is used at the start of a new game or loading from save.
        /// </summary>
        public void ResetToDefault()
        {
            Wood = 10;
            Stone = 10;
            Mercury = 5;
            Sulfur = 5;
            Crystals = 5;
            Gems = 5;
            Gold = 10000;
            _lstResource.Clear();
            _lstResource.Add(Wood);
            _lstResource.Add(Stone);
            _lstResource.Add(Mercury);
            _lstResource.Add(Sulfur);
            _lstResource.Add(Crystals);
            _lstResource.Add(Gems);
            _lstResource.Add(Gold);

            IsMine = false;
            _myMine[0] = 0;
            _myMine[1] = 0;
            _myMine[2] = 0;
            _myMine[3] = 0;
            _myMine[4] = 0;
            _myMine[5] = 0;
            _myMine[6] = 0;
            
            _day = 1;
            _week = 1;
            Month = 1;
            Turn = 0;

            FightOccurred = false;
            Win = 0;
            Actor = string.Empty;
            Experience = 0;
        }

        /// <summary>
        /// Get a list of all resources.
        /// </summary>
        /// <returns>List all resources.</returns>
        public IList<int> GetListAllResource()
        {
            return _lstResource;
        }

        /// <summary>
        /// Take the number of captured mines.
        /// </summary>
        /// <returns>The number of trapped mines.</returns>
        public int[] GetCountMine()
        {
            return _myMine;
        }

        /// <summary>
        /// Specify the number of captured mines (used when loading a game from save).
        /// </summary>
        /// <param name="i">The index of the desired mine in the array.</param>
        /// <param name="value">The value that comes from save.</param>
        public void SetCountMine(int i, int value)
        {
            _myMine[i] = value;
        }

        /// <summary>
        /// Refresh a list of all resources.
        /// </summary>
        public void UpdateListAllResource()
        {
            _lstResource.Clear();

            // If we have captured mine, it is necessary to increase the amount of resources at the end of each stroke.
            Wood = Wood + (5 * _myMine[0]);
            Stone = Stone + (5 * _myMine[1]);
            Mercury = Mercury + (3 * _myMine[2]);
            Sulfur = Sulfur + (3 * _myMine[3]);
            Crystals = Crystals + (3 * _myMine[4]);
            Gems = Gems + (3 * _myMine[5]);
            Gold = Gold + (500 * _myMine[6]);

            _lstResource.Add(Wood);
            _lstResource.Add(Stone);
            _lstResource.Add(Mercury);
            _lstResource.Add(Sulfur);
            _lstResource.Add(Crystals);
            _lstResource.Add(Gems);
            _lstResource.Add(Gold);
        }

        /// <summary>
        /// Function to update the level of squad by resources.
        /// </summary>
        public void UpdateSquad()
        {
            Wood = Wood - 20;
            Stone = Stone - 20;
            Mercury = Mercury - 10;
            Sulfur = Sulfur - 10;
            Crystals = Crystals - 10;
            Gems = Gems - 10;
            Gold = Gold - 20000;
            _lstResource.Clear();
            _lstResource.Add(Wood);
            _lstResource.Add(Stone);
            _lstResource.Add(Mercury);
            _lstResource.Add(Sulfur);
            _lstResource.Add(Crystals);
            _lstResource.Add(Gems);
            _lstResource.Add(Gold);
        }

        /// <summary>
        /// The function that implements the process of battle.
        /// </summary>
        /// <param name="monster">Object class Unit is a monster, which will be a fight.</param>
        /// <param name="squad">Object class Squad is the squad that attacked the monster.</param>
        private void Fight(Unit monster, Squad squad)
        {
            FightOccurred = true;
            var rnd = new Random();
            int squadDamage = squad.Damage, monsterDamage = monster.Damage;

            // The probability of about 10 % - Critical damage(at the squad).
            if (rnd.Next(0, 11) == 5)
                squadDamage = squadDamage + (squad.Damage / 2);
            // The probability of about 10% - miss (at the squad)
            if (rnd.Next(0, 11) == 5)
                squadDamage = squadDamage - (squadDamage / 10);

            // The probability of about 10 % - Critical Damage(the monster).
            if (rnd.Next(0, 11) == 5)
                monsterDamage = monsterDamage + (monster.Damage / 2);
            // The probability of about 10% - miss (the monster).
            if (rnd.Next(0, 11) == 5)
                monsterDamage = monsterDamage - (monsterDamage / 10);

            // Defeat.
            if (squad.Hp < monsterDamage)
            {
                Win = 1;
                Actor = monster.Name;
                return;
                // At this point the game ends, the processing of this event takes place in the View.
            }

            // Victory.
            if (squadDamage > monster.Hp)
            {
                Win = 0;
                Actor = monster.Name;
                Experience = monster.Experience;
                // Add experience.
                squad.Experience = monster.Experience;
                // Make damage.
                squad.Hp -= monsterDamage;
                return;
            }

            if (squadDamage >= monster.Hp || squad.Hp <= monsterDamage) return;

            Win = 2;
            Actor = monster.Name;
            squad.Hp -= monsterDamage;
            monster.Hp -= squadDamage;
        }

        /// <summary>
        /// The battle is squad vs squad.
        /// public because it uses AI.
        /// </summary>
        /// <param name="attacking">Squad class object is the squad that attacked.</param>
        /// <param name="protecting">Squad class object is the squad that defends.</param>
        public void SquadVsSquad(Squad attacking, Squad protecting)
        {
            // This function works the same way as the function Fight() see it.
            FightOccurred = true;
            var rnd = new Random();
            int attackingDamage = attacking.Damage, protectingDamage = protecting.Damage;

            if (rnd.Next(0, 11) == 5)
                attackingDamage = attackingDamage + (attacking.Damage / 2);
            if (rnd.Next(0, 11) == 5)
                attackingDamage = attackingDamage - (attackingDamage / 10);

            if (rnd.Next(0, 11) == 5)
                protectingDamage = protectingDamage + (protecting.Damage / 2);
            if (rnd.Next(0, 11) == 5)
                protectingDamage = protectingDamage - (protectingDamage / 10);

            // Defeat.
            if (attacking.Hp < protectingDamage)
            {
                Win = 4;
                Actor = "Squad of the enemy";
                return;
            }

            // Win: A double condition, nobody here can not be, so the winner of the one who has more initiative (the one who attacked first).
            if ((attackingDamage <= protecting.Hp) &&
                (attackingDamage >= protecting.Hp || attacking.Hp <= protectingDamage)) return;

            Win = 3;
            Actor = "Squad of the enemy";
            Experience = protecting.SumExperience;
            attacking.Experience = protecting.SumExperience;
            attacking.Hp -= attackingDamage;
        }

        /// <summary>
        /// The course that the player wants to make all possible?
        /// </summary>
        /// <param name="now">id contents of a cell, where we go.</param>
        /// <param name="previous">id contents of a cell, from we go.</param>
        /// <param name="nowName">The name of the cell, where we go.</param>
        /// <param name="previousName">The name of the cell from we go.</param>
        /// <param name="squadPl">An object class represents Squad Player.</param>
        /// <param name="squadAi">An object class represents Squad Computer.</param>
        /// <param name="monsters">Array monsters in the game.</param>
        /// <returns>True - it is possible; False - impossible.</returns>
        public bool SuchActionIsPossible(int now, int previous, string nowName, string previousName, Squad squadPl, Squad squadAi, Unit[] monsters)
        {
            // Align the battle flag of the event to the default.
            FightOccurred = false;
            // Align the flag of the event capture shaft to the default.
            IsMine = false;
            // Remove all of the letters of a name (for diversity through the regular season) - get the coordinates.
            var coordinatesNow = Convert.ToInt32(System.Text.RegularExpressions.Regex.Replace(nowName, @"[^\d]", ""));
            var coordinatesPrevious = Convert.ToInt32(System.Text.RegularExpressions.Regex.Replace(previousName, @"[^\d]", ""));

            if (coordinatesPrevious + 20 != coordinatesNow && // Do not up.
                coordinatesPrevious - 20 != coordinatesNow && // Do not back.
                coordinatesPrevious + 1 != coordinatesNow && // Not right.
                coordinatesPrevious - 1 != coordinatesNow) // Not left.
                return false;

            // If you go to the free cell, then immediately return true (the idea should be the most common course).
            if (now == 0) return true;

            // Objects that can not walk.
            int[] plant = { 1, 2, 11, 12, 13, 14, 15, 16, 17, 18 };
            // Mines.
            int[] mine = { 4, 5, 6, 7, 8, 9, 10 };

            if (plant.Any(t => now == t))
                return false;

            // Checking on resources.
            switch (now)
            {
                case 19:
                    Gold += 500;
                    squadPl.Experience = 100;
                    break;
                case 20:
                    Wood += 5;
                    break;
                case 21:
                    Stone += 5;
                    break;
                case 22:
                    Mercury += 3;
                    break;
                case 23:
                    Sulfur += 3;
                    break;
                case 24:
                    Crystals += 3;
                    break;
                case 25:
                    Gems += 3;
                    break;
                case 26:
                    Gold += 500;
                    break;
            }

            UpdateListAllResource();

            // Checking for monsters.
            if (now > 28 && now < 43)
            {
                Fight(monsters[42 - now], squadPl);
                return true;
            }

            // In the course of this battle was not.
            FightOccurred = false;

            // Check for mines.
            for (var i = 0; i < mine.Length; i++)
            {
                // If this mine and it has not yet invaded.
                if (now == mine[i] && _myMine[i] == 0)
                {
                    // Increase the number of trapped mine type and unit.
                    _myMine[i]++;
                    IsMine = true;
                    return false;
                }

                // If this mine is already captured.
                if (now == mine[i] && _myMine[i] > 0)
                    return false;
            }

            switch (now)
            {
                case 28:
                    SquadVsSquad(squadPl, squadAi);
                    return true;
                case 3:
                    // Then we won.
                    GlobalWin = true;
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Filling the map various monsters.
        /// </summary>
        /// <param name="map">The array, which is a card game in the world.</param>
        /// <param name="view">View class object required to render these monsters on the game map.</param>
        /// <param name="load">Object shaped ProcessBar'om showing loading status.</param>
        public void Create(System.Windows.Forms.PictureBox[,] map, View.View view, FrmLoading load)
        {
            #region Bright side
            view.WeakMonsters(map, 9, 1);
            System.Threading.Thread.Sleep(1000);
            load.PbValueNow++;

            view.WeakMonsters(map, 8, 5);
            System.Threading.Thread.Sleep(1000);
            load.PbValueNow++;

            view.WeakMonsters(map, 5, 2);
            System.Threading.Thread.Sleep(1000);
            load.PbValueNow++;

            view.AverageMonsters(map, 6, 6);
            System.Threading.Thread.Sleep(1000);
            load.PbValueNow++;

            view.AverageMonsters(map, 4, 3);
            System.Threading.Thread.Sleep(1000);
            load.PbValueNow++;

            view.AverageMonsters(map, 3, 6);
            System.Threading.Thread.Sleep(1000);
            load.PbValueNow++;

            view.StrongMonsters(map, 1, 5);
            System.Threading.Thread.Sleep(1000);
            load.PbValueNow++;
            #endregion

            // Boss - Dragon
            view.BossesMonsters(map, 4, 11);
            System.Threading.Thread.Sleep(1000);
            load.PbValueNow++;

            #region Dark side
            view.WeakMonsters(map, 0, 18);
            System.Threading.Thread.Sleep(1000);
            load.PbValueNow++;

            view.WeakMonsters(map, 2, 14);
            System.Threading.Thread.Sleep(1000);
            load.PbValueNow++;

            view.WeakMonsters(map, 0, 11);
            System.Threading.Thread.Sleep(1000);
            load.PbValueNow++;

            view.AverageMonsters(map, 5, 18);
            System.Threading.Thread.Sleep(1000);
            load.PbValueNow++;

            view.AverageMonsters(map, 6, 14);
            System.Threading.Thread.Sleep(1000);
            load.PbValueNow++;

            view.AverageMonsters(map, 7, 11);
            System.Threading.Thread.Sleep(1000);
            load.PbValueNow++;

            view.StrongMonsters(map, 7, 16);
            System.Threading.Thread.Sleep(1000);
            load.PbValueNow++;
            #endregion
        }
    }
}