namespace WebSharper.SlickGrid.Definition

module Definition =
    open WebSharper
    open WebSharper.InterfaceGenerator
    open WebSharper.JavaScript.Dom
    open WebSharper.JQuery
    open WebSharper

    module Res =

        let JQueryEventDrag =
            (Resource "JQueryEventDrag" "jquery.event.drag-2.2.js").AssemblyWide()

        let JQueryEventDrop =
            (Resource "JQueryEventDrop" "jquery.event.drop-2.2.js").AssemblyWide()

        let Css =
            (Resource "Css" "slick.grid.css").AssemblyWide()
            |> RequiresExternal [T<JQueryUI.Dependencies.JQueryUICss>]

        let Core =
            (Resource "Core" "slick.core.js").AssemblyWide()
            |> Requires [JQueryEventDrag; JQueryEventDrop; Css]
            |> RequiresExternal [T<JQuery.Resources.JQuery>]

        let DefaultTheme =
            (Resource "DefaultTheme" "slick-default-theme.css").AssemblyWide()
            |> Requires [Css]

        let Js =
            (Resource "Js" "slick.grid.js").AssemblyWide()
            |> Requires [Core]
            |> RequiresExternal [T<JQueryUI.Dependencies.JQueryUIJs>]

        let Dataview =
            Resource "Dataview" "slick.dataview.js"
            |> Requires [Js]

        let Editors =
            Resource "Editors" "slick.editors.js"
            |> Requires [Js]

        let Formatters =
            Resource "Formatters" "slick.formatters.js"
            |> Requires [Js]

        let GroupItemMetadataProvider =
            Resource "GrouptItemsMetadataProvider" "slick.groupitemmetadataprovider.js"
            |> Requires [Js]

        module Plugins =

            let Autotooltips =
                Resource "Autotooltips" "slick.autotooltips.js"

            let Cellcopymanager =
                Resource "Cellcopymanager" "slick.cellcopymanager.js"

            let Cellrangedecorator =
                Resource "Cellrangedecorator" "slick.cellrangedecorator.js"

            let Cellrangeselector =
                Resource "Cellrangeselector" "slick.cellrangeselector.js"
                |> Requires [Cellrangedecorator]

            let Cellselectionmodel =
                Resource "Cellselectionmodel" "slick.cellselectionmodel.js"
                |> Requires [Cellrangeselector]

            let Checkboxselectcolumn =
                Resource "Checkboxselectcolumn" "slick.checkboxselectcolumn.js"

            let HeaderButtonsCss =
                Resource "HeaderbuttonsCss" "slick.headerbuttons.css"

            let HeaderButtons =
                Resource "Headerbuttons" "slick.headerbuttons.js"
                |> Requires [HeaderButtonsCss]

            let HeaderMenuCss =
                Resource "HeadermneuCss" "slick.headermenu.css"

            let HeaderMenu =
                Resource "Headermenu" "slick.headermenu.js"
                |> Requires [HeaderMenuCss]

            let Rowmovemanager =
                Resource "Rowmovemanager" "slick.rowmovemanager.js"

            let Rowselectionmodel =
                Resource "Rowselectionmodel" "slick.rowselectionmodel.js"

        module Controls =
            let ColumnpickerCss =
                Resource "ColumnpickerCss" "slick.columnpicker.css"

            let Columnpicker =
                Resource "Columnpicker" "slick.columnpicker.js"
                |> Requires [ColumnpickerCss]

            let PagerCss =
                Resource "PagerCss" "slick.pager.css"

            let Pager =
                Resource "Pager" "slick.pager.js"
                |> Requires [PagerCss]

    let Box =
        Class "Slick.Box"
        |+> Instance [
                "height" =? T<int>
                "width" =? T<int>
                "visible" =? T<bool>
            ]

    let AbsBox =
        Class "Slick.AbsBox"
        |=> Inherits Box
        |+> Instance [
                "bottom" =? T<int>
                "left" =? T<int>
                "right" =? T<int>
                "top" =? T<int>
            ]

    let Grid_t = Type.New()

    let AutoTooltipsOptions =
        Pattern.Config "Slick.AutoTooltipsOptions" {
            Required = []
            Optional =
                [
                    "maxToolTipLength", T<int>
                ]
        }

    let AutoTooltips =
        Generic - fun t ->
            Class "Slick.AutoTooltips"
            |+> Static [
                    Constructor (!?AutoTooltipsOptions)
                ]
            |+> Instance [
                    "destroy" => T<unit -> unit>
                    "init" => Grid_t.[t] ^-> T<unit>
                ]

    let EditorGenerator =
        Generic - fun t ->
            Class "Slick.EditorGenerator"

    let ValidationResults =
        Pattern.Config "Slick.ValidationResults" {
            Optional =
                [
                    "msg", T<string>
                ]
            Required =
                [
                    "valid", T<bool>
                ]
        }

    let Column_t = Type.New()

    let Formatter (t: Type.IType) = (T<int> * T<int> * T<string> * Column_t.[t] * t) ^-> T<string>

    let Total =
        Class "Slick.Total"
        |+> Instance [
            "sum" =? T<obj>
            "avg" =? T<obj>
        ]

    let Column =
        Generic - fun (t: CodeModel.TypeParameter) ->
            Pattern.Config "Slick.Column" {
                Required =
                    [
                        "id", T<string>
                        "name", T<string>
                    ]
                Optional =
                    [
                        "behavior", T<string>
                        "cannotTriggerInsert", T<bool>
                        "cssClass", T<string>
                        "editor", EditorGenerator.[t]
                        "field", T<string>
                        "formatter", Formatter t
                        "headerCssClass", T<string>
                        "maxWidth", T<int>
                        "minWidth", T<int>
                        "rerenderOnResize", T<bool>
                        "resizable", T<bool>
                        "selectable", T<bool>
                        "sortable", T<bool>
                        "validator", T<string> ^-> ValidationResults
                        "width", T<int>
                        "groupTotalsFormatter", Total * T<string> ^-> T<string>
                    ]
            }
        |=> Column_t

    let BeforeEditCellEventArgs =
        Generic - fun t ->
            Class "Slick.BeforeEditCellEventArgs"
            |+> Instance [
                    "cell" =? T<int>
                    "column" =? Column.[t]
                    "item" =? t
                    "row" =? T<int>
                ]

    let BeforeMoveRowsArgs =
        Class "Slick.BeforeMoveRowsArgs"
        |+> Instance [
                "insertBefore" =? T<int>
                "rows" =? T<int[]>
            ]

    let CellChangeEventArgs =
        Generic - fun t ->
            Class "Slick.CellChangeEventArgs"
            |+> Instance [
                    "cell" =? T<int>
                    "item" =? t
                    "row" =? T<int>
                ]

    let CellCoords =
        Pattern.Config "Slick.CellCoords" {
            Required =
                [
                    "cell", T<int>
                    "row", T<int>
                ]
            Optional = []
        }

    let Event =
        Generic - fun t ->
            Class "Slick.Event"
            |+> Instance [
                    "notify" => t * !?T<Event> * !?T<obj> ^-> T<unit>
                    "subscribe" => (T<Event> * t ^-> T<unit>) ^-> T<unit>
                    "unsubscribe" => (T<Event> * t ^-> T<unit>) ^-> T<unit>
                ]

    let RangesArgs =
        Class "Slick.RangesArgs"
        |+> Instance [
                "ranges" =? T<obj>
            ]

    let CellCopyManager =
        Generic - fun t ->
            Class "Slick.CellCopyManager"
            |+> Instance [
                    "onCopyCancelled" =? Event.[RangesArgs]
                    "onCopyCells" =? Event.[RangesArgs]
                    "clearCopySelection" => T<unit> ^-> T<unit>
                    "destroy" => T<unit> ^-> T<unit>
                    "init" => Grid_t.[t] ^-> T<unit>
                ]

    let CellCssEventArgs =
        Class "Slick.CellCssEventArgs"
        |+> Instance [
                "hash" =? T<obj>
                "key" =? T<string>
            ]

    let Range =
        Class "Slick.Range"
        |+> Static [
                Constructor (T<int> * T<int> * T<int> * T<int>)
            ]
        |+> Instance [
                "fromCell" =? T<int>
                "fromRow" =? T<int>
                "toCell" =? T<int>
                "toRow" =? T<int>
                "isSingleCell" => T<unit -> bool>
                "isSingleRow" => T<unit -> bool>
            ]

    let CellRangeDecoratorOptions =
        Pattern.Config "Slick.CellRangeDecoratorOptions" {
            Required = []
            Optional =
                [
                    "selectionCss", T<obj>
                ]
        }

    let CellRangeDecorator =
        Generic - fun (t: CodeModel.TypeParameter) ->
            Class "Slick.CellRangeDecorator"
            |+> Static [Constructor (Grid_t.[t] * !?CellRangeDecoratorOptions)]
            |+> Instance [
                    "hide" => T<unit> ^-> T<unit>
                    "show" => Range ^-> T<JQuery>
                ]

    let CellRangeSelectorOptions =
        Pattern.Config "Slick.CellRangeSelectorOptions" {
            Required = []
            Optional =
                [
                    "selectionCss", T<obj>
                ]
        }

    let CellRangeSelector =
        Generic - fun t ->
            Class "Slick.CellRangeSelector"
            |+> Static [Constructor (!?CellRangeSelectorOptions)]
            |+> Instance [
                    "onBeforeCellRangeSelected" =? Event.[CellCoords]
                    "onCellRangeSelected" =? Event.[Range]
                    "destroy" => T<unit -> unit>
                    "init" => Grid_t.[t] ^-> T<unit>
                ]

    let SelectionModel =
        Generic - fun t ->
            Class "Slick.SelectionModel"
            |+> Instance [
                    "destroy" => T<unit -> unit>
                    "getSelectedRanges" => T<unit> ^-> Type.ArrayOf Range
                    "init" => Grid_t.[t] ^-> T<unit>
                    "setSelectedRanges" => Type.ArrayOf Range ^-> T<unit>
                ]

    let CellSelectionModelOptions =
        Pattern.Config "Slick.CellSelectionModelOptions" {
            Required = []
            Optional =
                [
                    "selectionCss", T<obj>
                ]
        }

    let CellSelectionModel =
        Generic - fun (t: CodeModel.TypeParameter) ->
            Class "Slick.CellSelectionModel"
            |=> Inherits SelectionModel.[t]
            |+> Static [Constructor (!?CellSelectionModelOptions)]

    let Change =
        Generic - fun t ->
            Pattern.Config "Slick.Change" {
                Required =
                    [
                        "current", t.Type
                        "previous", t.Type
                    ]
                Optional = []
            }

    let CheckboxSelectColumnOptions =
        Pattern.Config "Slick.CheckboxSelectColumnOptions" {
            Required = []
            Optional =
                [
                    "columnId", T<string>
                    "cssClass", T<string>
                    "toolTip", T<string>
                    "width", T<int>
                ]
        }

    let CheckboxSelectColumn =
        Generic - fun t ->
            Class "Slick.CheckboxSelectColumn"
            |+> Static [Constructor (!?CheckboxSelectColumnOptions)]
            |+> Instance [
                    "destroy" => T<unit -> unit>
                    "getColumnDefinition" => T<unit> ^-> Column.[t]
                    "init" => Grid_t.[t] ^-> T<unit>
                ]

    let EditController =
        Interface "Slick.EditController"
        |+> [
                "cancelCurrentEdit" => T<unit -> bool>
                "commitCurrentEdit" => T<unit -> bool>
            ]

    let Position =
        Pattern.Config "Slick.Position" {
            Required =
                [
                    "left", T<int>
                    "top", T<int>
                ]
            Optional = []
        }

    let EditorArgs =
        Generic - fun t ->
            Class "Slick.EditorArgs"
            |+> Instance [
                    "column" =? Column.[t]
                    "container" =? T<Element>
                    "position" =? Position
                    "cancelChanges" => T<unit -> unit>
                    "commitChanges" => T<unit -> unit>
                ]

    let Editor =
        let Editor_t = Type.New()
        Generic - fun t ->
            Pattern.Config "Slick.Editor" {
                Required =
                    [
                        "destroy", T<unit -> unit>
                        "focus", T<unit -> unit>
                        "loadValue", t ^-> T<unit>
                        "serializeValue", T<unit -> string>
                        "applyValue", t * T<string> ^-> T<unit>
                        "isValueChanged", T<unit -> bool>
                        "validate", T<unit> ^-> ValidationResults
                    ]
                Optional = []
            }
            |=> Editor_t
            |+> Instance [
                    "create" => (EditorArgs.[t] ^-> Editor_t.[t]) ^-> EditorGenerator.[t]
                ]

    let EditorEventArgs =
        Generic - fun t ->
            Class "Slick.EditorEventArgs"
            |+> Instance [
                    "editor" =? Editor.[t]
                ]

    let EditorLock =
        let EditorLock_t = Type.New()
        Class "Slick.EditorLock"
        |=> EditorLock_t
        |+> Static [
                Constructor T<unit>
                "global" =? EditorLock_t
            ]
        |+> Instance [
                "activate" => EditController ^-> T<unit>
                "cancelCurrentEdit" => T<unit -> bool>
                "commitCurrentEdit" => T<unit -> bool>
                "deactivate" => EditController ^-> T<unit>
                "isActive" => EditController ^-> T<bool>
            ]

    let Editors =
        Generic - fun t ->
            Class "Slick.Editors"
            |+> Static [
                    "Checkbox" =? EditorGenerator.[t]
                    "Date" =? EditorGenerator.[t]
                    "Integer" =? EditorGenerator.[t]
                    "LongText" =? EditorGenerator.[t]
                    "PercentComplete" =? EditorGenerator.[t]
                    "Text" =? EditorGenerator.[t]
                    "YesNoSelect" =? EditorGenerator.[t]
                ]

    let Formatters =
        Generic - fun t ->
            Class "Slick.Formatters"
            |+> Static [
                    "Checkmark" =? Formatter t
                    "PercentComplete" =? Formatter t
                    "PercentCompleteBar" =? Formatter t
                    "YesNo" =? Formatter t
                ]

    let FromToRangesArgs =
        Class "Slick.FromToRangesArgs"
        |+> Instance [
                "from" =? T<obj>
                "to" =? T<obj>
            ]

    let Group_t = Type.New()

    let NonDataItem =
        Interface "Slick.NonDataItem"

    let GroupTotals =
        Generic - fun (t: CodeModel.TypeParameter) ->
            Pattern.Config "Slick.GroupTotals" {
                Required = []
                Optional =
                    [
                        "group", Group_t.[t]
                    ]
            }
            |=> Implements [NonDataItem]

    let Group =
        Generic - fun (t: CodeModel.TypeParameter) ->
            Pattern.Config "Slick.Group" {
                Required = []
                Optional =
                    [
                        "collapsed", T<bool>
                        "count", T<int>
                        "title", T<string>
                        "totals", GroupTotals.[t]
                        "value", T<string>
                    ]
            }
            |=> Group_t
            |=> Implements [NonDataItem]

    let HeaderEventArgs =
        Generic - fun t ->
            Class "Slick.HeaderEventArgs"
            |+> Instance [
                    "column" =? Column.[t]
                ]

    let IEditorFactory =
        Generic - fun t ->
            Interface "Slick.IEditorFactory"
            |+> [
                    "getEditor" => Column.[t] ^-> Editor.[t]
                ]

    let IFormatterFactory =
        Generic - fun t ->
            Interface "Slick.IFormatterFactory"
            |+> [
                    "getFormatter" => Column.[t] ^-> Formatter t
                ]

    let NewRowEventArgs =
        Generic - fun t ->
            Class "Slick.NewRowEventArgs"
            |+> Instance [
                    "column" =? Column.[t]
                    "item" =? t
                ]

    let RowMoveManager =
        Generic - fun (t: CodeModel.TypeParameter) ->
            Class "Slick.RowMoveManager"
            |+> Static [Constructor T<unit>]
            |+> Instance [
                    "onBeforeMoveRows" =? Event.[BeforeMoveRowsArgs]
                    "onMoveRows" =? Event.[BeforeMoveRowsArgs]
                    "destroy" => T<unit -> unit>
                    "init" => Grid_t.[t] ^-> T<unit>
                ]

    let RowSelectionModelOptions =
        Pattern.Config "Slick.RowSelectionModelOptions" {
            Required = []
            Optional =
                [
                    "selectActiveRow", T<bool>
                ]
        }

    let RowSelectionModel =
        Generic - fun (t: CodeModel.TypeParameter) ->
            Class "Slick.RowSelectionModel"
            |=> Inherits SelectionModel.[t]
            |+> Static [Constructor (!?RowSelectionModelOptions)]
            |+> Instance [
                    "getSelectedRows" => T<unit -> int[]>
                    "setSelectedRows" => T<int[] -> unit>
                ]

    let RowsEventArgs =
        Class "Slick.RowsEventArgs"
        |+> Instance [
                "rows" =? T<int[]>
            ]

    let ScrollEventArgs =
        Class "Slick.ScrollEventArgs"
        |+> Instance [
                "scrollLeft" =? T<int>
                "scrollTop" =? T<int>
            ]

    let SortColumn =
        Pattern.Config "Slick.SortColumn" {
            Required =
                [
                    "columnId", T<string>
                    "sortAsc", T<bool>
                ]
            Optional = []
        }

    let SortEventArgs =
        Generic - fun t ->
            Class "Slick.SortEventArgs"
            |+> Instance [
                    "multiColumnSort" =? T<bool>
                    "sortAsc" =? T<bool>
                    "sortCol" =? Column.[t]
                    "sortCols" =? Type.ArrayOf SortColumn
                ]

    let ValidationError =
        Generic - fun t ->
            Class "Slick.ValidationError"
            |+> Instance [
                    "cell" =? T<int>
                    "cellNode" =? T<Element>
                    "column" =? Column.[t]
                    "editor" =? Editor.[t]
                    "row" =? T<int>
                    "validationResults" =? ValidationResults
                ]

    let VerticalRange =
        Class "Slick.VerticalRange"
        |+> Instance [
                "bottom" =? T<int>
                "top" =? T<int>
            ]

    let Options =
        Generic - fun t ->
            Pattern.Config "Slick.Options" {
                Required = []
                Optional =
                    [
                        "asyncEditorLoadDelay", T<int>
                        "asyncEditorLoading", T<bool>
                        "asyncPostRenderDelay", T<int>
                        "autoEdit", T<bool>
                        "autoHeight", T<bool>
                        "cellFlashingCssClass", T<string>
                        "cellHighlightCssClass", T<string>
                        "defaultColumnWidth", T<int>
                        "editable", T<bool>
                        "editCommandHandler", (t * Column.[t] * T<obj>) ^-> T<unit>
                        "editorFactory", IEditorFactory.[t]
                        "editorLock", EditorLock.Type
                        "enableAddRow", T<bool>
                        "enableAsyncPostRender", T<bool>
                        "enableCellNavigation", T<bool>
                        "enableCellRangeSelection", T<bool>
                        "enableColumnReorder", T<bool>
                        "enableRowReordering", T<bool>
                        "enableTextSelectionOnCells", T<bool>
                        "explicitInitialization", T<bool>
                        "forceFitColumns", T<bool>
                        "formatterFactory", IFormatterFactory.[t]
                        "headerRowHeight", T<int>
                        "leaveSpaceForNewRows", T<bool>
                        "multiColumnSort", T<bool>
                        "multiSelect", T<bool>
                        "rowHeight", T<int>
                        "showHeaderRow", T<bool>
                        "topPanelHeight", T<int>
                    ]
            }

    module Data =

        let Aggregator =
            Generic - fun t ->
                Class "Slick.Data.Aggregator"
                |+> Instance [
                        "accumulate" => t ^-> T<unit>
                        "init" => T<unit -> unit>
                        "storeResult" => T<obj -> unit>
                    ]

        let ColumnMetadata =
            Generic - fun t ->
                Pattern.Config "Slick.Data.ColumnMetadata" {
                    Required = []
                    Optional =
                        [
                            "colspan", T<string>
                            "editor", Editor.[t]
                            "formatter", Formatter t
                        ]
                }

        let Metadata =
            Generic - fun t ->
                Pattern.Config "Slick.Data.Metadata" {
                    Required = []
                    Optional =
                        [
                            "columns", Type.ArrayOf ColumnMetadata.[t]
                            "cssClasses", T<string>
                            "editor", Editor.[t]
                            "focusable", T<bool>
                            "formatter", Formatter t
                            "selectable", T<bool>
                        ]
                }

        let GroupItemMetadataProviderOptions =
            Pattern.Config "Slick.Data.GroupItemMetadataProviderOptions" {
                Required = []
                Optional =
                    [
                        "enableExpandCollapse", T<bool>
                        "groupCssClass", T<string>
                        "groupFocusable", T<bool>
                        "toggleCollapsedCssClass", T<string>
                        "toggleCssClass", T<string>
                        "toggleExpandedCssClass", T<string>
                        "totalsCssClass", T<string>
                        "totalsFocusable", T<bool>
                    ]
            }

        let GroupItemMetadataProvider =
            Generic - fun t ->
                Class "Slick.Data.GroupItemMetadataProvider"
                |+> Static [Constructor (!?GroupItemMetadataProviderOptions)]
                |+> Instance [
                        "destroy" => T<unit -> unit>
                        "getGroupRowMetadata" => t ^-> Metadata.[t]
                        "getTotalsRowMetadata" => t ^-> Metadata.[t]
                        "init" => Grid_t.[t] ^-> T<unit>
                    ]

        let PagingInfo =
            Class "Slick.Data.PagingInfo"
            |+> Instance [
                    "pageNum" =? T<int>
                    "pageSize" =? T<int>
                    "totalPages" =? T<int>
                    "totalRows" =? T<int>
                ]

        let PagingOptions =
            Pattern.Config "Slick.Data.PagingOptions" {
                Required = []
                Optional =
                    [
                        "pageNum", T<int>
                        "pageSize", T<int>
                    ]
            }

        let RefreshHints =
            Pattern.Config "Slick.Data.RefreshHints" {
                Required = []
                Optional =
                    [
                        "ignoreDiffsAfter", T<int>
                        "ignoreDiffsBefore", T<int>
                        "isFilterExpanding", T<bool>
                        "isFisterNarrowing", T<bool>
                        "isFilterUnchanged", T<bool>
                    ]
            }

        let DataViewOptions =
            Generic - fun t ->
                Pattern.Config "Slick.Data.DataViewOptions" {
                    Required = []
                    Optional =
                        [
                            "groupItemMetadataProvider", GroupItemMetadataProvider.[t]
                            "inlineFilters", T<bool>
                        ]
                }
                
        let GroupingOptions =
            Generic - fun t ->
                Pattern.Config "Slick.Data.GroupingOptions" {
                    Required = 
                        [
                            "getter", T<string>
                            "formatter", Group.[t] ^-> T<string>
                        ]
                    Optional = 
                        [
                            "aggregators", Type.ArrayOf Aggregator.[t]
                            "aggregateCollapsed", T<bool>
                            "aggregateEmpty", T<bool>
                            "aggregateChildGroups", T<bool>
                            "collapsed", T<bool>
                            "displayTotalsRow", T<bool>
                            "lazyTotalsCalculation", T<bool>
                            "comparer", Group.[t] * Group.[t] ^-> T<int>
                        ]
                }

        let DataView =
            Generic - fun (t: CodeModel.TypeParameter) ->
                Class "Slick.Data.DataView"
                |+> Static [Constructor !?DataViewOptions.[t]]
                |+> Instance [
                        "onPagingInfoChanged" =? Event.[PagingInfo]
                        "onRowCountChanged" =? Event.[Change.[T<int>]]
                        "onRowsChanged" =? Event.[RowsEventArgs]
                        "addItem" => t ^-> T<unit>
                        "beginUpdate" => T<unit -> unit>
                        "collapseGroup" => T<string -> unit>
                        "deleteItem" => T<string -> unit>
                        "endUpdate" => T<unit -> unit>
                        "expandGroup" => T<string -> unit>
                        "fastSort" => (T<string> + T<unit -> obj>) * T<bool> ^-> T<unit>
                        "getGroups" => T<unit> ^-> Type.ArrayOf Group.[t]
                        "getIdxById" => T<string -> int>
                        "getItem" => T<int> ^-> t
                        "getItemById" => T<string> ^-> t
                        "getItemByIdx" => T<int> ^-> t
                        "getItemMetadata" => T<int> ^-> Metadata.[t]
                        "getItems" => Type.ArrayOf t
                        "getLength" => T<unit -> int>
                        "getPagingInfo" => T<unit> ^-> PagingInfo
                        "getRowById" => T<string -> int>
                        "groupBy" => (T<string> + (t ^-> T<obj>)) * (Group.[t] ^-> T<string>) * (Group.[t] * Group.[t] ^-> T<int>) ^-> T<unit>
                        "insertItem" => T<int> * t ^-> T<unit>
                        "mapIdsToRows" => T<string[] -> int[]>
                        "mapRowsToIds" => T<int[] -> string[]>
                        "refresh" => T<unit -> unit>
                        "reSort" => T<unit -> unit>
                        "setAggregators" => Type.ArrayOf Aggregator.[t] * T<bool> ^-> T<unit>
                        Generic - fun t' -> "setFilter" => (t * t' ^-> T<bool>) ^-> T<unit>
                        Generic - fun t' -> "setFilterArgs" => t' ^-> T<unit>
                        "setItems" => Type.ArrayOf t * !?T<string> ^-> T<unit>
                        "setPagingOptions" => PagingOptions ^-> T<unit>
                        "setRefreshHints" => RefreshHints ^-> T<unit>
                        "sort" => (t * t ^-> T<int>) * T<bool> ^-> T<unit>
                        "syncGridCellCssStyles" => Grid_t.[t] * T<string> ^-> T<unit>
                        "syncGridSelection" => Grid_t.[t] * T<bool> ^-> T<unit>
                        "updateItem" => T<string> * t ^-> T<unit>

                        "setGrouping" => GroupingOptions.[t] ^-> T<unit>
                        "setGrouping" => (Type.ArrayOf GroupingOptions.[t]) ^-> T<unit>
                    ]

        module Aggregators =

            let AvgAggregator =
                Generic - fun (t: CodeModel.TypeParameter) ->
                    Class "Slick.Data.Aggregators.Avg"
                    |=> Inherits Aggregator.[t]
                    |+> Static [Constructor T<string>]

            let MaxAggregator =
                Generic - fun (t: CodeModel.TypeParameter) ->
                    Class "Slick.Data.Aggregators.Max"
                    |=> Inherits Aggregator.[t]
                    |+> Static [Constructor T<string>]

            let MinAggregator =
                Generic - fun (t: CodeModel.TypeParameter) ->
                    Class "Slick.Data.Aggregators.Min"
                    |=> Inherits Aggregator.[t]
                    |+> Static [Constructor T<string>]

            let SumAggregator =
                Generic - fun (t: CodeModel.TypeParameter) ->
                    Class "Slick.Data.Aggregators.Sum"
                    |=> Inherits Aggregator.[t]
                    |+> Static [Constructor T<string>]

    let Grid =
        Generic - fun (t: CodeModel.TypeParameter) ->
            Class "Slick.Grid"
            |=> Grid_t
            |+> Static [Constructor ((T<Element> + T<string>)?container *
                              (Data.DataView.[t] + Type.ArrayOf t)?data *
                              (Type.ArrayOf Column.[t])?columns *
                              !?Options.[t])]
            |+> Instance [
                    "onActiveCellChanged" =? Event.[CellCoords]
                    "onActiveCellPositionChanged" =? Event.[T<unit>]
                    "onAddNewRow" =? Event.[NewRowEventArgs.[t]]
                    "onBeforeCellEditorDestroy" =? Event.[EditorEventArgs.[t]]
                    "onBeforeDestroy" =? Event.[T<unit>]
                    "onBeforeEditCell" =? Event.[BeforeEditCellEventArgs.[t]]
                    "onCellChange" =? Event.[CellChangeEventArgs.[t]]
                    "onCellCssStylesChanged" =? Event.[CellCssEventArgs]
                    "onClick" =? Event.[CellCoords]
                    "onColumnsReordered" =? Event.[T<unit>]
                    "onColumnsResized" =? Event.[T<unit>]
                    "onContextMenu" =? Event.[T<unit>]
                    "onDblClick" =? Event.[CellCoords]
                    "onDrag" =? Event.[T<obj>]
                    "onDragEnd" =? Event.[T<obj>]
                    "onDragInit" =? Event.[T<obj>]
                    "onDragStart" =? Event.[T<obj>]
                    "onHeaderClick" =? Event.[HeaderEventArgs.[t]]
                    "onHeaderContextMenu" =? Event.[HeaderEventArgs.[t]]
                    "onKeyDown" =? Event.[CellCoords]
                    "onMouseEnter" =? Event.[T<unit>]
                    "onMouseLeave" =? Event.[T<unit>]
                    "onScroll" =? Event.[ScrollEventArgs]
                    "onSelectedRangesChanged" =? Event.[RowsEventArgs]
                    "onSort" =? Event.[SortEventArgs.[t]]
                    "onValidationError" =? Event.[ValidationError.[t]]
                    "onViewportChanged" =? Event.[T<unit>]
                    "slickGridVersion" =? T<string>
                    // methods
                    "addCellCssStyles" => T<string> * T<obj> ^-> T<unit>
                    "autosizeColumns" => T<unit -> unit>
                    "canCellBeActive" => T<int> * T<int> ^-> T<bool>
                    "canCellBeSelected" => T<int> * T<int> ^-> T<bool>
                    "destroy" => T<unit -> unit>
                    "editActiveCell" => !?Editor.[t] ^-> T<unit>
                    "finishInitialization" => T<unit -> unit>
                    "flashCell" => T<int>?x * T<int>?y * !?T<int> ^-> T<unit>
                    "focus" => T<unit -> unit>
                    "getActiveCell" => T<unit> ^-> CellCoords
                    "getActiveCellNode" => T<unit -> Element>
                    "getActiveCellPosition" => T<unit> ^-> AbsBox
                    "getCanvasNode" => T<unit -> JQuery>
                    "getCellCssStyles" => T<string -> obj>
                    "getCellEditor" => Editor.[t]
                    "getCellFromEvent" => T<Event> ^-> CellCoords
                    "getCellFromPoint" => T<int> * T<int> ^-> CellCoords
                    "getCellNode" => T<int> * T<int> ^-> T<Element>
                    "getCellNodeBox" => T<int> * T<int> ^-> Box
                    "getColumnIndex" => T<string -> int>
                    "getColumns" => T<unit> ^-> Type.ArrayOf Column.[t]
                    "getData" => Type.ArrayOf t
                    "getDataItem" => T<int> ^-> t
                    "getDataLength" => T<unit -> int>
                    "getEditController" => T<unit> ^-> EditController
                    "getEditorLock" => T<unit> ^-> EditorLock
                    "getGridPosition" => T<unit> ^-> AbsBox
                    "getHeaderRow" => T<unit -> JQuery>
                    "getHeaderRowColumn" => T<int -> Element>
                    "getOptions" => T<unit> ^-> Options.[t]
                    "getRenderedRange" => !?T<int> ^-> VerticalRange
                    "getSelectedRows" => T<unit -> int[]>
                    "getSelectionModel" => T<unit> ^-> SelectionModel.[t]
                    "getSortColumns" => Type.ArrayOf SortColumn
                    "getTopPanel" => T<unit -> JQuery>
                    "getViewport" => !?T<int> ^-> VerticalRange
                    "gotoCell" => T<int>?x * T<int>?y * !?T<bool> ^-> T<unit>
                    "hideHeaderRowColumns" => T<unit -> unit>
                    "hideTopPanel" => T<unit -> unit>
                    "invalidate" => T<unit -> unit>
                    "invalidateAllRows" => T<unit -> unit>
                    "invalidateRow" => T<int -> unit>
                    "invalidateRows" => T<int[] -> unit>
                    "navigateDown" => T<unit -> unit>
                    "navigateLeft" => T<unit -> unit>
                    "navigateNext" => T<unit -> unit>
                    "navigatePrev" => T<unit -> unit>
                    "navigateRight" => T<unit -> unit>
                    "navigateUp" => T<unit -> unit>
                    "registerPlugin" => T<obj -> unit>
                    "removeCellCssStyles" => T<string -> unit>
                    "render" => T<unit -> unit>
                    "resetActiveCell" => T<unit -> unit>
                    "resizeCanvas" => T<unit -> unit>
                    "scrollRowIntoView" => T<int> * T<bool> ^-> T<unit>
                    "setActiveCell" => T<int> * T<int> ^-> T<unit>
                    "setCellCssStyles" => T<string> * T<obj> ^-> T<unit>
                    "setColumns" => Type.ArrayOf Column.[t] ^-> T<unit>
                    "setData" => Type.ArrayOf t * !?T<bool> ^-> T<unit>
                    "setOptions" => Options.[t] ^-> T<unit>
                    "setSelectedRows" => T<int[] -> unit>
                    "setSelectionModel" => SelectionModel.[t] ^-> T<unit>
                    "setSortColumn" => T<string> * T<bool> ^-> T<unit>
                    "setSortColumns" => Type.ArrayOf SortColumn ^-> T<unit>
                    "showHeaderRowColumns" => T<unit -> unit>
                    "showTopPanel" => T<unit -> unit>
                    "unregisterPlugin" => T<obj -> unit>
                    "updateCell" => T<int> * T<int> ^-> T<unit>
                    "updateColumnHeader" => T<string> * T<string> * T<string> * T<unit>
                    "updateRow" => T<int -> unit>
                    "updateRowCount" => T<unit -> unit>
                ]

    module Controls =

        let ColumnPickerOptions =
            Generic - fun (t: CodeModel.TypeParameter) ->
                Pattern.Config "Slick.Controls.ColumnPickerOptions" {
                    Required = []
                    Optional =
                        [
                            "fadeSpeed", T<int>
                        ]
                }

        let ColumnPicker =
            Generic - fun (t: CodeModel.TypeParameter) ->
                Class "Slick.Controls.ColumnPicker"
                |+> Static [
                    Constructor (Type.ArrayOf Column.[t] * Grid_t.[t] * ColumnPickerOptions.[t])
                    Constructor (Type.ArrayOf Column.[t] * Grid_t.[t] * Options.[t])
                ]
                |+> Instance [
                        "handleHeaderContextMenu" => T<Event> * T<obj> ^-> T<unit>
                        "init" => T<unit -> unit>
                        "updateColumn" => T<Event> ^-> T<unit>
                    ]

        let NavState =
            Class "Slick.Controls.NavState"
            |+> Instance [
                    "canGotoFirst" => T<bool>
                    "canGotoLast" => T<bool>
                    "canGotoNext" => T<bool>
                    "canGotoPrev" => T<bool>
                    "pagingInfo" => Data.PagingInfo
                ]

        let Pager =
            Generic - fun (t: CodeModel.TypeParameter) ->
                Class "Slick.Controls.Pager"
                |+> Static [Constructor (Data.DataView.[t] * Grid.[t] * T<JQuery>)]
                |+> Instance [
                        "getNavState" => T<unit> ^-> NavState
                        "gotoFirst" => T<unit -> unit>
                        "gotoLast" => T<unit -> unit>
                        "gotoNext" => T<unit -> unit>
                        "gotoPrev" => T<unit -> unit>
                        "init" => T<unit -> unit>
                        "setPageSize" => T<int -> unit>
                    ]

    let Assembly =
        Assembly [
            Namespace "WebSharper.SlickGrid.Resources.Lib" [
                Res.JQueryEventDrag
                Res.JQueryEventDrop
            ]
            Namespace "WebSharper.SlickGrid.Resources.Plugins" [
                Res.Plugins.Autotooltips
                Res.Plugins.Cellcopymanager
                Res.Plugins.Cellrangedecorator
                Res.Plugins.Cellrangeselector
                Res.Plugins.Cellselectionmodel
                Res.Plugins.Checkboxselectcolumn
                Res.Plugins.HeaderMenuCss
                Res.Plugins.HeaderMenu
                Res.Plugins.HeaderButtonsCss
                Res.Plugins.HeaderButtons
                Res.Plugins.Rowmovemanager
                Res.Plugins.Rowselectionmodel
            ]
            Namespace "WebSharper.SlickGrid.Resources.Controls" [
                Res.Controls.ColumnpickerCss
                Res.Controls.Columnpicker
                Res.Controls.PagerCss
                Res.Controls.Pager
            ]
            Namespace "WebSharper.SlickGrid.Resources" [
                Res.Core
                Res.Css
                Res.DefaultTheme
                Res.Js
                Res.Dataview
                Res.Editors
                Res.Formatters
                Res.GroupItemMetadataProvider
            ]
            Namespace "WebSharper.SlickGrid.Slick" [
                Box
                AbsBox
                AutoTooltipsOptions
                AutoTooltips
                |> Requires [Res.Plugins.Autotooltips]
                EditorGenerator
                ValidationResults
                Column
                |> Requires [Res.Plugins.HeaderMenu; Res.Plugins.HeaderButtons]
                BeforeEditCellEventArgs
                BeforeMoveRowsArgs
                CellChangeEventArgs
                CellCoords
                Event
                RangesArgs
                CellCopyManager
                |> Requires [Res.Plugins.Cellcopymanager]
                CellCssEventArgs
                Range
                CellRangeDecoratorOptions
                CellRangeDecorator
                |> Requires [Res.Plugins.Cellrangedecorator]
                CellRangeSelectorOptions
                CellRangeSelector
                |> Requires [Res.Plugins.Cellrangeselector]
                SelectionModel
                CellSelectionModelOptions
                CellSelectionModel
                |> Requires [Res.Plugins.Cellselectionmodel]
                Change
                CheckboxSelectColumnOptions
                CheckboxSelectColumn
                |> Requires [Res.Plugins.Checkboxselectcolumn]
                EditController
                Position
                EditorArgs
                Editor
                EditorEventArgs
                EditorLock
                Editors
                |> Requires [Res.Editors]
                Formatters
                |> Requires [Res.Formatters]
                FromToRangesArgs
                NonDataItem
                GroupTotals
                Group
                HeaderEventArgs
                IEditorFactory
                IFormatterFactory
                NewRowEventArgs
                RowMoveManager
                |> Requires [Res.Plugins.Rowmovemanager]
                RowSelectionModelOptions
                RowSelectionModel
                |> Requires [Res.Plugins.Rowselectionmodel]
                RowsEventArgs
                ScrollEventArgs
                SortColumn
                SortEventArgs
                Total
                ValidationError
                VerticalRange
                Options
                Grid
            ]
            Namespace "WebSharper.SlickGrid.Slick.Data" [
                Data.Aggregator
                Data.ColumnMetadata
                Data.Metadata
                Data.GroupItemMetadataProviderOptions
                Data.GroupItemMetadataProvider
                |> Requires [Res.GroupItemMetadataProvider]
                Data.PagingInfo
                Data.PagingOptions
                Data.RefreshHints
                Data.DataViewOptions
                Data.GroupingOptions
                Data.DataView
                |> Requires [Res.Dataview]
            ]
            Namespace "WebSharper.SlickGrid.Slick.Data.Aggregators" [
                Data.Aggregators.AvgAggregator
                Data.Aggregators.MaxAggregator
                Data.Aggregators.MinAggregator
                Data.Aggregators.SumAggregator
            ]
            Namespace "WebSharper.SlickGrid.Slick.Controls" [
                Controls.ColumnPickerOptions
                Controls.ColumnPicker
                |> Requires [Res.Controls.Columnpicker]
                Controls.NavState
                Controls.Pager
                |> Requires [Res.Controls.Pager]
            ]
        ]

open WebSharper.InterfaceGenerator

[<Sealed>]
type SlickExtension() =
    interface IExtension with
        member ext.Assembly = Definition.Assembly

[<assembly: Extension(typeof<SlickExtension>)>]
do ()
