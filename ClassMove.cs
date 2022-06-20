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
                        if (word.Length == newListLabel.Count + 1)
                        {
                            listString.Add(word);
                        }
                    }
                    Random rnd = new Random();
                    string newWord = listString[rnd.Next(0, listString.Count - 1)];
                    label.Content += newWord + "\n";
                    //MessageBox.Show(newWord);
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
        List<Label> LoopsRight(int numColumn, int numRow, List<Border> listBorder, List<Label> listLabel)
        {
            List<Label> newListLabel = new List<Label>();
            for (int i = numColumn; i < 30; i++)
            {
                foreach (Border border2 in listBorder)
                {
                    if (Grid.GetRow(border2) == numRow)
                    {
                        if (Grid.GetColumn(border2) == i)
                        {
                            if (border2.Background == Brushes.Transparent || border2.Background == Brushes.Yellow)
                            {
                                foreach (Label label in listLabel)
                                {
                                    if (Grid.GetRow(label) == numRow)
                                    {
                                        if (Grid.GetColumn(label) == i)
                                        {
                                            if (i == numColumn)
                                            {
                                                newListLabel.Add(label);
                                            }
                                            else
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
                                }
                            }
                            else
                            {
                                return newListLabel;
                            }
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
                    if (word.Length == newListLabel.Count + 1)
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
                            border1.Background = Brushes.Blue;
                            //MessageBox.Show("Нет подходящего слова");
                            border1.Background = Brushes.Yellow;
                            return false;
                        }
                    }
                }
                Random rnd = new Random();
                string newWord = listString[rnd.Next(0, listString.Count - 1)];
                label.Content += newWord + "\n";
                //MessageBox.Show(newWord);
                for (int i = 0; i < newListLabel.Count; i++)
                {
                    newListLabel[i].Content = newWord[i];
                }
            }
            return true;
        }
        List<Label> LoopsDown(int numColumn, int numRow, List<Border> listBorder, List<Label> listLabel)
        {
            List<Label> newListLabel = new List<Label>();
            for (int i = numRow; i < 30; i++)
            {
                foreach (Border border2 in listBorder)
                {
                    if (Grid.GetColumn(border2) == numColumn)
                    {
                        if (Grid.GetRow(border2) == i)
                        {
                            if (border2.Background == Brushes.Transparent || border2.Background == Brushes.Yellow)
                            {
                                border2.Background = Brushes.Transparent;
                                foreach (Label label in listLabel)
                                {
                                    if (Grid.GetColumn(label) == numColumn)
                                    {
                                        if (Grid.GetRow(label) == i)
                                        {

                                            newListLabel.Add(label);

                                        }
                                    }
                                }
                            }
                            else
                            {
                                return newListLabel;
                            }
                        }
                    }
                }
            }
            return newListLabel;
        }
    }
}
