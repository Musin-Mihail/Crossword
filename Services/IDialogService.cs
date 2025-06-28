using System.Collections.Generic;

namespace Crossword.Services;

public interface IDialogService
{
    bool? ShowChangeFillDialog(ref int horizontal, ref int vertical);
    bool? ShowLoadGridDialog(out string[] listEmptyCellStruct);
    bool? ShowDictionariesSelectionDialog(out List<string> selectedDictionaries);
    void ShowRequiredDictionaryDialog();
    void ShowMessage(string message);
}