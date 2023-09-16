using System;
using System.Collections.Generic;
using App.Scripts.Libs.Factory;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;
using Unity.VisualScripting;

namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel
{
    public class FactoryLevelModel : IFactory<LevelModel, LevelInfo, int>
    {
        public LevelModel Create(LevelInfo value, int levelNumber)
        {
            var model = new LevelModel();

            model.LevelNumber = levelNumber;

            model.Words = value.words;
            model.InputChars = BuildListChars(value.words);

            return model;
        }

        private List<char> BuildListChars(List<string> words)
        {
            //напиши реализацию не меняя сигнатуру функции

            List<char> listChars = new List<char>();
            Dictionary<char, int> numberChars = new Dictionary<char, int>();
            foreach(string word in words)
            {
                Dictionary<char, int> wordChars = new Dictionary<char, int>();
                for (int i = 0; i < word.Length; i++)
                {
                    if(wordChars.ContainsKey(word[i])) wordChars[word[i]]++;
                    else wordChars.Add(word[i], 1);
                }

                foreach(var char_num in wordChars)
                {
                    if (!numberChars.ContainsKey(char_num.Key))
                    {
                        numberChars.Add(char_num.Key, char_num.Value);
                        continue;
                    }

                    if (numberChars[char_num.Key] < char_num.Value)
                        numberChars[char_num.Key] = char_num.Value;

                }
            }

            foreach(var pair in numberChars)
                for (int i = 0; i < pair.Value; i++)
                    listChars.Add(pair.Key);
            
            return listChars;
        }
    }
}