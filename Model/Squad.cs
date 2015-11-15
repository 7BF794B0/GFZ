// Squad.cs
// Class to describe squad.
using System.Collections.Generic;

namespace GFZ.Model
{
    class Squad
    {
        // Experience an increase will raise squad.
        private int _experience;
        // The current total health squad (needed for the formula for calculating the fight).
        private int _sumHp;
        // Army squad.
        private readonly IList<Unit> _lstSquadArmy = new List<Unit>();

        #region getter/setter
        // Level = level of squad of all beings in the unit.
        public int Level { get; private set; } = 1;

        public int Experience
        {
            get { return _experience; }
            set
            {
                var tempLevel = Level;

                _experience = _experience + value;
                // Every new 1000 points of experience.
                Level = _experience / 1000;
                
                if (Level == 0)
                    Level = 1;
                else
                    Level++;

                // LevelUP.
                if (Level > tempLevel)
                    UpdateLevelUnits();
            }
        }

        // The total value of the experience, which is given for a victory over him.
        public int SumExperience { get; private set; }

        public int Hp
        {
            get { return _sumHp; }
            set { _sumHp = value > MaxHp ? MaxHp : value; }
        }

        // The maximum total health squad (needed for the formula for calculating the fight).
        public int MaxHp { get; private set; }
        // The total damage squad (needed for the formula for calculating the fight).
        public int Damage { get; private set; }
        #endregion

        /// <summary>
        /// Reset all fields to the initial.
        /// It is used at the start of a new game or loading from save.
        /// </summary>
        public void ResetToDefault()
        {
            Level = 1;
            _experience = 0;
            _sumHp = 0;
            MaxHp = 0;
            Damage = 0;
            _lstSquadArmy.Clear();
        }

        /// <summary>
        /// Add a unit in the army.
        /// </summary>
        /// <param name="unit">Object Class Unit is a unit that must be added to squad.</param>
        public void AddUnitToArmy(Unit unit)
        {
            _lstSquadArmy.Add(unit);
            Damage += unit.Damage;
            _sumHp += unit.Hp;
            MaxHp += unit.Hp;
            SumExperience += unit.Experience;
        }

        /// <summary>
        /// Update level creatures in the army.
        /// </summary>
        private void UpdateLevelUnits()
        {
            foreach (var t in _lstSquadArmy)
                t.Level++;

            MaxHp = 0;

            foreach (var t in _lstSquadArmy)
                MaxHp += t.Hp;
        }
    }
}