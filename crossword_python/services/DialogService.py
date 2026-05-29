from PySide6.QtWidgets import QMessageBox

class DialogService:
    def __init__(self, viewmodel_factory):
        """
        viewmodel_factory - функция или словарь для получения инстансов ViewModel 
        без создания циклических зависимостей (DI контейнер).
        """
        self.get_viewmodel = viewmodel_factory

    def show_message(self, message: str):
        msg = QMessageBox()
        msg.setIcon(QMessageBox.Information)
        msg.setText(message)
        msg.setWindowTitle("Информация")
        msg.exec()

    def show_change_fill_dialog(self, horizontal: int, vertical: int):
        from crossword_python.views.ChangeFill import ChangeFill
        vm = self.get_viewmodel('change_fill')
        vm.horizontal = str(horizontal)
        vm.vertical = str(vertical)
        dialog = ChangeFill(vm)
        result = dialog.exec()
        if result:
            return True, vm.result_horizontal, vm.result_vertical
        return False, horizontal, vertical

    def show_load_grid_dialog(self):
        from crossword_python.views.LoadGrid import LoadGrid
        vm = self.get_viewmodel('load_grid')
        vm.load_saved_grids()
        dialog = LoadGrid(vm)
        result = dialog.exec()
        if result:
            return True, vm.selected_grid_content
        return False, [ ]

    def show_dictionaries_selection_dialog(self):
        from crossword_python.views.DictionariesSelection import DictionariesSelection
        vm = self.get_viewmodel('dictionaries_selection')
        dialog = DictionariesSelection(vm)
        result = dialog.exec()
        if result:
            return True, vm.selection_result
        return False, [ ]

    def show_required_dictionary_dialog(self, available_dictionaries):
        from crossword_python.views.RequiredDictionary import RequiredDictionary
        vm = self.get_viewmodel('required_dictionary')
        vm.initialize(available_dictionaries)
        dialog = RequiredDictionary(vm)
        dialog.exec()