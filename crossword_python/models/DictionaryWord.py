from dataclasses import dataclass, field

@dataclass
class DictionaryWord:
    answers: str = ""
    definitions: list[str] = field(default_factory=lambda: [ ])