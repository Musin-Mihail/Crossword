using System;
using System.Collections.Generic;
using System.Windows.Controls;
namespace Crossword
{
    struct Word
    {
        public List<Label> listLabel;
        public float difficulty;
        public Label firstLabel;
        List<string> listWords;
        public List<string> listTempWords;
        public List<Label> ConnectionLabel;
        public List<Word> ConnectionWords;
        public Word()
        {
            listLabel = new List<Label>();
            difficulty = 0;
            firstLabel = new Label();
            listWords = new List<string>();
            listTempWords = new List<string>();
            ConnectionLabel = new List<Label>();
            ConnectionWords = new List<Word>();
        }
        public void RestoreDictionary()
        {
            listTempWords = new List<string>(listWords);
        }
        public void ClearLabel()
        {
            foreach (var item in listLabel)
            {
                bool Match = false;
                foreach (var item2 in ConnectionLabel)
                {
                    if (item == item2)
                    {
                        Match = true;
                        break;
                    }
                }
                if (Match == false)
                {
                    if (item.Content != null)
                    {
                        item.Content = null;
                    }
                }
            }
        }
        public void DeleteWord(string word)
        {
            for (int i = 0; i < listTempWords.Count; i++)
            {
                if (word == listTempWords[i])
                {
                    listTempWords.RemoveAt(i);
                }
            }
        }
        public void AddWords(List<string> listWords)
        {
            listWords = new List<string>(listWords);
            listTempWords = new List<string>(listWords);
        }
        public void ListWordsRandomization()
        {
            Random rnd = new Random();
            for (int i = 0; i < listTempWords.Count; i++)
            {
                string temp = listTempWords[i];
                int randomIndex = rnd.Next(0, listTempWords.Count - 1);
                listTempWords[i] = listTempWords[randomIndex];
                listTempWords[randomIndex] = temp;
            }
        }
        public bool SearchForMatches(Label firstLabel)
        {
            bool match = false;
            foreach (Label label in listLabel)
            {
                if (firstLabel == label)
                {
                    match = true;
                }
            }
            return match;
        }
        public void SetListLabel(List<Label> listLabel)
        {
            this.listLabel = new List<Label>(listLabel);
            foreach (var item in listLabel)
            {
                if (item.Content != null)
                {
                    string tempString = item.Content.ToString();
                    if (tempString.Length == 1)
                    {
                        ConnectionLabel.Add(item);
                    }
                }
            }
            firstLabel = listLabel[0];
        }
        public void RefreshListLabel()
        {
            foreach (var item in listLabel)
            {
                if (item.Content != null)
                {
                    string tempString = item.Content.ToString();
                    if (tempString.Length == 1)
                    {
                        ConnectionLabel.Add(item);
                    }
                }
            }
        }
        public void Reset()
        {
            ConnectionLabel.Clear();
            RestoreDictionary();
            foreach (var item in listLabel)
            {
                item.Content = null;
            }
            ListWordsRandomization();
        }
    }
}