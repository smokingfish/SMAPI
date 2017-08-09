using StardewValley;
using StardewValley.Locations;

namespace MarcusUndAnneMod.Models
{
    public enum ModActionTypes
    {
        None,
        Letter,
        Heal,
        DownTheMine
    }

    public class ModActions
    {
        public void Letter(string letterName)
        {
            if (string.IsNullOrEmpty(letterName)) return;

            if (!Game1.mailbox.Contains(letterName))
            {
                Game1.mailbox.Enqueue(letterName);
            }
        }

        public void Heal()
        {
            Game1.player.health = Game1.player.maxHealth;
            Game1.player.stamina = Game1.player.maxStamina;
        }

        public void DownTheMine()
        {
            if (!(Game1.currentLocation is MineShaft)) return;

            int level = (Game1.currentLocation as MineShaft)?.mineLevel ?? 0;
            if (level >= 120) return;
            Game1.enterMine(false, level + 1, "");
        }

        public void Execute(ModActionTypes type, string parameter)
        {
            switch(type)
            {
                case ModActionTypes.Heal:
                    Heal();
                    break;
                case ModActionTypes.DownTheMine:
                    DownTheMine();
                    break;
                case ModActionTypes.Letter:
                    Letter(parameter);
                    break;
            }
        }
    }
}
