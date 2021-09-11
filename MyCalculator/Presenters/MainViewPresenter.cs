using SimpleCalculator.Views.Abstract;
using System;
using System.Drawing;
using System.Windows.Forms;
using static SimpleCalculator.CalculateOperators;

namespace SimpleCalculator.Presenters
{
    class MainViewPresenter
    {
        private IMainView _mainView; 

        private bool isExceptionTrow = false; // Флаг, указывающий о возникновении исключения при вычислении
        private int maxCharCount = 10; //Максимальное количество символов в поле вода

        Point currentPosition; //Текщее положение калькулятора

        private int minOpenedHeight = 498; // Высота калькулятора в закрытом состоянии
        private int maxOpenedHeight = 780; // Высота калькулятор в открытом положении
        private int panelAdvancedHeight = 282; // Высота вспомогательной панели


        public MainViewPresenter(IMainView mainView)
        {
            _mainView = mainView;
            ChangeExtension(minOpenedHeight, 
                            Properties.Resources.expand_arrow_15px, 
                            false);
        }

        /// <summary>
        /// Обработчик ввода цифр в калькулятор. 
        /// </summary>
        /// <param name="inputNumber">Введенная цифра</param>
        internal void InputNumber(string inputNumber)
        {
            if (_mainView.MainInput[0] == '0' && _mainView.MainInput.Length == 1)
                _mainView.MainInput = inputNumber;
            else
                _mainView.MainInput += inputNumber;
        }

        /// <summary>
        /// Обработчик ввода операции.
        /// </summary>
        /// <param name="operation">Введенная операция</param>
        internal void InputBaseOperation(string operation)
        {
            GetResult();
            _mainView.Operation = operation;
            _mainView.Result = _mainView.MainInput;
            ResetInput();
        }

        /// <summary>
        /// Обработчик удаления полседнего введенного символа.
        /// </summary>
        internal void ClearLastChar()
        {
            if (_mainView.MainInput.Length == 1)
                ResetInput();
            else
                _mainView.MainInput = _mainView.MainInput.Substring(0, _mainView.MainInput.Length - 1);
        }

        /// <summary>
        /// Обработчик очистка поля для ввода
        /// </summary>
        internal void ClearAll()
        {
            ResetInput();
            ResetOperationData();
        }

        /// <summary>
        /// Сброс поле для ввода в состояние 'по умолчанию'
        /// </summary>
        private void ResetInput()
        {
            _mainView.MainInput = "0";
        }

        /// <summary>
        /// Сброс поля для ввода операции
        /// </summary>
        private void ResetOperationData()
        {
            _mainView.Operation = string.Empty;
            _mainView.Result = string.Empty;
        }

        /// <summary>
        /// Обработчик расчета операции
        /// </summary>
        internal void GetResult()
        {
            try
            {
                double first = GetFirstOperand();

                double second = Convert.ToDouble(_mainView.MainInput);

                if (string.IsNullOrWhiteSpace(_mainView.Operation))
                    return;

                _mainView.MainInput = Math.Round(Calculate(_mainView.Operation, first, second), maxCharCount).ToString();
                ResetOperationData();
            }
            catch (ArgumentException ex)
            {
                HandleException(ex.Message);
            }
            catch (Exception)
            {
                HandleException("Ошибка");
            }
        }

        /// <summary>
        /// Обработчик ввода числа 'PI'
        /// </summary>
        internal void InputPI()
        {
            _mainView.MainInput = Math.Round(Math.PI, maxCharCount).ToString();
        }

        /// <summary>
        /// Обработчик закрытия калькулятора
        /// </summary>
        internal void CloseApplication()
        {
            Application.Exit();
        }

        /// <summary>
        /// Обработчик свертывания калькулятора
        /// </summary>
        internal void MinimizeApplication()
        {
            ((Form)_mainView).WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// Обработчик изменения текстового поля ввода
        /// </summary>
        /// <param name="text">Введенный текст</param>
        internal void InputDataChanged(string text)
        {
            if (_mainView.MainInput == string.Empty)
                ResetInput();

            if (isExceptionTrow)
            {
                ResetInput();
                isExceptionTrow = false;
            }
            ChangeInputFontSize();
        }

        /// <summary>
        /// Обработчик перемещения формы 
        /// </summary>
        /// <param name="e">Аргумент события 'Mouse Move'</param>
        internal void MoveForm(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ((Form)_mainView).Left += e.X - currentPosition.X;
                ((Form)_mainView).Top += e.Y - currentPosition.Y;
            }
        }

        /// <summary>
        /// Установка позиция кнопки  
        /// </summary>
        /// <param name="e"></param>
        internal void SetMousePosition(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                currentPosition = new Point(e.X, e.Y);
            }
        }

        /// <summary>
        /// Установка свойства 'TopMost' формы
        /// </summary>
        internal void SetTopMostProperty()
        {
            Form currentForm = (Form)_mainView;

            if (currentForm.TopMost == false)
            {
                _mainView.TopMostButton.Image = Properties.Resources.pin_28px;
            }
            else
            {
                _mainView.TopMostButton.Image = Properties.Resources.unpin_28px;
            }
            currentForm.TopMost = !currentForm.TopMost;
        }

        /// <summary>
        /// Установка состяния панели с раширенными функциями
        /// </summary>
        internal void SetExtensionButton()
        {
            Form currentForm = (Form)_mainView;
            if (currentForm.Height == minOpenedHeight)
            {
                Bitmap image = Properties.Resources.collapse_arrow_15px;
                ChangeExtension(maxOpenedHeight, image, false);
            }
            else
            {
                Bitmap image = Properties.Resources.expand_arrow_15px;
                ChangeExtension(minOpenedHeight, image, true);
            }
        }

        /// <summary>
        /// Изменяет необходимые свойства при изменении состянии 
        /// панели с раширенными свойствами
        /// </summary>
        /// <param name="height">Устанавливаемая высота формы</param>
        /// <param name="bitmapImage">Устанавливаемое изображение формы</param>
        /// <param name="isExpended">Текущее состояние панели с расширеными своствами</param>
        private void ChangeExtension(int height, Bitmap bitmapImage, bool isExpended)
        {
            Form form = (Form)_mainView;
            form.Height = height;
            form.Top = isExpended ? form.Top + panelAdvancedHeight / 2
                                  : form.Top - panelAdvancedHeight / 2;

            _mainView.ExtensionButton.Image = bitmapImage;
        }

        /// <summary>
        /// Изменения шрифта для поля ввода.
        /// </summary>
        private void ChangeInputFontSize()
        {
            if (_mainView.MainInput.Length > 14)
            {
                _mainView.ChangeInputDataFont(new Font("Lucida Console", 
                                                        14F, 
                                                        FontStyle.Regular, 
                                                        GraphicsUnit.Point, 
                                                        ((byte)(0))));
                ((Control)_mainView).Refresh();
            }
            else
            {
                _mainView.ChangeInputDataFont(new Font("Lucida Console", 
                                                        21.75f, 
                                                        FontStyle.Regular, 
                                                        GraphicsUnit.Point, 
                                                        ((byte)(0))));
                ((Control)_mainView).Refresh();
            }
        }

        /// <summary>
        /// Обработчик для ввода запятой 
        /// </summary>
        internal void InputComma()
        {
            if (!_mainView.MainInput.Contains(",")) _mainView.MainInput += ",";
        }

        /// <summary>
        /// Обработчки операций выполнений расширенных операций
        /// </summary>
        /// <param name="operation"></param>
        internal void InvokeAdvanceOperation(string operation)
        {
            ResetOperationData();
            _mainView.Operation = operation;
            GetResult();
        }

        /// <summary>
        /// Возвращает первый операнд операции с 2-я операндами
        /// </summary>
        /// <returns>Первый операнд</returns>
        private double GetFirstOperand()
        {
            if (!string.IsNullOrWhiteSpace(_mainView.Result))
            {
                return Convert.ToDouble(_mainView.Result);
            }
            return 0;
        }

        /// <summary>
        /// Метод, определящйи метод для обработки введенной операции
        /// </summary>
        /// <param name="operation">Введенная операция</param>
        /// <param name="first">Первый операнд</param>
        /// <param name="second">Второй операнд</param>
        /// <returns>Результат операции</returns>
        private double Calculate(string operation, double first, double second)
        {
            double corner = second;
            if (_mainView.IsDegree) corner = DegreeToRadian(second);

            switch (operation)
            {
                case "+":
                    return Add(first, second);
                case "-":
                    return Sub(first, second);
                case "x":
                    return Mult(first, second);
                case "÷":
                    return Div(first, second);
                case "xⁿ":
                    return PowN(first, second);
                case "ⁿ√x":
                    return RootN(first, second);
                case "±":
                    return InverstSign(second);
                case "√x":
                    return SquareRoot(second);
                case "2ⁿ":
                    return TwoPowN(second);
                case "10ⁿ":
                    return TenPowN(second);
                case "x³":
                    return PowThree(second);
                case "ln":
                    return Ln(second);
                case "log":
                    return Log(second);
                case "|x|":
                    return AbsValue(second);
                case "x²":
                    return SquareValue(second);
                case "x!":
                    return Factorial(second);
                case "³√x":
                    return RootThree(second);
                case "1/x":
                    return ReverseValue(second);
                case "exp(x)":
                    return Exp(second);
                case "sin":
                    return Sin(corner);
                case "cos":
                    return Cos(corner);
                case "tg":
                    return Tan(corner);
                case "ctg":
                    return Ctan(corner);
                case "sin⁻¹":
                    return ArcSin(second);
                case "cos⁻¹":
                    return ArcSin(second);
                default:
                    throw new ArgumentException("Ошибка ввода");
            }
        }

        /// <summary>
        /// Записывает информацию в поле ввода
        /// </summary>
        /// <param name="info">Выводимая информация</param>
        private void HandleException(string info)
        {
            _mainView.MainInput = info;
            ResetOperationData();
            isExceptionTrow = true;
        }
    }
}
