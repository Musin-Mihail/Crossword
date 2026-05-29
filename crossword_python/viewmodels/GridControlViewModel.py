from crossword_python.core.viewmodel_base import ViewModelBase
from crossword_python.core.command import RelayCommand
from crossword_python.viewmodels.CellViewModel import CellViewModel

class GridControlViewModel(ViewModelBase):
    def __init__(self, dialog_service, grid_manager_service, can_interact_func):
        super().__init__()
        self._dialog_service = dialog_service
        self._grid_manager_service = grid_manager_service
        self._can_interact_func = can_interact_func
        
        self._number_of_cells_horizontally = 30
        self._number_of_cells_vertically = 30

        self._is_clear_mirror = True
        self._is_vertically_mirror = False
        self._is_horizontally_mirror = False
        self._is_all_mirror = False
        self._is_vertically_mirror_revers = False
        self._is_horizontally_mirror_revers = False

        self.change_field_size_command = RelayCommand(self.change_field_size)
        self.cell_interaction_command = RelayCommand(self.handle_cell_interaction, lambda _: self._can_interact_func())
        self.clear_grid_command = RelayCommand(self.clear_grid, lambda _: self._can_interact_func())

        self._grid_manager_service.property_changed.connect(self.on_grid_manager_property_changed)
        self.update_mirror_lines()

    def on_grid_manager_property_changed(self, prop_name: str):
        if prop_name in ["field_width", "field_height"]:
            self.property_changed.emit(prop_name)

    @property
    def field_width(self) -> int:
        return self._grid_manager_service.field_width

    @property
    def field_height(self) -> int:
        return self._grid_manager_service.field_height

    # Зеркальные свойства
    @property
    def is_clear_mirror(self) -> bool: return self._is_clear_mirror
    @is_clear_mirror.setter
    def is_clear_mirror(self, value: bool):
        if self.set_property('_is_clear_mirror', value): self.update_mirror_lines()

    @property
    def is_vertically_mirror(self) -> bool: return self._is_vertically_mirror
    @is_vertically_mirror.setter
    def is_vertically_mirror(self, value: bool):
        if self.set_property('_is_vertically_mirror', value): self.update_mirror_lines()

    @property
    def is_horizontally_mirror(self) -> bool: return self._is_horizontally_mirror
    @is_horizontally_mirror.setter
    def is_horizontally_mirror(self, value: bool):
        if self.set_property('_is_horizontally_mirror', value): self.update_mirror_lines()

    @property
    def is_all_mirror(self) -> bool: return self._is_all_mirror
    @is_all_mirror.setter
    def is_all_mirror(self, value: bool):
        if self.set_property('_is_all_mirror', value): self.update_mirror_lines()

    @property
    def is_vertically_mirror_revers(self) -> bool: return self._is_vertically_mirror_revers
    @is_vertically_mirror_revers.setter
    def is_vertically_mirror_revers(self, value: bool):
        if self.set_property('_is_vertically_mirror_revers', value): self.update_mirror_lines()

    @property
    def is_horizontally_mirror_revers(self) -> bool: return self._is_horizontally_mirror_revers
    @is_horizontally_mirror_revers.setter
    def is_horizontally_mirror_revers(self, value: bool):
        if self.set_property('_is_horizontally_mirror_revers', value): self.update_mirror_lines()

    # Свойства линий центра
    @property
    def line_center_v_visible(self) -> bool:
        return self.is_vertically_mirror or self.is_vertically_mirror_revers or self.is_all_mirror

    @property
    def line_center_h_visible(self) -> bool:
        return self.is_horizontally_mirror or self.is_horizontally_mirror_revers or self.is_all_mirror

    @property
    def line_center_h_x(self) -> float:
        return (self._number_of_cells_horizontally * CellViewModel.CELL_SIZE / 2.0) + CellViewModel.CELL_SIZE

    @property
    def line_center_h_y2(self) -> float:
        return (self._number_of_cells_vertically + 1) * CellViewModel.CELL_SIZE

    @property
    def line_center_v_y(self) -> float:
        return (self._number_of_cells_vertically * CellViewModel.CELL_SIZE / 2.0) + CellViewModel.CELL_SIZE

    @property
    def line_center_v_x2(self) -> float:
        return (self._number_of_cells_horizontally + 1) * CellViewModel.CELL_SIZE

    def handle_cell_interaction(self, params: tuple):
        cell_vm, new_color = params
        self._grid_manager_service.set_mirror_modes(
            self.is_vertically_mirror, self.is_horizontally_mirror, 
            self.is_all_mirror, self.is_vertically_mirror_revers, self.is_horizontally_mirror_revers
        )
        self._grid_manager_service.handle_cell_interaction(cell_vm, new_color)

    def change_field_size(self, parameter=None):
        result, new_h, new_v = self._dialog_service.show_change_fill_dialog(
            self._number_of_cells_horizontally, self._number_of_cells_vertically
        )
        if result:
            self._number_of_cells_horizontally = new_h
            self._number_of_cells_vertically = new_v
            self._grid_manager_service.initialize_grid(self._number_of_cells_horizontally, self._number_of_cells_vertically)
            self.update_mirror_lines()

    def clear_grid(self, parameter=None):
        self._grid_manager_service.initialize_grid(self._number_of_cells_horizontally, self._number_of_cells_vertically)
        self.update_mirror_lines()

    def update_mirror_lines(self):
        self.property_changed.emit("line_center_v_visible")
        self.property_changed.emit("line_center_h_visible")
        self.property_changed.emit("line_center_h_x")
        self.property_changed.emit("line_center_h_y2")
        self.property_changed.emit("line_center_v_y")
        self.property_changed.emit("line_center_v_x2")