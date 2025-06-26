using System.Windows.Controls;
using Crossword.Objects;

namespace Crossword.GridFill.SelectionAndInstallation;

public class CheckConnectionLabel
{
    public static bool Get(Word word, Label label)
    {
        foreach (var labelConnection in word.ConnectionLabel)
        {
            if (label == labelConnection)
            {
                foreach (var wordConnection in word.ConnectionWords)
                {
                    if (wordConnection.Full)
                    {
                        foreach (var labelConnectionWord in wordConnection.ConnectionLabel)
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