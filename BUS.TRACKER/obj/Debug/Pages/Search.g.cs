﻿

#pragma checksum "C:\Users\EricProm\Documents\bustracker\BUS.TRACKER\BUS.TRACKER\Pages\Search.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F0B9F478F260970154F47BDC6A5A2532"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BUS.TRACKER.Pages
{
    partial class Search : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 28 "..\..\Pages\Search.xaml"
                ((global::Windows.UI.Xaml.Controls.Pivot)(target)).SelectionChanged += this.Pivot_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 311 "..\..\Pages\Search.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.StopPivot_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 255 "..\..\Pages\Search.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.ArlPivot_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 197 "..\..\Pages\Search.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.BrtPivot_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 139 "..\..\Pages\Search.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.MrtPivot_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 85 "..\..\Pages\Search.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.BtsPivot_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 7:
                #line 30 "..\..\Pages\Search.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.RoutePivot_SelectionChanged;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}

