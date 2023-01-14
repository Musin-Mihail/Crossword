using System.Windows.Controls;
using Crossword.Objects;

namespace Crossword.GridFill.SelectionAndInstallation;

public class CheckConnectionLabel
{
    public static bool Get(Word word, Label label)
    {
        foreach (var labelConnection in word.connectionLabel)
        {
            if (label == labelConnection)
            {
                foreach (var wordConnection in word.connectionWords)
                {
                    if (wordConnection.full)
                    {
                        foreach (var labelConnectionWord in wordConnection.connectionLabel)
                        {
                            if (label == labelConnectionWord)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }
}