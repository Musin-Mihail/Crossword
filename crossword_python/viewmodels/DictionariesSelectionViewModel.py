import os
from crossword_python.core.viewmodel_base import ViewModelBase
from crossword_python.core.command import RelayCommand
from crossword_python.viewmodels.DictionaryInfoViewModel import DictionaryInfoViewModel
from PySide6.QtCore import Signal

class DictionariesSelectionViewModel(ViewModelBase):
    close_requested = Signal()

    def __init__(self, dictionary_service):
        super().__init__()
        self.dictionaries: list[DictionaryInfoViewModel] = [ ]
        self.selection_result: list[str] = [ ]
        self.accept_command = RelayCommand(self.accept_selection)

        dict_paths = dictionary_service.get_dictionary_paths()
        for path in dict_paths:
            with open(path, 'r', encoding='utf-8-sig') as f:
                count_words = len(f.readlines())
            name = os.path.splitext(os.path.basename(path))[0]
            self.dictionaries.append(DictionaryInfoViewModel(name, count_words))

    def accept_selection(self, parameter=None):
        self.selection_result.clear()
        for dict_info in self.dictionaries:
            if dict_info.selected_word_count.isdigit():
                count = int(dict_info.selected_word_count)
                if count > 0:
                    final_count = dict_info.total_word_count if count > dict_info.total_word_count else count
                    self.selection_result.append(f"{dict_info.name};{final_count}")
        self.close_requested.emit()