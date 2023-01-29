using System.Collections.Generic;
using System.Windows.Controls;
using Crossword.Objects;
using Crossword.Words;

namespace Crossword.FormationOfAQueue;

public class SearchForConnectedWords
{
    public static void Get()
    {
        foreach (Word word in Global.listWordsGrid)
        {
            List<Label> tempListLabel = word.listLabel;
            CreateWordDictionary.Get(word);
            foreach (Label label in tempListLabel)
            {
                foreach (Word word2 in Global.listWordsGrid)
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