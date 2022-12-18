using System.Collections.Generic;
using System.Windows.Controls;
using Crossword.Words;

namespace Crossword.FormationOfAQueue;

public class SearchForConnectedWords
{
    public static void Get(List<Word> listWordStruct)
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
}