using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
namespace Crossword
{
    internal class GenerationWord
    {
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
                        newListLabel.Add(label);
                    }
                }
            }
            return newListLabel;
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
                        newListLabel.Add(label);
                    }
                }
            }
            
            return newListLabel;
        }
        public bool InsertWord(bool right, Label label1, List<Border> listBorder, List<Label> listLabel, List<string> words, ref List<string> wordsGrid, ref Word word)
        {
            int numColumn = Grid.GetColumn(label1);
            int numRow = Grid.GetRow(label1);
            List<Label> newListLabel;
            if (right == true)
            {
                newListLabel = LoopsRight(numColumn, numRow, listBorder, listLabel);
            }
            else
            {
                newListLabel = LoopsDown(numColumn, numRow, listBorder, listLabel);
            }
            
            if (newListLabel.Count < 16)
            {
                if (newListLabel.Count > 1)
                {
                    List<string> listWords = new List<string>(words);
                    List<string> tempListString = new List<string>();
                    for (int i = 0; i < newListLabel.Count; i++)
                    {
                        if (newListLabel[i].Content != null)
                        {
                            if(right == true)
                            {
                                word.AddConnectionPointRight(newListLabel[i]);
                            }
                            else
                            {
                                word.AddConnectionPointDown(newListLabel[i]);
                            }
                            foreach (string item in listWords)
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
                                listWords = new List<string>(tempListString);
                                tempListString.Clear();
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                    Random rnd = new Random();
                    string newWord = listWords[rnd.Next(0, listWords.Count - 1)];
                    if (right == true)
                    {
                        word.DeleteWordRight(newWord);
                    }
                    else
                    {
                        word.DeleteWordDown(newWord);
                    }
                    wordsGrid.Add(newWord);
                    //label.Content += newWord + "\n";
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
            return false;
        }
    }
}
