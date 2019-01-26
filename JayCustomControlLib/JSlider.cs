using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Collections;
using System.Windows.Threading;

using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

using System.Windows.Input;
using System.Windows.Media;

using MS.Win32;
using MS.Internal;
using MS.Internal.Commands;


// For typeconverter
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Windows.Controls;
using System.Security;
using JayLib;
using JayCustomControlLib.CommonBasicClass;
using System.Collections.Generic;

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
    ///     <MyNamespace:JSlider/>
    ///
    /// </summary>
    /// 
    [Localizability(LocalizationCategory.Ignore)]
    [DefaultEvent("ValueChanged"), DefaultProperty("Value")]
    [TemplatePart(Name = "PART_Track", Type = typeof(Track))]
    [TemplatePart(Name = "PART_SelectionRange", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "PART_TrackBackground",Type =typeof(Border))]
    public class JSlider : RangeBase
    {
        #region Constructors

        /// <summary>
        /// Instantiates a new instance of a JSlider with out Dispatcher.
        /// </summary>
        /// <ExternalAPI/>
        public JSlider() : base()
        {
        }

        /// <summary>
        /// This is the static constructor for the JSlider class.  It
        /// simply registers the appropriate class handlers for the input
        /// devices, and defines a default style sheet.
        /// </summary>
        static JSlider()
        {
            // Initialize CommandCollection & CommandLink(s)
            InitializeCommands();

            // Register all PropertyTypeMetadata
            MinimumProperty.OverrideMetadata(typeof(JSlider), new FrameworkPropertyMetadata(0.0d, FrameworkPropertyMetadataOptions.AffectsMeasure));
            MaximumProperty.OverrideMetadata(typeof(JSlider), new FrameworkPropertyMetadata(10.0d, FrameworkPropertyMetadataOptions.AffectsMeasure));
            ValueProperty.OverrideMetadata(typeof(JSlider), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure));

            // Register Event Handler for the Thumb
            EventManager.RegisterClassHandler(typeof(JSlider), Thumb.DragStartedEvent, new DragStartedEventHandler(JSlider.OnThumbDragStarted));
            EventManager.RegisterClassHandler(typeof(JSlider), Thumb.DragDeltaEvent, new DragDeltaEventHandler(JSlider.OnThumbDragDelta));
            EventManager.RegisterClassHandler(typeof(JSlider), Thumb.DragCompletedEvent, new DragCompletedEventHandler(JSlider.OnThumbDragCompleted));

            // Listen to MouseLeftButtonDown event to determine if slide should move focus to itself
            EventManager.RegisterClassHandler(typeof(JSlider), Mouse.MouseDownEvent, new MouseButtonEventHandler(JSlider._OnMouseLeftButtonDown), true);
            EventManager.RegisterClassHandler(typeof(JSlider), Mouse.MouseUpEvent, new MouseButtonEventHandler(JSlider._OnMouseLeftButtonUp), true);

            DefaultStyleKeyProperty.OverrideMetadata(typeof(JSlider), new FrameworkPropertyMetadata(typeof(JSlider)));
        }

        #endregion Constructors

        #region Commands

        /// <summary>
        /// Increase JSlider value
        /// </summary>
        public static RoutedCommand IncreaseLarge { get; private set; } = null;

        /// <summary>
        /// Decrease JSlider value
        /// </summary>
        public static RoutedCommand DecreaseLarge { get; private set; } = null;

        /// <summary>
        /// Increase JSlider value
        /// </summary>
        public static RoutedCommand IncreaseSmall { get; private set; } = null;

        /// <summary>
        /// Decrease JSlider value
        /// </summary>
        public static RoutedCommand DecreaseSmall { get; private set; } = null;

        /// <summary>
        /// Set JSlider value to mininum
        /// </summary>
        public static RoutedCommand MinimizeValue { get; private set; } = null;

        /// <summary>
        /// Set JSlider value to maximum
        /// </summary>
        public static RoutedCommand MaximizeValue { get; private set; } = null;

        static void InitializeCommands()
        {
            IncreaseLarge = new RoutedCommand("IncreaseLarge", typeof(JSlider));
            DecreaseLarge = new RoutedCommand("DecreaseLarge", typeof(JSlider));
            IncreaseSmall = new RoutedCommand("IncreaseSmall", typeof(JSlider));
            DecreaseSmall = new RoutedCommand("DecreaseSmall", typeof(JSlider));
            MinimizeValue = new RoutedCommand("MinimizeValue", typeof(JSlider));
            MaximizeValue = new RoutedCommand("MaximizeValue", typeof(JSlider));

            MyCommandHelper.RegisterCommandHandler(typeof(JSlider), IncreaseLarge, new ExecutedRoutedEventHandler(OnIncreaseLargeCommand),
                                                  new SliderGesture(Key.PageUp, Key.PageDown, false));

            MyCommandHelper.RegisterCommandHandler(typeof(JSlider), DecreaseLarge, new ExecutedRoutedEventHandler(OnDecreaseLargeCommand),
                                                  new SliderGesture(Key.PageDown, Key.PageUp, false));

            MyCommandHelper.RegisterCommandHandler(typeof(JSlider), IncreaseSmall, new ExecutedRoutedEventHandler(OnIncreaseSmallCommand),
                                                  new SliderGesture(Key.Up, Key.Down, false),
                                                  new SliderGesture(Key.Right, Key.Left, true));

            MyCommandHelper.RegisterCommandHandler(typeof(JSlider), DecreaseSmall, new ExecutedRoutedEventHandler(OnDecreaseSmallCommand),
                                                  new SliderGesture(Key.Down, Key.Up, false),
                                                  new SliderGesture(Key.Left, Key.Right, true));

            MyCommandHelper.RegisterCommandHandler(typeof(JSlider), MinimizeValue, new ExecutedRoutedEventHandler(OnMinimizeValueCommand),
                                                  Key.Home);

            MyCommandHelper.RegisterCommandHandler(typeof(JSlider), MaximizeValue, new ExecutedRoutedEventHandler(OnMaximizeValueCommand),
                                                  Key.End);

        }


        private class SliderGesture : InputGesture
        {
            public SliderGesture(Key normal, Key inverted, bool forHorizontal)
            {
                _normal = normal;
                _inverted = inverted;
                _forHorizontal = forHorizontal;
            }

            /// <summary>
            /// Sees if the InputGesture matches the input associated with the inputEventArgs
            /// </summary>
            public override bool Matches(object targetElement, InputEventArgs inputEventArgs)
            {
                JSlider JSlider = targetElement as JSlider;
                if (inputEventArgs is KeyEventArgs keyEventArgs && JSlider != null && Keyboard.Modifiers == ModifierKeys.None)
                {
                    if ((int)_normal == (int)keyEventArgs.Key)
                    {
                        return !IsInverted(JSlider);
                    }
                    if ((int)_inverted == (int)keyEventArgs.Key)
                    {
                        return IsInverted(JSlider);
                    }
                }
                return false;
            }

            private bool IsInverted(JSlider JSlider)
            {
                if (_forHorizontal)
                {
                    return JSlider.IsDirectionReversed != (JSlider.FlowDirection == FlowDirection.RightToLeft);
                }
                else
                {
                    return JSlider.IsDirectionReversed;
                }
            }

            private Key _normal, _inverted;
            private bool _forHorizontal;
        }



        private static void OnIncreaseSmallCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is JSlider JSlider)
            {
                JSlider.OnIncreaseSmall();
            }
        }

        private static void OnDecreaseSmallCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is JSlider JSlider)
            {
                JSlider.OnDecreaseSmall();
            }
        }

        private static void OnMaximizeValueCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is JSlider JSlider)
            {
                JSlider.OnMaximizeValue();
            }
        }

        private static void OnMinimizeValueCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is JSlider JSlider)
            {
                JSlider.OnMinimizeValue();
            }
        }

        private static void OnIncreaseLargeCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is JSlider JSlider)
            {
                JSlider.OnIncreaseLarge();
            }
        }

        private static void OnDecreaseLargeCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is JSlider JSlider)
            {
                JSlider.OnDecreaseLarge();
            }
        }

        #endregion Commands

        #region Properties

        #region Orientation Property

        /// <summary>
        /// DependencyProperty for <see cref="Orientation" /> property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
                DependencyProperty.Register("Orientation", typeof(Orientation), typeof(JSlider),
                                          new FrameworkPropertyMetadata(Orientation.Horizontal,FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// Get/Set Orientation property
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        #endregion

        #region IsDirectionReversed Property
        /// <summary>
        /// JSlider ThumbProportion property
        /// </summary>
        public static readonly DependencyProperty IsDirectionReversedProperty
            = DependencyProperty.Register("IsDirectionReversed", typeof(bool), typeof(JSlider),
                                          new FrameworkPropertyMetadata(false));

        /// <summary>
        /// Get/Set IsDirectionReversed property
        /// </summary>
        [Bindable(true), Category("Appearance")]
        public bool IsDirectionReversed
        {
            get
            {
                return (bool)GetValue(IsDirectionReversedProperty);
            }
            set
            {
                SetValue(IsDirectionReversedProperty, value);
            }
        }
        #endregion

        #region Delay Property
        /// <summary>
        ///     The Property for the Delay property.
        /// </summary>
        public static readonly DependencyProperty DelayProperty = RepeatButton.DelayProperty.AddOwner(typeof(JSlider), new FrameworkPropertyMetadata(40));

        /// <summary>
        ///     Specifies the amount of time, in milliseconds, to wait before repeating begins.
        /// Must be non-negative.
        /// </summary>
        [Bindable(true), Category("Behavior")]
        public int Delay
        {
            get
            {
                return (int)GetValue(DelayProperty);
            }
            set
            {
                SetValue(DelayProperty, value);
            }
        }

        #endregion Delay Property

        #region Interval Property
        /// <summary>
        ///     The Property for the Interval property.
        /// </summary>
        public static readonly DependencyProperty IntervalProperty = RepeatButton.IntervalProperty.AddOwner(typeof(JSlider), new FrameworkPropertyMetadata(40));

        /// <summary>
        ///     Specifies the amount of time, in milliseconds, between repeats once repeating starts.
        /// Must be non-negative
        /// </summary>
        [Bindable(true), Category("Behavior")]
        public int Interval
        {
            get
            {
                return (int)GetValue(IntervalProperty);
            }
            set
            {
                SetValue(IntervalProperty, value);
            }
        }

        #endregion Interval Property

        #region AutoToolTipPlacement Property
        /// <summary>
        ///     The DependencyProperty for the AutoToolTipPlacement property.
        /// </summary>
        public static readonly DependencyProperty AutoToolTipPlacementProperty
            = DependencyProperty.Register("AutoToolTipPlacement", typeof(AutoToolTipPlacement), typeof(JSlider),
                                          new FrameworkPropertyMetadata(System.Windows.Controls.Primitives.AutoToolTipPlacement.None),
                                          new ValidateValueCallback(IsValidAutoToolTipPlacement));

        /// <summary>
        ///     AutoToolTipPlacement property specifies the placement of the AutoToolTip
        /// </summary>
        [Bindable(true), Category("Behavior")]
        public System.Windows.Controls.Primitives.AutoToolTipPlacement AutoToolTipPlacement
        {
            get
            {
                return (System.Windows.Controls.Primitives.AutoToolTipPlacement)GetValue(AutoToolTipPlacementProperty);
            }
            set
            {
                SetValue(AutoToolTipPlacementProperty, value);
            }
        }

        private static bool IsValidAutoToolTipPlacement(object o)
        {
            AutoToolTipPlacement placement = (AutoToolTipPlacement)o;
            return placement == AutoToolTipPlacement.None ||
                   placement == AutoToolTipPlacement.TopLeft ||
                   placement == AutoToolTipPlacement.BottomRight;
        }

        #endregion

        #region AutoToolTipPrecision Property
        /// <summary>
        ///     The DependencyProperty for the AutoToolTipPrecision property.
        ///     Flags:              None
        ///     Default Value:      0
        /// </summary>
        public static readonly DependencyProperty AutoToolTipPrecisionProperty
            = DependencyProperty.Register("AutoToolTipPrecision", typeof(int), typeof(JSlider),
            new FrameworkPropertyMetadata(0), new ValidateValueCallback(IsValidAutoToolTipPrecision));

        /// <summary>
        ///     Get or set number of decimal digits of JSlider's Value shown in AutoToolTip
        /// </summary>
        [Bindable(true), Category("Appearance")]
        public int AutoToolTipPrecision
        {
            get
            {
                return (int)GetValue(AutoToolTipPrecisionProperty);
            }
            set
            {
                SetValue(AutoToolTipPrecisionProperty, value);
            }
        }

        /// <summary>
        /// Validates AutoToolTipPrecision value
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private static bool IsValidAutoToolTipPrecision(object o)
        {
            return (((int)o) >= 0);
        }

        #endregion

        /*
         * TickMark support
         *
         *   - double           TickFrequency
         *   - bool             IsSnapToTickEnabled
         *   - Enum             TickPlacement
         *   - IEumerable<string> TickTexts
         *   - doubleCollection   Ticks
         */
        #region TickMark support
        /// <summary>
        ///     The DependencyProperty for the IsSnapToTickEnabled property.
        /// </summary>
        public static readonly DependencyProperty IsSnapToTickEnabledProperty
            = DependencyProperty.Register("IsSnapToTickEnabled", typeof(bool), typeof(JSlider),
            new FrameworkPropertyMetadata(true));

        /// <summary>
        ///     When 'true', JSlider will automatically move the Thumb (and/or change current value) to the closest TickMark.
        /// </summary>
        [Bindable(true), Category("Behavior")]
        public bool IsSnapToTickEnabled
        {
            get
            {
                return (bool)GetValue(IsSnapToTickEnabledProperty);
            }
            set
            {
                SetValue(IsSnapToTickEnabledProperty, value);
            }
        }

        /// <summary>
        ///     The DependencyProperty for the TickPlacement property.
        /// </summary>
        public static readonly DependencyProperty TickPlacementProperty
            = DependencyProperty.Register("TickPlacement", typeof(System.Windows.Controls.Primitives.TickPlacement), typeof(JSlider),
                                          new FrameworkPropertyMetadata(System.Windows.Controls.Primitives.TickPlacement.None),
                                          new ValidateValueCallback(IsValidTickPlacement));

        /// <summary>
        ///     JSlider uses this value to determine where to show the Ticks.
        /// When Ticks is not 'null', JSlider will ignore 'TickFrequency', and draw only TickMarks
        /// that specified in Ticks collection.
        /// </summary>
        [Bindable(true), Category("Appearance")]
        public System.Windows.Controls.Primitives.TickPlacement TickPlacement
        {
            get
            {
                return (System.Windows.Controls.Primitives.TickPlacement)GetValue(TickPlacementProperty);
            }
            set
            {
                SetValue(TickPlacementProperty, value);
            }
        }

        private static bool IsValidTickPlacement(object o)
        {
            TickPlacement value = (TickPlacement)o;
            return value == TickPlacement.None ||
                   value == TickPlacement.TopLeft ||
                   value == TickPlacement.BottomRight ||
                   value == TickPlacement.Both;
        }

        /// <summary>
        ///     The DependencyProperty for the TickFrequency property.
        ///     Default Value is 1.0
        /// </summary>
        public static readonly DependencyProperty TickFrequencyProperty
            = DependencyProperty.Register("TickFrequency", typeof(double), typeof(JSlider),
            new FrameworkPropertyMetadata(1.0),
            new ValidateValueCallback(IsValidDoubleValue));

        /// <summary>
        ///     JSlider uses this value to determine where to show the Ticks.
        /// When Ticks is not 'null', JSlider will ignore 'TickFrequency', and draw only TickMarks
        /// that specified in Ticks collection.
        /// </summary>
        [Bindable(true), Category("Appearance")]
        public double TickFrequency
        {
            get
            {
                return (double)GetValue(TickFrequencyProperty);
            }
            set
            {
                SetValue(TickFrequencyProperty, value);
            }
        }



        [Bindable(true), Category("Appearance")]
        public IEnumerable<string> TickTexts
        {
            get { return (IEnumerable<string>)GetValue(TickTextsProperty); }
            set { SetValue(TickTextsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TickTexts.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TickTextsProperty =
            DependencyProperty.Register("TickTexts", typeof(IEnumerable<string>), typeof(JSlider), new FrameworkPropertyMetadata(default(IEnumerable<string>),FrameworkPropertyMetadataOptions.Inherits));

        

        #endregion TickMark support

        /*
         * Selection support
         *
         *   - bool   IsSelectionRangeEnabled
         *   - double SelectionStart
         *   - double SelectionEnd
         */

        #region Selection supports

        /// <summary>
        ///     The DependencyProperty for the IsSelectionRangeEnabled property.
        /// </summary>
        public static readonly DependencyProperty IsSelectionRangeEnabledProperty
            = DependencyProperty.Register("IsSelectionRangeEnabled", typeof(bool), typeof(JSlider),
            new FrameworkPropertyMetadata(false));

        /// <summary>
        ///     Enable or disable selection support on JSlider
        /// </summary>
        [Bindable(true), Category("Appearance")]
        public bool IsSelectionRangeEnabled
        {
            get
            {
                return (bool)GetValue(IsSelectionRangeEnabledProperty);
            }
            set
            {
                SetValue(IsSelectionRangeEnabledProperty, value);
            }
        }

        /// <summary>
        ///     The DependencyProperty for the SelectionStart property.
        /// </summary>
        public static readonly DependencyProperty SelectionStartProperty
            = DependencyProperty.Register("SelectionStart", typeof(double), typeof(JSlider),
                    new FrameworkPropertyMetadata(0.0d,
                        FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                        new PropertyChangedCallback(OnSelectionStartChanged),
                        new CoerceValueCallback(CoerceSelectionStart)),
                    new ValidateValueCallback(IsValidDoubleValue));

        /// <summary>
        ///     Get or set starting value of selection.
        /// </summary>
        [Bindable(true), Category("Appearance")]
        public double SelectionStart
        {
            get { return (double)GetValue(SelectionStartProperty); }
            set { SetValue(SelectionStartProperty, value); }
        }

        private static void OnSelectionStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            JSlider ctrl = (JSlider)d;
            double oldValue = (double)e.OldValue;
            double newValue = (double)e.NewValue;

            ctrl.CoerceValue(SelectionEndProperty);
            ctrl.UpdateSelectionRangeElementPositionAndSize();
        }

        private static object CoerceSelectionStart(DependencyObject d, object value)
        {
            JSlider JSlider = (JSlider)d;
            double selection = (double)value;

            double min = JSlider.Minimum;
            double max = JSlider.Maximum;

            if (selection < min)
            {
                return min;
            }
            if (selection > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        ///     The DependencyProperty for the SelectionEnd property.
        /// </summary>
        public static readonly DependencyProperty SelectionEndProperty
            = DependencyProperty.Register("SelectionEnd", typeof(double), typeof(JSlider),
                    new FrameworkPropertyMetadata(0.0d,
                        FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                        new PropertyChangedCallback(OnSelectionEndChanged),
                        new CoerceValueCallback(CoerceSelectionEnd)),
                    new ValidateValueCallback(IsValidDoubleValue));

        /// <summary>
        ///     Get or set starting value of selection.
        /// </summary>
        [Bindable(true), Category("Appearance")]
        public double SelectionEnd
        {
            get { return (double)GetValue(SelectionEndProperty); }
            set { SetValue(SelectionEndProperty, value); }
        }

        private static void OnSelectionEndChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            JSlider ctrl = (JSlider)d;
            ctrl.UpdateSelectionRangeElementPositionAndSize();
        }

        private static object CoerceSelectionEnd(DependencyObject d, object value)
        {
            JSlider JSlider = (JSlider)d;
            double selection = (double)value;

            double min = JSlider.SelectionStart;
            double max = JSlider.Maximum;

            if (selection < min)
            {
                return min;
            }
            if (selection > max)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        ///     Called when the value of SelectionEnd is required by the property system.
        /// </summary>
        /// <param name="d">The object on which the property was queried.</param>
        /// <returns>The value of the SelectionEnd property on "d."</returns>
        private static object OnGetSelectionEnd(DependencyObject d)
        {
            return ((JSlider)d).SelectionEnd;
        }

        /// <summary>
        ///     This method is invoked when the Minimum property changes.
        /// </summary>
        /// <param name="oldMinimum">The old value of the Minimum property.</param>
        /// <param name="newMinimum">The new value of the Minimum property.</param>
        protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
        {
            CoerceValue(SelectionStartProperty);
        }

        /// <summary>
        ///     This method is invoked when the Maximum property changes.
        /// </summary>
        /// <param name="oldMaximum">The old value of the Maximum property.</param>
        /// <param name="newMaximum">The new value of the Maximum property.</param>
        protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
        {
            CoerceValue(SelectionStartProperty);
            CoerceValue(SelectionEndProperty);
        }

        #endregion Selection supports

        /*
         * Move-To-Point support
         *
         * Property
         *   - bool   IsMoveToPointEnabled
         *
         * Event Handlers
         *   - OnPreviewMouseLeftButtonDown
         *   - double SelectionEnd
         */
        #region Move-To-Point support

        /// <summary>
        ///     The DependencyProperty for the IsMoveToPointEnabled property.
        /// </summary>
        public static readonly DependencyProperty IsMoveToPointEnabledProperty
            = DependencyProperty.Register("IsMoveToPointEnabled", typeof(bool), typeof(JSlider),
            new FrameworkPropertyMetadata(true));

        /// <summary>
        ///     Enable or disable Move-To-Point support on JSlider.
        ///     Move-To-Point feature, enables JSlider to immediately move the Thumb directly to the location where user
        /// clicked the Mouse.
        /// </summary>
        [Bindable(true), Category("Behavior")]
        public bool IsMoveToPointEnabled
        {
            get
            {
                return (bool)GetValue(IsMoveToPointEnabledProperty);
            }
            set
            {
                SetValue(IsMoveToPointEnabledProperty, value);
            }
        }

        /// <summary>
        /// When IsMoveToPointEneabled is 'true', JSlider needs to preview MouseLeftButtonDown event, in order prevent its RepeatButtons
        /// from handle Left-Click.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {

            if (IsMoveToPointEnabled && Track != null && Track.Thumb != null && !Track.Thumb.IsMouseOver)
            {
                // Move Thumb to the Mouse location

                Point pt = e.MouseDevice.GetPosition(Track);

                _MoveToPointValue = Track.ValueFromPoint(pt);

                e.Handled = true;
            }

            base.OnPreviewMouseLeftButtonDown(e);
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (_IsDragging)
            {
                return;
            }
            if (!(Double.IsInfinity(_MoveToPointValue) || DoubleUtil.IsNaN(_MoveToPointValue)))
            {
                UpdateValue(_MoveToPointValue);
            }
            base.OnPreviewMouseLeftButtonUp(e);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (IsMoveToPointEnabled && Track != null && Track.Thumb != null && !Track.Thumb.IsMouseOver)
            {
                // Move Thumb to the Mouse location

                Point pt = e.MouseDevice.GetPosition(Track);

                _MoveToPointValue = Track.ValueFromPoint(pt);

                e.Handled = true;
            }
            base.OnMouseEnter(e);
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            if (!_IsDragging&&IsMoveToPointEnabled && Track != null && Track.Thumb != null && !Track.Thumb.IsMouseOver)
            {
                // Move Thumb to the Mouse location

                Point pt = e.MouseDevice.GetPosition(Track);

                _MoveToPointValue = Track.ValueFromPoint(pt);

                e.Handled = true;
            }
            base.OnPreviewMouseMove(e);
        }
        #endregion Move-To-Point support

        #region Exterior



        public bool IsExteriorEnable
        {
            get { return (bool)GetValue(IsExteriorEnableProperty); }
            set { SetValue(IsExteriorEnableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsExteriorEnable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsExteriorEnableProperty =
            DependencyProperty.Register("IsExteriorEnable", typeof(bool), typeof(JSlider), new PropertyMetadata(false));



        public ImageSource ThumbImage
        {
            get { return (ImageSource)GetValue(ThumbImageProperty); }
            set { SetValue(ThumbImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ThumbImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThumbImageProperty =
            DependencyProperty.Register("ThumbImage", typeof(ImageSource), typeof(JSlider), new PropertyMetadata(null));

        

        public double ThumbImageWidth
        {
            get { return (double)GetValue(ThumbImageWidthProperty); }
            set { SetValue(ThumbImageWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ThumbImageWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThumbImageWidthProperty =
            DependencyProperty.Register("ThumbImageWidth", typeof(double), typeof(JSlider), new FrameworkPropertyMetadata(11d,FrameworkPropertyMetadataOptions.AffectsRender,new PropertyChangedCallback(OnThumbImageSizeChanged)));


        public double ThumbImageHeight
        {
            get { return (double)GetValue(ThumbImageHeightProperty); }
            set { SetValue(ThumbImageHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ThumbImageHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThumbImageHeightProperty =
            DependencyProperty.Register("ThumbImageHeight", typeof(double), typeof(JSlider), new FrameworkPropertyMetadata(22d,FrameworkPropertyMetadataOptions.AffectsRender,new PropertyChangedCallback(OnThumbImageSizeChanged)));

        private static void OnThumbImageSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is JSlider slider)
            {
                slider.RefreshTrackBackground();
            }
        }

        #endregion

        /*
         * Connection support
         *
         *   - bool             IsUIFireEventDirectly
         */
        #region Connection support
        public bool IsUIFireEventDirectly
        {
            get { return (bool)GetValue(IsUIFireEventDirectlySliderProperty); }
            set { SetValue(IsUIFireEventDirectlySliderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsUIFireEventDirectly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsUIFireEventDirectlySliderProperty =
            DependencyProperty.Register("IsUIFireEventDirectly", typeof(bool), typeof(JSlider),new PropertyMetadata(false));



        public double SendValue
        {
            get { return (double)GetValue(SendValueProperty); }
            set { SetValue(SendValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SendValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SendValueProperty =
            DependencyProperty.Register("SendValue", typeof(double), typeof(JSlider), new PropertyMetadata(0d));




        public ICommand SendValueChangedCommand
        {
            get { return (ICommand)GetValue(SendValueChangedCommandProperty); }
            set { SetValue(SendValueChangedCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SendValueChangedCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SendValueChangedCommandProperty =
            DependencyProperty.Register("SendValueChangedCommand", typeof(ICommand), typeof(JSlider), new PropertyMetadata(null));



        public object SendValueChangedCommandParameter
        {
            get { return (object)GetValue(SendValueChangedCommandParameterProperty); }
            set { SetValue(SendValueChangedCommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SendValueChangedCommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SendValueChangedCommandParameterProperty =
            DependencyProperty.Register("SendValueChangedCommandParameter", typeof(object), typeof(JSlider), new PropertyMetadata(null));



        #endregion

        #endregion // Properties

        #region Event Handlers
        /// <summary>
        /// Listen to Thumb DragStarted event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnThumbDragStarted(object sender, DragStartedEventArgs e)
        {
            JSlider JSlider = sender as JSlider;
            JSlider.OnThumbDragStarted(e);
        }

        /// <summary>
        /// Listen to Thumb DragDelta event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnThumbDragDelta(object sender, DragDeltaEventArgs e)
        {
            JSlider JSlider = sender as JSlider;

            JSlider.OnThumbDragDelta(e);
        }

        /// <summary>
        /// Listen to Thumb DragCompleted event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnThumbDragCompleted(object sender, DragCompletedEventArgs e)
        {
            JSlider JSlider = sender as JSlider;
            JSlider.OnThumbDragCompleted(e);
        }

        /// <summary>
        /// Called when user start dragging the Thumb.
        /// This function can be override to customize the way JSlider handles Thumb movement.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnThumbDragStarted(DragStartedEventArgs e)
        {
            _IsDragging = true;
            // Show AutoToolTip if needed.
            Thumb thumb = e.OriginalSource as Thumb;

            if ((thumb == null) || (this.AutoToolTipPlacement == System.Windows.Controls.Primitives.AutoToolTipPlacement.None))
            {
                return;
            }

            // Save original tooltip
            _thumbOriginalToolTip = thumb.ToolTip;

            if (_autoToolTip == null)
            {
                _autoToolTip = new ToolTip
                {
                    Placement = PlacementMode.Custom,
                    PlacementTarget = thumb,
                    CustomPopupPlacementCallback = new CustomPopupPlacementCallback(this.AutoToolTipCustomPlacementCallback)
                };
            }

            thumb.ToolTip = _autoToolTip;
            _autoToolTip.Content = GetAutoToolTipNumber();
            _autoToolTip.IsOpen = true;
            //((Popup)_autoToolTip.Parent).Reposition();
        }

        /// <summary>
        /// Called when user dragging the Thumb.
        /// This function can be override to customize the way JSlider handles Thumb movement.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnThumbDragDelta(DragDeltaEventArgs e)
        {
            Thumb thumb = e.OriginalSource as Thumb;
            // Convert to Track's co-ordinate
            if (Track != null && thumb == Track.Thumb)
            {
                double newValue = Value + Track.ValueFromDistance(e.HorizontalChange, e.VerticalChange);
                if (!(Double.IsInfinity(newValue) || DoubleUtil.IsNaN(newValue)))
                {
                    UpdateValue(newValue);
                }

                // Show AutoToolTip if needed
                if (this.AutoToolTipPlacement != System.Windows.Controls.Primitives.AutoToolTipPlacement.None)
                {
                    if (_autoToolTip == null)
                    {
                        _autoToolTip = new ToolTip();
                    }

                    _autoToolTip.Content = GetAutoToolTipNumber();

                    if (thumb.ToolTip != _autoToolTip)
                    {
                        thumb.ToolTip = _autoToolTip;
                    }

                    if (!_autoToolTip.IsOpen)
                    {
                        _autoToolTip.IsOpen = true;
                    }
                    //((Popup)_autoToolTip.Parent).Reposition();
                }
            }
        }

        private string GetAutoToolTipNumber()
        {
            NumberFormatInfo format = (NumberFormatInfo)(NumberFormatInfo.CurrentInfo.Clone());
            format.NumberDecimalDigits = this.AutoToolTipPrecision;
            return this.Value.ToString("N", format);
        }

        /// <summary>
        /// Called when user stop dragging the Thumb.
        /// This function can be override to customize the way JSlider handles Thumb movement.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnThumbDragCompleted(DragCompletedEventArgs e)
        {
            // Show AutoToolTip if needed.
            Thumb thumb = e.OriginalSource as Thumb;

            if ((thumb == null) || (this.AutoToolTipPlacement == System.Windows.Controls.Primitives.AutoToolTipPlacement.None))
            {
                _IsDragging = false;

                return;
            }

            if (_autoToolTip != null)
            {
                _autoToolTip.IsOpen = false;
            }

            thumb.ToolTip = _thumbOriginalToolTip;
            _IsDragging = false;
        }


        private CustomPopupPlacement[] AutoToolTipCustomPlacementCallback(Size popupSize, Size targetSize, Point offset)
        {
            switch (this.AutoToolTipPlacement)
            {
                case System.Windows.Controls.Primitives.AutoToolTipPlacement.TopLeft:
                    if (Orientation == Orientation.Horizontal)
                    {
                        // Place popup at top of thumb
                        return new CustomPopupPlacement[]{new CustomPopupPlacement(
                            new Point((targetSize.Width - popupSize.Width) * 0.5, -popupSize.Height),
                            PopupPrimaryAxis.Horizontal)
                        };
                    }
                    else
                    {
                        // Place popup at left of thumb
                        return new CustomPopupPlacement[] {
                            new CustomPopupPlacement(
                            new Point(-popupSize.Width, (targetSize.Height - popupSize.Height) * 0.5),
                            PopupPrimaryAxis.Vertical)
                        };
                    }

                case System.Windows.Controls.Primitives.AutoToolTipPlacement.BottomRight:
                    if (Orientation == Orientation.Horizontal)
                    {
                        // Place popup at bottom of thumb
                        return new CustomPopupPlacement[] {
                            new CustomPopupPlacement(
                            new Point((targetSize.Width - popupSize.Width) * 0.5, targetSize.Height) ,
                            PopupPrimaryAxis.Horizontal)
                        };

                    }
                    else
                    {
                        // Place popup at right of thumb
                        return new CustomPopupPlacement[] {
                            new CustomPopupPlacement(
                            new Point(targetSize.Width, (targetSize.Height - popupSize.Height) * 0.5),
                            PopupPrimaryAxis.Vertical)
                        };
                    }

                default:
                    return new CustomPopupPlacement[] { };
            }
        }


        /// <summary>
        /// Resize and resposition the SelectionRangeElement.
        /// </summary>
        private void UpdateSelectionRangeElementPositionAndSize()
        {
            Size trackSize = new Size(0d, 0d);
            Size thumbSize = new Size(0d, 0d);

            if (Track == null || DoubleUtil.LessThan(SelectionEnd, SelectionStart))
            {
                return;
            }

            trackSize = Track.RenderSize;
            thumbSize = (Track.Thumb != null) ? Track.Thumb.RenderSize : new Size(0d, 0d);

            double range = Maximum - Minimum;
            double valueToSize;

            FrameworkElement rangeElement = this.SelectionRangeElement as FrameworkElement;

            if (rangeElement == null)
            {
                return;
            }

            if (Orientation == Orientation.Horizontal)
            {
                // Calculate part size for HorizontalSlider
                if (DoubleUtil.AreClose(range, 0d) || (DoubleUtil.AreClose(trackSize.Width, thumbSize.Width)))
                {
                    valueToSize = 0d;
                }
                else
                {
                    valueToSize = Math.Max(0.0, (trackSize.Width - thumbSize.Width) / range);
                }

                rangeElement.Width = ((SelectionEnd - SelectionStart) * valueToSize);
                if (IsDirectionReversed)
                {
                    Canvas.SetLeft(rangeElement, (thumbSize.Width * 0.5) + Math.Max(Maximum - SelectionEnd, 0) * valueToSize);
                }
                else
                {
                    Canvas.SetLeft(rangeElement, (thumbSize.Width * 0.5) + Math.Max(SelectionStart - Minimum, 0) * valueToSize);
                }
            }
            else
            {
                // Calculate part size for VerticalSlider
                if (DoubleUtil.AreClose(range, 0d) || (DoubleUtil.AreClose(trackSize.Height, thumbSize.Height)))
                {
                    valueToSize = 0d;
                }
                else
                {
                    valueToSize = Math.Max(0.0, (trackSize.Height - thumbSize.Height) / range);
                }

                rangeElement.Height = ((SelectionEnd - SelectionStart) * valueToSize);
                if (IsDirectionReversed)
                {
                    Canvas.SetTop(rangeElement, (thumbSize.Height * 0.5) + Math.Max(SelectionStart - Minimum, 0) * valueToSize);
                }
                else
                {
                    Canvas.SetTop(rangeElement, (thumbSize.Height * 0.5) + Math.Max(Maximum - SelectionEnd, 0) * valueToSize);
                }
            }
        }


        /// <summary>
        /// Gets or sets reference to JSlider's Track element.
        /// </summary>
        internal Track Track
        {
            get
            {
                return _track;
            }
            set
            {
                _track = value;
            }
        }



        /// <summary>
        /// Gets or sets reference to JSlider's SelectionRange element.
        /// </summary>
        internal FrameworkElement SelectionRangeElement
        {
            get
            {
                return _selectionRangeElement;
            }
            set
            {
                _selectionRangeElement = value;
            }
        }

        internal Border TrackBackground
        {
            get { return _trackBackground; }
            set { _trackBackground = value; }
        }

        /// <summary>
        /// Snap the input 'value' to the closest tick.
        /// If input value is exactly in the middle of 2 surrounding ticks, it will be snapped to the tick that has greater value.
        /// </summary>
        /// <param name="value">Value that want to snap to closest Tick.</param>
        /// <returns>Snapped value if IsSnapToTickEnabled is 'true'. Otherwise, returns un-snaped value.</returns>
        private double SnapToTick(double value)
        {
            if (IsSnapToTickEnabled)
            {
                double previous = Minimum;
                double next = Maximum;

                // This property is rarely set so let's try to avoid the GetValue
                // caching of the mutable default value


                if (DoubleUtil.GreaterThan(TickFrequency, 0.0))
                {
                    previous = Minimum + (Math.Round(((value - Minimum) / TickFrequency)) * TickFrequency);
                    next = Math.Min(Maximum, previous + TickFrequency);
                }

                // Choose the closest value between previous and next. If tie, snap to 'next'.
                value = DoubleUtil.GreaterThanOrClose(value, (previous + next) * 0.5) ? next : previous;
            }

            return value;
        }

        // Sets Value = SnapToTick(value+direction), unless the result of SnapToTick is Value,
        // then it searches for the next tick greater(if direction is positive) than value
        // and sets Value to that tick
        private void MoveToNextTick(double direction)
        {
            if (direction != 0.0)
            {
                double value = this.Value;

                // Find the next value by snapping
                double next = SnapToTick(Math.Max(this.Minimum, Math.Min(this.Maximum, value + direction)));

                bool greaterThan = direction > 0; //search for the next tick greater than value?

                // If the snapping brought us back to value, find the next tick point
                if (next == value
                    && !(greaterThan && value == Maximum)  // Stop if searching up if already at Max
                    && !(!greaterThan && value == Minimum)) // Stop if searching down if already at Min
                {
                    // This property is rarely set so let's try to avoid the GetValue
                    // caching of the mutable default value

                    if (DoubleUtil.GreaterThan(TickFrequency, 0.0))
                    {
                        // Find the current tick we are at
                        double tickNumber = Math.Round((value - Minimum) / TickFrequency);

                        if (greaterThan)
                            tickNumber += 1.0;
                        else
                            tickNumber -= 1.0;

                        next = Minimum + tickNumber * TickFrequency;
                    }
                }


                // Update if we've found a better value
                if (next != value)
                {
                    this.SetCurrentValue(ValueProperty, next);
                }
            }
        }
        #endregion Event Handlers

        #region Private Functions
        private void RefreshTrackBackground()
        {
            if (Orientation == Orientation.Horizontal)
            {
                if (TrackBackground != null)
                {
                    TrackBackground.Margin = new Thickness(ThumbImageWidth / 2, 0d, ThumbImageWidth / 2, 0d);
                }
            }
            else
            {
                if (TrackBackground != null)
                {
                    TrackBackground.Margin = new Thickness(0d, ThumbImageHeight / 2, 0d, ThumbImageHeight / 2);
                }
            }
        }
        #endregion

        #region Events
        public delegate void SendValueChangedEventHandle(double value);
        public event SendValueChangedEventHandle SendValueChanged;
        #endregion

        #region Override Functions

        /// <summary>
        /// This is a class handler for MouseLeftButtonDown event.
        /// The purpose of this handle is to move input focus to JSlider when user pressed
        /// mouse left button on any part of JSlider that is not focusable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void _OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;

            JSlider JSlider = (JSlider)sender;
            JSlider._IsMouseStartDrag = true;
            // When someone click on the JSlider's part, and it's not focusable
            // JSlider need to take the focus in order to process keyboard correctly
            if (!JSlider.IsKeyboardFocusWithin)
            {
                e.Handled = JSlider.Focus() || e.Handled;
            }
        }

        private static void _OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;

            JSlider JSlider = (JSlider)sender;
            JSlider._IsMouseStartDrag = false;
        }

        /// <summary>
        /// Perform arrangement of JSlider's children
        /// </summary>
        /// <param name="finalSize"></param>
        protected override Size ArrangeOverride(Size finalSize)
        {
            Size size = base.ArrangeOverride(finalSize);

            UpdateSelectionRangeElementPositionAndSize();

            return size;
        }

        /// <summary>
        /// Update SelectionRange Length.
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected override void OnValueChanged(double oldValue, double newValue)
        {
            if (IsUIFireEventDirectly&& _IsMouseStartDrag)
            {
                SendValue = Math.Max(Minimum, Math.Min(Maximum, newValue));
                SendValueChanged?.Invoke(SendValue);
                if (SendValueChangedCommand!=null)
                {
                    SendValueChangedCommand.Execute(SendValueChangedCommandParameter);
                }
            }
            else
            {
                base.OnValueChanged(oldValue, newValue);
            }
            UpdateSelectionRangeElementPositionAndSize();
        }

        /// <summary>
        /// JSlider locates the SelectionRangeElement when its visual tree is created
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            SelectionRangeElement = GetTemplateChild(SelectionRangeElementName) as FrameworkElement;
            Track = GetTemplateChild(TrackName) as Track;
            TrackBackground = GetTemplateChild(TrackBackgroundName) as Border;
            if (_autoToolTip != null)
            {
                _autoToolTip.PlacementTarget = Track != null ? Track.Thumb : null;
            }
            RefreshTrackBackground();
        }
        

        #endregion Override Functions

        #region Virtual Functions

        /// <summary>
        /// Call when JSlider.IncreaseLarge command is invoked.
        /// </summary>
        protected virtual void OnIncreaseLarge()
        {
            MoveToNextTick(this.LargeChange);
        }

        /// <summary>
        /// Call when JSlider.DecreaseLarge command is invoked.
        /// </summary>
        protected virtual void OnDecreaseLarge()
        {
            MoveToNextTick(-this.LargeChange);
        }

        /// <summary>
        /// Call when JSlider.IncreaseSmall command is invoked.
        /// </summary>
        protected virtual void OnIncreaseSmall()
        {
            MoveToNextTick(this.SmallChange);
        }

        /// <summary>
        /// Call when JSlider.DecreaseSmall command is invoked.
        /// </summary>
        protected virtual void OnDecreaseSmall()
        {
            MoveToNextTick(-this.SmallChange);
        }

        /// <summary>
        /// Call when JSlider.MaximizeValue command is invoked.
        /// </summary>
        protected virtual void OnMaximizeValue()
        {
            this.SetCurrentValue(ValueProperty, this.Maximum);
        }

        /// <summary>
        /// Call when JSlider.MinimizeValue command is invoked.
        /// </summary>
        protected virtual void OnMinimizeValue()
        {
            this.SetCurrentValue(ValueProperty, this.Minimum);
        }

        #endregion Virtual Functions

        #region Helper Functions
        /// <summary>
        /// Helper function for value update.
        /// This function will also snap the value to tick, if IsSnapToTickEnabled is true.
        /// </summary>
        /// <param name="value"></param>
        private void UpdateValue(double value)
        {
            Double snappedValue = SnapToTick(value);
            if (snappedValue != Value)
            {
                if (!IsUIFireEventDirectly)
                {
                    this.SetCurrentValue(ValueProperty, Math.Max(this.Minimum, Math.Min(this.Maximum, snappedValue)));
                }
                else
                {
                    OnValueChanged(_OldSnappedValue, snappedValue);
                }
                _OldSnappedValue = snappedValue;
            }
        }

        /// <summary>
        /// Validate input value in JSlider (LargeChange, SmallChange, SelectionStart, SelectionEnd, and TickFrequency).
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returns False if value is NaN or NegativeInfinity or PositiveInfinity. Otherwise, returns True.</returns>
        private static bool IsValidDoubleValue(object value)
        {
            double d = (double)value;

            return !(DoubleUtil.IsNaN(d) || double.IsInfinity(d));
        }

        #endregion Helper Functions
        
        #region Private Fields

        private const string TrackName = "PART_Track";
        private const string SelectionRangeElementName = "PART_SelectionRange";
        private const string TrackBackgroundName = "PART_TrackBackground";
        // JSlider required parts
        private Border _trackBackground;
        private FrameworkElement _selectionRangeElement;
        private Track _track;
        private System.Windows.Controls.ToolTip _autoToolTip = null;
        private object _thumbOriginalToolTip = null;
        private double _MoveToPointValue;
        private bool _IsDragging;
        //
        private double _OldSnappedValue = 0d;
        private bool _IsMouseStartDrag;
        #endregion Private Fields
    }
}
