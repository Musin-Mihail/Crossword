using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using Crossword.Words;
using Brushes = System.Windows.Media.Brushes;

namespace Crossword
{
    internal class Screenshot
    {
        Bitmap img = new Bitmap(1150, 1150);
        Pen blackPen = new Pen(Color.Black, 1);
        SolidBrush blackBrush = new SolidBrush(Color.Black);
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush WatermarksBrush = new SolidBrush(Color.LightGray);
        Font drawFont1 = new Font("Arial", 16);
        Font drawFont2 = new Font("Arial", 8);
        List<string> listDefinitionRight = new List<string>();
        List<string> listDefinitionDown = new List<string>();
        int topMaxX = 99;
        int leftMaxY = 99;
        int downMaxX = 99;
        int rightMaxY = 99;
        float sizeCell = 37.938105f;

        public void CreateImage(List<Cell> listCell, List<Word> listWord)
        {
            listDefinitionRight.Clear();
            listDefinitionDown.Clear();
            img.SetResolution(300, 300);
            MaxCoordinateSearch(listCell);
            int width = (int)((downMaxX - topMaxX + 1) * sizeCell);
            int height = (int)((rightMaxY - leftMaxY + 1) * sizeCell);
            img = new Bitmap(width + 2, height + 2);
            Graphics graphics = Graphics.FromImage(img);
            CreateEmptyGrid(graphics, listCell, listWord);
            CreateFillGrid(graphics, listCell, listWord);
            CreateDefinition();
        }

        void MaxCoordinateSearch(List<Cell> listCell)
        {
            topMaxX = 99;
            leftMaxY = 99;
            downMaxX = 0;
            rightMaxY = 0;
            foreach (Cell cell in listCell)
            {
                if (cell.border.Background == Brushes.Transparent)
                {
                    if (cell.x < topMaxX)
                    {
                        topMaxX = cell.x;
                    }

                    if (cell.y < leftMaxY)
                    {
                        leftMaxY = cell.y;
                    }

                    if (cell.x > downMaxX)
                    {
                        downMaxX = cell.x;
                    }

                    if (cell.y > rightMaxY)
                    {
                        rightMaxY = cell.y;
                    }
                }
            }
        }

        void CreateEmptyGrid(Graphics graphics, List<Cell> listCell, List<Word> listWord)
        {
            graphics.Clear(Color.White);

            int count = 0;
            graphics.FillRectangle(blackBrush, 0, 0, (downMaxX - topMaxX + 1) * sizeCell, (rightMaxY - leftMaxY + 1) * sizeCell);
            graphics.DrawRectangle(blackPen, 0, 0, (downMaxX - topMaxX + 1) * sizeCell, (rightMaxY - leftMaxY + 1) * sizeCell);
            AddingWatermarks(graphics);
            foreach (Cell cell in listCell)
            {
                if (cell.border.Background == Brushes.Transparent)
                {
                    graphics.FillRectangle(whiteBrush, (cell.x - topMaxX) * sizeCell, (cell.y - leftMaxY) * sizeCell, sizeCell, sizeCell);
                    graphics.DrawRectangle(blackPen, (cell.x - topMaxX) * sizeCell, (cell.y - leftMaxY) * sizeCell, sizeCell, sizeCell);
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

                            graphics.DrawString(count.ToString(), drawFont2, blackBrush, (cell.x - topMaxX) * sizeCell, (cell.y - leftMaxY) * sizeCell);
                        }
                    }
                }
            }

            img.Save("EmptyGrid.png", ImageFormat.Png);
        }

        void CreateFillGrid(Graphics graphics, List<Cell> listCell, List<Word> listWord)
        {
            graphics.Clear(Color.White);
            AddingWatermarks(graphics);
            foreach (Cell cell in listCell)
            {
                if (cell.border.Background == Brushes.Black)
                {
                    if (cell.x >= topMaxX && cell.x <= downMaxX)
                    {
                        if (cell.y >= leftMaxY && cell.y <= rightMaxY)
                        {
                            graphics.FillRectangle(blackBrush, (cell.x - topMaxX) * sizeCell, (cell.y - leftMaxY) * sizeCell, sizeCell, sizeCell);
                            graphics.DrawRectangle(blackPen, (cell.x - topMaxX) * sizeCell, (cell.y - leftMaxY) * sizeCell, sizeCell, sizeCell);
                        }
                    }
                }
                else
                {
                    graphics.DrawRectangle(blackPen, (cell.x - topMaxX) * sizeCell, (cell.y - leftMaxY) * sizeCell, sizeCell, sizeCell);
                    graphics.DrawString(cell.label.Content.ToString(), drawFont1, blackBrush, ((cell.x - topMaxX) * sizeCell) + 5, ((cell.y - leftMaxY) * sizeCell) + 4);
                }
            }

            img.Save("FillGrid.png", ImageFormat.Png);
        }

        void CreateDefinition()
        {
            string[] array = File.ReadAllLines("dict.txt");
            List<string> listWordsString = array.ToList();

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

            File.WriteAllText("Definition.txt", definitionString);
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