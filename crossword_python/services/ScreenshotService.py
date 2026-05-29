import random
from PIL import Image, ImageDraw, ImageFont
from crossword_python.models.Word import Word
from crossword_python.models.Dictionary import Dictionary
from crossword_python.viewmodels.CellViewModel import CellViewModel

class ScreenshotService:
    def __init__(self, dialog_service):
        self._dialog_service = dialog_service

    def export_crossword(self, list_words_grid: list[Word], list_dictionaries: list[Dictionary], cells: list[CellViewModel]):
        try:
            top_max_x = 99
            left_max_y = 99
            down_max_x = 0
            right_max_y = 0
            size_cell = 37.938105

            transparent_cells = [c for c in cells if c.background == "transparent"]
            if not transparent_cells:
                top_max_x = 1
                down_max_x = 0
            else:
                top_max_x = min(c.x for c in transparent_cells)
                left_max_y = min(c.y for c in transparent_cells)
                down_max_x = max(c.x for c in transparent_cells)
                right_max_y = max(c.y for c in transparent_cells)

            if down_max_x < top_max_x or right_max_y < left_max_y:
                self._dialog_service.show_message("Не найдено ячеек для создания скриншота. Убедитесь, что кроссворд сгенерирован.")
                return

            width = int((down_max_x - top_max_x + 1) * size_cell)
            height = int((right_max_y - left_max_y + 1) * size_cell)

            img_empty = Image.new('RGB', (width, height), color='white')
            draw_empty = ImageDraw.Draw(img_empty)
            horizontal_definitions = [ ]
            vertical_definitions = [ ]

            self._create_empty_grid(draw_empty, top_max_x, down_max_x, left_max_y, right_max_y, size_cell, horizontal_definitions, vertical_definitions, cells, list_words_grid)
            img_empty.save("EmptyGrid.png", format="PNG")

            img_fill = Image.new('RGB', (width, height), color='white')
            draw_fill = ImageDraw.Draw(img_fill)
            self._create_fill_grid(draw_fill, top_max_x, down_max_x, left_max_y, right_max_y, size_cell, cells)
            img_fill.save("FillGrid.png", format="PNG")

            self._create_answer_file(horizontal_definitions, vertical_definitions)
            self._create_definition_file(horizontal_definitions, vertical_definitions, list_dictionaries)
            self._dialog_service.show_message("Кроссворд сохранён")

        except Exception as e:
            self._dialog_service.show_message(f"Ошибка при создании скриншота:\n{str(e)}")

    def _create_empty_grid(self, draw: ImageDraw.ImageDraw, top_max_x: int, down_max_x: int, left_max_y: int, right_max_y: int, size_cell: float, horizontal_definitions: list[str], vertical_definitions: list[str], cells: list[CellViewModel], list_words_grid: list[Word]):
        try:
            font = ImageFont.truetype("arial.ttf", 12)
        except Exception:
            font = ImageFont.load_default()
        
        for cell in (c for c in cells if top_max_x <= c.x <= down_max_x and left_max_y <= c.y <= right_max_y):
            x0 = (cell.x - top_max_x) * size_cell
            y0 = (cell.y - left_max_y) * size_cell
            x1 = x0 + size_cell
            y1 = y0 + size_cell
            if cell.background == "transparent":
                draw.rectangle([x0, y0, x1, y1], fill="white", outline="black")
            else:
                draw.rectangle([x0, y0, x1, y1], fill="black")

        numbered_start_cells = {}
        number_counter = 1

        valid_words = [w for w in list_words_grid if len(w.cells) > 0]
        ordered_words = sorted(valid_words, key=lambda w: (w.cells[0].y, w.cells[0].x, 0 if w.right else 1))

        for word in ordered_words:
            start_cell = word.cells[0]
            cell_point = (start_cell.x, start_cell.y)

            if cell_point not in numbered_start_cells:
                word_number = number_counter
                number_counter += 1
                numbered_start_cells[cell_point] = word_number
                draw_x = (start_cell.x - top_max_x) * size_cell
                draw_y = (start_cell.y - left_max_y) * size_cell
                draw.text((draw_x + 2, draw_y + 2), str(word_number), fill="black", font=font)
            else:
                word_number = numbered_start_cells[cell_point]

            text = f"{word_number};{word.word_string}"
            if word.right:
                horizontal_definitions.append(text)
            else:
                vertical_definitions.append(text)

    def _create_fill_grid(self, draw: ImageDraw.ImageDraw, top_max_x: int, down_max_x: int, left_max_y: int, right_max_y: int, size_cell: float, cells: list[CellViewModel]):
        try:
            font = ImageFont.truetype("arial.ttf", 20)
        except Exception:
            font = ImageFont.load_default()
        
        for cell in (c for c in cells if top_max_x <= c.x <= down_max_x and left_max_y <= c.y <= right_max_y):
            x0 = (cell.x - top_max_x) * size_cell
            y0 = (cell.y - left_max_y) * size_cell
            x1 = x0 + size_cell
            y1 = y0 + size_cell

            if cell.background == "black":
                draw.rectangle([x0, y0, x1, y1], fill="black")
            else:
                draw.rectangle([x0, y0, x1, y1], fill="white", outline="black")
                content = cell.content.upper() if cell.content else ""
                if content:
                    # Базовое центрирование текста
                    draw.text((x0 + (size_cell / 3), y0 + (size_cell / 4)), content, fill="black", font=font)

    def _create_answer_file(self, horizontal_definitions: list[str], vertical_definitions: list[str]):
        answer_string = "По горизонтали: "
        for hd in horizontal_definitions:
            parts = hd.split(';')
            answer_string += f"{parts[0]}. {parts[1]}. "

        answer_string += "\nПо вертикали: "
        for vd in vertical_definitions:
            parts = vd.split(';')
            answer_string += f"{parts[0]}. {parts[1]}. "

        with open("Answer.txt", "w", encoding="utf-8") as f:
            f.write(answer_string)

    def _create_definition_file(self, horizontal_definitions: list[str], vertical_definitions: list[str], list_dictionaries: list[Dictionary]):
        list_words_string = [ ]
        for dictionary in list_dictionaries:
            list_words_string.extend(dictionary.words)

        definition_string = "По горизонтали: "
        for hd in horizontal_definitions:
            parts = hd.split(';')
            word1 = parts[1]
            for definition in list_words_string:
                if word1 == definition.answers and definition.definitions:
                    random_index = random.randint(0, len(definition.definitions) - 1)
                    definition_string += f"{parts[0]}. {definition.definitions[random_index]}. "
                    break

        definition_string += "\nПо вертикали: "
        for vd in vertical_definitions:
            parts = vd.split(';')
            word1 = parts[1]
            for definition in list_words_string:
                if word1 == definition.answers and definition.definitions:
                    random_index = random.randint(0, len(definition.definitions) - 1)
                    definition_string += f"{parts[0]}. {definition.definitions[random_index]}. "
                    break

        with open("Definition.txt", "w", encoding="utf-8") as f:
            f.write(definition_string)