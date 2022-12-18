using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Crossword.Words;

namespace Crossword.GridFill;

public class SearchWord
{
    public static bool Get(List<string> allInsertedWords, List<Label> newListLabel, List<string> words, Word word)
        {
            if (newListLabel.Count < 21)
            {
                if (newListLabel.Count > 1)
                {
                    List<string> listWordsString = new List<string>(words);
                    List<string> tempListString = new List<string>();
                    for (int i = 0; i < newListLabel.Count; i++)
                    {
                        if (newListLabel[i].Content != null)
                        {
                            string tempString = newListLabel[i].Content.ToString();
                            foreach (string item in listWordsString)
                            {
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
                                return true;
                            }
                        }
                    }

                    string newWord = "";
                    foreach (string item in listWordsString)
                    {
                        if (allInsertedWords.Contains(item) == false)
                        {
                            newWord = item;
                            allInsertedWords.Add(newWord);
                            DeleteWord.Get(newWord, word);
                            break;
                        }
                    }

                    if (newWord.Length < 2)
                    {
                        return true;
                    }

                    for (int i = 0; i < newListLabel.Count; i++)
                    {
                        newListLabel[i].Content = newWord[i];
                    }

                    word.full = true;
                    word.wordString = newWord;
                }
            }
            else
            {
                MessageBox.Show("Есть поле больше 20");
            }

            return false;
        }
}