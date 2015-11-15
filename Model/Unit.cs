// Unit.cs
// Class for a description of all the monsters in the game.

namespace GFZ.Model
{
    class Unit
    {
        // Id monster.
        public int MId { get; }
        // Level.
        private int _level;
        // The name of the monster (displayed in the MessageBox with information on the outcome of the battle).
        public string Name { get; }
        // Level.
        public int Level
        {
            get { return _level; }
            set
            {
                var tempLevel = _level;
                _level = value;
                // LevelUP.
                if (_level <= tempLevel) return;
                Hp += 10;
                Damage += 10;
            }
        }
        // Health.
        public int Hp { get; set; }
        // Damage.
        public int Damage { get; private set; }
        // The amount of experience, which is given for victory over the monster.
        public int Experience { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">Id monster.</param>
        /// <param name="name">Title monster.</param>
        /// <param name="lvl">Level.</param>
        /// <param name="hp">Health.</param>
        /// <param name="dmg">Damage.</param>
        /// <param name="exp">The amount of experience, which is given for victory over the monster.</param>
        public Unit(int id, string name, int lvl, int hp, int dmg, int exp)
        {
            MId = id;
            Name = name;

            if (lvl <= 0)
                _level = 1;
            else if (lvl > 10)
                _level = 10;
            else
                _level = lvl;

            Hp = hp;
            Damage = dmg;
            Experience = exp;
        }
    }
}