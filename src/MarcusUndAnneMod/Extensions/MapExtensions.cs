using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StardewModdingAPI;
using xTile;
using xTile.Tiles;

namespace MarcusUndAnneMod.Extensions
{
    public class Warp
    {
        public int FromX { get; set; }
        public int FromY { get; set; }

        public string MapName { get; set; }

        public int ToX { get; set; }

        public int ToY { get; set; }
    }

    public static class MapExtensions
    {
        public static Map AsMap(this IAssetData asset)
        {
            return asset.GetData<Map>();
        }

        public static Map Merge(this Map contentMap, Map loadedMap)
        {
            // merge layers
            contentMap.Layers.ForEach(contentLayer =>
            {
                var loadedLayer = loadedMap.Layers.FirstOrDefault(l => l.Id == contentLayer.Id);

                for (int x = 0; x < contentLayer.LayerWidth; x++)
                {
                    for (int y = 0; y < contentLayer.LayerHeight; y++)
                    {
                        var loadedTile = loadedLayer.Tiles[x, y];
                        var contentTile = contentLayer.Tiles[x, y];

                        if (loadedTile != null)
                        {
                            if (contentTile == null)
                            {
                                Tile newTile = null;
                                int tilesheetIndex = loadedMap.TileSheets.IndexOf(loadedTile.TileSheet);

                                if (tilesheetIndex > contentMap.TileSheets.Count - 1)
                                {
                                    contentMap.AddTileSheet(new TileSheet(contentMap, loadedTile.TileSheet.ImageSource,
                                                            new xTile.Dimensions.Size(loadedTile.TileSheet.SheetWidth, loadedTile.TileSheet.SheetHeight),
                                                            new xTile.Dimensions.Size(loadedTile.TileSheet.TileWidth, loadedTile.TileSheet.TileHeight)));
                                }

                                if (loadedTile is StaticTile)
                                {
                                    newTile = new StaticTile(contentLayer, contentMap.TileSheets[tilesheetIndex], loadedTile.BlendMode, loadedTile.TileIndex);
                                }
                                else if (loadedTile is AnimatedTile)
                                {
                                    var animatedLoadedTile = loadedTile as AnimatedTile;
                                    var animatedTiles = new List<StaticTile>();

                                    foreach (var staticTiles in animatedLoadedTile.TileFrames)
                                    {
                                        animatedTiles.Add(new StaticTile(contentLayer, contentMap.TileSheets[tilesheetIndex], loadedTile.BlendMode, loadedTile.TileIndex));
                                    }

                                    newTile = new AnimatedTile(contentLayer, animatedTiles.ToArray(), animatedLoadedTile.FrameInterval);
                                }

                                contentLayer.Tiles[x, y] = newTile;
                            }
                            else
                            {
                                if (contentTile.TileIndex != loadedTile.TileIndex)
                                {
                                    contentTile.TileIndex = loadedTile.TileIndex;
                                }
                            }
                        }
                        else
                        {
                            contentLayer.Tiles[x, y] = null;
                        }
                    }
                }
            });

            // load warps from loaded map
            var contentWarps = contentMap.Properties.FirstOrDefault(p => p.Key == "Warp").Value;
            var loadedWarps = loadedMap.Properties.FirstOrDefault(p => p.Key == "Warp").Value;                
            contentMap.Properties["Warp"] = loadedWarps;
        
            // TODO merge warps           
            //var loadedWarps = loadedMap.Properties.FirstOrDefault(p => p.Key == "Warp").ToString().GetWarpsFromString();

            return contentMap;
        }
        

        public static List<Warp> GetWarpsFromString(this string parameter)
        {
            var warps = new List<Warp>();

            if(!string.IsNullOrEmpty(parameter))
            {
                var splitData = parameter.Split(' ');

                if(splitData.Count() % 5 == 0)
                {
                    for(int x = 0; x < splitData.Count() -1; x += 5)
                    {
                        try
                        {
                            int fromX = Convert.ToInt32(splitData[x]);
                            int fromY = Convert.ToInt32(splitData[x + 1]);
                            string mapName = splitData[x + 2];
                            int toX = Convert.ToInt32(splitData[x + 3]);
                            int toY = Convert.ToInt32(splitData[x + 4]);

                            warps.Add(new Warp()
                            {
                                FromX = fromX,
                                FromY = fromY,
                                MapName = mapName,
                                ToX = toX,
                                ToY = toY
                            });
                        }
                        catch
                        {

                        }
                    }
                }
            }

            return warps;
        }
    }
}
