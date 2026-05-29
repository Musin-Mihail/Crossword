from crossword_python.core.viewmodel_base import ViewModelBase
from crossword_python.core.command import RelayCommand
from PySide6.QtCore import Signal

class ChangeFillViewModel(ViewModelBase):
    close_requested = Signal(bool)

    def __init__(self):
        super().__init__()
        self._horizontal = "30"
        self._vertical = "30"
        self.result_horizontal = 30
        self.result_vertical = 30

        self.accept_command = RelayCommand(self.accept)

    @property
    def horizontal(self) -> str:
        return self._horizontal

    @horizontal.setter
    def horizontal(self, value: str):
        self.set_property('_horizontal', value)

    @property
    def vertical(self) -> str:
        return self._vertical

    @vertical.setter
    def vertical(self, value: str):
        self.set_property('_vertical', value)

    def accept(self, parameter=None):
        if self.horizontal.isdigit() and self.vertical.isdigit():
            h = int(self.horizontal)
            v = int(self.vertical)
            self.result_horizontal = 30 if h > 30 else h
            self.result_vertical = 30 if v > 30 else v
            self.close_requested.emit(True)