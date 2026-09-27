using UnityEngine;
using System.Collections.Generic;


public class SeaMapGenerator 
{
    public static void Generate(CountryConstantData data)
    {
        const int widthTiles = 120;
        const int heightTiles = 68;

        int minX = -widthTiles / 2;
        int maxX = minX + widthTiles - 1;

        int minY = -heightTiles / 2;
        int maxY = minY + heightTiles - 1;

        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                Vector2Int pos =
                                    new Vector2Int(x, y);
                if (data.worldMapTiles.ContainsKey(pos))
                    continue;

                MapTile seaTile = new MapTile();

                seaTile.position = pos;
                seaTile.isSea = true;
                seaTile.countryName = "";
                
                data.worldMapTiles[pos] = seaTile;
            }
        }
    }
}
