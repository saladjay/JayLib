using JayCustomControlLib.CommonBasicClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JayCustomControlLib
{
    /// <summary>
    /// 依照步驟 1a 或 1b 執行，然後執行步驟 2，以便在 XAML 檔中使用此自訂控制項。
    ///
    /// 步驟 1a) 於存在目前專案的 XAML 檔中使用此自訂控制項。
    /// 加入此 XmlNamespace 屬性至標記檔案的根項目為 
    /// 要使用的: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:JayCustomControlLib"
    ///
    ///
    /// 步驟 1b) 於存在其他專案的 XAML 檔中使用此自訂控制項。
    /// 加入此 XmlNamespace 屬性至標記檔案的根項目為 
    /// 要使用的: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:JayCustomControlLib;assembly=JayCustomControlLib"
    ///
    /// 您還必須將 XAML 檔所在專案的專案參考加入
    /// 此專案並重建，以免發生編譯錯誤: 
    ///
    ///     在 [方案總管] 中以滑鼠右鍵按一下目標專案，並按一下
    ///     [加入參考]->[專案]->[瀏覽並選取此專案]
    ///
    ///
    /// 步驟 2)
    /// 開始使用 XAML 檔案中的控制項。
    ///
    ///     <MyNamespace:JColorPicker/>
    ///
    /// </summary>
    public class JColorPicker : Control
    {
        static JColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(JColorPicker), new FrameworkPropertyMetadata(typeof(JColorPicker)));
            InitializeCommands();
        }

        #region Commands

        public static RoutedUICommand SelectColorCommand { get; private set; } = null;
        public static RoutedUICommand SelectAdvancedColorCommand { get; private set; } = null;
        private static void InitializeCommands()
        {
            //create instance
            SelectColorCommand = new RoutedUICommand("SelectColorCommand", "SelectColorCommand", typeof(JColorPicker));
            SelectAdvancedColorCommand = new RoutedUICommand("SelectAdvancedColorCommand", "SelectAdvancedColorCommand", typeof(JColorPicker));
            //register the command bindings, if the button gets clicked, the method will be called.
            CommandManager.RegisterClassCommandBinding(typeof(JColorPicker), new CommandBinding(SelectColorCommand, SelectedColorExecute));
            CommandManager.RegisterClassCommandBinding(typeof(JColorPicker), new CommandBinding(SelectAdvancedColorCommand, SelectedAdvancedColorExecute));
            //  lastly bind some inputs:  i.e. if the user presses up/down arrow 
            //  keys, call the appropriate commands.
            //MyCommandHelper.RegisterCommandHandler(typeof(JColorPicker), SelectColorCommand, SelectedColorExecute);
            //MyCommandHelper.RegisterCommandHandler(typeof(JColorPicker), SelectAdvancedColorCommand, SelectedAdvancedColorExecute);
        }

        private static void SelectedColorExecute(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is JColorPicker jColorPicker && e.Parameter is string color)
            {
                jColorPicker.CurrentColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
            }
        }

        private static void SelectedAdvancedColorExecute(object sender, ExecutedRoutedEventArgs e)
        {
            
        }


        #endregion


        public Brush CurrentColor
        {
            get { return (Brush)GetValue(CurrentColorProperty); }
            set { SetValue(CurrentColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentColorProperty =
            DependencyProperty.Register("CurrentColor", typeof(Brush), typeof(JColorPicker), new PropertyMetadata(Brushes.Black));

    }
}
