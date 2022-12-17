using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Crossword.Words;

namespace Crossword
{
    internal class FormationOfAQueue
    {
        List<Cell> listEmptyCellStruct = new();
        List<Word> listWordStruct = new();

        public List<Word> FormationQueue(List<Cell> listEmptyCellStruct)
        {
            this.listEmptyCellStruct = listEmptyCellStruct;
            listWordStruct.Clear();
            SearchForTheBeginningAndLengthOfAllWords();
            SearchForConnectedWords();
            Sorting();
            SortingConnectionWords();
            return listWordStruct;
        }

        public void SearchForTheBeginningAndLengthOfAllWords()
        {
            foreach (Cell cell in listEmptyCellStruct)
            {
                int x = cell.x;
                int y = cell.y;
                bool black = true;

                foreach (Cell cell2 in listEmptyCellStruct)
                {
                    if (cell2.x == x - 1 && cell2.y == y)
                    {
                        black = false;
                        break;
                    }
                }

                if (black == true)
                {
                    SaveWordRight(x, y);
                }

                black = true;
                foreach (Cell cell2 in listEmptyCellStruct)
                {
                    if (cell2.x == x && cell2.y == y - 1)
                    {
                        black = false;
                        break;
                    }
                }

                if (black == true)
                {
                    SaveWordDown(x, y);
                }
            }
        }

        public void SaveWordRight(int x, int y)
        {
            List<Label> newListLabel = new List<Label>();
            for (int i = x; i < 30; i++)
            {
                bool match = false;
                foreach (Cell cell in listEmptyCellStruct)
                {
                    if (cell.y == y && cell.x == i)
                    {
                        newListLabel.Add(cell.label);
                        match = true;
                        break;
                    }
                }

                if (match == false)
                {
                    break;
                }
            }

            if (newListLabel.Count > 1)
            {
                Word newWord = new Word();
                newWord.listLabel = newListLabel;
                newWord.right = true;
                listWordStruct.Add(newWord);
            }
        }

        public void SaveWordDown(int x, int y)
        {
            List<Label> newListLabel = new List<Label>();
            for (int i = y; i < 30; i++)
            {
                bool match = false;
                foreach (Cell cell in listEmptyCellStruct)
                {
                    if (cell.y == i && cell.x == x)
                    {
                        newListLabel.Add(cell.label);
                        match = true;
                        break;
                    }
                }

                if (match == false)
                {
                    break;
                }
            }

            if (newListLabel.Count > 1)
            {
                Word newWord = new Word();
                newWord.listLabel = newListLabel;
                listWordStruct.Add(newWord);
            }
        }

        public void SearchForConnectedWords()
        {
            foreach (Word word in listWordStruct)
            {
                List<Label> tempListLabel = word.listLabel;
                foreach (Label label in tempListLabel)
                {
                    foreach (Word word2 in listWordStruct)
                    {
                        if (word != word2 && SearchForMatches.Get(word2, label) == true)
                        {
                            if (word.connectionWords.Contains(word2) == false)
                            {
                                word.connectionWords.Add(word2);
                            }

                            if (word.connectionLabel.Contains(label) == false)
                            {
                                word.connectionLabel.Add(label);
                            }
                        }
                    }
                }
            }
        }

        void Sorting()
        {
            List<Word> tempList = new List<Word>();
            List<Word> NewList = new List<Word>();
            List<Word> matchList = new List<Word>();
            foreach (var item in listWordStruct)
            {
                if (NewList.Contains(item) == false)
                {
                    NewList.Add(item);
                }

                foreach (var item2 in item.connectionWords)
                {
                    if (matchList.Contains(item2) == false)
                    {
                        matchList.Add(item2);
                        tempList.Add(item2);
                    }
                }

                while (tempList.Count > 0)
                {
                    Word newWord = tempList[0];
                    tempList.RemoveAt(0);
                    if (NewList.Contains(newWord) == false)
                    {
                        NewList.Add(newWord);
                    }

                    foreach (var item2 in newWord.connectionWords)
                    {
                        if (matchList.Contains(item2) == false)
                        {
                            matchList.Add(item2);
                            tempList.Add(item2);
                        }
                    }
                }
            }

            listWordStruct = NewList;
        }

        void SortingConnectionWords()
        {
            foreach (var item in listWordStruct)
            {
                item.connectionWords = item.connectionWords.OrderByDescending(word => (float)word.connectionLabel.Count / word.listLabel.Count).ToList();
            }
        }
    }
}