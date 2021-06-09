import sys
import random
from PyQt6.QtCore import Qt
from PyQt6.QtWidgets import (QApplication, QLabel, QWidget,
                             QPushButton, QVBoxLayout)


class MyWidget(QWidget):
    def __init__(self):
        super().__init__()
        # List for all hello text
        self.hello = ["Hallo Welt", "Hei maailma", "Hola Mundo"]
        # Add a button for changing the hello text
        self.button = QPushButton("Click me!")
        # Connect the button to magic function
        self.button.clicked.connect(self.sayHello)
        # Add a label to display the text
        self.text = QLabel("Hello World", )
        # Set alignment to center
        self.text.setAlignment(Qt.AlignmentFlag.AlignCenter)
        # Set layout of the widget
        self.layout = QVBoxLayout(self)
        self.layout.addWidget(self.text)
        self.layout.addWidget(self.button)

    def sayHello(self):
        self.text.setText(random.choice(self.hello))


if __name__ == "__main__":
    # Create a new application
    app = QApplication(sys.argv)
    # Create a new widegt
    widget = MyWidget()
    # Resize the widget
    widget.resize(800, 600)
    # Show the widget
    widget.show()
    # Run the app
    sys.exit(app.exec())
