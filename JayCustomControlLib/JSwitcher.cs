using JayCustomControlLib.CommonBasicClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    ///     <MyNamespace:JSwitcher/>
    ///
    /// </summary>
    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
    public class JSwitcher : ToggleButton
    {
        #region Construction
        public JSwitcher()
        {
            Background = InactiveBackgroundBrush;
            Foreground = InactiveForegroundBrush;
        }
        static JSwitcher()
        {
            InitializeCommands();
            DefaultStyleKeyProperty.OverrideMetadata(typeof(JSwitcher), new FrameworkPropertyMetadata(typeof(JSwitcher)));
        }
        #endregion


        #region Command
        public static RoutedCommand ChangeContentCommand { get; private set; } = null;
        public static RoutedCommand DoubleClickCommand { get; private set; } = null;
        public static RoutedCommand ClickCommand { get; private set; } = null;
        static void InitializeCommands()
        {
            ChangeContentCommand = new RoutedCommand("ChangeContentCommand", typeof(JSwitcher));
            DoubleClickCommand = new RoutedCommand("DoubleClickCommand", typeof(JSwitcher));
            //ClickCommand = new RoutedCommand("ClickCommand", typeof(JSwitcher));
            MyCommandHelper.RegisterCommandHandler(typeof(JSwitcher), ChangeContentCommand, new ExecutedRoutedEventHandler(ChangeContentExecute), new KeyGesture(Key.Enter));
            MyCommandHelper.RegisterCommandHandler(typeof(JSwitcher), DoubleClickCommand, new ExecutedRoutedEventHandler(DoubleClickExecute), new MouseGesture(MouseAction.LeftDoubleClick));
            //MyCommandHelper.RegisterCommandHandler(typeof(JSwitcher), ClickCommand, new ExecutedRoutedEventHandler(ClickExecute), new CanExecuteRoutedEventHandler(CanClickExecute), new MouseGesture(MouseAction.LeftClick));
        }

        private static void CanClickExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (sender is JSwitcher switcher)
            {
                e.CanExecute = !switcher.IsEditing;
            }
        }

        private static void ClickExecute(object sender, ExecutedRoutedEventArgs e)
        {
            
        }

        private static void DoubleClickExecute(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is JSwitcher switcher&&switcher.CanEdit)
            {
                switcher.IsEditing = true;
                if (switcher._TextBox != null)
                {
                    switcher._TextBox.Text = switcher.Content.ToString();
                    switcher._TextBox.Focus();
                    switcher._TextBox.SelectAll();
                }
            }
        }

        private static void ChangeContentExecute(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is JSwitcher switcher )
            {
                if (!string.IsNullOrWhiteSpace(switcher.InputText))
                {
                    switcher.Content = switcher.InputText;
                    switcher.ContentChangedEvent?.Invoke(switcher, switcher.InputText);
                }
                switcher.IsEditing = false;
            }
        }
        #endregion

        #region Event
        public delegate void ContentChangedEventDelegate(object sender, string content);
        public event ContentChangedEventDelegate ContentChangedEvent;


        #endregion

        #region Propertys

        #region Edit support
        public bool CanEdit
        {
            get { return (bool)GetValue(CanEditProperty); }
            set { SetValue(CanEditProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanEdit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanEditProperty =
            DependencyProperty.Register("CanEdit", typeof(bool), typeof(JSwitcher), new PropertyMetadata(false));



        public string InputText
        {
            get { return (string)GetValue(InputTextProperty); }
            set { SetValue(InputTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InputText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InputTextProperty =
            DependencyProperty.Register("InputText", typeof(string), typeof(JSwitcher), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        public bool IsEditing
        {
            get { return (bool)GetValue(IsEditingProperty); }
            set { SetValue(IsEditingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsEditing.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsEditingProperty =
            DependencyProperty.Register("IsEditing", typeof(bool), typeof(JSwitcher), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));


        #endregion

        #region Brushes

        public Brush ActiveBackgroundBrush
        {
            get { return (Brush)GetValue(ActiveBackgroundBrushProperty); }
            set { SetValue(ActiveBackgroundBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActiveBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActiveBackgroundBrushProperty =
            DependencyProperty.Register("ActiveBackgroundBrush", typeof(Brush), typeof(JSwitcher), new PropertyMetadata(Brushes.Red, OnBrushChanged));



        public Brush ActiveForegroundBrush
        {
            get { return (Brush)GetValue(ActiveForegroundBrushProperty); }
            set { SetValue(ActiveForegroundBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActiveForegroundBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActiveForegroundBrushProperty =
            DependencyProperty.Register("ActiveForegroundBrush", typeof(Brush), typeof(JSwitcher), new PropertyMetadata(Brushes.White, OnBrushChanged));



        public Brush InactiveBackgroundBrush
        {
            get { return (Brush)GetValue(InactiveBackgroundBrushProperty); }
            set { SetValue(InactiveBackgroundBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InactiveBackgroundBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InactiveBackgroundBrushProperty =
            DependencyProperty.Register("InactiveBackgroundBrush", typeof(Brush), typeof(JSwitcher), new PropertyMetadata(Brushes.Gray, OnBrushChanged));



        public Brush InactiveForegroundBrush
        {
            get { return (Brush)GetValue(InactiveForegroundBrushProperty); }
            set { SetValue(InactiveForegroundBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InactiveForegroundBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InactiveForegroundBrushProperty =
            DependencyProperty.Register("InactiveForegroundBrush", typeof(Brush), typeof(JSwitcher), new PropertyMetadata(Brushes.Black, OnBrushChanged));

        private static void OnBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is JSwitcher switcher)
            {
                if (switcher.IsChecked == true)
                {
                    switcher.Background = switcher.ActiveBackgroundBrush;
                    switcher.Foreground = switcher.ActiveForegroundBrush;
                }
                else
                {
                    switcher.Background = switcher.InactiveBackgroundBrush;
                    switcher.Foreground = switcher.InactiveForegroundBrush;
                }
            }
        }
        #endregion

        #region Group
        public string GroupName
        {
            get { return (string)GetValue(GroupNameProperty); }
            set { SetValue(GroupNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GroupName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GroupNameProperty =
            DependencyProperty.Register("GroupName", typeof(string), typeof(JSwitcher), new PropertyMetadata(string.Empty,OnGroupNameChanged));

        private static void OnGroupNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is JSwitcher switcher)
            {
                if (e.NewValue is string newGroup && !string.IsNullOrWhiteSpace(newGroup))
                {
                    if (JSwitcherGroup.TryGetValue(newGroup,out List<JSwitcher> switchers))
                    {
                        if (!switchers.Contains(switcher))
                        {
                            switchers.Add(switcher);
                        }
                    }
                    else
                    {
                        JSwitcherGroup[newGroup] = new List<JSwitcher>() { switcher };
                    }
                }
                if (e.OldValue is string oldGroup && !string.IsNullOrWhiteSpace(oldGroup))
                {
                    if (JSwitcherGroup.TryGetValue(oldGroup,out List<JSwitcher> switchers))
                    {
                        if (switchers.Contains(switcher))
                        {
                            switchers.Remove(switcher);
                        }
                    }
                }
            }
        }
        #endregion


        #endregion

        #region Static Propertys
        public static Dictionary<string, List<JSwitcher>> JSwitcherGroup { get; set; } = new Dictionary<string, List<JSwitcher>>();
        #endregion

        #region Private fields
        private const string _TextBoxName = "PART_TextBox";
        private TextBox _TextBox = null;
        #endregion

        #region Override methods

        
        protected override void OnChecked(RoutedEventArgs e)
        {
            Background = ActiveBackgroundBrush;
            Foreground = ActiveForegroundBrush;
            UpdateSwitcherGroup();
            base.OnChecked(e);
        }

        protected override void OnUnchecked(RoutedEventArgs e)
        {
            Background = InactiveBackgroundBrush;
            Foreground = InactiveForegroundBrush;
            base.OnUnchecked(e);
        }

        protected override void OnToggle()
        {
            if (IsEditing||(!string.IsNullOrWhiteSpace(GroupName)&&IsChecked==true))
            {
                return;
            }
            base.OnToggle();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _TextBox = GetTemplateChild(_TextBoxName) as TextBox;
            if (_TextBox!=null)
            {
                _TextBox.LostFocus += _TextBox_LostFocus;
            }
        }

        private void _TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            _TextBox.Text = Content.ToString();
            IsEditing = false;
        }
        #endregion

        #region Private methods
        private void UpdateSwitcherGroup()
        {
            if (JSwitcherGroup.TryGetValue(GroupName, out List<JSwitcher> swithcers) && swithcers.Count != 0)
            {
                for (int i = 0; i < swithcers.Count;)
                {
                    if (swithcers[i] == null)
                    {
                        swithcers.RemoveAt(i);
                    }
                    else
                    {
                        if (swithcers[i].IsChecked == true && swithcers[i] != this)
                        {
                            swithcers[i].IsChecked = false;
                        }
                        i++;
                    }
                }
            }
        }
        #endregion
    }
}
