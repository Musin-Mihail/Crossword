from dataclasses import dataclass, field
from .DictionaryWord import DictionaryWord

@dataclass
class Dictionary:
    name: str = ""
    max_count: int = 9999
    current_count: int = 0
    words: list[DictionaryWord] = field(default_factory=lambda: [ ])