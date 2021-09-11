using System;
using System.Drawing;
using System.Windows.Forms;

namespace SimpleCalculator.Views.Abstract
{
    interface IMainView
    {
        string MainInput { get; set; }
        string Result { get; set; }
        string Operation { get; set; }
        bool IsDegree { get; set; }

        void NumberInput_Click(object sender, EventArgs e);
        void ChangeInputDataFont(Font font);

        Button ExtensionButton { get; }
        Button TopMostButton { get; }
        Panel TopPanel { get; }
    }
}
