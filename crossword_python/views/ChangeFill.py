from PySide6.QtWidgets import QDialog, QVBoxLayout, QHBoxLayout, QLabel, QLineEdit, QPushButton
from PySide6.QtCore import Qt

class ChangeFill(QDialog):
    def __init__(self, viewmodel, parent=None):
        super().__init__(parent)
        self.vm = viewmodel
        self.setWindowTitle("Изменение размера поля")
        self.resize(350, 200)

        layout = QVBoxLayout(self)

        labels_layout = QHBoxLayout()
        labels_layout.addWidget(QLabel("Ширина"))
        labels_layout.addWidget(QLabel("Высота"))
        layout.addLayout(labels_layout)

        inputs_layout = QHBoxLayout()
        self.hor_input = QLineEdit(self.vm.horizontal)
        self.ver_input = QLineEdit(self.vm.vertical)
        self.hor_input.setAlignment(Qt.AlignCenter)
        self.ver_input.setAlignment(Qt.AlignCenter)
        inputs_layout.addWidget(self.hor_input)
        inputs_layout.addWidget(self.ver_input)
        layout.addLayout(inputs_layout)

        btn_apply = QPushButton("Применить")
        layout.addWidget(btn_apply)

        # Bindings
        self.hor_input.textChanged.connect(lambda text: setattr(self.vm, 'horizontal', text))
        self.ver_input.textChanged.connect(lambda text: setattr(self.vm, 'vertical', text))
        btn_apply.clicked.connect(self.vm.accept_command.execute)
        self.vm.close_requested.connect(self.accept)