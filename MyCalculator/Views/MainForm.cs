using SimpleCalculator.Presenters;
using SimpleCalculator.Views.Abstract;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static SimpleCalculator.CalculateOperators;

namespace SimpleCalculator
{
    public partial class MainForm : Form, IMainView
    {
        private MainViewPresenter _presenter;
        public MainForm()
        {
            InitializeComponent();
            _presenter = new MainViewPresenter(this);

        }

        #region Свойства для паттера MVP

        /// <summary>
        /// Свойство, представляющее содержание поля для ввода
        /// </summary>
        public string MainInput
        {
            get => tbxInput.Text;
            set => tbxInput.Text = value;
        }
        
        /// <summary>
        /// Свойство, представляющее содержание поля для вывода результата операций
        /// </summary>
        public string Result
        {
            get => lblPrevInput.Text;   
            set => lblPrevInput.Text = value;
        }

        /// <summary>
        /// Свойство, представляющее содержание поля для вывода введеннйо операции
        /// </summary>
        public string Operation
        {
            get => lblOperation.Text;
            set => lblOperation.Text = value;
        }

        /// <summary>
        /// Свойство, устанавливающее единицы для операций с углами (градусы/радианы)
        /// </summary>
        public bool IsDegree
        {
            get => rbDegree.Checked;
            set => rbDegree.Checked = value;
        }

        /// <summary>
        /// Кнопка раскрытия панели с расширенными функциями
        /// </summary>
        public Button ExtensionButton
        {
            get => btnShowAdvanced;
        }

        /// <summary>
        /// Кнопка установки свойства 'TopMost' формы
        /// </summary>
        public Button TopMostButton
        {
            get => btnPinActivate;
        }

        /// <summary>
        /// Свойство, представляющее вернюю панель калькулятора
        /// </summary>
        public Panel TopPanel
        {
            get => controlPanel;
        }

        #endregion


        #region Обработчики событий элементов управления формы 

        /// <summary>
        /// Обработчик ввода цифры
        /// </summary>
        /// <param name="sender">Объект вызвавший событи</param>
        /// <param name="e">Аргумент события</param>
        public void NumberInput_Click(object sender, EventArgs e)
        {
            _presenter.InputNumber(((Button)sender).Text);
        }

        /// <summary>
        /// Обработчик ввода базовых операций
        /// </summary>
        /// <param name="sender">Объект вызвавший событи</param>
        /// <param name="e">Аргумент события</param>
        private void BaseOperation_Click(object sender, EventArgs e)
        {
            _presenter.InputBaseOperation(((Button)sender).Tag.ToString());
        }

        /// <summary>
        /// Обработчки кнопки очистки последнего введенного символа  
        /// </summary>
        /// <param name="sender">Объект вызвавший событие</param>
        /// <param name="e">Аргумент события</param>
        private void ClearLast_Click(object sender, EventArgs e)
        {
            _presenter.ClearLastChar();
        }

        /// <summary>
        /// Очистки поля для ввода
        /// </summary>
        /// <param name="sender">Объект вызвавший событие</param>
        /// <param name="e">Аргумент события</param>
        private void ClearAll_Click(object sender, EventArgs e)
        {
            _presenter.ClearAll();
        }

        /// <summary>
        /// Обработки кнопки '='.
        /// </summary>
        /// <param name="sender">Объект вызвавший событие</param>
        /// <param name="e">Аргумент события</param>
        private void GetResult_Click(object sender, EventArgs e)
        {
            _presenter.GetResult();
        }

        /// <summary>
        /// Обработчки расщиренных операций вычисления.
        /// </summary>
        /// <param name="sender">Объект вызвавший событие</param>
        /// <param name="e">Аргумент события</param>
        private void AnvancedOperation_Click(object sender, EventArgs e)
        {
            _presenter.InvokeAdvanceOperation(((Button)sender).Text);
        }

        /// <summary>
        /// Обработчик ввода ',' .
        /// </summary>
        /// <param name="sender">Объект вызвавший событие</param>
        /// <param name="e">Аргумент события</param>
        private void CommaInput_Click(object sender, EventArgs e)
        {
            _presenter.InputComma();
        }

        /// <summary>
        /// Обработчик изменения текста в поле ввода
        /// </summary>
        /// <param name="sender">Объект вызвавший событие</param>
        /// <param name="e">Аргумент события</param>
        private void InputDataChanged_TextChanged(object sender, EventArgs e)
        {
            _presenter.InputDataChanged(((TextBox)sender).Text);
        }

        /// <summary>
        /// Метод, изменяющий шрифт поля ввода
        /// </summary>
        /// <param name="font">Шрифт для установки</param>
        public void ChangeInputDataFont(Font font)
        {
            tbxInput.Font = font;
        }

        /// <summary>
        /// Обработчки кнопки ввода числа 'PI'.
        /// </summary>
        /// <param name="sender">Объект вызвавший событие</param>
        /// <param name="e">Аргумент события</param>
        public void InputPI_Clock(object sender, EventArgs e)
        {
            _presenter.InputPI();
        }

        /// <summary>
        /// Закрытие приложения
        /// </summary>
        /// <param name="sender">Объект вызвавший событие</param>
        /// <param name="e">Аргумент события</param>
        private void Exit_Click(object sender, EventArgs e)
        {
            _presenter.CloseApplication();
        }

        /// <summary>
        /// Сворачивание приложения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Minimize_Click(object sender, EventArgs e)
        {
            _presenter.MinimizeApplication();
        }

        /// <summary>
        /// Обработки кнопки отображения расширенных функций для вычисления.
        /// </summary>
        /// <param name="sender">Объект вызвавший событие</param>
        /// <param name="e">Аргумент события</param>
        private void ShowAdvanced_Click(object sender, EventArgs e)
        {
            _presenter.SetExtensionButton();
        }

        /// <summary>
        /// Изменение свойства 'TopMost' формы.
        /// </summary>
        /// <param name="sender">Объект вызвавший событие</param>
        /// <param name="e">Аргумент события</param>
        private void TopMosPropertyActivate_Click(object sender, EventArgs e)
        {
            _presenter.SetTopMostProperty();
        }

        /// <summary>
        /// Обработчик нажатия на верхнюю пнанель формы.
        /// </summary>
        /// <param name="sender">Объект вызвавший событие</param>
        /// <param name="e">Аргумент события</param>
        private void controlPanel_MouseDown(object sender, MouseEventArgs e)
        {
            _presenter.SetMousePosition(e);
        }

        /// <summary>
        /// Обработчик перемещени формы, удерживая клавишу мыши.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void controlPanel_MouseMove(object sender, MouseEventArgs e)
        {
            _presenter.MoveForm(e);
        }

        #endregion


    }
}

