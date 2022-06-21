using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
namespace Crossword
{
    internal class ClassMove
    {
        public void MoveRight(Border border1, List<Border> listBorder, List<Label> listLabel, List<string> words, Label label)
        {
            int numColumn = Grid.GetColumn(border1);
            int numRow = Grid.GetRow(border1);
            List<Label> newListLabel = LoopsRight(numColumn, numRow, listBorder, listLabel);
            if (newListLabel.Count < 16)
            {
                if (newListLabel.Count >= 2)
                {
                    List<string> listString = new List<string>();
                    foreach (string word in words)
                    {
                        if (word.Length == newListLabel.Count)
                        {
                            listString.Add(word);
                        }
                    }
                    Random rnd = new Random();
                    string newWord = listString[rnd.Next(0, listString.Count - 1)];
                    label.Content += newWord + "\n";
                    for (int i = 0; i < newListLabel.Count; i++)
                    {
                        newListLabel[i].Content = newWord[i];
                    }
                }
            }
            else
            {
                MessageBox.Show("Есть поле больше 15");
            }
        }
        List<Label> LoopsRight(int firstNumColumn, int firstNumRow, List<Border> listBorder, List<Label> listLabel)
        {
            int letterСount = 0;
            for (int i = firstNumColumn; i < 30; i++)
            {
                bool black = true;
                foreach (Border border in listBorder)
                {
                    if (Grid.GetRow(border) == firstNumRow && Grid.GetColumn(border) == i)
                    {
                        letterСount++;
                        black = false;
                        break;
                    }
                }
                if (black == true)
                {
                    break;
                }
            }
            List<Label> newListLabel = new List<Label>();
            for (int i = firstNumColumn; i < firstNumColumn + letterСount; i++)
            {
                foreach (Label label in listLabel)
                {
                    if (Grid.GetRow(label) == firstNumRow && Grid.GetColumn(label) == i)
                    {
                        if (label.Content == null)
                        {
                            newListLabel.Add(label);
                        }
                        else
                        {
                            return newListLabel;
                        }
                    }
                }
            }
            return newListLabel;
        }
        public bool MoveDown(Border border1, List<Border> listBorder, List<Label> listLabel, List<string> words, Label label)
        {
            int numColumn = Grid.GetColumn(border1);
            int numRow = Grid.GetRow(border1);
            List<Label> newListLabel = LoopsDown(numColumn, numRow, listBorder, listLabel);
            if (newListLabel.Count >= 2)
            {
                List<string> listString = new List<string>();
                foreach (string word in words)
                {
                    if (word.Length == newListLabel.Count)
                    {
                        listString.Add(word);
                    }
                }
                List<string> tempListString = new List<string>();
                for (int i = 0; i < newListLabel.Count; i++)
                {
                    if (newListLabel[i].Content != null)
                    {
                        foreach (string item in listString)
                        {
                            string tempString = newListLabel[i].Content.ToString();
                            string tempString2 = item[i].ToString();
                            if (tempString2 == tempString)
                            {
                                tempListString.Add(item);
                            }
                        }
                        if (tempListString.Count > 0)
                        {
                            listString = new List<string>(tempListString);
                            tempListString.Clear();
                        }
                        else
                        {
                            //border1.Background = Brushes.Yellow;
                            return true;
                        }
                    }
                }
                Random rnd = new Random();
                string newWord = listString[rnd.Next(0, listString.Count - 1)];
                label.Content += newWord + "\n";
                for (int i = 0; i < newListLabel.Count; i++)
                {
                    newListLabel[i].Content = newWord[i];
                }
            }
            return false;
        }
        List<Label> LoopsDown(int firstNumColumn, int firstNumRow, List<Border> listBorder, List<Label> listLabel)
        {
            int letterСount = 0;
            for (int i = firstNumRow; i < 30; i++)
            {
                bool black = true;
                foreach (Border border in listBorder)
                {
                    if (Grid.GetRow(border) == i && Grid.GetColumn(border) == firstNumColumn)
                    {
                        letterСount++;
                        black = false;
                        break;
                    }
                }
                if (black == true)
                {
                    break;
                }
            }
            List<Label> newListLabel = new List<Label>();
            for (int i = firstNumRow; i < firstNumRow + letterСount; i++)
            {
                foreach (Label label in listLabel)
                {
                    if (Grid.GetColumn(label) == firstNumColumn && Grid.GetRow(label) == i)
                    {
                        //foreach (Label label2 in listLabel)
                        //{
                        //    if (Grid.GetColumn(label2) == firstNumColumn && Grid.GetRow(label2) == i + 1)
                        //    {
                        //        if (label2.Content == null)
                        //        {
                                    newListLabel.Add(label);
                        //        }
                        //    }
                        //}
                        //if (label.Content == null)
                        //{
                        //newListLabel.Add(label);
                        //}
                        //else
                        //{
                        //    return newListLabel;
                        //}
                        //newListLabel.Add(label);
                    }
                }
            }
            return newListLabel;
        }
    }
}
