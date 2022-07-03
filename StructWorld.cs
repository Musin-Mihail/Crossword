using System;
using System.Collections.Generic;
using System.Windows.Controls;
namespace Crossword
{
    class Word
    {
        public List<Label> listLabel = new List<Label>();
        public Label firstLabel = new Label();
        List<string> listWords = new List<string>();
        public List<string> listTempWords = new List<string>();
        public List<Label> ConnectionLabel = new List<Label>();
        public List<Word> ConnectionWords = new List<Word>();
        public bool full = false;
        public string wordString = "";
        public List<string> insertedWords = new List<string>();
        public bool right = false;
        public int error = 0;
        public Word lastWord = null;
        public void RestoreDictionary()
        {
            listTempWords = new List<string>(listWords);
            ListWordsRandomization();
        }
        public void ClearLabel()
        {
            foreach (Label label in listLabel)
            {
                if (label.Content != null)
                {
                    if (SearchConnectWord1(label) == false)
                    {
                        label.Content = null;
                    }
                    else if (SearchConnectWord2(label) == true)
                    {
                        label.Content = null;
                    }
                }
            }
        }
        bool SearchConnectWord1(Label label)
        {
            foreach (Word word in ConnectionWords)
            {
                if (word.ConnectionLabel.Contains(label) == true)
                {
                    return true;
                }
            }
            return false;
        }
        bool SearchConnectWord2(Label label)
        {
            foreach (Word word in ConnectionWords)
            {
                if (word.ConnectionLabel.Contains(label) == true)
                {
                    if (word.full == false)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void DeleteWord(string word)
        {
            for (int i = 0; i < listTempWords.Count; i++)
            {
                if (word == listTempWords[i])
                {
                    listTempWords.RemoveAt(i);
                    if (listTempWords.Count == 0)
                    {
                        RestoreDictionary();
                    }
                    return;
                }
            }
        }
        public void AddWords(List<string> listWords)
        {
            this.listWords = listWords;
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
        public bool SearchForMatches(Label matchLabel)
        {
            if (listLabel.Contains(matchLabel) == true)
            {
                return true;
            }
            return false;
        }
        public void Reset()
        {
            ClearLabel();
            full = false;
            for (int i = 0; i < insertedWords.Count; i++)
            {
                if (insertedWords[i] == wordString)
                {
                    insertedWords.RemoveAt(i);
                    break;
                }
            }
            wordString = "";
        }
    }
}