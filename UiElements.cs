using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace Crossword
{
     class UiElements
    {
        public Button GenButton = new Button();
        public Button GenStopButton = new Button();
        public CheckBox VisualCheckBox = new CheckBox();
        public Label WindowsText = new Label();
        public Label CountGoodGen = new Label();
        public TextBox CountGen = new TextBox();

        public void AddElements(Button GenButton, Button GenStopButton, CheckBox VisualCheckBox, Label WindowsText, Label CountGoodGen, TextBox CountGen)
        {
            this.GenButton = GenButton;
            this.GenStopButton = GenStopButton;
            this.VisualCheckBox = VisualCheckBox;
            this.WindowsText = WindowsText;
            this.CountGoodGen = CountGoodGen;
            this.CountGen = CountGen;
        }
    }
}
