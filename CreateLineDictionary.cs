using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Crossword;

public class CreateLineDictionary
{
    public static Canvas CreateLine(string nameD, int countWords)
    {
        Canvas convas = new();
        convas.Background = Brushes.Aqua;
        convas.Height = 30;
        convas.VerticalAlignment = VerticalAlignment.Top;
        convas.HorizontalAlignment = HorizontalAlignment.Left;

        Border myBorder = new Border();
        myBorder.BorderBrush = Brushes.Black;
        myBorder.BorderThickness = new Thickness(1, 0, 1, 1);
        myBorder.Padding = new Thickness(300, 30, 0, 0);
        convas.Children.Add(myBorder);

        convas.Children.Add(GetLabelName(nameD));
        convas.Children.Add(GetTextBox());
        convas.Children.Add(GetLabelCountWords(countWords));

        return convas;
    }

    private static Label GetLabelName(string nameD)
    {
        Label name = new();
        name.Content = nameD;
        name.Height = 30;
        name.Width = 150;
        name.VerticalAlignment = VerticalAlignment.Top;
        name.HorizontalAlignment = HorizontalAlignment.Left;
        name.VerticalContentAlignment = VerticalAlignment.Center;
        return name;
    }

    private static TextBox GetTextBox()
    {
        TextBox count = new();
        count.Text = "0";
        count.Height = 30;
        count.Width = 50;
        count.Margin = new Thickness(150, 0, 0, 0);
        count.VerticalAlignment = VerticalAlignment.Top;
        count.HorizontalAlignment = HorizontalAlignment.Left;
        count.VerticalContentAlignment = VerticalAlignment.Center;
        count.TextChanged += textChangedEventHandler;
        return count;
    }

    private static Label GetLabelCountWords(int countWords)
    {
        Label countWordsL = new();
        countWordsL.Content = countWords;
        countWordsL.Height = 30;
        countWordsL.Width = 100;
        countWordsL.Margin = new Thickness(200, 0, 0, 0);
        countWordsL.VerticalAlignment = VerticalAlignment.Top;
        countWordsL.HorizontalAlignment = HorizontalAlignment.Left;
        countWordsL.VerticalContentAlignment = VerticalAlignment.Center;
        return countWordsL;
    }

    private static void textChangedEventHandler(object sender, TextChangedEventArgs args)
    {
        TextBox textBox = (TextBox)sender;
        string newText = "";

        foreach (var char1 in textBox.Text)
        {
            if (Char.IsDigit(char1))
            {
                newText += char1;
            }
        }

        textBox.Text = newText;
        textBox.SelectionStart = textBox.Text.Length;
    }
}