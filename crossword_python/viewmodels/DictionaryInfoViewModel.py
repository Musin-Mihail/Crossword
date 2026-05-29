from crossword_python.core.viewmodel_base import ViewModelBase

class DictionaryInfoViewModel(ViewModelBase):
    def __init__(self, name: str, total_word_count: int):
        super().__init__()
        self.name = name
        self.total_word_count = total_word_count
        self._selected_word_count = "0"

    @property
    def selected_word_count(self) -> str:
        return self._selected_word_count

    @selected_word_count.setter
    def selected_word_count(self, value: str):
        if value.isdigit() or value == "":
            self.set_property('_selected_word_count', value)