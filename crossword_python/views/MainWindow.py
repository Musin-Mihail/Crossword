from PySide6.QtWidgets import QMainWindow, QWidget, QHBoxLayout, QVBoxLayout, QGraphicsView, QGraphicsScene, QPushButton, QLabel, QLineEdit, QCheckBox, QRadioButton, QGraphicsRectItem, QGraphicsTextItem, QButtonGroup
from PySide6.QtGui import QBrush, QColor, QPen, QTransform
from PySide6.QtCore import Qt

class CrosswordGraphicsView(QGraphicsView):
    """Кастомный вьювер для обработки рисования по сетке"""
    def __init__(self, vm, parent=None):
        super().__init__(parent)
        self.vm = vm

    def mousePressEvent(self, event):
        self._handle_mouse(event)

    def mouseMoveEvent(self, event):
        if event.buttons(): # Если кнопка зажата (drag)
            self._handle_mouse(event)

    def _handle_mouse(self, event):
        if not self.vm.is_ui_enabled:
            return
            
        pos = self.mapToScene(event.pos())
        # Текст перекрывает квадрат, поэтому просматриваем все элементы под курсором
        for item in self.scene().items(pos):
            if hasattr(item, 'cell_vm'):
                btns = event.buttons()
                if hasattr(event, 'button'): # Объединяем нажатые кнопки
                    btns |= event.button()
                
                if btns & Qt.LeftButton:
                    self.vm.grid_controls.cell_interaction_command.execute((item.cell_vm, "transparent"))
                elif btns & Qt.RightButton:
                    self.vm.grid_controls.cell_interaction_command.execute((item.cell_vm, "black"))
                break

class MainWindow(QMainWindow):
    def __init__(self, viewmodel):
        super().__init__()
        self.vm = viewmodel
        self.setWindowTitle("Crossword")
        self.resize(1500, 1000)

        main_widget = QWidget()
        self.setCentralWidget(main_widget)
        main_layout = QHBoxLayout(main_widget)

        # Левая часть - сетка
        self.scene = QGraphicsScene()
        self.view = CrosswordGraphicsView(self.vm)
        self.view.setScene(self.scene)
        main_layout.addWidget(self.view, stretch=2)

        # Правая часть - контролы
        controls_layout = QVBoxLayout()
        main_layout.addLayout(controls_layout, stretch=1)

        self._build_controls(controls_layout)

        # Подписки
        self.vm.property_changed.connect(self._on_vm_property_changed)
        self.vm.grid_controls.property_changed.connect(self._on_grid_property_changed)
        self.vm.generation_controls.property_changed.connect(self._on_gen_property_changed)
        self.vm.dictionary_controls.property_changed.connect(self._on_dict_property_changed)

        self._rebuild_grid()

    def _build_controls(self, layout):
        btn_change_size = QPushButton("Изменить поле")
        btn_change_size.clicked.connect(self.vm.grid_controls.change_field_size_command.execute)
        layout.addWidget(btn_change_size)

        self.lbl_difficulty = QLabel(self.vm.difficulty)
        layout.addWidget(self.lbl_difficulty)

        self.lbl_status = QLabel(self.vm.generation_controls.status_message)
        layout.addWidget(self.lbl_status)

        layout.addWidget(QLabel("Количество секунд на одно слово:"))
        self.inp_max_sec = QLineEdit(self.vm.generation_controls.max_seconds_text)
        self.inp_max_sec.textChanged.connect(lambda t: setattr(self.vm.generation_controls, 'max_seconds_text', t))
        layout.addWidget(self.inp_max_sec)

        self.chk_vis = QCheckBox("Визуализация")
        self.chk_vis.setChecked(self.vm.generation_controls.is_visualization_checked)
        self.chk_vis.toggled.connect(lambda checked: setattr(self.vm.generation_controls, 'is_visualization_checked', checked))
        layout.addWidget(self.chk_vis)

        layout.addWidget(QLabel("Задержка визуализации (мс):"))
        self.inp_delay = QLineEdit(self.vm.generation_controls.task_delay_text)
        self.inp_delay.textChanged.connect(lambda t: setattr(self.vm.generation_controls, 'task_delay_text', t))
        layout.addWidget(self.inp_delay)

        self.btn_gen = QPushButton("Генерация")
        self.btn_stop = QPushButton("Стоп")
        self.btn_stop.setVisible(False)
        self.btn_gen.clicked.connect(self.vm.generation_controls.start_generation_command.execute)
        self.btn_stop.clicked.connect(self.vm.generation_controls.stop_generation_command.execute)
        layout.addWidget(self.btn_gen)
        layout.addWidget(self.btn_stop)

        # Зеркальность
        self.group_mirror = QButtonGroup(self)
        
        self.rad_clear = QRadioButton("Обычный")
        self.rad_clear.setChecked(True)
        self.rad_clear.toggled.connect(lambda c: setattr(self.vm.grid_controls, 'is_clear_mirror', c))
        
        self.rad_hor = QRadioButton("Горизонтальный")
        self.rad_hor.toggled.connect(lambda c: setattr(self.vm.grid_controls, 'is_horizontally_mirror', c))
        
        self.rad_ver = QRadioButton("Вертикальный")
        self.rad_ver.toggled.connect(lambda c: setattr(self.vm.grid_controls, 'is_vertically_mirror', c))
        
        self.rad_all = QRadioButton("Полный")
        self.rad_all.toggled.connect(lambda c: setattr(self.vm.grid_controls, 'is_all_mirror', c))

        layout.addWidget(self.rad_clear)
        layout.addWidget(self.rad_hor)
        layout.addWidget(self.rad_ver)
        layout.addWidget(self.rad_all)

        btn_req_dict = QPushButton("Обязательные слова")
        btn_req_dict.clicked.connect(self.vm.dictionary_controls.create_required_dictionary_command.execute)
        layout.addWidget(btn_req_dict)

        btn_sel_dict = QPushButton("Выбор словарей")
        btn_sel_dict.clicked.connect(self.vm.dictionary_controls.select_dictionaries_command.execute)
        layout.addWidget(btn_sel_dict)

        btn_base_dict = QPushButton("Основной словарь")
        btn_base_dict.clicked.connect(self.vm.dictionary_controls.reset_dictionaries_command.execute)
        layout.addWidget(btn_base_dict)

        self.lbl_dict_info = QLabel(self.vm.dictionary_controls.selected_dictionary_info)
        layout.addWidget(self.lbl_dict_info)

        btn_clear = QPushButton("Очистить поле")
        btn_clear.clicked.connect(self.vm.grid_controls.clear_grid_command.execute)
        layout.addWidget(btn_clear)

        btn_save = QPushButton("Сохранить сетку")
        btn_save.clicked.connect(self.vm.file_controls.save_grid_command.execute)
        layout.addWidget(btn_save)

        btn_load = QPushButton("Загрузить сетку")
        btn_load.clicked.connect(self.vm.file_controls.load_grid_command.execute)
        layout.addWidget(btn_load)

        btn_screen = QPushButton("Скриншот")
        btn_screen.clicked.connect(self.vm.file_controls.screenshot_command.execute)
        layout.addWidget(btn_screen)

        layout.addStretch()

    def _rebuild_grid(self):
        self.scene.clear()
        self.cell_items = {} # cell_vm -> (rect, text)

        for h_vm in self.vm.headers:
            rect = self.scene.addRect(h_vm.display_x, h_vm.display_y, h_vm.width, h_vm.height, QPen(Qt.black), QBrush(Qt.white))
            text = self.scene.addText(h_vm.content)
            text.setDefaultTextColor(Qt.black)
            text.setPos(h_vm.display_x + 5, h_vm.display_y + 5)

        for cell_vm in self.vm.cells:
            color = Qt.white if cell_vm.background == "transparent" else (Qt.red if cell_vm.background == "red" else (Qt.green if cell_vm.background == "green" else Qt.black))
            rect = self.scene.addRect(cell_vm.display_x, cell_vm.display_y, cell_vm.width, cell_vm.height, QPen(Qt.black), QBrush(color))
            rect.cell_vm = cell_vm # Привязываем vm к QGraphicsRectItem для мыши
            text = self.scene.addText(cell_vm.content if cell_vm.content else "")
            text.setDefaultTextColor(Qt.black)
            text.setPos(cell_vm.display_x + 5, cell_vm.display_y + 5)
            self.cell_items[cell_vm] = (rect, text)
            
            # Замыкаем локальные переменные для сигнала
            cell_vm.property_changed.connect(lambda prop, cv=cell_vm: self._update_cell(cv, prop))

        self.line_h = self.scene.addLine(0, 0, 0, 0, QPen(Qt.green, 4))
        self.line_h.setZValue(10)
        self.line_v = self.scene.addLine(0, 0, 0, 0, QPen(Qt.green, 4))
        self.line_v.setZValue(10)
        self._update_mirror_lines()

        self.scene.setSceneRect(0, 0, self.vm.grid_controls.field_width, self.vm.grid_controls.field_height)

    def _update_mirror_lines(self):
        gc = self.vm.grid_controls
        
        if gc.line_center_v_visible:
            self.line_v.setLine(0, gc.line_center_v_y, gc.line_center_v_x2, gc.line_center_v_y)
            self.line_v.setVisible(True)
        else:
            self.line_v.setVisible(False)
            
        if gc.line_center_h_visible:
            self.line_h.setLine(gc.line_center_h_x, 0, gc.line_center_h_x, gc.line_center_h_y2)
            self.line_h.setVisible(True)
        else:
            self.line_h.setVisible(False)

    def _update_cell(self, cell_vm, prop_name):
        if cell_vm not in self.cell_items: return
        rect, text = self.cell_items[cell_vm]
        
        if prop_name == "background":
            bg = cell_vm.background
            color = Qt.white if bg == "transparent" else (Qt.red if bg == "red" else (Qt.green if bg == "green" else Qt.black))
            rect.setBrush(QBrush(color))
        elif prop_name == "content":
            text.setPlainText(cell_vm.content if cell_vm.content else "")

    def _on_vm_property_changed(self, prop_name):
        if prop_name == "difficulty":
            self.lbl_difficulty.setText(self.vm.difficulty)
        elif prop_name == "is_ui_enabled":
            self.view.setEnabled(self.vm.is_ui_enabled)

    def _on_grid_property_changed(self, prop_name):
        if prop_name in ["field_width", "field_height"]:
            self._rebuild_grid()
        elif prop_name.startswith("line_center"):
            self._update_mirror_lines()

    def _on_gen_property_changed(self, prop_name):
        if prop_name == "status_message":
            self.lbl_status.setText(self.vm.generation_controls.status_message)
        elif prop_name == "is_generating":
            is_gen = self.vm.generation_controls.is_generating
            self.btn_gen.setVisible(not is_gen)
            self.btn_stop.setVisible(is_gen)

    def _on_dict_property_changed(self, prop_name):
        if prop_name == "selected_dictionary_info":
            self.lbl_dict_info.setText(self.vm.dictionary_controls.selected_dictionary_info)