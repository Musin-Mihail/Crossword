import sys
import os

# Добавляем родительскую директорию в sys.path, чтобы работал прямой запуск `py main.py`
sys.path.insert(0, os.path.abspath(os.path.join(os.path.dirname(__file__), '..')))

from PySide6.QtWidgets import QApplication

# Импорт сервисов
from crossword_python.services.CrosswordStateService import CrosswordStateService
from crossword_python.services.DictionaryService import DictionaryService
from crossword_python.services.GridManagerService import GridManagerService
from crossword_python.services.GenerationService import GenerationService
from crossword_python.services.ScreenshotService import ScreenshotService
from crossword_python.services.DialogService import DialogService

# Импорт ViewModels
from crossword_python.viewmodels.MainViewModel import MainViewModel
from crossword_python.viewmodels.DictionaryControlViewModel import DictionaryControlViewModel
from crossword_python.viewmodels.FileControlViewModel import FileControlViewModel
from crossword_python.viewmodels.GenerationControlViewModel import GenerationControlViewModel
from crossword_python.viewmodels.GridControlViewModel import GridControlViewModel
from crossword_python.viewmodels.ChangeFillViewModel import ChangeFillViewModel
from crossword_python.viewmodels.LoadGridViewModel import LoadGridViewModel
from crossword_python.viewmodels.DictionariesSelectionViewModel import DictionariesSelectionViewModel
from crossword_python.viewmodels.RequiredDictionaryViewModel import RequiredDictionaryViewModel

# Импорт Views
from crossword_python.views.MainWindow import MainWindow

def main():
    # Инициализация приложения Qt
    app = QApplication(sys.argv)
    app.setStyle("Fusion") # Устанавливаем кроссплатформенный стиль

    # 1. Инициализация базовых сервисов
    crossword_state_service = CrosswordStateService()
    dictionary_service = DictionaryService()
    grid_manager_service = GridManagerService()
    generation_service = GenerationService()

    # 2. Фабрика для ViewModels диалоговых окон (DI Container)
    # Позволяет создавать изолированные инстансы VM при каждом открытии диалога
    def viewmodel_factory(name: str):
        if name == 'change_fill':
            return ChangeFillViewModel()
        elif name == 'load_grid':
            return LoadGridViewModel()
        elif name == 'dictionaries_selection':
            return DictionariesSelectionViewModel(dictionary_service)
        elif name == 'required_dictionary':
            return RequiredDictionaryViewModel(dialog_service)
        raise ValueError(f"Неизвестная ViewModel: {name}")

    dialog_service = DialogService(viewmodel_factory)
    screenshot_service = ScreenshotService(dialog_service)

    # 3. Инициализация главных контроллеров (ViewModels)
    dictionary_controls = DictionaryControlViewModel(dialog_service, dictionary_service, crossword_state_service)
    file_controls = FileControlViewModel(dialog_service, screenshot_service, grid_manager_service, crossword_state_service)
    generation_controls = GenerationControlViewModel(generation_service, dialog_service, grid_manager_service, crossword_state_service)
    
    # Функция обратного вызова для GridControl, чтобы блокировать поле во время генерации
    def can_interact() -> bool:
        return not generation_controls.is_generating

    grid_controls = GridControlViewModel(dialog_service, grid_manager_service, can_interact)

    main_viewmodel = MainViewModel(
        generation_controls=generation_controls,
        grid_controls=grid_controls,
        file_controls=file_controls,
        dictionary_controls=dictionary_controls,
        grid_manager_service=grid_manager_service
    )

    # 4. Инициализация и запуск главного окна
    main_window = MainWindow(main_viewmodel)
    main_window.show()

    # Запуск основного цикла обработки событий
    sys.exit(app.exec())

if __name__ == '__main__':
    main()