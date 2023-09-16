using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;
using Newtonsoft.Json;
using UnityEngine;

namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel.ProviderWordLevel
{
    public class ProviderWordLevel : IProviderWordLevel
    {
        public LevelInfo LoadLevelData(int levelIndex)
        {
            //напиши реализацию не меняя сигнатуру функции

            TextAsset levelsTextAsset = Resources.Load<TextAsset>("WordSearch/Levels/" + levelIndex.ToString());
            string jsonlevelContent = levelsTextAsset.text.Replace("\n", "").Replace("\t", "").Replace(" ", "");

            LevelInfo lvl = JsonConvert.DeserializeObject<LevelInfo>(jsonlevelContent);
            return lvl;           
        }
    }
}