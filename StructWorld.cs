using System;
using System.Collections.Generic;
using System.Windows.Controls;
namespace Crossword
{
    struct Word
    {
        public List<Label> listLabelRight;
        public List<Label> listLabelDown;
        public bool right;
        public bool down;
        public int count;
        public Label firstLabel;
        List<string> listWordsRight;
        List<string> listWordsDown;
        public List<string> listTempWordsRight;
        public List<string> listTempWordsDown;
        public List<Label> ConnectionPointRight;
        public List<Label> ConnectionPointDown;
        public List<Word> ConnectionWordsRight;
        public List<Word> ConnectionWordsDown;
        public Word()
        {
            listLabelRight = new List<Label>();
            listLabelDown = new List<Label>();
            firstLabel = new Label();
            right = false;
            down = false;
            listWordsRight = new List<string>();
            listWordsDown = new List<string>();
            listTempWordsRight = new List<string>();
            listTempWordsDown = new List<string>();
            count = 999;
            ConnectionPointRight = new List<Label>();
            ConnectionPointDown = new List<Label>();
            ConnectionWordsRight = new List<Word>();
            ConnectionWordsDown = new List<Word>();
        }
        public void RestoreDictionary()
        {
            listTempWordsRight = new List<string>(listWordsRight);
            listTempWordsDown = new List<string>(listWordsDown);
        }
        public void ClearLabelRight()
        {
            foreach (var item in listLabelRight)
            {
                bool Match = false;
                foreach (var item2 in ConnectionPointRight)
                {
                    if (item == item2)
                    {
                        Match = true;
                        break;
                    }
                }
                if (Match == false)
                {
                    item.Content = null;
                }
            }
        }
        public void ClearLabelDown()
        {
            foreach (var item in listLabelDown)
            {
                bool Match = false;
                foreach (var item2 in ConnectionPointDown)
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
        public void DeleteWordRight(string word)
        {
            for (int i = 0; i < listTempWordsRight.Count; i++)
            {
                if (word == listTempWordsRight[i])
                {
                    listTempWordsRight.RemoveAt(i);
                }
            }
        }
        public void DeleteWordDown(string word)
        {
            for (int i = 0; i < listTempWordsDown.Count; i++)
            {
                if (word == listTempWordsDown[i])
                {
                    listTempWordsDown.RemoveAt(i);
                }
            }
        }
        public void AddWordsRight(List<string> listWords)
        {
            listWordsRight = new List<string>(listWords);
            listTempWordsRight = new List<string>(listWords);
        }
        public void AddWordsDown(List<string> listWords)
        {
            listWordsDown = new List<string>(listWords);
            listTempWordsDown = new List<string>(listWords);
        }
        public void ListWordsRandomization()
        {
            Random rnd = new Random();
            for (int i = 0; i < listTempWordsRight.Count; i++)
            {
                string temp = listTempWordsRight[i];
                int randomIndex = rnd.Next(0, listTempWordsRight.Count - 1);
                listTempWordsRight[i] = listTempWordsRight[randomIndex];
                listTempWordsRight[randomIndex] = temp;
            }
            for (int i = 0; i < listTempWordsDown.Count; i++)
            {
                string temp = listTempWordsDown[i];
                int randomIndex = rnd.Next(0, listTempWordsDown.Count - 1);
                listTempWordsDown[i] = listTempWordsDown[randomIndex];
                listTempWordsDown[randomIndex] = temp;
            }
        }
        public bool SearchForMatchesRight(Label firstLabel)
        {
            bool match = false;
            foreach (Label label in listLabelRight)
            {
                if (firstLabel == label)
                {
                    match = true;
                }
            }
            return match;
        }
        public bool SearchForMatchesDown(Label firstLabel)
        {
            bool match = false;
            foreach (Label label in listLabelDown)
            {
                if (firstLabel == label)
                {
                    match = true;
                }
            }
            return match;
        }
        public void SetListLabelRight(List<Label> listLabel)
        {
            listLabelRight = new List<Label>(listLabel);
            foreach (var item in listLabelRight)
            {
                if (item.Content != null)
                {
                    ConnectionPointRight.Add(item);
                }
            }
            firstLabel = listLabel[0];
        }
        public void SetListLabelDown(List<Label> listLabel)
        {
            listLabelDown = new List<Label>(listLabel);
            foreach (var item in listLabelDown)
            {
                if (item.Content != null)
                {
                    ConnectionPointDown.Add(item);
                }
            }
            firstLabel = listLabel[0];
        }
    }
}