using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Crossword.Objects;
using Crossword.ViewModel;
using Point = System.Drawing.Point;
using Brushes = System.Windows.Media.Brushes;

namespace Crossword.Services;

public class ScreenshotService : IScreenshotService
{
    private readonly IDialogService _dialogService;

    public ScreenshotService(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }

    public void CreateCrosswordFiles(List<Word> listWordsGrid, List<Dictionary> listDictionaries, IEnumerable<CellViewModel> cells)
    {
        try
        {
            var topMaxX = 99;
            var leftMaxY = 99;
            var downMaxX = 0;
            var rightMaxY = 0;
            const float sizeCell = 37.938105f;
            SearchMaxCoordinates(ref topMaxX, ref downMaxX, ref leftMaxY, ref rightMaxY, cells);
            if (downMaxX < topMaxX || rightMaxY < leftMaxY)
            {
                _dialogService.ShowMessage("Не найдено ячеек для создания скриншота. Убедитесь, что кроссворд сгенерирован.");
                return;
            }

            var width = (int)((downMaxX - topMaxX + 1) * sizeCell);
            var height = (int)((rightMaxY - leftMaxY + 1) * sizeCell);
            using var img = new Bitmap(width, height);
            img.SetResolution(300, 300);
            using var graphics = Graphics.FromImage(img);
            var listDefinitionRight = new List<string>();
            var listDefinitionDown = new List<string>();
            CreateEmptyGrid(img, graphics, topMaxX, downMaxX, leftMaxY, rightMaxY, sizeCell, listDefinitionRight, listDefinitionDown, cells, listWordsGrid);
            CreateFillGrid(img, graphics, topMaxX, downMaxX, leftMaxY, rightMaxY, sizeCell, cells);
            CreateAnswerFile(listDefinitionRight, listDefinitionDown);
            CreateDefinitionFile(listDefinitionRight, listDefinitionDown, listDictionaries);
            _dialogService.ShowMessage("Кросворд сохранён");
        }
        catch (Exception e)
        {
            _dialogService.ShowMessage($"Ошибка при создании скриншота:\n{e.Message}");
        }
    }

    private void SearchMaxCoordinates(ref int topMaxX, ref int downMaxX, ref int leftMaxY, ref int rightMaxY, IEnumerable<CellViewModel> cells)
    {
        var transparentCells = cells.Where(c => c.Background == Brushes.Transparent).ToList();
        if (!transparentCells.Any())
        {
            topMaxX = 1;
            downMaxX = 0;
            return;
        }

        topMaxX = transparentCells.Min(c => c.X);
        leftMaxY = transparentCells.Min(c => c.Y);
        downMaxX = transparentCells.Max(c => c.X);
        rightMaxY = transparentCells.Max(c => c.Y);
    }

    private void CreateEmptyGrid(Bitmap img, Graphics graphics, int topMaxX, int downMaxX, int leftMaxY, int rightMaxY, float sizeCell, List<string> listDefinitionRight, List<string> listDefinitionDown, IEnumerable<CellViewModel> cells, List<Word> listWordsGrid)
    {
        var blackBrush = new SolidBrush(Color.Black);
        var whiteBrush = new SolidBrush(Color.White);
        using var blackPen = new Pen(Color.Black, 1);
        using var font = new Font("Arial", 4);
        graphics.Clear(Color.White);

        foreach (var cellVm in cells.Where(c => c.X >= topMaxX && c.X <= downMaxX && c.Y >= leftMaxY && c.Y <= rightMaxY))
        {
            if (cellVm.Background == Brushes.Transparent)
            {
                graphics.FillRectangle(whiteBrush, (cellVm.X - topMaxX) * sizeCell, (cellVm.Y - leftMaxY) * sizeCell, sizeCell, sizeCell);
                graphics.DrawRectangle(blackPen, (cellVm.X - topMaxX) * sizeCell, (cellVm.Y - leftMaxY) * sizeCell, sizeCell, sizeCell);
            }
            else
            {
                graphics.FillRectangle(blackBrush, (cellVm.X - topMaxX) * sizeCell, (cellVm.Y - leftMaxY) * sizeCell, sizeCell, sizeCell);
            }
        }

        var numberedStartCells = new Dictionary<Point, int>();
        var numberCounter = 1;
        Cell GetStartingCell(Word word) => word.Cells.FirstOrDefault();
        var orderedWords = listWordsGrid
            .Select(w => new { WordObject = w, StartCell = GetStartingCell(w) })
            .Where(x => x.StartCell != null)
            .OrderBy(x => x.StartCell.Y)
            .ThenBy(x => x.StartCell.X)
            .ThenBy(x => x.WordObject.Right ? 0 : 1)
            .Select(x => x.WordObject);

        foreach (var word in orderedWords)
        {
            var startingCellState = GetStartingCell(word);
            if (startingCellState == null) continue;
            var cellPoint = new Point(startingCellState.X, startingCellState.Y);
            if (!numberedStartCells.TryGetValue(cellPoint, out var wordNumber))
            {
                wordNumber = numberCounter++;
                numberedStartCells[cellPoint] = wordNumber;
                var drawX = (startingCellState.X - topMaxX) * sizeCell;
                var drawY = (startingCellState.Y - leftMaxY) * sizeCell;
                graphics.DrawString(wordNumber.ToString(), font, blackBrush, drawX + 1, drawY + 1);
            }

            var text = wordNumber + ";" + word.WordString;
            if (word.Right)
            {
                listDefinitionRight.Add(text);
            }
            else
            {
                listDefinitionDown.Add(text);
            }
        }

        img.Save("EmptyGrid.png", ImageFormat.Png);
    }

    private void CreateFillGrid(Bitmap img, Graphics graphics, int topMaxX, int downMaxX, int leftMaxY, int rightMaxY, float sizeCell, IEnumerable<CellViewModel> cells)
    {
        var blackBrush = new SolidBrush(Color.Black);
        using var blackPen = new Pen(Color.Black, 1);
        using var font = new Font("Arial", 7);
        graphics.Clear(Color.White);

        foreach (var cell in cells.Where(c => c.X >= topMaxX && c.X <= downMaxX && c.Y >= leftMaxY && c.Y <= rightMaxY))
        {
            var drawX = (cell.X - topMaxX) * sizeCell;
            var drawY = (cell.Y - leftMaxY) * sizeCell;

            if (cell.Background == Brushes.Black)
            {
                graphics.FillRectangle(blackBrush, drawX, drawY, sizeCell, sizeCell);
            }
            else
            {
                var format = new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Center
                };
                graphics.FillRectangle(System.Drawing.Brushes.White, drawX, drawY, sizeCell, sizeCell);
                graphics.DrawRectangle(blackPen, drawX, drawY, sizeCell, sizeCell);
                graphics.DrawString(cell.Content?.ToUpper(), font, blackBrush, drawX + (sizeCell / 2), drawY + (sizeCell / 2), format);
            }
        }

        img.Save("FillGrid.png", ImageFormat.Png);
    }

    private void CreateAnswerFile(List<string> listDefinitionRight, List<string> listDefinitionDown)
    {
        var answerString = "По горизонтали: ";
        for (var i = 0; i < listDefinitionRight.Count; i++)
        {
            var newListWord = new List<string>(listDefinitionRight[i].Split(';'));
            var word = newListWord[1];
            answerString += newListWord[0] + ". " + word + ". ";
        }

        answerString += "\nПо вертикали: ";
        for (var i = 0; i < listDefinitionDown.Count; i++)
        {
            var newListWord = new List<string>(listDefinitionDown[i].Split(';'));
            var word = newListWord[1];
            answerString += newListWord[0] + ". " + word + ". ";
        }

        File.WriteAllText("Answer.txt", answerString);
    }

    private void CreateDefinitionFile(List<string> listDefinitionRight, List<string> listDefinitionDown, List<Dictionary> listDictionaries)
    {
        try
        {
            List<DictionaryWord> listWordsString = new();
            foreach (var dictionary in listDictionaries)
            {
                listWordsString.AddRange(dictionary.Words);
            }

            var definitionString = "По горизонтали: ";
            for (var i = 0; i < listDefinitionRight.Count; i++)
            {
                var newListWord = new List<string>(listDefinitionRight[i].Split(';'));
                var word1 = newListWord[1];
                foreach (var definition in listWordsString)
                {
                    var word2 = definition.Answers;
                    if (word1 != word2) continue;
                    var rnd = new Random();
                    var randomIndex = rnd.Next(0, definition.Definitions.Count);
                    definitionString += newListWord[0] + ". " + definition.Definitions[randomIndex] + ". ";
                    break;
                }
            }

            definitionString += "\nПо вертикали: ";
            for (var i = 0; i < listDefinitionDown.Count; i++)
            {
                var newListWord = new List<string>(listDefinitionDown[i].Split(';'));
                var word1 = newListWord[1];
                foreach (var definition in listWordsString)
                {
                    var word2 = definition.Answers;
                    if (word1 == word2)
                    {
                        var rnd = new Random();
                        var randomIndex = rnd.Next(0, definition.Definitions.Count);
                        definitionString += newListWord[0] + ". " + definition.Definitions[randomIndex] + ". ";
                        break;
                    }
                }
            }

            File.WriteAllText("Definition.txt", definitionString);
        }
        catch (Exception e)
        {
            _dialogService.ShowMessage("CreateDefinition\n" + e);
        }
    }
}