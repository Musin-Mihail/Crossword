using System;
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
        public Word()
        {
            listLabelRight = new List<Label>();
            listLabelDown = new List<Label>();
            firstLabel = new Label();
            right = false;
            down = false;
            listWordsRight = new List<string>();
            listWordsDown = new List<string>();
            count = 999;
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
        public void AddWordsRight(List<string> listWords)
        {
            listWordsRight = new List<string>(listWords);
        }
        public void AddWordsDown(List<string> listWords)
        {
            listWordsDown = new List<string>(listWords);
        }
        public List<string> GetRightListWords()
        {
            return listWordsRight;
        }
        public List<string> GetDownListWords()
        {
            return listWordsDown;
        }
        public int GetRightLetterCount()
        {
            return listLabelRight.Count;
        }
        public List<Label> GetRightLabel()
        {
            return listLabelRight;
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
            firstLabel = listLabel[0];
        }
        public void SetListLabelDown(List<Label> listLabel)
        {
            listLabelDown = new List<Label>(listLabel);
            firstLabel = listLabel[0];
        }
    }
}
