import sys
from PyQt6.QtWidgets import QApplication
from PyQt6.QtCore import Qt
from PyQt6.QtTest import QTest
from src.helloworld import MyWidget


app = QApplication(sys.argv)


def test_initWidget():
    """
    Test the initial state of the widget.
    """
    widget = MyWidget()
    hello = ["Hallo Welt", "Hei maailma", "Hola Mundo", "Привет мир"]
    assert widget.text.text() == 'Hello World'
    assert widget.button.text() == 'Click me!'
    assert widget.hello == hello


def test_sayHello():
    """
    Whitebox test the sayHello function.
    Execute sayHello 10000 times, remove each random result from the list.
    The final list should be empty.
    """
    widget = MyWidget()
    hello = ["Hallo Welt", "Hei maailma", "Hola Mundo", "Привет мир"]
    for i in range(10000):
        widget.sayHello()
        text = widget.text.text()
        if text in hello:
            hello.remove(widget.text.text())
    assert len(hello) == 0


def test_mouseClick():
    """
    Blackbox test the sayHello function.
    Use QTest.mouseClick to click the button 10000 times.
    Remove each random result from the list.
    The final list should be empty as well.
    """
    widget = MyWidget()
    hello = ["Hallo Welt", "Hei maailma", "Hola Mundo", "Привет мир"]
    for i in range(10000):
        QTest.mouseClick(widget.button, Qt.MouseButton.LeftButton)
        text = widget.text.text()
        if text in hello:
            hello.remove(widget.text.text())
    assert len(hello) == 0
