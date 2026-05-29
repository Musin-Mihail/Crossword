from dataclasses import dataclass, field
from .Cell import Cell
from .Dictionary import Dictionary

@dataclass
class Word:
    full: bool = False
    word_string: str = ""
    right: bool = False
    fix: bool = False
    cells: list[Cell] = field(default_factory=lambda: [ ])
    connection_cells: list[Cell] = field(default_factory=lambda: [ ])
    connection_words: list['Word'] = field(default_factory=lambda: [ ])
    full_dictionaries: list[Dictionary] = field(default_factory=lambda: [ ])