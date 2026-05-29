from PySide6.QtWidgets import QDialog, QVBoxLayout, QHBoxLayout, QLabel, QLineEdit, QPushButton, QScrollArea, QWidget

class DictionariesSelection(QDialog):
    def __init__(self, viewmodel, parent=None):
        super().__init__(parent)
        self.vm = viewmodel
        self.setWindowTitle("Выбор словарей")
        self.resize(400, 600)

        layout = QVBoxLayout(self)
        layout.addWidget(QLabel("Выберите словари и количество слов"))

        scroll = QScrollArea()
        scroll.setWidgetResizable(True)
        container = QWidget()
        self.container_layout = QVBoxLayout(container)
        
        for dict_info in self.vm.dictionaries:
            row_layout = QHBoxLayout()
            lbl_name = QLabel(dict_info.name)
            
            input_count = QLineEdit(dict_info.selected_word_count)
            input_count.setFixedWidth(60)
            # Замыкание для корректного биндинга
            input_count.textChanged.connect(lambda text, d=dict_info: setattr(d, 'selected_word_count', text))

            lbl_total = QLabel(f" / {dict_info.total_word_count}")

            row_layout.addWidget(lbl_name)
            row_layout.addStretch()
            row_layout.addWidget(input_count)
            row_layout.addWidget(lbl_total)

            self.container_layout.addLayout(row_layout)

        scroll.setWidget(container)
        layout.addWidget(scroll)

        btn_accept = QPushButton("Принять")
        btn_accept.clicked.connect(self.vm.accept_command.execute)
        layout.addWidget(btn_accept)

        self.vm.close_requested.connect(self.accept)