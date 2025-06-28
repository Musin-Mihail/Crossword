using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using Crossword.Objects;
using Crossword.ViewModel;

namespace Crossword.Screenshot;

public class CreateImage
{
    public static void Get(List<Word> listWordsGrid, List<Dictionary> listDictionaries, IEnumerable<CellViewModel> cells)
    {
        try
        {
            var topMaxX = 99;
            var leftMaxY = 99;
            var downMaxX = 0;
            var rightMaxY = 0;
            const float sizeCell = 37.938105f;
            MaxCoordinateSearch.Get(ref topMaxX, ref downMaxX, ref leftMaxY, ref rightMaxY, cells);
            if (downMaxX < topMaxX || rightMaxY < leftMaxY)
            {
                MessageBox.Show("Не найдено ячеек для создания скриншота. Убедитесь, что кроссворд сгенерирован.");
                return;
            }

            var width = (int)((downMaxX - topMaxX + 1) * sizeCell);
            var height = (int)((rightMaxY - leftMaxY + 1) * sizeCell);
            var img = new Bitmap(width, height);
            img.SetResolution(300, 300);
            var graphics = Graphics.FromImage(img);
            var listDefinitionRight = new List<string>();
            var listDefinitionDown = new List<string>();
            CreateEmptyGrid.Get(img, graphics, topMaxX, downMaxX, leftMaxY, rightMaxY, sizeCell, listDefinitionRight, listDefinitionDown, cells, listWordsGrid);
            CreateFillGrid.Get(img, graphics, topMaxX, downMaxX, leftMaxY, rightMaxY, sizeCell, cells);
            CreateAnswer.Get(listDefinitionRight, listDefinitionDown);
            CreateDefinition.Get(listDefinitionRight, listDefinitionDown, listDictionaries);
            MessageBox.Show("Кросворд сохранён");
        }
        catch (Exception e)
        {
            MessageBox.Show("Ошибка при создании скриншота:\n" + e.Message);
        }
    }
}