import os
from datetime import datetime
from crossword_python.core.viewmodel_base import ViewModelBase
from crossword_python.core.command import RelayCommand

class FileControlViewModel(ViewModelBase):
    def __init__(self, dialog_service, screenshot_service, grid_manager_service, crossword_state_service):
        super().__init__()
        self._dialog_service = dialog_service
        self._screenshot_service = screenshot_service
        self._grid_manager_service = grid_manager_service
        self._crossword_state_service = crossword_state_service

        self.save_grid_command = RelayCommand(self.save_grid)
        self.load_grid_command = RelayCommand(self.load_grid)
        self.screenshot_command = RelayCommand(self.screenshot)

    def save_grid(self, parameter=None):
        list_empty_cell_struct = self._grid_manager_service.get_empty_cells()
        save_file = ""
        for cell in list_empty_cell_struct:
            save_file += f"{cell.x};{cell.y}\n"

        name = datetime.now().strftime("%m_%d_%Y-%H_%M_%S")
        try:
            if not os.path.exists("SaveGrid"):
                os.makedirs("SaveGrid")
            with open(f"SaveGrid/{name}.grid", 'w', encoding='utf-8') as f:
                f.write(save_file)
            self._dialog_service.show_message("Сетка сохранена")
        except Exception as ex:
            self._dialog_service.show_message(f"Ошибка сохранения сетки: {ex}")

    def load_grid(self, parameter=None):
        result, list_empty_cell_struct = self._dialog_service.show_load_grid_dialog()
        if result and list_empty_cell_struct:
            self._crossword_state_service.empty_cells.clear()
            self._crossword_state_service.empty_cells.extend(
                self._grid_manager_service.load_grid_from_struct(list_empty_cell_struct)
            )

    def screenshot(self, parameter=None):
        if self._crossword_state_service.words_grid and any(c.background == "transparent" for c in self._grid_manager_service.cells):
            self._screenshot_service.export_crossword(
                self._crossword_state_service.words_grid,
                self._crossword_state_service.dictionaries,
                self._grid_manager_service.cells
            )
        else:
            self._dialog_service.show_message("Сетка не заполнена словами после генерации.")