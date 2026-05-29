from PySide6.QtWidgets import QDialog, QVBoxLayout, QHBoxLayout, QLabel, QPushButton, QScrollArea, QWidget, QGridLayout
from PySide6.QtCore import Qt

class LoadGrid(QDialog):
    def __init__(self, viewmodel, parent=None):
        super().__init__(parent)
        self.vm = viewmodel
        self.setWindowTitle("Загрузка сетки")
        self.resize(1100, 700)

        layout = QVBoxLayout(self)
        
        self.scroll = QScrollArea()
        self.scroll.setWidgetResizable(True)
        self.container = QWidget()
        self.grid_layout = QGridLayout(self.container)
        self.scroll.setWidget(self.container)
        layout.addWidget(self.scroll)

        self.vm.property_changed.connect(self._on_vm_property_changed)
        self.vm.close_requested.connect(self.accept)
        self._populate_grid()

    def _on_vm_property_changed(self, prop_name: str):
        if prop_name == "saved_grids":
            self._populate_grid()

    def _populate_grid(self):
        # Очистка предыдущих элементов
        for i in reversed(range(self.grid_layout.count())): 
            widget_to_remove = self.grid_layout.itemAt(i).widget()
            self.grid_layout.removeWidget(widget_to_remove)
            widget_to_remove.setParent(None)

        row, col = 0, 0
        for grid_vm in self.vm.saved_grids:
            item_widget = QWidget()
            item_layout = QVBoxLayout(item_widget)
            
            lbl = QLabel(grid_vm.file_path.split("/")[-1].split("\\")[-1])
            item_layout.addWidget(lbl)
            
            # Превью сетки
            from PySide6.QtWidgets import QGraphicsView, QGraphicsScene
            from PySide6.QtGui import QBrush, QPen
            from PySide6.QtCore import Qt
            
            preview_view = QGraphicsView()
            preview_view.setFixedSize(160, 160)
            preview_view.setStyleSheet("background-color: black; border: 1px solid gray;")
            preview_view.setHorizontalScrollBarPolicy(Qt.ScrollBarAlwaysOff)
            preview_view.setVerticalScrollBarPolicy(Qt.ScrollBarAlwaysOff)
            
            scene = QGraphicsScene()
            preview_view.setScene(scene)
            
            for cell_vm in grid_vm.preview_cells:
                scene.addRect(
                    cell_vm.display_x, 
                    cell_vm.display_y, 
                    cell_vm.width, 
                    cell_vm.height, 
                    QPen(Qt.black, 0.5), 
                    QBrush(Qt.white)
                )
            
            item_layout.addWidget(preview_view)
            
            # Кнопки
            btn_load = QPushButton("Загрузить")
            btn_delete = QPushButton("Удалить")
            btn_load.clicked.connect(lambda _, g=grid_vm: g.load_command.execute())
            btn_delete.clicked.connect(lambda _, g=grid_vm: g.delete_command.execute())
            
            item_layout.addWidget(btn_load)
            item_layout.addWidget(btn_delete)
            
            self.grid_layout.addWidget(item_widget, row, col)
            col += 1
            if col > 4:
                col = 0
                row += 1