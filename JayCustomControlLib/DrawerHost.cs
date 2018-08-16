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
    ///     <MyNamespace:DrawerHost/>
    ///
    /// </summary>
    [TemplateVisualState(GroupName = TemplateAllDrawersGroupName, Name = TemplateAllDrawersAllClosedStateName)]
    [TemplateVisualState(GroupName = TemplateAllDrawersGroupName, Name = TemplateAllDrawersAnyOpenStateName)]
    [TemplateVisualState(GroupName = TemplateLeftDrawerGroupName, Name = TemplateLeftClosedStateName)]
    [TemplateVisualState(GroupName = TemplateLeftDrawerGroupName, Name = TemplateLeftOpenStateName)]
    [TemplateVisualState(GroupName = TemplateTopDrawerGroupName, Name = TemplateTopClosedStateName)]
    [TemplateVisualState(GroupName = TemplateTopDrawerGroupName, Name = TemplateTopOpenStateName)]
    [TemplateVisualState(GroupName = TemplateRightDrawerGroupName, Name = TemplateRightClosedStateName)]
    [TemplateVisualState(GroupName = TemplateRightDrawerGroupName, Name = TemplateRightOpenStateName)]
    [TemplateVisualState(GroupName = TemplateBottomDrawerGroupName, Name = TemplateBottomClosedStateName)]
    [TemplateVisualState(GroupName = TemplateBottomDrawerGroupName, Name = TemplateBottomOpenStateName)]
    [TemplatePart(Name = TemplateContentCoverPartName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = TemplateLeftDrawerPartName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = TemplateTopDrawerPartName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = TemplateRightDrawerPartName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = TemplateBottomDrawerPartName, Type = typeof(FrameworkElement))]
    public class DrawerHost : ContentControl
    {
        public const string TemplateAllDrawersGroupName = "AllDrawers";
        public const string TemplateAllDrawersAllClosedStateName = "AllClosed";
        public const string TemplateAllDrawersAnyOpenStateName = "AnyOpen";
        public const string TemplateLeftDrawerGroupName = "LeftDrawer";
        public const string TemplateLeftClosedStateName = "LeftDrawerClosed";
        public const string TemplateLeftOpenStateName = "LeftDrawerOpen";
        public const string TemplateTopDrawerGroupName = "TopDrawer";
        public const string TemplateTopClosedStateName = "TopDrawerClosed";
        public const string TemplateTopOpenStateName = "TopDrawerOpen";
        public const string TemplateRightDrawerGroupName = "RightDrawer";
        public const string TemplateRightClosedStateName = "RightDrawerClosed";
        public const string TemplateRightOpenStateName = "RightDrawerOpen";
        public const string TemplateBottomDrawerGroupName = "BottomDrawer";
        public const string TemplateBottomClosedStateName = "BottomDrawerClosed";
        public const string TemplateBottomOpenStateName = "BottomDrawerOpen";

        public const string TemplateContentCoverPartName = "PART_ContentCover";
        public const string TemplateLeftDrawerPartName = "PART_LeftDrawer";
        public const string TemplateTopDrawerPartName = "PART_TopDrawer";
        public const string TemplateRightDrawerPartName = "PART_RightDrawer";
        public const string TemplateBottomDrawerPartName = "PART_BottomDrawer";

        public static RoutedCommand OpenDrawerCommand = new RoutedCommand();
        public static RoutedCommand CloseDrawerCommand = new RoutedCommand();

        private FrameworkElement _templateContentCoverElement;
        private FrameworkElement _leftDrawerElement;
        private FrameworkElement _topDrawerElement;
        private FrameworkElement _rightDrawerElement;
        private FrameworkElement _bottomDrawerElement;

        private bool _lockZIndexes;

        private readonly IDictionary<DependencyProperty, DependencyPropertyKey> _zIndexPropertyLookup = new Dictionary<DependencyProperty, DependencyPropertyKey>
        {
            { IsLeftDrawerOpenProperty, LeftDrawerZIndexPropertyKey },
            { IsTopDrawerOpenProperty, TopDrawerZIndexPropertyKey },
            { IsRightDrawerOpenProperty, RightDrawerZIndexPropertyKey },
            { IsBottomDrawerOpenProperty, BottomDrawerZIndexPropertyKey }
        };

        #region Construction
        static DrawerHost()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DrawerHost), new FrameworkPropertyMetadata(typeof(DrawerHost)));
        }

        public DrawerHost()
        {
            CommandBindings.Add(new CommandBinding(OpenDrawerCommand, OpenDrawerHandler));
            CommandBindings.Add(new CommandBinding(CloseDrawerCommand, CloseDrawerHandler));
        }
        #endregion

        #region Event
        public static readonly RoutedEvent DrawerChangedEvent = EventManager.RegisterRoutedEvent("DrawerChangedEvent", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(DrawerHost));

        public event RoutedEventHandler DrawerChanged
        {
            add { AddHandler(DrawerChangedEvent, value); }
            remove { RemoveHandler(DrawerChangedEvent, value); }
        }

        public static readonly RoutedEvent DrawerLeftOpenEvent = EventManager.RegisterRoutedEvent("DrawerLeftOpenEvent", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(DrawerHost));

        public event RoutedEventHandler DrawerLeftOpen
        {
            add { AddHandler(DrawerLeftOpenEvent, value); }
            remove { RemoveHandler(DrawerLeftOpenEvent, value); }
        }

        public static readonly RoutedEvent DrawerLeftCloseEvent = EventManager.RegisterRoutedEvent("DrawerLeftCloseEvent", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(DrawerHost));

        public event RoutedEventHandler DrawerLeftClose
        {
            add { AddHandler(DrawerLeftCloseEvent, value); }
            remove { RemoveHandler(DrawerLeftCloseEvent, value); }
        }

        public static readonly RoutedEvent DrawerRightOpenEvent = EventManager.RegisterRoutedEvent("DrawerRightOpenEvent", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(DrawerHost));

        public event RoutedEventHandler DrawerRightOpen
        {
            add { AddHandler(DrawerRightOpenEvent, value); }
            remove { RemoveHandler(DrawerRightOpenEvent, value); }
        }

        public static readonly RoutedEvent DrawerRightCloseEvent = EventManager.RegisterRoutedEvent("DrawerRightCloseEvent", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(DrawerHost));

        public event RoutedEventHandler DrawerRightClose
        {
            add { AddHandler(DrawerRightCloseEvent, value); }
            remove { RemoveHandler(DrawerRightCloseEvent, value); }
        }

        public static readonly RoutedEvent DrawerTopOpenEvent = EventManager.RegisterRoutedEvent("DrawerTopOpenEvent", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(DrawerHost));

        public event RoutedEventHandler DrawerTopOpen
        {
            add { AddHandler(DrawerTopOpenEvent, value); }
            remove { RemoveHandler(DrawerTopOpenEvent, value); }
        }

        public static readonly RoutedEvent DrawerTopCloseEvent = EventManager.RegisterRoutedEvent("DrawerTopCloseEvent", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(DrawerHost));

        public event RoutedEventHandler DrawerTopClose
        {
            add { AddHandler(DrawerTopCloseEvent, value); }
            remove { RemoveHandler(DrawerTopCloseEvent, value); }
        }

        public static readonly RoutedEvent DrawerBottomOpenEvent = EventManager.RegisterRoutedEvent("DrawerBottomOpenEvent", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(DrawerHost));

        public event RoutedEventHandler DrawerBottomOpen
        {
            add { AddHandler(DrawerBottomOpenEvent, value); }
            remove { RemoveHandler(DrawerBottomOpenEvent, value); }
        }

        public static readonly RoutedEvent DrawerBottomCloseEvent = EventManager.RegisterRoutedEvent("DrawerBottomCloseEvent", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(DrawerHost));

        public event RoutedEventHandler DrawerBottomClose
        {
            add { AddHandler(DrawerBottomCloseEvent, value); }
            remove { RemoveHandler(DrawerBottomCloseEvent, value); }
        }

        public static readonly RoutedEvent DrawerAllOpenEvent = EventManager.RegisterRoutedEvent("DrawerAllOpenEvent", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(DrawerHost));

        public event RoutedEventHandler DrawerAllOpen
        {
            add { AddHandler(DrawerAllOpenEvent, value); }
            remove { RemoveHandler(DrawerAllOpenEvent, value); }
        }

        public static readonly RoutedEvent DrawerAllCloseEvent = EventManager.RegisterRoutedEvent("DrawerAllCloseEvent", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(DrawerHost));

        public event RoutedEventHandler DrawerAllClose
        {
            add { AddHandler(DrawerAllCloseEvent, value); }
            remove { RemoveHandler(DrawerAllCloseEvent, value); }
        }

        public class DrawerArgs : RoutedEventArgs
        {
            public DrawerArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source) { }
            public DrawerDockComponent DrawerDockComponent { get; set; }
            public bool IsOpen { get; set; }
        }
        #endregion

        #region top drawer

        public static readonly DependencyProperty TopDrawerContentProperty = DependencyProperty.Register(
            nameof(TopDrawerContent), typeof(object), typeof(DrawerHost), new PropertyMetadata(default(object)));

        public object TopDrawerContent
        {
            get { return (object)GetValue(TopDrawerContentProperty); }
            set { SetValue(TopDrawerContentProperty, value); }
        }

        public static readonly DependencyProperty TopDrawerContentTemplateProperty = DependencyProperty.Register(
            nameof(TopDrawerContentTemplate), typeof(DataTemplate), typeof(DrawerHost), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate TopDrawerContentTemplate
        {
            get { return (DataTemplate)GetValue(TopDrawerContentTemplateProperty); }
            set { SetValue(TopDrawerContentTemplateProperty, value); }
        }

        public static readonly DependencyProperty TopDrawerContentTemplateSelectorProperty = DependencyProperty.Register(
            nameof(TopDrawerContentTemplateSelector), typeof(DataTemplateSelector), typeof(DrawerHost), new PropertyMetadata(default(DataTemplateSelector)));

        public DataTemplateSelector TopDrawerContentTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(TopDrawerContentTemplateSelectorProperty); }
            set { SetValue(TopDrawerContentTemplateSelectorProperty, value); }
        }

        public static readonly DependencyProperty TopDrawerContentStringFormatProperty = DependencyProperty.Register(
            nameof(TopDrawerContentStringFormat), typeof(string), typeof(DrawerHost), new PropertyMetadata(default(string)));

        public string TopDrawerContentStringFormat
        {
            get { return (string)GetValue(TopDrawerContentStringFormatProperty); }
            set { SetValue(TopDrawerContentStringFormatProperty, value); }
        }

        public static readonly DependencyProperty TopDrawerBackgroundProperty = DependencyProperty.Register(
            nameof(TopDrawerBackground), typeof(Brush), typeof(DrawerHost), new PropertyMetadata(default(Brush)));

        public Brush TopDrawerBackground
        {
            get { return (Brush)GetValue(TopDrawerBackgroundProperty); }
            set { SetValue(TopDrawerBackgroundProperty, value); }
        }

        public static readonly DependencyProperty IsTopDrawerOpenProperty = DependencyProperty.Register(
            nameof(IsTopDrawerOpen), typeof(bool), typeof(DrawerHost), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsDrawerOpenPropertyChangedCallback));

        public bool IsTopDrawerOpen
        {
            get { return (bool)GetValue(IsTopDrawerOpenProperty); }
            set { SetValue(IsTopDrawerOpenProperty, value); }
        }

        private static readonly DependencyPropertyKey TopDrawerZIndexPropertyKey =
                                    DependencyProperty.RegisterReadOnly(
                                    "TopDrawerZIndex", typeof(int), typeof(DrawerHost),
                                    new PropertyMetadata(4));

        public static readonly DependencyProperty TopDrawerZIndexProperty = TopDrawerZIndexPropertyKey.DependencyProperty;

        public int TopDrawerZIndex
        {
            get { return (int)GetValue(TopDrawerZIndexProperty); }
            private set { SetValue(TopDrawerZIndexPropertyKey, value); }
        }

        #endregion

        #region left drawer

        public static readonly DependencyProperty LeftDrawerContentProperty = DependencyProperty.Register(
            nameof(LeftDrawerContent), typeof(object), typeof(DrawerHost), new PropertyMetadata(default(object)));

        public object LeftDrawerContent
        {
            get { return (object)GetValue(LeftDrawerContentProperty); }
            set { SetValue(LeftDrawerContentProperty, value); }
        }

        public static readonly DependencyProperty LeftDrawerContentTemplateProperty = DependencyProperty.Register(
            nameof(LeftDrawerContentTemplate), typeof(DataTemplate), typeof(DrawerHost), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate LeftDrawerContentTemplate
        {
            get { return (DataTemplate)GetValue(LeftDrawerContentTemplateProperty); }
            set { SetValue(LeftDrawerContentTemplateProperty, value); }
        }

        public static readonly DependencyProperty LeftDrawerContentTemplateSelectorProperty = DependencyProperty.Register(
            nameof(LeftDrawerContentTemplateSelector), typeof(DataTemplateSelector), typeof(DrawerHost), new PropertyMetadata(default(DataTemplateSelector)));

        public DataTemplateSelector LeftDrawerContentTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(LeftDrawerContentTemplateSelectorProperty); }
            set { SetValue(LeftDrawerContentTemplateSelectorProperty, value); }
        }

        public static readonly DependencyProperty LeftDrawerContentStringFormatProperty = DependencyProperty.Register(
            nameof(LeftDrawerContentStringFormat), typeof(string), typeof(DrawerHost), new PropertyMetadata(default(string)));

        public string LeftDrawerContentStringFormat
        {
            get { return (string)GetValue(LeftDrawerContentStringFormatProperty); }
            set { SetValue(LeftDrawerContentStringFormatProperty, value); }
        }

        public static readonly DependencyProperty LeftDrawerBackgroundProperty = DependencyProperty.Register(
            nameof(LeftDrawerBackground), typeof(Brush), typeof(DrawerHost), new PropertyMetadata(default(Brush)));

        public Brush LeftDrawerBackground
        {
            get { return (Brush)GetValue(LeftDrawerBackgroundProperty); }
            set { SetValue(LeftDrawerBackgroundProperty, value); }
        }

        public static readonly DependencyProperty IsLeftDrawerOpenProperty = DependencyProperty.Register(
            nameof(IsLeftDrawerOpen), typeof(bool), typeof(DrawerHost), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsDrawerOpenPropertyChangedCallback));

        public bool IsLeftDrawerOpen
        {
            get { return (bool)GetValue(IsLeftDrawerOpenProperty); }
            set { SetValue(IsLeftDrawerOpenProperty, value); }
        }

        private static readonly DependencyPropertyKey LeftDrawerZIndexPropertyKey =
                                            DependencyProperty.RegisterReadOnly(
                                            "LeftDrawerZIndex", typeof(int), typeof(DrawerHost),
                                            new PropertyMetadata(2));

        public static readonly DependencyProperty LeftDrawerZIndexProperty = LeftDrawerZIndexPropertyKey.DependencyProperty;

        public int LeftDrawerZIndex
        {
            get { return (int)GetValue(LeftDrawerZIndexProperty); }
            private set { SetValue(LeftDrawerZIndexPropertyKey, value); }
        }

        #endregion

        #region right drawer

        public static readonly DependencyProperty RightDrawerContentProperty = DependencyProperty.Register(
            nameof(RightDrawerContent), typeof(object), typeof(DrawerHost), new PropertyMetadata(default(object)));

        public object RightDrawerContent
        {
            get { return (object)GetValue(RightDrawerContentProperty); }
            set { SetValue(RightDrawerContentProperty, value); }
        }

        public static readonly DependencyProperty RightDrawerContentTemplateProperty = DependencyProperty.Register(
            nameof(RightDrawerContentTemplate), typeof(DataTemplate), typeof(DrawerHost), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate RightDrawerContentTemplate
        {
            get { return (DataTemplate)GetValue(RightDrawerContentTemplateProperty); }
            set { SetValue(RightDrawerContentTemplateProperty, value); }
        }

        public static readonly DependencyProperty RightDrawerContentTemplateSelectorProperty = DependencyProperty.Register(
            nameof(RightDrawerContentTemplateSelector), typeof(DataTemplateSelector), typeof(DrawerHost), new PropertyMetadata(default(DataTemplateSelector)));

        public DataTemplateSelector RightDrawerContentTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(RightDrawerContentTemplateSelectorProperty); }
            set { SetValue(RightDrawerContentTemplateSelectorProperty, value); }
        }

        public static readonly DependencyProperty RightDrawerContentStringFormatProperty = DependencyProperty.Register(
            nameof(RightDrawerContentStringFormat), typeof(string), typeof(DrawerHost), new PropertyMetadata(default(string)));

        public string RightDrawerContentStringFormat
        {
            get { return (string)GetValue(RightDrawerContentStringFormatProperty); }
            set { SetValue(RightDrawerContentStringFormatProperty, value); }
        }

        public static readonly DependencyProperty RightDrawerBackgroundProperty = DependencyProperty.Register(
            nameof(RightDrawerBackground), typeof(Brush), typeof(DrawerHost), new PropertyMetadata(default(Brush)));

        public Brush RightDrawerBackground
        {
            get { return (Brush)GetValue(RightDrawerBackgroundProperty); }
            set { SetValue(RightDrawerBackgroundProperty, value); }
        }

        public static readonly DependencyProperty IsRightDrawerOpenProperty = DependencyProperty.Register(
            nameof(IsRightDrawerOpen), typeof(bool), typeof(DrawerHost), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsDrawerOpenPropertyChangedCallback));

        public bool IsRightDrawerOpen
        {
            get { return (bool)GetValue(IsRightDrawerOpenProperty); }
            set { SetValue(IsRightDrawerOpenProperty, value); }
        }

        private static readonly DependencyPropertyKey RightDrawerZIndexPropertyKey =
                                    DependencyProperty.RegisterReadOnly(
                                    "RightDrawerZIndex", typeof(int), typeof(DrawerHost),
                                    new PropertyMetadata(1));

        public static readonly DependencyProperty RightDrawerZIndexProperty = RightDrawerZIndexPropertyKey.DependencyProperty;

        public int RightDrawerZIndex
        {
            get { return (int)GetValue(RightDrawerZIndexProperty); }
            private set { SetValue(RightDrawerZIndexPropertyKey, value); }
        }

        #endregion

        #region bottom drawer

        public static readonly DependencyProperty BottomDrawerContentProperty = DependencyProperty.Register(
            nameof(BottomDrawerContent), typeof(object), typeof(DrawerHost), new PropertyMetadata(default(object)));

        public object BottomDrawerContent
        {
            get { return (object)GetValue(BottomDrawerContentProperty); }
            set { SetValue(BottomDrawerContentProperty, value); }
        }

        public static readonly DependencyProperty BottomDrawerContentTemplateProperty = DependencyProperty.Register(
            nameof(BottomDrawerContentTemplate), typeof(DataTemplate), typeof(DrawerHost), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate BottomDrawerContentTemplate
        {
            get { return (DataTemplate)GetValue(BottomDrawerContentTemplateProperty); }
            set { SetValue(BottomDrawerContentTemplateProperty, value); }
        }

        public static readonly DependencyProperty BottomDrawerContentTemplateSelectorProperty = DependencyProperty.Register(
            nameof(BottomDrawerContentTemplateSelector), typeof(DataTemplateSelector), typeof(DrawerHost), new PropertyMetadata(default(DataTemplateSelector)));

        public DataTemplateSelector BottomDrawerContentTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(BottomDrawerContentTemplateSelectorProperty); }
            set { SetValue(BottomDrawerContentTemplateSelectorProperty, value); }
        }

        public static readonly DependencyProperty BottomDrawerContentStringFormatProperty = DependencyProperty.Register(
            nameof(BottomDrawerContentStringFormat), typeof(string), typeof(DrawerHost), new PropertyMetadata(default(string)));

        public string BottomDrawerContentStringFormat
        {
            get { return (string)GetValue(BottomDrawerContentStringFormatProperty); }
            set { SetValue(BottomDrawerContentStringFormatProperty, value); }
        }

        public static readonly DependencyProperty BottomDrawerBackgroundProperty = DependencyProperty.Register(
            nameof(BottomDrawerBackground), typeof(Brush), typeof(DrawerHost), new PropertyMetadata(default(Brush)));

        public Brush BottomDrawerBackground
        {
            get { return (Brush)GetValue(BottomDrawerBackgroundProperty); }
            set { SetValue(BottomDrawerBackgroundProperty, value); }
        }

        public static readonly DependencyProperty IsBottomDrawerOpenProperty = DependencyProperty.Register(
            nameof(IsBottomDrawerOpen), typeof(bool), typeof(DrawerHost), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsDrawerOpenPropertyChangedCallback));

        public bool IsBottomDrawerOpen
        {
            get { return (bool)GetValue(IsBottomDrawerOpenProperty); }
            set { SetValue(IsBottomDrawerOpenProperty, value); }
        }

        private static readonly DependencyPropertyKey BottomDrawerZIndexPropertyKey =
                                    DependencyProperty.RegisterReadOnly(
                                    "BottomDrawerZIndex", typeof(int), typeof(DrawerHost),
                                    new PropertyMetadata(3));

        public static readonly DependencyProperty BottomDrawerZIndexProperty = BottomDrawerZIndexPropertyKey.DependencyProperty;

        public int BottomDrawerZIndex
        {
            get { return (int)GetValue(BottomDrawerZIndexProperty); }
            private set { SetValue(BottomDrawerZIndexPropertyKey, value); }
        }

        #endregion

        public override void OnApplyTemplate()
        {
            if (_templateContentCoverElement != null)
                _templateContentCoverElement.PreviewMouseLeftButtonUp += TemplateContentCoverElementOnPreviewMouseLeftButtonUp;
            WireDrawer(_leftDrawerElement, true);
            WireDrawer(_topDrawerElement, true);
            WireDrawer(_rightDrawerElement, true);
            WireDrawer(_bottomDrawerElement, true);

            base.OnApplyTemplate();

            _templateContentCoverElement = GetTemplateChild(TemplateContentCoverPartName) as FrameworkElement;
            if (_templateContentCoverElement != null)
                _templateContentCoverElement.PreviewMouseLeftButtonUp += TemplateContentCoverElementOnPreviewMouseLeftButtonUp;
            _leftDrawerElement = WireDrawer(GetTemplateChild(TemplateLeftDrawerPartName) as FrameworkElement, false);
            _topDrawerElement = WireDrawer(GetTemplateChild(TemplateTopDrawerPartName) as FrameworkElement, false);
            _rightDrawerElement = WireDrawer(GetTemplateChild(TemplateRightDrawerPartName) as FrameworkElement, false);
            _bottomDrawerElement = WireDrawer(GetTemplateChild(TemplateBottomDrawerPartName) as FrameworkElement, false);

            UpdateVisualStates(false);
        }

        private FrameworkElement WireDrawer(FrameworkElement drawerElement, bool unwire)
        {
            if (drawerElement == null) return null;

            if (unwire)
            {
                drawerElement.GotFocus -= DrawerElement_GotFocus;
                drawerElement.MouseDown -= DrawerElement_MouseDown;

                return drawerElement;
            }

            drawerElement.GotFocus += DrawerElement_GotFocus;
            drawerElement.MouseDown += DrawerElement_MouseDown;

            return drawerElement;
        }

        private void DrawerElement_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ReactToFocus(sender);
        }

        private void DrawerElement_GotFocus(object sender, RoutedEventArgs e)
        {
            ReactToFocus(sender);
        }

        private void ReactToFocus(object sender)
        {
            if (sender == _leftDrawerElement)
                PrepareZIndexes(LeftDrawerZIndexPropertyKey);
            else if (sender == _topDrawerElement)
                PrepareZIndexes(TopDrawerZIndexPropertyKey);
            else if (sender == _rightDrawerElement)
                PrepareZIndexes(RightDrawerZIndexPropertyKey);
            else if (sender == _bottomDrawerElement)
                PrepareZIndexes(BottomDrawerZIndexPropertyKey);
        }

        private void TemplateContentCoverElementOnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            SetCurrentValue(IsLeftDrawerOpenProperty, false);
            SetCurrentValue(IsRightDrawerOpenProperty, false);
            SetCurrentValue(IsTopDrawerOpenProperty, false);
            SetCurrentValue(IsBottomDrawerOpenProperty, false);
        }

        private void UpdateVisualStates(bool? useTransitions = null)
        {
            var anyOpen = IsTopDrawerOpen || IsLeftDrawerOpen || IsBottomDrawerOpen || IsRightDrawerOpen;

            VisualStateManager.GoToState(this,
                !anyOpen ? TemplateAllDrawersAllClosedStateName : TemplateAllDrawersAnyOpenStateName, useTransitions.HasValue ? useTransitions.Value : !TransitionAssist.GetDisableTransitions(this));

            VisualStateManager.GoToState(this,
                IsLeftDrawerOpen ? TemplateLeftOpenStateName : TemplateLeftClosedStateName, useTransitions.HasValue ? useTransitions.Value : !TransitionAssist.GetDisableTransitions(this));

            VisualStateManager.GoToState(this,
                IsTopDrawerOpen ? TemplateTopOpenStateName : TemplateTopClosedStateName, useTransitions.HasValue ? useTransitions.Value : !TransitionAssist.GetDisableTransitions(this));

            VisualStateManager.GoToState(this,
                IsRightDrawerOpen ? TemplateRightOpenStateName : TemplateRightClosedStateName, useTransitions.HasValue ? useTransitions.Value : !TransitionAssist.GetDisableTransitions(this));

            VisualStateManager.GoToState(this,
                IsBottomDrawerOpen ? TemplateBottomOpenStateName : TemplateBottomClosedStateName, useTransitions.HasValue ? useTransitions.Value : !TransitionAssist.GetDisableTransitions(this));
        }

        private static void IsDrawerOpenPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var drawerHost = (DrawerHost)dependencyObject;
            if (!drawerHost._lockZIndexes && (bool)dependencyPropertyChangedEventArgs.NewValue)
                drawerHost.PrepareZIndexes(drawerHost._zIndexPropertyLookup[dependencyPropertyChangedEventArgs.Property]);
            drawerHost.UpdateVisualStates();
        }

        private void PrepareZIndexes(DependencyPropertyKey zIndexDependencyPropertyKey)
        {
            var newOrder = new[] { zIndexDependencyPropertyKey }
                .Concat(_zIndexPropertyLookup.Values.Except(new[] { zIndexDependencyPropertyKey })
                .OrderByDescending(dpk => (int)GetValue(dpk.DependencyProperty)))
                .Reverse()
                .Select((dpk, idx) => new { dpk, idx });

            foreach (var a in newOrder)
                SetValue(a.dpk, a.idx);
        }

        private void CloseDrawerHandler(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            if (executedRoutedEventArgs.Handled) return;

            SetOpenFlag(executedRoutedEventArgs, false);

            executedRoutedEventArgs.Handled = true;
        }

        private void OpenDrawerHandler(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            if (executedRoutedEventArgs.Handled) return;

            SetOpenFlag(executedRoutedEventArgs, true);

            executedRoutedEventArgs.Handled = true;
        }

        private void SetOpenFlag(ExecutedRoutedEventArgs executedRoutedEventArgs, bool value)
        {
            DrawerArgs drawerArgs = new DrawerArgs(DrawerChangedEvent, this) { IsOpen = value };
            RoutedEventArgs eventArgs = null;
            if (executedRoutedEventArgs.Parameter is Dock)
            {
                switch ((Dock)executedRoutedEventArgs.Parameter)
                {
                    case Dock.Left:
                        SetCurrentValue(IsLeftDrawerOpenProperty, value);
                        drawerArgs.DrawerDockComponent = DrawerDockComponent.Left;
                        eventArgs = value ? new RoutedEventArgs(DrawerLeftOpenEvent,this) : new RoutedEventArgs(DrawerLeftCloseEvent,this);
                        break;
                    case Dock.Top:
                        SetCurrentValue(IsTopDrawerOpenProperty, value);
                        drawerArgs.DrawerDockComponent = DrawerDockComponent.Top;
                        eventArgs = value ? new RoutedEventArgs(DrawerTopOpenEvent, this) : new RoutedEventArgs(DrawerTopCloseEvent, this);
                        break;
                    case Dock.Right:
                        SetCurrentValue(IsRightDrawerOpenProperty, value);
                        drawerArgs.DrawerDockComponent = DrawerDockComponent.Right;
                        eventArgs = value ? new RoutedEventArgs(DrawerRightOpenEvent, this) : new RoutedEventArgs(DrawerRightCloseEvent, this);
                        break;
                    case Dock.Bottom:
                        SetCurrentValue(IsBottomDrawerOpenProperty, value);
                        drawerArgs.DrawerDockComponent = DrawerDockComponent.Bottom;
                        eventArgs = value ? new RoutedEventArgs(DrawerBottomOpenEvent, this) : new RoutedEventArgs(DrawerBottomCloseEvent, this);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                RaiseEvent(eventArgs);
                RaiseEvent(drawerArgs);
            }
            else
            {
                try
                {
                    _lockZIndexes = true;
                    SetCurrentValue(IsLeftDrawerOpenProperty, value);
                    SetCurrentValue(IsTopDrawerOpenProperty, value);
                    SetCurrentValue(IsRightDrawerOpenProperty, value);
                    SetCurrentValue(IsBottomDrawerOpenProperty, value);
                    drawerArgs.DrawerDockComponent = DrawerDockComponent.All;
                    eventArgs = value ? new RoutedEventArgs(DrawerAllOpenEvent, this) : new RoutedEventArgs(DrawerAllCloseEvent, this);
                    RaiseEvent(eventArgs);
                    RaiseEvent(drawerArgs);
                }
                finally
                {
                    _lockZIndexes = false;
                }
            }
        }
    }

    public enum DrawerDockComponent
    {
        Left,
        Top,
        Right,
        Bottom,
        All,
    }

    /// <summary>
    /// Allows transitions to be disabled where supported.
    /// </summary>
    public static class TransitionAssist
    {
        /// <summary>
        /// Allows transitions to be disabled where supported.  Note this is an inheritable property.
        /// </summary>
        public static readonly DependencyProperty DisableTransitionsProperty = DependencyProperty.RegisterAttached(
            "DisableTransitions", typeof(bool), typeof(TransitionAssist), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// Allows transitions to be disabled where supported.  Note this is an inheritable property.
        /// </summary>
        public static void SetDisableTransitions(DependencyObject element, bool value)
        {
            element.SetValue(DisableTransitionsProperty, value);
        }

        /// <summary>
        /// Allows transitions to be disabled where supported.  Note this is an inheritable property.
        /// </summary>
        public static bool GetDisableTransitions(DependencyObject element)
        {
            return (bool)element.GetValue(DisableTransitionsProperty);
        }
    }
}
