from crossword_python.viewmodels.CellViewModel import CellViewModel

class HeaderViewModel:
    def __init__(self):
        self.content = ""
        self.x = 0
        self.y = 0

    @property
    def display_x(self) -> int:
        return self.x * CellViewModel.CELL_SIZE

    @property
    def display_y(self) -> int:
        return self.y * CellViewModel.CELL_SIZE

    @property
    def width(self) -> int:
        return CellViewModel.CELL_SIZE

    @property
    def height(self) -> int:
        return CellViewModel.CELL_SIZE