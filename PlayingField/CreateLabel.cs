using System.Windows;
using System.Windows.Controls;

namespace Crossword.PlayingField;

public class CreateLabel
{
    public static Label Get()
    {
        Label myLabel = new Label();
        myLabel.HorizontalAlignment = HorizontalAlignment.Center;
        myLabel.VerticalAlignment = VerticalAlignment.Center;
        return myLabel;
    }
}