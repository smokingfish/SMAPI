using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarcusUndAnneMod.Extensions;
using StardewModdingAPI;
using xTile;
using xTile.Tiles;

namespace MarcusUndAnneMod.Editors
{
    public class FarmEditor : IAssetEditor
    {
        public bool CanEdit<T>(IAssetInfo asset)
        {
            return asset.AssetNameEquals(@"Maps\Farm");
        }

        public void Edit<T>(IAssetData asset)
        {
            var helper = ModEntry.Instance.Helper;
            var loadedMap = helper.Content.Load<Map>(@"Content\Farm", ContentSource.ModFolder);

            asset.AsMap().Merge(loadedMap);
        }
    }
}
