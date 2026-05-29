import time
import random
from PySide6.QtCore import QThread, Signal
from crossword_python.models.Word import Word
from crossword_python.models.Dictionary import Dictionary
from crossword_python.models.Cell import Cell

class GenerationService(QThread):
    # Сигналы для общения с главным (UI) потоком
    status_updated = Signal(str)
    visualize_word_placement = Signal(object, str)  # word (Word), color (str)
    clear_grid_visualization = Signal()
    set_word_request = Signal(object, str)          # word (Word), answer (str)
    clear_word_request = Signal(object)             # word (Word)
    generation_finished = Signal(list)              # list[Word]

    def __init__(self):
        super().__init__()
        self._empty_cell_models: list[Cell] = [ ]
        self._dictionaries: list[Dictionary] = [ ]
        self._word_grid: list[Word] = [ ]
        self._all_inserted_words: list[str] = [ ]
        self._stop = False
        self._max_seconds = 2
        self._index = 0
        self._is_visualization_enabled = False

    def setup(self, empty_cells: list[Cell], dictionaries: list[Dictionary], max_seconds: int, is_visualization_enabled: bool, task_delay: int):
        self._empty_cell_models = empty_cells
        self._dictionaries = dictionaries
        self._max_seconds = max_seconds
        self._is_visualization_enabled = is_visualization_enabled
        self._task_delay = task_delay / 1000.0  # переводим мс в секунды
        self._stop = False
        self._all_inserted_words.clear()
        self._word_grid.clear()

    def request_stop(self):
        self._stop = True

    def run(self):
        """Основной метод потока."""
        self._initialize_word_grid()
        if self._stop:
            self.status_updated.emit("Ошибка: не удалось сформировать очередь слов.")
            self.generation_finished.emit([ ])
            return

        self.status_updated.emit("Сложность - " + str(self._calculate_difficulty_level()))
        self._generate_words()
        self.generation_finished.emit(self._word_grid)

    def _initialize_word_grid(self):
        self._find_all_word_spans()
        self._search_for_connected_words()

    def _find_all_word_spans(self):
        for cell in self._empty_cell_models:
            x = cell.x
            y = cell.y
            is_start_of_horizontal = not any(c.x == x - 1 and c.y == y for c in self._empty_cell_models)
            if is_start_of_horizontal:
                self._save_word_right(x, y)
            
            is_start_of_vertical = not any(c.x == x and c.y == y - 1 for c in self._empty_cell_models)
            if is_start_of_vertical:
                self._save_word_down(x, y)

    def _save_word_right(self, x: int, y: int):
        new_cells = [ ]
        for i in range(x, 31):
            cell = next((c for c in self._empty_cell_models if c.y == y and c.x == i), None)
            if cell:
                new_cells.append(cell)
            else:
                break
        
        if len(new_cells) > 1:
            new_word = Word(right=True)
            new_word.cells.extend(new_cells)
            self._word_grid.append(new_word)

    def _save_word_down(self, x: int, y: int):
        new_cells = [ ]
        for i in range(y, 31):
            cell = next((c for c in self._empty_cell_models if c.y == i and c.x == x), None)
            if cell:
                new_cells.append(cell)
            else:
                break
        
        if len(new_cells) > 1:
            new_word = Word(right=False)
            new_word.cells.extend(new_cells)
            self._word_grid.append(new_word)

    def _search_for_connected_words(self):
        for word in self._word_grid:
            if self._stop:
                return
            self._create_word_specific_dictionaries(word)
            for cell in word.cells:
                for word2 in self._word_grid:
                    if word != word2 and any(c.x == cell.x and c.y == cell.y for c in word2.cells):
                        if word2 not in word.connection_words:
                            word.connection_words.append(word2)
                        
                        connection_cell = next(c for c in word2.cells if c.x == cell.x and c.y == cell.y)
                        if connection_cell not in word.connection_cells:
                            word.connection_cells.append(connection_cell)

    def _generate_words(self):
        max_index = 0
        start_date = time.time()
        single_attempt_date = time.time()
        self._restart_generation_attempt()

        while self._index < len(self._word_grid):
            if (time.time() - single_attempt_date) > self._max_seconds:
                max_index = 0
                single_attempt_date = time.time()
                self._restart_generation_attempt()
                continue

            if self._index > max_index:
                single_attempt_date = time.time()
                max_index = self._index
                self.status_updated.emit(f"Подобрано {self._index} из {len(self._word_grid)}")

            if self._stop:
                self.status_updated.emit("Генерация остановлена пользователем.")
                return

            current_word = self._word_grid[self._index]
            if current_word.full:
                self._index += 1
                continue

            if self._try_insert_word_into_grid(current_word):
                if self._is_visualization_enabled:
                    self.visualize_word_placement.emit(current_word, "green")
                    time.sleep(self._task_delay)  # Пауза для визуализации
                self._index += 1
                continue

            if self._is_visualization_enabled:
                self.visualize_word_placement.emit(current_word, "red")
                time.sleep(self._task_delay)

            self._step_back(current_word)

        if self._index >= len(self._word_grid):
            self._handle_successful_generation(start_date)

    def _try_insert_word_into_grid(self, word: Word) -> bool:
        answer = self._find_matching_word(word)
        if not answer:
            return False

        for i, cell in enumerate(word.cells):
            cell.content = answer[i]

        self.set_word_request.emit(word, answer)
        word.full = True
        word.word_string = answer
        return True

    def _get_grid_pattern(self, word: Word) -> str:
        pattern = ""
        for cell in word.cells:
            pattern += cell.content if cell.content else "*"
        return pattern

    def _find_matching_word(self, word: Word) -> str:
        if len(word.cells) <= 1:
            return ""
        
        grid_pattern = self._get_grid_pattern(word)
        if not grid_pattern:
            return ""

        self._randomize_word_lists(word)
        for dictionary in word.full_dictionaries:
            if not self._is_dictionary_available(dictionary.name):
                continue
            for dictionary_word in dictionary.words:
                if len(dictionary_word.answers) != len(grid_pattern):
                    continue
                
                matches = True
                for i in range(len(grid_pattern)):
                    if grid_pattern[i] != '*' and grid_pattern[i] != dictionary_word.answers[i]:
                        matches = False
                        break
                        
                if matches and dictionary_word.answers not in self._all_inserted_words:
                    self._all_inserted_words.append(dictionary_word.answers)
                    self._add_dictionary_entry(dictionary_word.answers, word)
                    return dictionary_word.answers
        return ""

    def _step_back(self, current_word: Word):
        connected_words = list(current_word.connection_words)
        random.shuffle(connected_words)

        for word_to_clear in connected_words:
            if word_to_clear.full and not word_to_clear.fix:
                self._clear_word_from_grid(word_to_clear)
                if self._try_insert_word_into_grid(current_word):
                    if self._is_visualization_enabled:
                        self.visualize_word_placement.emit(current_word, "green")
                        time.sleep(self._task_delay)
                    self._index = 0
                    return
        
        self._index = 0

    def _clear_word_from_grid(self, word: Word):
        for cell in word.cells:
            is_intersection = any(
                connected_word.full and any(c.x == cell.x and c.y == cell.y for c in connected_word.cells)
                for connected_word in word.connection_words
            )
            if not is_intersection:
                cell.content = None

        self.clear_word_request.emit(word)
        self._remove_word(word)

    def _remove_word(self, word: Word):
        if word.word_string:
            self._remove_dictionary_entry(word.word_string)
            if word.word_string in self._all_inserted_words:
                self._all_inserted_words.remove(word.word_string)
        word.word_string = ""
        word.full = False

    def _restart_generation_attempt(self):
        self._index = 0
        self._all_inserted_words.clear()
        for word in self._word_grid:
            word.full = False
            word.word_string = ""
            word.fix = False
            for cell in word.cells:
                cell.content = None

        self.clear_grid_visualization.emit()
        for dictionary in self._dictionaries:
            dictionary.current_count = 0

        random.shuffle(self._word_grid)

    def _handle_successful_generation(self, start_date: float):
        time_spent = time.time() - start_date
        message = ""
        for dictionary in self._dictionaries:
            message += f"\n{dictionary.name} - {dictionary.current_count}/{dictionary.max_count}"
            dictionary.current_count = 0
        
        self.status_updated.emit(f"ГЕНЕРАЦИЯ УДАЛАСЬ\nза {time_spent:.2f} секунд\n{message}")

    def _calculate_difficulty_level(self) -> float:
        if not self._word_grid:
            return 0.0
        difficulty_level = 0.0
        for word in self._word_grid:
            if len(word.cells) > 0:
                difficulty_level += len(word.connection_cells) / len(word.cells)
        return difficulty_level / len(self._word_grid)

    def _create_word_specific_dictionaries(self, word: Word):
        has_words = False
        word.full_dictionaries.clear()
        for dictionary in self._dictionaries:
            matching_words = [dw for dw in dictionary.words if len(dw.answers) == len(word.cells)]
            if matching_words:
                has_words = True
                new_dict = Dictionary(name=dictionary.name, words=matching_words)
                word.full_dictionaries.append(new_dict)
        
        if not has_words and self._empty_cell_models:
            self._stop = True
            self.status_updated.emit(f"Для слова длиной {len(word.cells)} нет подходящих слов в словарях.")

    def _randomize_word_lists(self, word: Word):
        for dictionary in word.full_dictionaries:
            random.shuffle(dictionary.words)

    def _is_dictionary_available(self, name_dictionary: str) -> bool:
        dictionary = next((d for d in self._dictionaries if d.name == name_dictionary), None)
        return dictionary is None or dictionary.current_count < dictionary.max_count

    def _add_dictionary_entry(self, answer: str, word: Word):
        for dictionary in (d for d in self._dictionaries if d.current_count < d.max_count):
            if any(w.answers == answer for w in dictionary.words):
                if dictionary.name == "!ОБЯЗАТЕЛЬНЫЕ":
                    word.fix = True
                dictionary.current_count += 1
                return

    def _remove_dictionary_entry(self, answer: str):
        for dictionary in self._dictionaries:
            if any(w.answers == answer for w in dictionary.words):
                dictionary.current_count -= 1
                return