using System.Windows;
using System.Windows.Controls;

namespace Crossword.PlayingField;

public class CreateLabel
{
    public static Label Get()
    {
        var myLabel = new Label
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        return myLabel;
    }
}