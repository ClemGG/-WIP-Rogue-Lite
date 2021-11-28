using Project.Generation;
using UnityEngine;
using System.Reflection;

namespace Project.Tiles
{
    /// <summary>
    /// Stores all types of Tiles and loads them automatically when called
    /// instead of having to manually select which Tile to use 
    /// each time we generate a new dungeon.
    /// </summary>
    public static class TileLibrary
    {
        public static Tile Floor { get { return Object.Instantiate(Resources.Load<Tile>("Tiles/Map/Floor")); } } 
        public static Tile Wall { get { return Object.Instantiate(Resources.Load<Tile>("Tiles/Map/Wall")); } }
        public static Tile Player { get { return Object.Instantiate(Resources.Load<Tile>("Tiles/Actors/Player")); } }
        public static Tile Rat 
        { 
            //rat set explicitely in case we want to modify its stats before returning it
            get 
            {
                ActorTile rat = Object.Instantiate(Resources.Load<ActorTile>("Tiles/Actors/Rat"));
                rat.Stats.Gold = Random.Range(0, rat.Stats.Gold);
                return rat;
            }
        }

        private static PropertyInfo[] _fieldsNames
        { get 
            {
                return _fields ??= typeof(TileLibrary).GetProperties();
            }
        }
        private static PropertyInfo[] _fields;

        internal static EnemyTile GetRandomEnemy(DungeonGenerationSettingsSO settings)
        {
            string enemyName = settings.EnemiesToSpawn[Random.Range(0, settings.EnemiesToSpawn.Length)].TileName;
            for (int i = 0; i < _fieldsNames.Length; i++)
            {
                if (enemyName.Equals(_fieldsNames[i].Name))
                {
                    return (EnemyTile)_fieldsNames[i].GetValue(_fieldsNames[i]);
                }
            }

            return null;
        }
    }
}