using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace Crossword
{
    internal class Screenshot
    {
        Bitmap img = new Bitmap(1150, 1150);
        Pen blackPen = new Pen(Color.Black, 1);
        SolidBrush blackBrush = new SolidBrush(Color.Black);
        SolidBrush WatermarksBrush = new SolidBrush(Color.LightGray);
        Font drawFont1 = new Font("Arial", 16);
        Font drawFont2 = new Font("Arial", 8);
        List<string> listDefinitionRight = new List<string>();
        List<string> listDefinitionDown = new List<string>();

        float sizeCell = 37.938105f;
        public void CreateImage(List<Cell> listCell, List<Word> listWord)
        {
            Graphics graphics = Graphics.FromImage(img);

            CreateEmptyDrid(graphics, listCell, listWord);
            CreateFillDrid(graphics, listCell, listWord);
            CreateDefinition(graphics);

        }
        void CreateEmptyDrid(Graphics graphics, List<Cell> listCell, List<Word> listWord)
        {
            graphics.Clear(Color.White);
            AddingWatermarks(graphics);
            int count = 0;
            foreach (Cell cell in listCell)
            {
                if (cell.border.Background == System.Windows.Media.Brushes.Black)
                {
                    graphics.FillRectangle(blackBrush, cell.x * sizeCell, cell.y * sizeCell, sizeCell, sizeCell);
                }
                else
                {
                    graphics.DrawRectangle(blackPen, cell.x * sizeCell, cell.y * sizeCell, sizeCell, sizeCell);
                    bool match = false;
                    foreach (Word word in listWord)
                    {
                        if (cell.label == word.listLabel[0])
                        {
                            if (match == false)
                            {
                                count++;
                                match = true;
                            }
                            string text = count + ";";
                            foreach (Label label in word.listLabel)
                            {
                                text += label.Content.ToString();
                            }
                            if (word.right)
                            {


                                listDefinitionRight.Add(text);
                            }
                            else
                            {
                                listDefinitionDown.Add(text);

                            }
                            graphics.DrawString(count.ToString(), drawFont2, blackBrush, (cell.x * sizeCell), (cell.y * sizeCell));
                        }
                    }
                }
            }
            img.Save("EmptyGrid.png", ImageFormat.Png);
        }
        void CreateFillDrid(Graphics graphics, List<Cell> listCell, List<Word> listWord)
        {
            graphics.Clear(Color.White);
            AddingWatermarks(graphics);
            foreach (Cell cell in listCell)
            {
                if (cell.border.Background == System.Windows.Media.Brushes.Black)
                {
                    graphics.FillRectangle(blackBrush, cell.x * sizeCell, cell.y * sizeCell, sizeCell, sizeCell);
                }
                else
                {
                    graphics.DrawRectangle(blackPen, cell.x * sizeCell, cell.y * sizeCell, sizeCell, sizeCell);
                    graphics.DrawString(cell.label.Content.ToString(), drawFont1, blackBrush, (cell.x * sizeCell) + 4, (cell.y * sizeCell) + 4);
                }
            }
            img.Save("FillGrid.png", ImageFormat.Png);
        }
        void CreateDefinition(Graphics graphics)
        {

            string[] array = File.ReadAllLines("dict.txt");
            List<string> listWordsString = array.ToList();

            graphics.Clear(Color.White);
            AddingWatermarks(graphics);

            string definitionString = "По горизонтали: ";
            for (int i = 0; i < listDefinitionRight.Count; i++)
            {
                foreach (string definition in listWordsString)
                {
                    List<string> newListDefinition = new List<string>(definition.Split(';'));
                    List<string> newListWord = new List<string>(listDefinitionRight[i].Split(';'));
                    string word1 = newListWord[1];
                    string word2 = newListDefinition[0];
                    if (word1 == word2)
                    {
                        Random rnd = new Random();
                        int randomIndex = rnd.Next(1, newListDefinition.Count - 1);
                        definitionString += newListWord[0] + "." + newListDefinition[randomIndex] + ". ";
                        break;
                    }
                }
            }
            definitionString += "\nПо вертикали: ";
            for (int i = 0; i < listDefinitionDown.Count; i++)
            {
                foreach (string definition in listWordsString)
                {
                    List<string> newListDefinition = new List<string>(definition.Split(';'));
                    List<string> newListWord = new List<string>(listDefinitionDown[i].Split(';'));
                    string word1 = newListWord[1];
                    string word2 = newListDefinition[0];
                    if (word1 == word2)
                    {
                        Random rnd = new Random();
                        int randomIndex = rnd.Next(1, newListDefinition.Count - 1);
                        definitionString += newListWord[0] + "." + newListDefinition[randomIndex] + ". ";
                        break;
                    }
                }
            }
            graphics.DrawString(definitionString, drawFont2, blackBrush, 10, 2 * 20);
            img.Save("Definition.png", ImageFormat.Png);
        }
        void AddingWatermarks(Graphics graphics)
        {
            string text = "";
            for (int i = 0; i < 100; i++)
            {
                for (int y = 0; y < 10; y++)
                {
                    text += "Разработчик Мусин Михаил. ";
                }
                text += "\n";
            }
            graphics.DrawString(text, drawFont2, WatermarksBrush, 0, 0);
        }
    }
}