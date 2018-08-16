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
    ///     <MyNamespace:ActiveDisplayer/>
    ///
    /// </summary>
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(ActiveDisplayItem))]
    public class ActiveDisplayer : ItemsControl
    {
        static ActiveDisplayer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ActiveDisplayer), new FrameworkPropertyMetadata(typeof(ActiveDisplayer)));
            ItemsPanelTemplate itemsPanelTemplate = new ItemsPanelTemplate(new FrameworkElementFactory(typeof(VirtualizingStackPanel)));
            itemsPanelTemplate.Seal();
            ItemsPanelProperty.OverrideMetadata(typeof(ActiveDisplayer), new FrameworkPropertyMetadata(itemsPanelTemplate));
        }


        #region Property

        public Brush ActiveBrush
        {
            get { return (Brush)GetValue(ActiveBrushProperty); }
            set { SetValue(ActiveBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActiveBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActiveBrushProperty =
            DependencyProperty.Register("ActiveBrush", typeof(Brush), typeof(ActiveDisplayer), new FrameworkPropertyMetadata(Brushes.LightGreen, FrameworkPropertyMetadataOptions.Inherits));



        public Brush InactiveBrush
        {
            get { return (Brush)GetValue(InactiveBrushProperty); }
            set { SetValue(InactiveBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InactiveBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InactiveBrushProperty =
            DependencyProperty.Register("InactiveBrush", typeof(Brush), typeof(ActiveDisplayer), new FrameworkPropertyMetadata(Brushes.LightGray, FrameworkPropertyMetadataOptions.Inherits));
        #endregion

        #region Override method
        /// <summary>
        /// Return true if the item is (or is eligible to be) its own ItemContainer
        /// </summary>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is ActiveDisplayItem);
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ActiveDisplayItem();
        }
        #endregion
    }
}
