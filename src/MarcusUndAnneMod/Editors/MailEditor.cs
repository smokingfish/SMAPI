using System.Collections.Generic;
using System.Linq;
using MarcusUndAnneMod.Extensions;
using StardewModdingAPI;

namespace MarcusUndAnneMod.Editors
{
    public class MailEditor : IAssetEditor
    {
        public bool CanEdit<T>(IAssetInfo asset)
        {
            return asset.AssetNameEquals(@"Data\Mail");
        }

        public void Edit<T>(IAssetData asset)
        {
            var dic = asset.AsDictionary<string, string>();
            var helper = ModEntry.Instance.Helper;

            var data = helper.Content.Load<Dictionary<string, string>>(@"Content\mail", ContentSource.ModFolder);

            if(data.Any())
            {                
                data.ForEach(d =>
                {
                    dic.Data.Add(d);
                });
            }
        }
    }
}
