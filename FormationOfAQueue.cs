using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

using System.Windows;

namespace Crossword
{
    internal class FormationOfAQueue
    {

        List<Cell> listAllCellStruct = new List<Cell>();
        List<Cell> listEmptyCellStruct = new List<Cell>();
        List<Word> listWordStruct = new List<Word>();
        public List<Word> FormationQueue(List<Cell> listAllCellStruct, List<Cell> listEmptyCellStruct)
        {
            this.listAllCellStruct = listAllCellStruct;
            this.listEmptyCellStruct = listEmptyCellStruct;
            listWordStruct.Clear();
            SearchForTheBeginningAndLengthOfAllWords();
            SearchForConnectedWords();
            //DefiningTheGenerationQueue();
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
                bool black = true;
                foreach (Cell cell in listAllCellStruct)
                {
                    if (cell.y == y && cell.x == i)
                    {
                        if (cell.border.Background == Brushes.Transparent)
                        {
                            newListLabel.Add(cell.label);
                            black = false;
                            break;
                        }
                    }
                }
                if (black == true)
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
        public void SaveWordDown(int x, int y)
        {
            List<Label> newListLabel = new List<Label>();
            for (int i = y; i < 30; i++)
            {
                bool black = true;
                foreach (Cell cell in listAllCellStruct)
                {
                    if (cell.y == i && cell.x == x)
                    {
                        if (cell.border.Background == Brushes.Transparent)
                        {
                            newListLabel.Add(cell.label);
                            black = false;
                            break;
                        }
                    }
                }
                if (black == true)
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
            List<Word> newListWordStruct = new List<Word>();
            int index = 0;

            if (listWordStruct.Count > 0)
            {
                newListWordStruct.Add(listWordStruct[0]);
                while (index < listWordStruct.Count)
                {
                    Word word = newListWordStruct[index];
                    List<Label> tempListLabel = word.listLabel;
                    int count = 0;
                    foreach (Label label in tempListLabel)
                    {
                        foreach (Word word2 in listWordStruct)
                        {
                            if (word.listLabel != word2.listLabel && word2.SearchForMatches(label) == true)
                            {
                                count++;
                                if (word.ConnectionWords.Contains(word2) == false)
                                {
                                    word.ConnectionWords.Add(word2);
                                }
                                if (word.ConnectionLabel.Contains(label) == false)
                                {
                                    word.ConnectionLabel.Add(label);
                                }
                                if (word2.ConnectionLabel.Contains(label) == false)
                                {
                                    word2.ConnectionLabel.Add(label);
                                }
                                if (newListWordStruct.Contains(word2) == false)
                                {
                                    newListWordStruct.Add(word2);
                                }
                            }
                        }
                    }
                    index++;
                    word.difficulty = (float)count / tempListLabel.Count;
                }
            }
            listWordStruct = newListWordStruct;
        }
        public void DefiningTheGenerationQueue()
        {
            if (listWordStruct.Count > 0)
            {
                Word theHardestWord = FindingTheHardestWord(listWordStruct);

                List<Word> listMatchWord = new List<Word>();
                List<Word> listAllHardestWord = new List<Word>();
                List<Word> tempListWord = new List<Word>();
                List<Word> allRightWord = new List<Word>();

                listAllHardestWord.Add(theHardestWord);

                tempListWord.Add(theHardestWord);

                while (tempListWord.Count > 0)
                {
                    Word word = tempListWord[0];
                    tempListWord.RemoveAt(0);

                    if (allRightWord.Contains(word) == false)
                    {
                        allRightWord.Add(word);
                        foreach (Word word2 in word.ConnectionWords)
                        {
                            bool match = false;
                            foreach (Word word3 in listAllHardestWord)
                            {
                                if (word2.listLabel == word3.listLabel)
                                {
                                    match = true;
                                    break;
                                }
                            }
                            foreach (Word word3 in listMatchWord)
                            {
                                if (word2.listLabel == word3.listLabel)
                                {
                                    match = true;
                                    break;
                                }
                            }
                            if (match == false)
                            {
                                listMatchWord.Add(word2);
                            }
                        }
                    }

                    if (listMatchWord.Count > 0)
                    {
                        theHardestWord = FindingTheHardestWord(listMatchWord);

                        int index = listMatchWord.IndexOf(theHardestWord);
                        listMatchWord.RemoveAt(index);

                        tempListWord.Add(theHardestWord);
                        listAllHardestWord.Add(theHardestWord);
                    }
                }
                listWordStruct = listAllHardestWord;

            }
        }
        public Word FindingTheHardestWord(List<Word> listWords)
        {
            Word theHardestWord = listWords[0];
            float maxDifficulty = 0;
            int maxWordCount = 0;
            foreach (Word word in listWords)
            {
                if (word.difficulty == maxDifficulty)
                {
                    if (word.listLabel.Count > maxWordCount)
                    {
                        maxWordCount = word.listLabel.Count;
                        theHardestWord = word;
                    }
                }
                else if (word.difficulty > maxDifficulty)
                {
                    maxDifficulty = word.difficulty;
                    theHardestWord = word;
                    maxWordCount = word.listLabel.Count;
                }
            }
            return theHardestWord;
        }
    }
}
