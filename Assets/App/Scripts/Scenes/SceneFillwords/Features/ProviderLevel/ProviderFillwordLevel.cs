using System;
using System.Collections.Generic;
using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;

namespace App.Scripts.Scenes.SceneFillwords.Features.ProviderLevel
{
    public class ProviderFillwordLevel : IProviderFillwordLevel
    {
        private Dictionary<int, GridFillWords> lastLevels = new Dictionary<int, GridFillWords>();
        private int lastValidLevel = -1;

        public GridFillWords LoadModel(int index)
        {
            //напиши реализацию не меняя сигнатуру функции

            if (lastLevels.ContainsKey(index)) return lastLevels[index];

            TextAsset levelsTextAsset = Resources.Load<TextAsset>("Fillwords/pack_0");
            string[] fillWordsLevels = levelsTextAsset.text.Split("\r\n");

            lastValidLevel = index - 1 <= lastValidLevel ? lastValidLevel + 1 : index - 1;

            if(lastValidLevel >= fillWordsLevels.Length) throw new Exception("Нет доступных уровней!");

            Dictionary<int, char> parsedLevel = new Dictionary<int, char>();
            while (!ValidateLevel(fillWordsLevels[lastValidLevel], parsedLevel))
            {
                lastValidLevel++;
                if (lastValidLevel >= fillWordsLevels.Length)
                    throw new Exception("Нет доступных уровней!");
            }

            int size = (int)Math.Sqrt(parsedLevel.Count);
            GridFillWords gfw = new GridFillWords(new Vector2Int(size, size));
            foreach (var letter in parsedLevel)
                gfw.Set(letter.Key / size, letter.Key % size, new CharGridModel(letter.Value));

            lastLevels.Add(index, gfw);

            return gfw;
        }

        private bool ValidateLevel(string level, Dictionary<int, char> parsedLevel)
        {
            TextAsset wordsTextAsset = Resources.Load<TextAsset>("Fillwords/words_list");
            string[] allWords = wordsTextAsset.text.Split("\r\n");
            string[] splitedWords = Regex.Split(level, @"\s+(?=\d+\s+)"); //dd d;d;d

            foreach(string word in splitedWords)
            {
                string[] wordParts = word.Split(' ');
                int wordIndex = int.Parse(wordParts[0]);
                if (wordIndex < 0 && wordIndex >= allWords.Length) return false;
                string[] charsPos = wordParts[1].Split(';');

                string wordForLevel = allWords[wordIndex];
                if (wordForLevel.Length != charsPos.Length) return false;

                for (int i = 0; i < charsPos.Length; i++)
                    parsedLevel.Add(int.Parse(charsPos[i]), wordForLevel[i]);
            }

            if(!Enumerable.Range(0, parsedLevel.Count - 1).All(num => parsedLevel.ContainsKey(num)) 
                || Math.Sqrt(parsedLevel.Count) % 1 != 0)
                return false;

            return true;
        }
    }
}