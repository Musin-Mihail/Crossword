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
            try
            {
                SearchForConnectedWords();
            }
            catch
            {
                MessageBox.Show("SearchForConnectedWords");
            }
            Sorting();
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
                newWord.right = true;
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
            foreach (Word word in listWordStruct)
            {
                if (listWordStruct.Count > 0)
                {
                    List<Label> tempListLabel = word.listLabel;
                    foreach (Label label in tempListLabel)
                    {
                        foreach (Word word2 in listWordStruct)
                        {
                            if (word.listLabel != word2.listLabel && word2.SearchForMatches(label) == true)
                            {
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

                            }
                        }
                    }
                }
            }
        }
        public void Sorting()
        {
            try
            {
                // Хранить оставшеся слова, если они не соединены. И запускать по новой. Если соединённые закончились.
                // Добавить цикла на стырый списко. Удалять в процессе. Искать совпадение при добавлении в темп
                List<Word> OldList = new List<Word>(listWordStruct);
                List<Word> tempList = new List<Word>();
                List<Word> NewList = new List<Word>();
                List<Word> matchList = new List<Word>();
                float maxD = 0;
                int index = 0;
                for (int i = 0; i < listWordStruct.Count; i++)
                {
                    float D = (float)listWordStruct[i].ConnectionLabel.Count / listWordStruct[i].listLabel.Count;
                    if (D > maxD)
                    {
                        maxD = D;
                        index = i;
                    }
                }
                tempList.Add(listWordStruct[index]);
                matchList.Add(listWordStruct[index]);
                OldList.RemoveAt(index);

                while (tempList.Count > 0)
                {
                    maxD = 0;
                    index = 0;
                    for (int i = 0; i < tempList.Count; i++)
                    {
                        float D = (float)tempList[i].ConnectionLabel.Count / tempList[i].listLabel.Count;
                        if (D > maxD)
                        {
                            maxD = D;
                            index = i;
                        }
                    }
                    foreach (var item in tempList[index].ConnectionWords)
                    {
                        if (matchList.Contains(item) == false)
                        {
                            int index2 = OldList.IndexOf(item);
                            OldList.RemoveAt(index2);
                            tempList.Add(item);
                            matchList.Add(item);
                        }
                    }
                    NewList.Add(tempList[index]);
                    tempList.RemoveAt(index);
                    if (tempList.Count == 0 && OldList.Count > 0)
                    {
                        maxD = 0;
                        index = 0;
                        for (int i = 0; i < OldList.Count; i++)
                        {
                            float D = (float)OldList[i].ConnectionLabel.Count / OldList[i].listLabel.Count;
                            if (D > maxD)
                            {
                                maxD = D;
                                index = i;
                            }
                        }
                        tempList.Add(OldList[index]);
                        matchList.Add(OldList[index]);
                        OldList.RemoveAt(index);
                    }
                }
                listWordStruct = NewList;
            }
            catch
            {
                MessageBox.Show("Error");
            }
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
            int maxDifficulty = 0;
            int maxWordCount = 0;
            foreach (Word word in listWords)
            {
                if (word.ConnectionLabel.Count == maxDifficulty)
                {
                    if (word.listLabel.Count > maxWordCount)
                    {
                        maxWordCount = word.listLabel.Count;
                        theHardestWord = word;
                    }
                }
                else if (word.ConnectionLabel.Count > maxDifficulty)
                {
                    maxDifficulty = word.ConnectionLabel.Count;
                    theHardestWord = word;
                    maxWordCount = word.listLabel.Count;
                }
            }
            return theHardestWord;
        }
    }
}