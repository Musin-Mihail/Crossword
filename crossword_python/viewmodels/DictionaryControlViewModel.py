import os
from crossword_python.core.viewmodel_base import ViewModelBase
from crossword_python.core.command import RelayCommand

class DictionaryControlViewModel(ViewModelBase):
    def __init__(self, dialog_service, dictionary_service, crossword_state_service):
        super().__init__()
        self._dialog_service = dialog_service
        self._dictionary_service = dictionary_service
        self._crossword_state_service = crossword_state_service
        self._selected_dictionary_info = "Основной словарь"

        self.reset_dictionaries_command = RelayCommand(self.reset_dictionaries)
        self.select_dictionaries_command = RelayCommand(self.select_dictionaries)
        self.create_required_dictionary_command = RelayCommand(self.create_required_dictionary)
        self.reset_dictionaries()

    @property
    def selected_dictionary_info(self) -> str:
        return self._selected_dictionary_info

    @selected_dictionary_info.setter
    def selected_dictionary_info(self, value: str):
        self.set_property('_selected_dictionary_info', value)

    def reset_dictionaries(self, parameter=None):
        self._crossword_state_service.dictionaries.clear()
        common_dictionary = self._dictionary_service.load_dictionary("dict.txt")
        common_dictionary.name = "Общий"
        common_dictionary.max_count = len(common_dictionary.words)
        self._crossword_state_service.dictionaries.append(common_dictionary)
        self.selected_dictionary_info = "Основной словарь"

    def select_dictionaries(self, parameter=None):
        result, selected_dictionaries = self._dialog_service.show_dictionaries_selection_dialog()
        if result and selected_dictionaries:
            self._crossword_state_service.dictionaries.clear()
            message = "Выбранные словари:\n"
            dict_paths = self._dictionary_service.get_dictionary_paths()
            
            for selected_dict in selected_dictionaries:
                parts = selected_dict.split(';')
                path = next((p for p in dict_paths if os.path.splitext(os.path.basename(p))[0] == parts[0]), None)
                if path:
                    message += selected_dict + "\n"
                    dictionary = self._dictionary_service.load_dictionary(path)
                    dictionary.name = parts[0]
                    dictionary.max_count = int(parts[1])
                    self._crossword_state_service.dictionaries.append(dictionary)

            common_dictionary = self._dictionary_service.load_dictionary("dict.txt")
            common_dictionary.name = "Общий"
            common_dictionary.max_count = len(common_dictionary.words)
            self._crossword_state_service.dictionaries.append(common_dictionary)
            
            self._dialog_service.show_message(message)
            self.selected_dictionary_info = message

    def create_required_dictionary(self, parameter=None):
        self._dialog_service.show_required_dictionary_dialog(self._crossword_state_service.dictionaries)