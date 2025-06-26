using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Crossword;

public class CreateLineDictionary
{
    public static Canvas CreateLine(string nameD, int countWords)
    {
        var myBorder = new Border
        {
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(1, 0, 1, 1),
            Padding = new Thickness(300, 30, 0, 0)
        };
        Canvas canvas = new()
        {
            Background = Brushes.Aqua,
            Height = 30,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left
        };
        canvas.Children.Add(myBorder);
        canvas.Children.Add(GetLabelName(nameD));
        canvas.Children.Add(GetTextBox());
        canvas.Children.Add(GetLabelCountWords(countWords));

        return canvas;
    }

    private static Label GetLabelName(string nameD)
    {
        Label name = new()
        {
            Content = nameD,
            Height = 30,
            Width = 150,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalContentAlignment = VerticalAlignment.Center
        };
        return name;
    }

    private static TextBox GetTextBox()
    {
        TextBox count = new()
        {
            Text = "0",
            Height = 30,
            Width = 50,
            Margin = new Thickness(150, 0, 0, 0),
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalContentAlignment = VerticalAlignment.Center
        };
        count.TextChanged += TextChangedEventHandler;
        return count;
    }

    private static Label GetLabelCountWords(int countWords)
    {
        Label countWordsL = new()
        {
            Content = countWords,
            Height = 30,
            Width = 100,
            Margin = new Thickness(200, 0, 0, 0),
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalContentAlignment = VerticalAlignment.Center
        };
        return countWordsL;
    }

    private static void TextChangedEventHandler(object sender, TextChangedEventArgs args)
    {
        var textBox = (TextBox)sender;
        var newText = "";

        foreach (var char1 in textBox.Text)
        {
            if (char.IsDigit(char1))
            {
                newText += char1;
            }
        }

        textBox.Text = newText;
        textBox.SelectionStart = textBox.Text.Length;
    }
}