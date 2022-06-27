﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
namespace Crossword
{
    struct Word
    {
        List<Label> listLabelRight;
        List<Label> listLabelDown;
        bool right;
        bool down;
        int count;
        Label firstLabel;
        List<string> listWordsRight;
        List<string> listWordsDown;
        List<string> listTempWordsRight;
        List<string> listTempWordsDown;
        List<Label> ConnectionPointRight;
        List<Label> ConnectionPointDown;
        List<Word> ConnectionWordsRight;
        List<Word> ConnectionWordsDown;
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
        public void AddConnectionWordsRight(Word word)
        {
            ConnectionWordsRight.Add(word);
        }
        public void AddConnectionWordsDown(Word word)
        {
            ConnectionWordsDown.Add(word);
        }
        public List<Word> GetConnectionWordsRight()
        {
            return ConnectionWordsRight;
        }
        public List<Word> GetConnectionWordsDown()
        {
            return ConnectionWordsDown;
        }
        public void ClearConnectionPointRight()
        {
            ConnectionPointRight.Clear();
        }
        public void ClearConnectionPointDown()
        {
            ConnectionPointDown.Clear();
        }
        public void AddConnectionPointRight(Label point)
        {
            ConnectionPointRight.Add(point);
        }
        public void AddConnectionPointDown(Label point)
        {
            ConnectionPointDown.Add(point);
        }
        public List<Label> GetConnectionPointRight()
        {
            return ConnectionPointRight;
        }
        public List<Label> GetConnectionPointDown()
        {
            return ConnectionPointDown;
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
        public void SetCount(int count)
        {
            this.count = count;
        }
        public int GetCount()
        {
            return count;
        }
        public void ChangeRight()
        {
            right = true;
        }
        public void ChangeDown()
        {
            down = true;
        }
        public Label GetFirstLabel()
        {
            return firstLabel;
        }
        public bool GetRight()
        {
            return right;
        }
        public bool GetDown()
        {
            return down;
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
        public List<string> GetRightListWords()
        {
            return listTempWordsRight;
        }
        public List<string> GetDownListWords()
        {
            return listTempWordsDown;
        }
        public int GetRightLetterCount()
        {
            return listLabelRight.Count;
        }
        public List<Label> GetRightLabel()
        {
            return listLabelRight;
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
        public List<Label> GetDownLabel()
        {
            return listLabelDown;
        }
        public int GetDownLetterCount()
        {
            return listLabelDown.Count;
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