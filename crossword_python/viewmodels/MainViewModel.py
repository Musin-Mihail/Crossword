from crossword_python.core.viewmodel_base import ViewModelBase
from crossword_python.core.command import RelayCommand

class MainViewModel(ViewModelBase):
    def __init__(self, generation_controls, grid_controls, file_controls, dictionary_controls, grid_manager_service):
        super().__init__()
        self.generation_controls = generation_controls
        self.grid_controls = grid_controls
        self.file_controls = file_controls
        self.dictionary_controls = dictionary_controls
        self._grid_manager_service = grid_manager_service
        self._difficulty = "Сложность: -"

        self.generation_controls.property_changed.connect(self.on_generation_controls_property_changed)

    def on_generation_controls_property_changed(self, prop_name: str):
        if prop_name == "is_generating":
            self.property_changed.emit("is_ui_enabled")
            self.grid_controls.cell_interaction_command.raise_can_execute_changed()
            self.grid_controls.clear_grid_command.raise_can_execute_changed()
            # Обновление других команд если нужно
        elif prop_name == "status_message":
            status = self.generation_controls.status_message
            if status.startswith("Сложность"):
                self.difficulty = status

    @property
    def is_ui_enabled(self) -> bool:
        return not self.generation_controls.is_generating

    @property
    def difficulty(self) -> str:
        return self._difficulty

    @difficulty.setter
    def difficulty(self, value: str):
        self.set_property('_difficulty', value)

    @property
    def cells(self):
        return self._grid_manager_service.cells

    @property
    def headers(self):
        return self._grid_manager_service.headers