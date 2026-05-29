from PySide6.QtCore import QObject, Signal
from typing import Any

class ViewModelBase(QObject):
    """
    Базовый класс для всех ViewModel.
    Заменяет INotifyPropertyChanged из C# с использованием сигналов Qt.
    """
    property_changed = Signal(str)

    def __init__(self, parent=None):
        super().__init__(parent)

    def set_property(self, prop_name: str, value: Any) -> bool:
        """
        Устанавливает значение атрибута и вызывает сигнал, если значение изменилось.
        """
        current_val = getattr(self, prop_name, None)
        if current_val == value:
            return False
            
        setattr(self, prop_name, value)
        
        # Убираем нижнее подчеркивание у приватных переменных для сигнала, 
        # чтобы UI реагировал на публичные имена свойств
        signal_name = prop_name.lstrip('_')
        self.property_changed.emit(signal_name)
        return True