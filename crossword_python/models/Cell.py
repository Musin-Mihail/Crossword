from dataclasses import dataclass

@dataclass
class Cell:
    x: int
    y: int
    content: str | None = None