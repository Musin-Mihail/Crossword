from crossword_python.core.viewmodel_base import ViewModelBase
from crossword_python.models.Cell import Cell
from crossword_python.viewmodels.CellViewModel import CellViewModel
from crossword_python.viewmodels.HeaderViewModel import HeaderViewModel

class GridManagerService(ViewModelBase):
    def __init__(self):
        super().__init__()
        self._number_of_cells_horizontally = 30
        self._number_of_cells_vertically = 30

        self._list_all_cell_struct: list[Cell] = [ ]
        self.cells: list[CellViewModel] = [ ]
        self.headers: list[HeaderViewModel] = [ ]

        self._is_vertically_mirror = False
        self._is_horizontally_mirror = False
        self._is_all_mirror = False
        self._is_vertically_mirror_revers = False
        self._is_horizontally_mirror_revers = False

        self.initialize_grid(self._number_of_cells_horizontally, self._number_of_cells_vertically)

    @property
    def field_width(self) -> int:
        return (self._number_of_cells_horizontally + 1) * CellViewModel.CELL_SIZE

    @property
    def field_height(self) -> int:
        return (self._number_of_cells_vertically + 1) * CellViewModel.CELL_SIZE

    def initialize_grid(self, horizontal_size: int, vertical_size: int):
        self._number_of_cells_horizontally = horizontal_size
        self._number_of_cells_vertically = vertical_size

        self.cells.clear()
        self.headers.clear()
        self._list_all_cell_struct.clear()

        for y in range(1, self._number_of_cells_vertically + 1):
            for x in range(1, self._number_of_cells_horizontally + 1):
                cell_vm = CellViewModel()
                cell_vm.x = x
                cell_vm.y = y
                self.cells.append(cell_vm)
                self._list_all_cell_struct.append(Cell(x, y))

        for y in range(0, self._number_of_cells_vertically + 1):
            h_vm = HeaderViewModel()
            h_vm.content = "" if y == 0 else str(y)
            h_vm.x = 0
            h_vm.y = y
            self.headers.append(h_vm)

        for x in range(1, self._number_of_cells_horizontally + 1):
            h_vm = HeaderViewModel()
            h_vm.content = str(x)
            h_vm.x = x
            h_vm.y = 0
            self.headers.append(h_vm)

        self.property_changed.emit("field_width")
        self.property_changed.emit("field_height")

    def handle_cell_interaction(self, cell_vm: CellViewModel, new_color: str):
        if new_color == cell_vm.background:
            return

        if self._is_vertically_mirror:
            self._coloring_horizontal(cell_vm, new_color)
        elif self._is_horizontally_mirror:
            self._coloring_vertical(cell_vm, new_color)
        elif self._is_all_mirror:
            self._coloring_all(cell_vm, new_color)
        elif self._is_vertically_mirror_revers:
            self._coloring_horizontal_revers(cell_vm, new_color)
        elif self._is_horizontally_mirror_revers:
            self._coloring_vertical_revers(cell_vm, new_color)
        else:
            cell_vm.background = new_color

    def set_mirror_modes(self, is_vertically: bool, is_horizontally: bool, is_all: bool, is_vertically_revers: bool, is_horizontally_revers: bool):
        self._is_vertically_mirror = is_vertically
        self._is_horizontally_mirror = is_horizontally
        self._is_all_mirror = is_all
        self._is_vertically_mirror_revers = is_vertically_revers
        self._is_horizontally_mirror_revers = is_horizontally_revers

    def get_empty_cells(self) -> list[Cell]:
        list_empty_cell_struct = [ ]
        for cell_vm in [c for c in self.cells if c.background == "transparent"]:
            cell_model = next((c for c in self._list_all_cell_struct if c.x == cell_vm.x and c.y == cell_vm.y), None)
            if cell_model:
                list_empty_cell_struct.append(cell_model)
        return list_empty_cell_struct

    def find_cell_vm(self, x: int, y: int) -> CellViewModel | None:
        return next((c for c in self.cells if c.x == x and c.y == y), None)

    def clear_all_cells_content(self):
        for cell_vm in self.cells:
            cell_vm.content = None

    def load_grid_from_struct(self, list_empty_cell_struct: list[str]) -> list[Cell]:
        for cell_vm in self.cells:
            cell_vm.background = "black"
            cell_vm.content = None

        empty_cells = [ ]
        for item in list_empty_cell_struct:
            strings = item.split(';')
            if len(strings) == 2 and strings[0].isdigit() and strings[1].isdigit():
                x = int(strings[0])
                y = int(strings[1])
                cell_vm = self.find_cell_vm(x, y)
                if cell_vm:
                    cell_vm.background = "transparent"

                cell_model = next((c for c in self._list_all_cell_struct if c.x == x and c.y == y), None)
                if cell_model:
                    empty_cells.append(cell_model)
        return empty_cells

    # Логика симметричной отрисовки
    def _coloring_cell(self, x: int, y: int, color: str):
        cell_to_color = self.find_cell_vm(x, y)
        if cell_to_color:
            cell_to_color.background = color

    def _coloring_vertical(self, cell: CellViewModel, c: str):
        center = self._number_of_cells_horizontally // 2
        if cell.x <= center:
            cell.background = c
            mx = self._number_of_cells_horizontally - cell.x + 1
            self._coloring_cell(mx, cell.y, c)
        if self._number_of_cells_horizontally % 2 != 0 and cell.x == center + 1:
            cell.background = c

    def _coloring_horizontal(self, cell: CellViewModel, c: str):
        center = self._number_of_cells_vertically // 2
        if cell.y <= center:
            cell.background = c
            my = self._number_of_cells_vertically - cell.y + 1
            self._coloring_cell(cell.x, my, c)
        if self._number_of_cells_vertically % 2 != 0 and cell.y == center + 1:
            cell.background = c

    def _coloring_vertical_revers(self, cell: CellViewModel, c: str):
        center = self._number_of_cells_horizontally // 2
        if cell.x <= center:
            cell.background = c
            mx = self._number_of_cells_horizontally - cell.x + 1
            my = self._number_of_cells_vertically - cell.y + 1
            self._coloring_cell(mx, my, c)
        if self._number_of_cells_horizontally % 2 != 0 and cell.x == center + 1:
            cell.background = c

    def _coloring_horizontal_revers(self, cell: CellViewModel, c: str):
        center = self._number_of_cells_vertically // 2
        if cell.y <= center:
            cell.background = c
            mx = self._number_of_cells_horizontally - cell.x + 1
            my = self._number_of_cells_vertically - cell.y + 1
            self._coloring_cell(mx, my, c)
        if self._number_of_cells_vertically % 2 != 0 and cell.y == center + 1:
            cell.background = c

    def _coloring_all(self, cell: CellViewModel, c: str):
        ch = self._number_of_cells_horizontally // 2
        cv = self._number_of_cells_vertically // 2
        if cell.x <= ch and cell.y <= cv:
            cell.background = c
            mx = self._number_of_cells_horizontally - cell.x + 1
            my = self._number_of_cells_vertically - cell.y + 1
            self._coloring_cell(mx, cell.y, c)
            self._coloring_cell(cell.x, my, c)
            self._coloring_cell(mx, my, c)

        if self._number_of_cells_horizontally % 2 != 0 and cell.x == ch + 1 and cell.y <= cv:
            cell.background = c
            my = self._number_of_cells_vertically - cell.y + 1
            self._coloring_cell(cell.x, my, c)

        if self._number_of_cells_vertically % 2 != 0 and cell.x <= ch and cell.y == cv + 1:
            cell.background = c
            mx = self._number_of_cells_horizontally - cell.x + 1
            self._coloring_cell(mx, cell.y, c)

        if self._number_of_cells_horizontally % 2 != 0 and self._number_of_cells_vertically % 2 != 0 and cell.x == ch + 1 and cell.y == cv + 1:
            cell.background = c