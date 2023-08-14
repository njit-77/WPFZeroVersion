using System;
using System.Diagnostics;
using System.Windows.Input;

namespace WPFZeroVersion.Common
{
    public class RelayCommand<T> : ICommand
    {

        #region 构造函数

        public RelayCommand(Action<T> execute) : this(execute, null)
        {
        }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        #endregion


        #region 字段

        private readonly Predicate<T> _canExecute;
        private readonly Action<T> _execute;

        #endregion


        #region ICommand的成员

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }
            return _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            _execute?.Invoke((T)parameter);
        }

        #endregion

    }

    public class RelayCommand : ICommand
    {

        #region 构造函数

        public RelayCommand(Action execute) : this(execute, null)
        {
        }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        #endregion


        #region 字段

        private readonly Func<bool> _canExecute;
        private readonly Action _execute;

        #endregion


        #region ICommand的成员

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }
            return _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute?.Invoke();
        }

        #endregion

    }
}
