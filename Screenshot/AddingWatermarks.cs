using System.Drawing;

namespace Crossword.Screenshot;

public class AddingWatermarks
{
    public static void Get(Graphics graphics)
    {
        Font font = new Font("Arial", 4);
        SolidBrush brush = new SolidBrush(Color.LightGray);
        string text = "";
        for (int i = 0; i < 100; i++)
        {
            for (int y = 0; y < 10; y++)
            {
                text += "Разработчик Мусин Михаил. ";
            }

            text += "\n";
        }

        graphics.DrawString(text, font, brush, 0, 0);
    }
}