from crossword_python.core.viewmodel_base import ViewModelBase
from crossword_python.core.command import RelayCommand
from crossword_python.viewmodels.CellViewModel import CellViewModel

class GridPreviewViewModel(ViewModelBase):
    def __init__(self, file_path: str, load_action, delete_action):
        super().__init__()
        self.file_path = file_path
        self.preview_cells: list[CellViewModel] = [ ]
        self.load_command = RelayCommand(lambda _: load_action(self))
        self.delete_command = RelayCommand(lambda _: delete_action(self))
        self._generate_preview()

    def _generate_preview(self):
        try:
            with open(self.file_path, 'r', encoding='utf-8') as f:
                lines = f.readlines()
            for line in lines:
                parts = line.strip().split(';')
                if len(parts) == 2 and parts[0].isdigit() and parts[1].isdigit():
                    cvm = CellViewModel()
                    cvm.x = int(parts[0])
                    cvm.y = int(parts[1])
                    cvm.background = "white"
                    cvm.is_preview = True
                    self.preview_cells.append(cvm)
        except Exception:
            pass