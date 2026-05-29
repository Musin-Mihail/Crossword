from crossword_python.core.viewmodel_base import ViewModelBase

class CellViewModel(ViewModelBase):
    CELL_SIZE = 30
    PREVIEW_CELL_SIZE = 5

    def __init__(self):
        super().__init__()
        self._background = "black"
        self._content = None
        self._x = 0
        self._y = 0
        self.is_preview = False

    @property
    def x(self) -> int:
        return self._x

    @x.setter
    def x(self, value: int):
        self.set_property('_x', value)

    @property
    def y(self) -> int:
        return self._y

    @y.setter
    def y(self, value: int):
        self.set_property('_y', value)

    @property
    def display_x(self) -> int:
        return (self.x - 1) * self.PREVIEW_CELL_SIZE if self.is_preview else self.x * self.CELL_SIZE

    @property
    def display_y(self) -> int:
        return (self.y - 1) * self.PREVIEW_CELL_SIZE if self.is_preview else self.y * self.CELL_SIZE

    @property
    def background(self) -> str:
        return self._background

    @background.setter
    def background(self, value: str):
        if value == "black":
            self.content = None
        self.set_property('_background', value)

    @property
    def content(self) -> str | None:
        return self._content

    @content.setter
    def content(self, value: str | None):
        self.set_property('_content', value)

    @property
    def width(self) -> int:
        return self.PREVIEW_CELL_SIZE if self.is_preview else self.CELL_SIZE

    @property
    def height(self) -> int:
        return self.PREVIEW_CELL_SIZE if self.is_preview else self.CELL_SIZE