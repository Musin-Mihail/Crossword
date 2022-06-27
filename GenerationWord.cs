using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;

namespace Crossword
{
    internal class GenerationWord
    {
        List<Label> SearchEmptyLineRight(int firstNumColumn, int firstNumRow, List<Border> listBorder, List<Label> listLabel)
        {
            //Поиск место под слово по горизонтали. И возрат списка клеток.
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
        List<Label> SearchEmptyLineDown(int firstNumColumn, int firstNumRow, List<Border> listBorder, List<Label> listLabel)
        {
            //Поиск место под слово по вертикали. И возрат списка клеток.
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
        public int InsertWord(List<Border> listBorder, List<Label> listLabel, ref Word word)
        {
            bool right = word.GetRight();
            bool down = word.GetDown();

            bool errorRight = false;
            bool errorDown = false;

            Label FirsLabel = word.GetFirstLabel();
            int numColumn = Grid.GetColumn(FirsLabel);
            int numRow = Grid.GetRow(FirsLabel);

            if (right == true)
            {
                MessageBox.Show("Вправо");
                List<string> words = word.GetRightListWords();
                List<Label> newListLabelRight = SearchEmptyLineRight(numColumn, numRow, listBorder, listLabel);
                errorRight = SearchWord(true, newListLabelRight, words, ref word);
            }
            if (down == true)
            {
                MessageBox.Show("Вниз");
                List<string> words = word.GetDownListWords();
                List<Label> newListLabelDown = SearchEmptyLineDown(numColumn, numRow, listBorder, listLabel);
                errorDown = SearchWord(false, newListLabelDown, words, ref word);
            }
            if (errorRight == true && errorDown == true)
            {
                return 3;
            }
            else if (errorRight == true)
            {
                return 1;
            }
            else if (errorDown == true)
            {
                return 2;
            }
            return 0;
            // 0 - нет ошибок
            // 1 - ошибка в вправа
            // 2 - ошибка во влево
            // 3 - ошибка в обоих словах
        }
        bool SearchWord(bool right, List<Label> newListLabel, List<string> words, ref Word word)
        {
            if (newListLabel.Count < 16)
            {
                if (newListLabel.Count > 1)
                {
                    List<string> listWordsString = new List<string>(words);
                    List<string> tempListString = new List<string>();
                    if (right == true)
                    {
                        word.ClearConnectionPointRight();
                    }
                    else
                    {
                        word.ClearConnectionPointDown();
                    }
                    for (int i = 0; i < newListLabel.Count; i++)
                    {
                        if (newListLabel[i].Content != null)
                        {
                            // Нужно добавить эти точки в обо слова. А лучше добавить сразу все слова.
                            if (right == true)
                            {
                                word.AddConnectionPointRight(newListLabel[i]);
                            }
                            else
                            {
                                word.AddConnectionPointDown(newListLabel[i]);
                            }
                            foreach (string item in listWordsString)
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
                                listWordsString = new List<string>(tempListString);
                                tempListString.Clear();
                            }
                            else
                            {
                                MessageBox.Show("Не нашёл слово");
                                return true;
                            }
                        }
                    }

                    string newWord = listWordsString[0];
                    if (right == true)
                    {
                        word.DeleteWordRight(newWord);
                    }
                    else
                    {
                        word.DeleteWordDown(newWord);
                    }
                    MessageBox.Show(newWord);
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
