from crossword_python.models.Word import Word
from crossword_python.models.Dictionary import Dictionary
from crossword_python.models.Cell import Cell

class CrosswordStateService:
    """Хранилище состояния кроссворда"""
    def __init__(self):
        self.words_grid: list[Word] = [ ]
        self.dictionaries: list[Dictionary] = [ ]
        self.empty_cells: list[Cell] = [ ]