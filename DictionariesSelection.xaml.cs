using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Crossword;

public partial class DictionariesSelection : Window
{
    public bool Ready;
    public readonly List<string> SelectedDictionaries = new();

    public DictionariesSelection()
    {
        InitializeComponent();
        var dictionariesPaths = GetAllDictionaryPaths.Get();
        foreach (var path in dictionariesPaths)
        {
            var countWords = File.ReadAllLines(path).Length;
            var name = Path.GetFileNameWithoutExtension(path);
            var canvas = CreateLineDictionary.CreateLine(name, countWords);
            ItemsControl.Items.Add(canvas);
        }
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        for (var i = 0; i < ItemsControl.Items.Count; i++)
        {
            var uiElement = (Canvas)ItemsControl.ItemContainerGenerator.ContainerFromIndex(i);
            var textBox = (TextBox)uiElement.Children[2];
            if (textBox.Text.Length > 0)
            {
                var count = int.Parse(textBox.Text);
                if (count > 0)
                {
                    var countWords = (Label)uiElement.Children[3];
                    var count2 = int.Parse(countWords.Content.ToString() ?? string.Empty);
                    if (count > count2)
                    {
                        count = count2;
                    }

                    var label = (Label)uiElement.Children[1];
                    SelectedDictionaries.Add(label.Content + ";" + count);
                }
            }
        }

        Ready = true;
        DialogResult = false;
    }
}