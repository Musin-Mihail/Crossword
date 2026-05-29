from PySide6.QtWidgets import QDialog, QVBoxLayout, QLabel, QTextEdit, QPushButton

class RequiredDictionary(QDialog):
    def __init__(self, viewmodel, parent=None):
        super().__init__(parent)
        self.vm = viewmodel
        self.setWindowTitle("Обязательные слова")
        self.resize(450, 450)

        layout = QVBoxLayout(self)
        
        lbl = QLabel("Введите слова через пробел")
        layout.addWidget(lbl)

        self.text_edit = QTextEdit(self.vm.words_text)
        layout.addWidget(self.text_edit)

        self.btn_create = QPushButton("Сформировать словарь")
        layout.addWidget(self.btn_create)

        # Bindings
        self.text_edit.textChanged.connect(self._on_text_changed)
        self.btn_create.clicked.connect(self.vm.create_dictionary_command.execute)
        self.vm.create_dictionary_command.can_execute_changed.connect(self._update_btn_state)
        self.vm.close_requested.connect(self.accept)

        self._update_btn_state()

    def _on_text_changed(self):
        self.vm.words_text = self.text_edit.toPlainText()

    def _update_btn_state(self):
        self.btn_create.setEnabled(self.vm.create_dictionary_command.can_execute())