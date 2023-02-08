using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re4QuadExtremeEditor.src.Class.Enums
{
    public enum eLang
    {
        #region necessarios

        // MessageBox texts
        MessageBoxErrorTitle,
        MessageBoxWarningTitle,
        MessageBoxFile16MB,
        MessageBoxFile16Bytes,
        MessageBoxFile0MB,
        MessageBoxFileNotOpen,

        MessageBoxFormClosingTitle,
        MessageBoxFormClosingDialog,

        // nodes names
        NodeESL,
        NodeETS,
        NodeITA,
        NodeAEV,
        NodeEXTRAS,

        // add new object
        AddNewETS,
        AddNewITA,
        AddNewAEV,
        AddNewNull,

        // delete object dialog
        DeleteObjWarning,
        DeleteObjDialog,

        //MultiSelectEditor Finish MessageBox
        MultiSelectEditorFinishMessageBoxTitle,
        MultiSelectEditorFinishMessageBoxDialog,

        //OptionsForm Diretory MessageBox
        OptionsFormDiretoryMessageBoxTitle,
        OptionsFormDiretoryMessageBoxDialog,

        //OptionsForm Warning Load Models
        OptionsFormWarningLoadModelsMessageBoxTitle,
        OptionsFormWarningLoadModelsMessageBoxDialog,

        //OptionsFormSelectDiretory
        OptionsFormSelectDiretoryXFILE,
        OptionsFormSelectDiretoryXSCR,

        // options Use internal language
        OptionsUseInternalLanguage,

        // labels
        labelCamSpeedPercentage,
        labelObjSpeed,

        // DiretorySalvepatch
        DiretoryESL,
        DiretoryETS,
        DiretoryITA,
        DiretoryAEV,

        // room
        SelectedRoom,
        SelectRoom,
        NoneRoom,

        // comboBoxMoveMode
        MoveMode_Enemy_PositionAndRotationAll,
        MoveMode_EtcModel_PositionAndRotationAll,
        MoveMode_EtcModel_Scale,
        MoveMode_Item_PositionAndRotationAll,
        MoveMode_TriggerZone_MoveAll,
        MoveMode_TriggerZone_Point0,
        MoveMode_TriggerZone_Point1,
        MoveMode_TriggerZone_Point2,
        MoveMode_TriggerZone_Point3,
        MoveMode_TriggerZone_Wall01,
        MoveMode_TriggerZone_Wall12,
        MoveMode_TriggerZone_Wall23,
        MoveMode_TriggerZone_Wall30,
        MoveMode_SpecialObj_Position,
        MoveMode_Obj_PositionAndRotationAll,
        MoveMode_Obj_PositionAndRotationY,
        MoveMode_Obj_Position,
        MoveMode_Ashley_Position,
        MoveMode_AshleyZone_MoveAll,
        MoveMode_AshleyZone_Point0,
        MoveMode_AshleyZone_Point1,
        MoveMode_AshleyZone_Point2,
        MoveMode_AshleyZone_Point3,


        // item rotation options
        RotationXYZ,
        RotationXZY,
        RotationYXZ,
        RotationYZX,
        RotationZYX,
        RotationZXY,
        RotationXY,
        RotationXZ,
        RotationYX,
        RotationYZ,
        RotationZX,
        RotationZY,
        RotationX,
        RotationY,
        RotationZ,

        // menus
        // subsubmenu Save,
        toolStripMenuItemSaveETS,
        toolStripMenuItemSaveITA,
        toolStripMenuItemSaveAEV,
        toolStripMenuItemSaveETS_Classic,
        toolStripMenuItemSaveITA_Classic,
        toolStripMenuItemSaveAEV_Classic,
        toolStripMenuItemSaveETS_UHD,
        toolStripMenuItemSaveITA_UHD,
        toolStripMenuItemSaveAEV_UHD,

        // subsubmenu Save As...,
        toolStripMenuItemSaveAsETS,
        toolStripMenuItemSaveAsITA,
        toolStripMenuItemSaveAsAEV,
        toolStripMenuItemSaveAsETS_Classic,
        toolStripMenuItemSaveAsITA_Classic,
        toolStripMenuItemSaveAsAEV_Classic,
        toolStripMenuItemSaveAsETS_UHD,
        toolStripMenuItemSaveAsITA_UHD,
        toolStripMenuItemSaveAsAEV_UHD,

        // subsubmenu Save As (Convert),
        toolStripMenuItemSaveConverterETS,
        toolStripMenuItemSaveConverterITA,
        toolStripMenuItemSaveConverterAEV,
        toolStripMenuItemSaveConverterETS_Classic,
        toolStripMenuItemSaveConverterITA_Classic,
        toolStripMenuItemSaveConverterAEV_Classic,
        toolStripMenuItemSaveConverterETS_UHD,
        toolStripMenuItemSaveConverterITA_UHD,
        toolStripMenuItemSaveConverterAEV_UHD,


        // enemy Sets

        EnemyExtraSegmentSegund,
        EnemyExtraSegmentThird,
        EnemyExtraSegmentNoSound,
        


        #endregion

        #region usado somente quando ah uma tradução carregada

        // main form

        // menu principal,
        toolStripMenuItemFile,
        toolStripMenuItemEdit,
        toolStripMenuItemView,
        toolStripMenuItemMisc,
        //submenu File,
        toolStripMenuItemNewFile,
        toolStripMenuItemOpen,
        toolStripMenuItemSave,
        toolStripMenuItemSaveAs,
        toolStripMenuItemSaveConverter,
        toolStripMenuItemClear,
        toolStripMenuItemClose,
        // subsubmenu New,
        toolStripMenuItemNewESL,
        toolStripMenuItemNewETS_Classic,
        toolStripMenuItemNewITA_Classic,
        toolStripMenuItemNewAEV_Classic,
        toolStripMenuItemNewETS_UHD,
        toolStripMenuItemNewITA_UHD,
        toolStripMenuItemNewAEV_UHD,
        // subsubmenu Open,
        toolStripMenuItemOpenESL,
        toolStripMenuItemOpenETS_Classic,
        toolStripMenuItemOpenITA_Classic,
        toolStripMenuItemOpenAEV_Classic,
        toolStripMenuItemOpenETS_UHD,
        toolStripMenuItemOpenITA_UHD,
        toolStripMenuItemOpenAEV_UHD,
        // subsubmenu Save,
        toolStripMenuItemSaveESL,
        toolStripMenuItemSaveDirectories,
        // subsubmenu Save As...,
        toolStripMenuItemSaveAsESL,
        // subsubmenu Clear,
        toolStripMenuItemClearESL,
        toolStripMenuItemClearETS,
        toolStripMenuItemClearITA,
        toolStripMenuItemClearAEV,

        // sub menu edit,
        toolStripMenuItemAddNewObj,
        toolStripMenuItemDeleteSelectedObj,
        toolStripMenuItemMoveUp,
        toolStripMenuItemMoveDown,
        toolStripMenuItemSearch,

        // sub menu Misc,
        toolStripMenuItemOptions,
        toolStripMenuItemCredits,

        // sub menu View,
        toolStripMenuItemHideRoomModel,
        toolStripMenuItemHideEnemyESL,
        toolStripMenuItemHideEtcmodelETS,
        toolStripMenuItemHideItemsITA,
        toolStripMenuItemHideEventsAEV,
        toolStripMenuItemSubMenuEnemy,
        toolStripMenuItemSubMenuItem,
        toolStripMenuItemSubMenuSpecial,
        toolStripMenuItemSubMenuEtcModel,
        toolStripMenuItemNodeDisplayNameInHex,
        toolStripMenuItemResetCamera,
        toolStripMenuItemRefresh,

        // sub menus de view,
        toolStripMenuItemHideDesabledEnemy,
        toolStripMenuItemShowOnlyDefinedRoom,
        toolStripMenuItemAutoDefineRoom,
        toolStripMenuItemItemPositionAtAssociatedObjectLocation,
        toolStripMenuItemHideItemTriggerZone,
        toolStripMenuItemHideItemTriggerRadius,
        toolStripMenuItemHideSpecialTriggerZone,
        toolStripMenuItemHideExtraObjs,
        toolStripMenuItemHideOnlyWarpDoor,
        toolStripMenuItemHideExtraExceptWarpDoor,
        toolStripMenuItemUseMoreSpecialColors,
        toolStripMenuItemEtcModelUseScale,

        //save and open windows

        openFileDialogAEV,
        openFileDialogESL,
        openFileDialogETS,
        openFileDialogITA,
        saveFileDialogAEV,
        saveFileDialogConvertAEV,
        saveFileDialogConvertETS,
        saveFileDialogConvertITA,
        saveFileDialogESL,
        saveFileDialogETS,
        saveFileDialogITA,


        //cameraMove
        buttonGrid,
        labelCamModeText,
        labelMoveCamText,
        CameraMode_Fly,
        CameraMode_Orbit,
        CameraMode_Top,
        CameraMode_Bottom,
        CameraMode_Left,
        CameraMode_Right,
        CameraMode_Front,
        CameraMode_Back,

        //objectMove
        buttonDropToGround,
        checkBoxKeepOnGround,
        checkBoxLockMoveSquareHorizontal,
        checkBoxLockMoveSquareVertical,
        checkBoxMoveRelativeCam,


        //AddNewObjForm
        AddNewObjForm,
        buttonCancel,
        buttonOK,
        labelAmountInfo,
        labelTypeInfo,


        //MultiSelectEditor
        MultiSelectEditor,
        labelValueSumText2,
        labelValueSumText,
        labelPropertyDescriptionText,
        labelChoiseText,
        checkBoxProgressiveSum,
        checkBoxHexadecimal,
        checkBoxDecimal,
        checkBoxCurrentValuePlusValueToAdd,
        buttonSetValue,
        buttonClose,



        //SelectRoomForm
        SelectRoomForm,
        labelInfo,
        buttonLoad,
        buttonCancel2,


        //OptionsForm
        OptionsForm,
        Options_buttonCancel,
        Options_buttonOK,
        checkBoxDisableItemRotations,
        checkBoxForceReloadModels,
        checkBoxIgnoreRotationForZeroXYZ,
        checkBoxIgnoreRotationForZisNotGreaterThanZero,
        groupBoxColors,
        groupBoxDiretory,
        groupBoxFloatStyle,
        groupBoxFractionalPart,
        groupBoxInputFractionalSymbol,
        groupBoxItemRotations,
        groupBoxLanguage,
        groupBoxOutputFractionalSymbol,
        labelDivider,
        labelItemExtraCalculation,
        labelitemRotationOrderText,
        labelLanguageWarning,
        labelMultiplier,
        labelSkyColor,
        labelxfile,
        labelxscr,
        radioButtonAcceptsCommaAndPeriod,
        radioButtonOnlyAcceptComma,
        radioButtonOnlyAcceptPeriod,
        radioButtonOutputComma,
        radioButtonOutputPeriod,
        tabPageDiretory,
        tabPageOthers,

        //SearchForm
        SearchForm,
        checkBoxFilterMode,


        #endregion

        Null
    }
}
