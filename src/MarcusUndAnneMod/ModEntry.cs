using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;

using System;
using System.Linq;
using StardewValley;
using MarcusUndAnneMod.Extensions;
using MarcusUndAnneMod.Models;
using MarcusUndAnneMod.Editors;

using xTile;

namespace MarcusUndAnneMod
{
    public class ModEntry : Mod
    {
        public static ModEntry Instance { get; set; }
        public ModContent Content;
        public ModActions Actions = new ModActions();

        private bool _initialized = false;
        private int _currentHour = 0;

        public override void Entry(IModHelper helper)
        {
            Instance = this;

            Instance.Helper.Content.AssetEditors.Add(new MailEditor());
            Instance.Helper.Content.AssetEditors.Add(new FarmEditor());

            TimeEvents.TimeOfDayChanged += this.TimeEvents_TimeOfDayChanged;
            GameEvents.OneSecondTick += this.GameEvents_UpdateTick;            
        }

        private void GameEvents_UpdateTick(object sender, EventArgs e)
        {
            if (Context.IsWorldReady)
            { 
                if(!this._initialized)
                {
                    this.Content = this.Helper.ReadJsonFile<ModContent>("content.json");

                    var loadedMap = Instance.Helper.Content.Load<Map>(@"Content\FarmSecret", ContentSource.ModFolder);
                    Game1.locations.Add(new GameLocation(loadedMap, "FarmSecret"));

                    this._initialized = true;
                }

                if (Game1.isEating)
                {
                    Game1.addHUDMessage(new HUDMessage("Yam Yam Yam! :P", HUDMessage.stamina_type));
                }
            }
        }

        private void TimeEvents_TimeOfDayChanged(object sender, EventArgsIntChanged e)
        {
            this._currentHour = e.NewInt;

            if (Context.IsWorldReady)
            {
                var currentDate = SDate.Now();

                var messagesSeason = this.Content.Notifications.Where(m => string.IsNullOrEmpty(m.Season) || m.Season == currentDate.Season);
                var messagesDay = messagesSeason.Where(m => m.Days == null || m.Days.Any(pd => pd == currentDate.Day));
                var messagesToday = messagesDay.Where(m => m.DaysOfWeek == null || m.DaysOfWeek.Any(pd => pd == currentDate.DayOfWeek));
                var messagesNow = messagesToday.Where(m => m.Times.Any(pt => pt == this._currentHour));

                messagesNow.ForEach(message =>
                {
                    var rnd = new Random();
                    int rndNumber = rnd.Next(1, 100);

                    if (message.Chance >= rndNumber)
                    {
                        Game1.addHUDMessage(new HUDMessage(message.Text, message.Category));
                        this.Actions.Execute(message.Action, message.ActionParameter);
                    }
                });
            }
        }
    }
}
