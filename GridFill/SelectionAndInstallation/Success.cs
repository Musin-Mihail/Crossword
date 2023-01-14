namespace Crossword.GridFill.SelectionAndInstallation;

public class Success
{
    public static void Get(int i, int maxError)
    {
        Global.windowsText.Content = "ГЕНЕРАЦИЯ УДАЛАСЬ\n";
        Global.windowsText.Content += "Было " + i + " попыток генерации\n";
        Global.windowsText.Content += "Максимум " + maxError + " ошибок в слове за одну генерацию\n";
    }
}