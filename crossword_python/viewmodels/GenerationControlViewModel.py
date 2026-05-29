from crossword_python.core.viewmodel_base import ViewModelBase
from crossword_python.core.command import RelayCommand
from PySide6.QtCore import Slot

class GenerationControlViewModel(ViewModelBase):
    def __init__(self, generation_service, dialog_service, grid_manager_service, crossword_state_service):
        super().__init__()
        self._generation_service = generation_service
        self._dialog_service = dialog_service
        self._grid_manager_service = grid_manager_service
        self._crossword_state_service = crossword_state_service

        self._status_message = "Готов к генерации."
        self._is_generating = False
        self._max_seconds_text = "2"
        self._task_delay_text = "100"
        self._is_visualization_checked = False

        self.start_generation_command = RelayCommand(self.start_generation, lambda _: not self.is_generating)
        self.stop_generation_command = RelayCommand(self.stop_generation, lambda _: self.is_generating)

        self._generation_service.status_updated.connect(self.on_status_updated)
        self._generation_service.visualize_word_placement.connect(self.on_visualize_word_placement)
        self._generation_service.clear_grid_visualization.connect(self.on_clear_grid_visualization)
        self._generation_service.set_word_request.connect(self.on_set_word_request)
        self._generation_service.clear_word_request.connect(self.on_clear_word_request)
        self._generation_service.generation_finished.connect(self.on_generation_finished)

    @property
    def status_message(self) -> str: return self._status_message
    @status_message.setter
    def status_message(self, value: str): self.set_property('_status_message', value)

    @property
    def is_generating(self) -> bool: return self._is_generating
    @is_generating.setter
    def is_generating(self, value: bool):
        if self.set_property('_is_generating', value):
            self.start_generation_command.raise_can_execute_changed()
            self.stop_generation_command.raise_can_execute_changed()

    @property
    def max_seconds_text(self) -> str: return self._max_seconds_text
    @max_seconds_text.setter
    def max_seconds_text(self, value: str): self.set_property('_max_seconds_text', value)

    @property
    def task_delay_text(self) -> str: return self._task_delay_text
    @task_delay_text.setter
    def task_delay_text(self, value: str): self.set_property('_task_delay_text', value)

    @property
    def is_visualization_checked(self) -> bool: return self._is_visualization_checked
    @is_visualization_checked.setter
    def is_visualization_checked(self, value: bool): self.set_property('_is_visualization_checked', value)

    def start_generation(self, parameter=None):
        if self.is_generating:
            return
        if not self.max_seconds_text.isdigit():
            self._dialog_service.show_message("ОШИБКА. Вводите только цифры в поле 'Макс. секунд'.")
            return

        max_seconds = int(self.max_seconds_text)
        self._crossword_state_service.empty_cells.clear()
        self._crossword_state_service.empty_cells.extend(self._grid_manager_service.get_empty_cells())
        
        if not self._crossword_state_service.empty_cells:
            self._dialog_service.show_message("На поле нет пустых ячеек для генерации.")
            return

        self._crossword_state_service.words_grid.clear()
        self.is_generating = True

        task_delay = int(self.task_delay_text) if self.task_delay_text.isdigit() else 100
        
        self._generation_service.setup(
            self._crossword_state_service.empty_cells,
            self._crossword_state_service.dictionaries,
            max_seconds,
            self.is_visualization_checked,
            task_delay
        )
        self._generation_service.start()

    def stop_generation(self, parameter=None):
        self._generation_service.request_stop()

    @Slot(str)
    def on_status_updated(self, status: str):
        if status.startswith("ГЕНЕРАЦИЯ УДАЛАСЬ"):
            self._dialog_service.show_message(status)
            self.status_message = "Генерация завершена."
        else:
            self.status_message = status

    @Slot()
    def on_clear_grid_visualization(self):
        self._grid_manager_service.clear_all_cells_content()

    @Slot(object, str)
    def on_visualize_word_placement(self, word, color: str):
        # Сложная анимация упрощена до подсветки для совместимости с QThread
        for cell in word.cells:
            cell_vm = self._grid_manager_service.find_cell_vm(cell.x, cell.y)
            if cell_vm:
                cell_vm.content = cell.content
                cell_vm.background = color

    @Slot(object, str)
    def on_set_word_request(self, word, answer: str):
        for i, cell in enumerate(word.cells):
            cell.content = answer[i]
            cell_vm = self._grid_manager_service.find_cell_vm(cell.x, cell.y)
            if cell_vm:
                cell_vm.content = cell.content

    @Slot(object)
    def on_clear_word_request(self, word):
        for cell in word.cells:
            is_intersection = any(
                connected_word.full and any(c.x == cell.x and c.y == cell.y for c in connected_word.cells)
                for connected_word in word.connection_words
            )
            if not is_intersection:
                cell.content = None
                cell_vm = self._grid_manager_service.find_cell_vm(cell.x, cell.y)
                if cell_vm:
                    cell_vm.content = None

    @Slot(list)
    def on_generation_finished(self, result: list):
        self._crossword_state_service.words_grid.extend(result)
        self.is_generating = False
        # Сбросим цвета обратно в transparent
        for cell_vm in self._grid_manager_service.cells:
            if cell_vm.background in ["red", "green"]:
                cell_vm.background = "transparent"