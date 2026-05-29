import os
from crossword_python.core.viewmodel_base import ViewModelBase
from crossword_python.core.command import RelayCommand
from crossword_python.models.DictionaryWord import DictionaryWord
from PySide6.QtCore import Signal

class RequiredDictionaryViewModel(ViewModelBase):
    close_requested = Signal()

    def __init__(self, dialog_service):
        super().__init__()
        self._dialog_service = dialog_service
        self._available_dictionaries = [ ]
        self._words_text = ""
        self.create_dictionary_command = RelayCommand(self.create_dictionary, lambda _: bool(self.words_text.strip()))

    @property
    def words_text(self) -> str:
        return self._words_text

    @words_text.setter
    def words_text(self, value: str):
        if self.set_property('_words_text', value):
            self.create_dictionary_command.raise_can_execute_changed()

    def initialize(self, available_dictionaries):
        self._available_dictionaries = available_dictionaries

    def create_dictionary(self, parameter=None):
        words = [w for w in self.words_text.replace('\n', ' ').replace('\r', ' ').split(' ') if w]
        dictionary_words: list[DictionaryWord] = [ ]

        for word in words:
            if not self._search_match(word, dictionary_words):
                self._dialog_service.show_message(f"{word}. Нет совпадений\nСловарь не сформирован")
                return

        self._save_file(dictionary_words)
        self._dialog_service.show_message("Словарь сформирован\n!ОБЯЗАТЕЛЬНЫЕ.txt")
        self.close_requested.emit()

    def _search_match(self, word: str, dictionary_words: list[DictionaryWord]) -> bool:
        for dictionary in self._available_dictionaries:
            for dictionary_word in dictionary.words:
                if dictionary_word.answers.lower() == word.lower():
                    dictionary_words.append(dictionary_word)
                    return True
        return False

    def _save_file(self, dictionary_words: list[DictionaryWord]):
        new_dictionary = [ ]
        for dict_word in dictionary_words:
            line = dict_word.answers + ";" + ";".join(dict_word.definitions)
            new_dictionary.append(line + "\n")
            
        dictionaries_folder = "Dictionaries"
        required_dict_file_name = "!ОБЯЗАТЕЛЬНЫЕ.txt"
        path = os.path.join(dictionaries_folder, required_dict_file_name)
        
        if not os.path.exists(dictionaries_folder):
            os.makedirs(dictionaries_folder)
            
        with open(path, 'w', encoding='utf-8-sig') as f:
            f.writelines(new_dictionary)