from typing import Callable, Any, Optional
from PySide6.QtCore import QObject, Signal

class RelayCommand(QObject):
    """
    Аналог ICommand из WPF.
    Управляет исполнением действия и проверкой возможности его исполнения.
    """
    can_execute_changed = Signal()

    def __init__(self, execute: Callable[[Any], None], can_execute: Optional[Callable[[Any], bool]] = None, parent=None):
        super().__init__(parent)
        self._execute = execute
        self._can_execute = can_execute

    def can_execute(self, parameter: Any = None) -> bool:
        """
        Определяет, может ли команда выполниться в текущем состоянии.
        """
        if self._can_execute is None:
            return True
        return self._can_execute(parameter)

    def execute(self, parameter: Any = None):
        """
        Выполняет команду.
        """
        if self.can_execute(parameter):
            self._execute(parameter)

    def raise_can_execute_changed(self):
        """
        Оповещает UI о том, что статус CanExecute мог измениться 
        (чтобы кнопка стала активной/неактивной).
        """
        self.can_execute_changed.emit()