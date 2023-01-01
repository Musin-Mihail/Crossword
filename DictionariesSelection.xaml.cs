using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Crossword;

public partial class DictionariesSelection : Window
{
    public bool ready = false;
    public List<string> selectedDictionaries = new();

    public DictionariesSelection()
    {
        InitializeComponent();

        List<string> dictionariesPaths = GetAllDictionaryPaths.Get();
        foreach (var path in dictionariesPaths)
        {
            int countWords = File.ReadAllLines(path).Length;
            string name = Path.GetFileNameWithoutExtension(path);
            Canvas canvas = CreateLineDictionary.CreateLine(name, countWords);
            ItemsControl.Items.Add(canvas);
        }
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        for (int i = 0; i < ItemsControl.Items.Count; i++)
        {
            Canvas uiElement = (Canvas)ItemsControl.ItemContainerGenerator.ContainerFromIndex(i);
            TextBox textBox = (TextBox)uiElement.Children[2];
            if (textBox.Text.Length > 0)
            {
                int count = int.Parse(textBox.Text);
                if (count > 0)
                {
                    Label countWords = (Label)uiElement.Children[3];
                    int count2 = int.Parse(countWords.Content.ToString() ?? string.Empty);
                    if (count > count2)
                    {
                        count = count2;
                    }

                    Label label = (Label)uiElement.Children[1];
                    selectedDictionaries.Add(label.Content + ";" + count);
                }
            }
        }

        ready = true;
        DialogResult = false;
    }
}