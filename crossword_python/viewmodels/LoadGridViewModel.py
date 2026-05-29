import os
from crossword_python.core.viewmodel_base import ViewModelBase
from crossword_python.viewmodels.GridPreviewViewModel import GridPreviewViewModel
from PySide6.QtCore import Signal

class LoadGridViewModel(ViewModelBase):
    close_requested = Signal(bool)

    def __init__(self):
        super().__init__()
        self.saved_grids: list[GridPreviewViewModel] = [ ]
        self.selected_grid_content: list[str] = [ ]
        self.load_saved_grids()

    def load_saved_grids(self):
        self.saved_grids.clear()
        save_directory = "SaveGrid"
        if not os.path.exists(save_directory):
            return
            
        for file_name in os.listdir(save_directory):
            if file_name.endswith(".grid"):
                full_path = os.path.join(save_directory, file_name)
                self.saved_grids.append(GridPreviewViewModel(full_path, self.on_load_grid, self.on_delete_grid))
        self.property_changed.emit("saved_grids")

    def on_load_grid(self, grid_vm: GridPreviewViewModel):
        with open(grid_vm.file_path, 'r', encoding='utf-8') as f:
            self.selected_grid_content = [line.strip() for line in f.readlines()]
        self.close_requested.emit(True)

    def on_delete_grid(self, grid_vm: GridPreviewViewModel):
        try:
            os.remove(grid_vm.file_path)
            self.saved_grids.remove(grid_vm)
            self.property_changed.emit("saved_grids")
        except Exception:
            pass