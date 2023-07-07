using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace HalconAndWPF_co_programming
{
    public class ModelService
    {
        public ModelService()
        {
            HOperatorSet.SetSystem("width", 512);
            HOperatorSet.SetSystem("height", 512);
            if (HalconAPI.isWindows)
                HOperatorSet.SetSystem("use_window_thread", "true");
        }
        public void Execute(HTuple hv_WindowHandle, HObject ho_Image, HTuple hv_ModelID)
        {
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Angle = new HTuple(), hv_Score = new HTuple();

            try
            {
                //检索图片,根据模板匹配图像
                HOperatorSet.FindShapeModel(ho_Image, hv_ModelID, 0, (new HTuple(360)).TupleRad()
                    , 0.6, 1, 0.5, "least_squares", 0, 0.9, out hv_Row, out hv_Column, out hv_Angle,
                    out hv_Score);
                //设置字体大小
                set_display_font(hv_WindowHandle, 26, "mono", "true", "false");
                ////显示匹配坐标,
                disp_message(hv_WindowHandle, (("匹配坐标" + hv_Row) + ";") + hv_Column, "window",
                        20, 20, "black", "true");
                HOperatorSet.SetColor(hv_WindowHandle, new HTuple("green"));
                //生成十字准星
                HOperatorSet.DispCross(hv_WindowHandle, hv_Row, hv_Column, 20, (new HTuple(45)).TupleRad());
                HOperatorSet.SetLineWidth(hv_WindowHandle, 2);

                //显示匹配结果               
                HOperatorSet.GetShapeModelContours(out HObject shapemodel, hv_ModelID, 1);
                HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_Row, hv_Column, hv_Angle, out HTuple homMat2D);
                HOperatorSet.AffineTransContourXld(shapemodel, out HObject contoursAffineTrans, homMat2D);
                HOperatorSet.DispObj(contoursAffineTrans, hv_WindowHandle);
                contoursAffineTrans.Dispose();
                homMat2D.Dispose();
            }
            catch (HalconException HDevExpDefaultException)
            {

                hv_Row.Dispose();
                hv_Column.Dispose();
                hv_Angle.Dispose();
                hv_Score.Dispose();
                throw HDevExpDefaultException;
            }
            hv_Row.Dispose();
            hv_Column.Dispose();
            hv_Angle.Dispose();
            hv_Score.Dispose();
        }
        #region 公共方法
        // Procedures 
        // External procedures 
        // Chapter: Matching / Shape-Based
        // Short Description: Display the results of Shape-Based Matching. 
        public void dev_display_shape_matching_results(HTuple hv_ModelID, HTuple hv_Color,
            HTuple hv_Row, HTuple hv_Column, HTuple hv_Angle, HTuple hv_ScaleR, HTuple hv_ScaleC,
            HTuple hv_Model)
        {



            // Local iconic variables 

            HObject ho_ClutterRegion = null, ho_ModelContours = null;
            HObject ho_ContoursAffinTrans = null, ho_RegionAffineTrans = null;

            // Local control variables 

            HTuple hv_WindowHandle = new HTuple();
            HTuple hv_UseClutter = new HTuple(), hv_UseClutter0 = new HTuple();
            HTuple hv_HomMat2D = new HTuple(), hv_ClutterContrast = new HTuple();
            HTuple hv_Index = new HTuple(), hv_Exception = new HTuple();
            HTuple hv_NumMatches = new HTuple(), hv_GenParamValue = new HTuple();
            HTuple hv_HomMat2DInvert = new HTuple(), hv_Match = new HTuple();
            HTuple hv_HomMat2DTranslate = new HTuple(), hv_HomMat2DCompose = new HTuple();
            HTuple hv_Model_COPY_INP_TMP = new HTuple(hv_Model);
            HTuple hv_ScaleC_COPY_INP_TMP = new HTuple(hv_ScaleC);
            HTuple hv_ScaleR_COPY_INP_TMP = new HTuple(hv_ScaleR);

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ClutterRegion);
            HOperatorSet.GenEmptyObj(out ho_ModelContours);
            HOperatorSet.GenEmptyObj(out ho_ContoursAffinTrans);
            HOperatorSet.GenEmptyObj(out ho_RegionAffineTrans);
            try
            {
                //This procedure displays the results of Shape-Based Matching.
                //
                //Ensure that the different models have the same use_clutter value.
                //
                //This procedure displays the results on the active graphics window.
                if (HDevWindowStack.IsOpen())
                {
                    hv_WindowHandle = HDevWindowStack.GetActive();
                }
                //If no graphics window is currently open, nothing can be displayed.
                if ((int)(new HTuple(hv_WindowHandle.TupleEqual(-1))) != 0)
                {
                    ho_ClutterRegion.Dispose();
                    ho_ModelContours.Dispose();
                    ho_ContoursAffinTrans.Dispose();
                    ho_RegionAffineTrans.Dispose();

                    hv_Model_COPY_INP_TMP.Dispose();
                    hv_ScaleC_COPY_INP_TMP.Dispose();
                    hv_ScaleR_COPY_INP_TMP.Dispose();
                    hv_WindowHandle.Dispose();
                    hv_UseClutter.Dispose();
                    hv_UseClutter0.Dispose();
                    hv_HomMat2D.Dispose();
                    hv_ClutterContrast.Dispose();
                    hv_Index.Dispose();
                    hv_Exception.Dispose();
                    hv_NumMatches.Dispose();
                    hv_GenParamValue.Dispose();
                    hv_HomMat2DInvert.Dispose();
                    hv_Match.Dispose();
                    hv_HomMat2DTranslate.Dispose();
                    hv_HomMat2DCompose.Dispose();

                    return;
                }
                //
                hv_UseClutter.Dispose();
                hv_UseClutter = "false";
                try
                {
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        // ho_ClutterRegion.Dispose(); hv_UseClutter0.Dispose(); hv_HomMat2D.Dispose(); hv_ClutterContrast.Dispose();
                        HOperatorSet.GetShapeModelClutter(out ho_ClutterRegion, hv_ModelID.TupleSelect(
                            0), "use_clutter", out hv_UseClutter0, out hv_HomMat2D, out hv_ClutterContrast);
                    }
                    for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_ModelID.TupleLength()
                        )) - 1); hv_Index = (int)hv_Index + 1)
                    {
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            ho_ClutterRegion.Dispose(); hv_UseClutter.Dispose(); hv_HomMat2D.Dispose(); hv_ClutterContrast.Dispose();
                            HOperatorSet.GetShapeModelClutter(out ho_ClutterRegion, hv_ModelID.TupleSelect(
                                hv_Index), "use_clutter", out hv_UseClutter, out hv_HomMat2D, out hv_ClutterContrast);
                        }
                        if ((int)(new HTuple(hv_UseClutter.TupleNotEqual(hv_UseClutter0))) != 0)
                        {
                            throw new HalconException("Shape models are not of the same clutter type");
                        }
                    }
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                }
                if ((int)(new HTuple(hv_UseClutter.TupleEqual("true"))) != 0)
                {
                    if (HDevWindowStack.IsOpen())
                    {
                        HOperatorSet.SetDraw(HDevWindowStack.GetActive(), "margin");
                    }
                    //For clutter-enabled models, the Color tuple should have either
                    //exactly 2 entries, or 2* the number of models. The first color
                    //is used for the match and the second for the clutter region,
                    //respectively.
                    if ((int)((new HTuple((new HTuple(hv_Color.TupleLength())).TupleNotEqual(
                        2 * (new HTuple(hv_ModelID.TupleLength()))))).TupleAnd(new HTuple((new HTuple(hv_Color.TupleLength()
                        )).TupleNotEqual(2)))) != 0)
                    {
                        throw new HalconException("Length of Color does not correspond to models with enabled clutter parameters");
                    }
                }

                hv_NumMatches.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_NumMatches = new HTuple(hv_Row.TupleLength()
                        );
                }
                if ((int)(new HTuple(hv_NumMatches.TupleGreater(0))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_ScaleR_COPY_INP_TMP.TupleLength())).TupleEqual(
                        1))) != 0)
                    {
                        {
                            HTuple ExpTmpOutVar_0;
                            HOperatorSet.TupleGenConst(hv_NumMatches, hv_ScaleR_COPY_INP_TMP, out ExpTmpOutVar_0);
                            hv_ScaleR_COPY_INP_TMP.Dispose();
                            hv_ScaleR_COPY_INP_TMP = ExpTmpOutVar_0;
                        }
                    }
                    if ((int)(new HTuple((new HTuple(hv_ScaleC_COPY_INP_TMP.TupleLength())).TupleEqual(
                        1))) != 0)
                    {
                        {
                            HTuple ExpTmpOutVar_0;
                            HOperatorSet.TupleGenConst(hv_NumMatches, hv_ScaleC_COPY_INP_TMP, out ExpTmpOutVar_0);
                            hv_ScaleC_COPY_INP_TMP.Dispose();
                            hv_ScaleC_COPY_INP_TMP = ExpTmpOutVar_0;
                        }
                    }
                    if ((int)(new HTuple((new HTuple(hv_Model_COPY_INP_TMP.TupleLength())).TupleEqual(
                        0))) != 0)
                    {
                        hv_Model_COPY_INP_TMP.Dispose();
                        HOperatorSet.TupleGenConst(hv_NumMatches, 0, out hv_Model_COPY_INP_TMP);
                    }
                    else if ((int)(new HTuple((new HTuple(hv_Model_COPY_INP_TMP.TupleLength()
                        )).TupleEqual(1))) != 0)
                    {
                        {
                            HTuple ExpTmpOutVar_0;
                            HOperatorSet.TupleGenConst(hv_NumMatches, hv_Model_COPY_INP_TMP, out ExpTmpOutVar_0);
                            hv_Model_COPY_INP_TMP.Dispose();
                            hv_Model_COPY_INP_TMP = ExpTmpOutVar_0;
                        }
                    }
                    //Redirect all display calls to a buffer window and update the
                    //graphics window only at the end, to speed up the visualization.
                    HOperatorSet.SetWindowParam(hv_WindowHandle, "flush", "false");
                    for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_ModelID.TupleLength()
                        )) - 1); hv_Index = (int)hv_Index + 1)
                    {
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            ho_ModelContours.Dispose();
                            HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_ModelID.TupleSelect(
                                hv_Index), 1);
                        }
                        if ((int)(new HTuple(hv_UseClutter.TupleEqual("true"))) != 0)
                        {
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                ho_ClutterRegion.Dispose(); hv_GenParamValue.Dispose(); hv_HomMat2D.Dispose(); hv_ClutterContrast.Dispose();
                                HOperatorSet.GetShapeModelClutter(out ho_ClutterRegion, hv_ModelID.TupleSelect(
                                    hv_Index), new HTuple(), out hv_GenParamValue, out hv_HomMat2D, out hv_ClutterContrast);
                            }
                            hv_HomMat2DInvert.Dispose();
                            HOperatorSet.HomMat2dInvert(hv_HomMat2D, out hv_HomMat2DInvert);
                        }
                        if (HDevWindowStack.IsOpen())
                        {
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                HOperatorSet.SetColor(HDevWindowStack.GetActive(), hv_Color.TupleSelect(
                                    hv_Index % (new HTuple(hv_Color.TupleLength()))));
                            }
                        }
                        HTuple end_val56 = hv_NumMatches - 1;
                        HTuple step_val56 = 1;
                        for (hv_Match = 0; hv_Match.Continue(end_val56, step_val56); hv_Match = hv_Match.TupleAdd(step_val56))
                        {
                            if ((int)(new HTuple(hv_Index.TupleEqual(hv_Model_COPY_INP_TMP.TupleSelect(
                                hv_Match)))) != 0)
                            {
                                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                                {
                                    hv_HomMat2DTranslate.Dispose();
                                    get_hom_mat2d_from_matching_result(hv_Row.TupleSelect(hv_Match), hv_Column.TupleSelect(
                                        hv_Match), hv_Angle.TupleSelect(hv_Match), hv_ScaleR_COPY_INP_TMP.TupleSelect(
                                        hv_Match), hv_ScaleC_COPY_INP_TMP.TupleSelect(hv_Match), out hv_HomMat2DTranslate);
                                }
                                ho_ContoursAffinTrans.Dispose();
                                HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_ContoursAffinTrans,
                                    hv_HomMat2DTranslate);
                                if ((int)(new HTuple(hv_UseClutter.TupleEqual("true"))) != 0)
                                {
                                    hv_HomMat2DCompose.Dispose();
                                    HOperatorSet.HomMat2dCompose(hv_HomMat2DTranslate, hv_HomMat2DInvert,
                                        out hv_HomMat2DCompose);
                                    ho_RegionAffineTrans.Dispose();
                                    HOperatorSet.AffineTransRegion(ho_ClutterRegion, out ho_RegionAffineTrans,
                                        hv_HomMat2DCompose, "constant");
                                    if ((int)(new HTuple((new HTuple(hv_Color.TupleLength())).TupleEqual(
                                        2))) != 0)
                                    {
                                        if (HDevWindowStack.IsOpen())
                                        {
                                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                                            {
                                                HOperatorSet.SetColor(HDevWindowStack.GetActive(), hv_Color.TupleSelect(
                                                    1));
                                            }
                                        }
                                        if (HDevWindowStack.IsOpen())
                                        {
                                            HOperatorSet.DispObj(ho_RegionAffineTrans, HDevWindowStack.GetActive()
                                                );
                                        }
                                        if (HDevWindowStack.IsOpen())
                                        {
                                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                                            {
                                                HOperatorSet.SetColor(HDevWindowStack.GetActive(), hv_Color.TupleSelect(
                                                    0));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (HDevWindowStack.IsOpen())
                                        {
                                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                                            {
                                                HOperatorSet.SetColor(HDevWindowStack.GetActive(), hv_Color.TupleSelect(
                                                    (hv_Index * 2) + 1));
                                            }
                                        }
                                        if (HDevWindowStack.IsOpen())
                                        {
                                            HOperatorSet.DispObj(ho_RegionAffineTrans, HDevWindowStack.GetActive()
                                                );
                                        }
                                        if (HDevWindowStack.IsOpen())
                                        {
                                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                                            {
                                                HOperatorSet.SetColor(HDevWindowStack.GetActive(), hv_Color.TupleSelect(
                                                    hv_Index * 2));
                                            }
                                        }
                                    }
                                }
                                if (HDevWindowStack.IsOpen())
                                {
                                    HOperatorSet.DispObj(ho_ContoursAffinTrans, HDevWindowStack.GetActive()
                                        );
                                }
                            }
                        }
                    }
                    //Copy the content of the buffer window to the graphics window.
                    HOperatorSet.SetWindowParam(hv_WindowHandle, "flush", "true");
                    HOperatorSet.FlushBuffer(hv_WindowHandle);
                }
                ho_ClutterRegion.Dispose();
                ho_ModelContours.Dispose();
                ho_ContoursAffinTrans.Dispose();
                ho_RegionAffineTrans.Dispose();

                hv_Model_COPY_INP_TMP.Dispose();
                hv_ScaleC_COPY_INP_TMP.Dispose();
                hv_ScaleR_COPY_INP_TMP.Dispose();
                hv_WindowHandle.Dispose();
                hv_UseClutter.Dispose();
                hv_UseClutter0.Dispose();
                hv_HomMat2D.Dispose();
                hv_ClutterContrast.Dispose();
                hv_Index.Dispose();
                hv_Exception.Dispose();
                hv_NumMatches.Dispose();
                hv_GenParamValue.Dispose();
                hv_HomMat2DInvert.Dispose();
                hv_Match.Dispose();
                hv_HomMat2DTranslate.Dispose();
                hv_HomMat2DCompose.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ClutterRegion.Dispose();
                ho_ModelContours.Dispose();
                ho_ContoursAffinTrans.Dispose();
                ho_RegionAffineTrans.Dispose();

                hv_Model_COPY_INP_TMP.Dispose();
                hv_ScaleC_COPY_INP_TMP.Dispose();
                hv_ScaleR_COPY_INP_TMP.Dispose();
                hv_WindowHandle.Dispose();
                hv_UseClutter.Dispose();
                hv_UseClutter0.Dispose();
                hv_HomMat2D.Dispose();
                hv_ClutterContrast.Dispose();
                hv_Index.Dispose();
                hv_Exception.Dispose();
                hv_NumMatches.Dispose();
                hv_GenParamValue.Dispose();
                hv_HomMat2DInvert.Dispose();
                hv_Match.Dispose();
                hv_HomMat2DTranslate.Dispose();
                hv_HomMat2DCompose.Dispose();

                throw HDevExpDefaultException;
            }
        }

        // Chapter: Graphics / Text
        // Short Description: Write one or multiple text messages. 
        public void disp_message(HTuple hv_WindowHandle, HTuple hv_String, HTuple hv_CoordSystem,
            HTuple hv_Row, HTuple hv_Column, HTuple hv_Color, HTuple hv_Box)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_GenParamName = new HTuple(), hv_GenParamValue = new HTuple();
            HTuple hv_Color_COPY_INP_TMP = new HTuple(hv_Color);
            HTuple hv_Column_COPY_INP_TMP = new HTuple(hv_Column);
            HTuple hv_CoordSystem_COPY_INP_TMP = new HTuple(hv_CoordSystem);
            HTuple hv_Row_COPY_INP_TMP = new HTuple(hv_Row);

            // Initialize local and output iconic variables 
            try
            {
                //This procedure displays text in a graphics window.
                //
                //Input parameters:
                //WindowHandle: The WindowHandle of the graphics window, where
                //   the message should be displayed.
                //String: A tuple of strings containing the text messages to be displayed.
                //CoordSystem: If set to 'window', the text position is given
                //   with respect to the window coordinate system.
                //   If set to 'image', image coordinates are used.
                //   (This may be useful in zoomed images.)
                //Row: The row coordinate of the desired text position.
                //   You can pass a single value or a tuple of values.
                //   See the explanation below.
                //   Default: 12.
                //Column: The column coordinate of the desired text position.
                //   You can pass a single value or a tuple of values.
                //   See the explanation below.
                //   Default: 12.
                //Color: defines the color of the text as string.
                //   If set to [] or '' the currently set color is used.
                //   If a tuple of strings is passed, the colors are used cyclically
                //   for every text position defined by Row and Column,
                //   or every new text line in case of |Row| == |Column| == 1.
                //Box: A tuple controlling a possible box surrounding the text.
                //   Its entries:
                //   - Box[0]: Controls the box and its color. Possible values:
                //     -- 'true' (Default): An orange box is displayed.
                //     -- 'false': No box is displayed.
                //     -- color string: A box is displayed in the given color, e.g., 'white', '#FF00CC'.
                //   - Box[1] (Optional): Controls the shadow of the box. Possible values:
                //     -- 'true' (Default): A shadow is displayed in
                //               darker orange if Box[0] is not a color and in 'white' otherwise.
                //     -- 'false': No shadow is displayed.
                //     -- color string: A shadow is displayed in the given color, e.g., 'white', '#FF00CC'.
                //
                //It is possible to display multiple text strings in a single call.
                //In this case, some restrictions apply on the
                //parameters String, Row, and Column:
                //They can only have either 1 entry or n entries.
                //Behavior in the different cases:
                //   - Multiple text positions are specified, i.e.,
                //       - |Row| == n, |Column| == n
                //       - |Row| == n, |Column| == 1
                //       - |Row| == 1, |Column| == n
                //     In this case we distinguish:
                //       - |String| == n: Each element of String is displayed
                //                        at the corresponding position.
                //       - |String| == 1: String is displayed n times
                //                        at the corresponding positions.
                //   - Exactly one text position is specified,
                //      i.e., |Row| == |Column| == 1:
                //      Each element of String is display in a new textline.
                //
                //
                //Convert the parameters for disp_text.
                if ((int)((new HTuple(hv_Row_COPY_INP_TMP.TupleEqual(new HTuple()))).TupleOr(
                    new HTuple(hv_Column_COPY_INP_TMP.TupleEqual(new HTuple())))) != 0)
                {

                    hv_Color_COPY_INP_TMP.Dispose();
                    hv_Column_COPY_INP_TMP.Dispose();
                    hv_CoordSystem_COPY_INP_TMP.Dispose();
                    hv_Row_COPY_INP_TMP.Dispose();
                    hv_GenParamName.Dispose();
                    hv_GenParamValue.Dispose();

                    return;
                }
                if ((int)(new HTuple(hv_Row_COPY_INP_TMP.TupleEqual(-1))) != 0)
                {
                    hv_Row_COPY_INP_TMP.Dispose();
                    hv_Row_COPY_INP_TMP = 12;
                }
                if ((int)(new HTuple(hv_Column_COPY_INP_TMP.TupleEqual(-1))) != 0)
                {
                    hv_Column_COPY_INP_TMP.Dispose();
                    hv_Column_COPY_INP_TMP = 12;
                }
                //
                //Convert the parameter Box to generic parameters.
                hv_GenParamName.Dispose();
                hv_GenParamName = new HTuple();
                hv_GenParamValue.Dispose();
                hv_GenParamValue = new HTuple();
                if ((int)(new HTuple((new HTuple(hv_Box.TupleLength())).TupleGreater(0))) != 0)
                {
                    if ((int)(new HTuple(((hv_Box.TupleSelect(0))).TupleEqual("false"))) != 0)
                    {
                        //Display no box
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_GenParamName = hv_GenParamName.TupleConcat(
                                    "box");
                                hv_GenParamName.Dispose();
                                hv_GenParamName = ExpTmpLocalVar_GenParamName;
                            }
                        }
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_GenParamValue = hv_GenParamValue.TupleConcat(
                                    "false");
                                hv_GenParamValue.Dispose();
                                hv_GenParamValue = ExpTmpLocalVar_GenParamValue;
                            }
                        }
                    }
                    else if ((int)(new HTuple(((hv_Box.TupleSelect(0))).TupleNotEqual(
                        "true"))) != 0)
                    {
                        //Set a color other than the default.
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_GenParamName = hv_GenParamName.TupleConcat(
                                    "box_color");
                                hv_GenParamName.Dispose();
                                hv_GenParamName = ExpTmpLocalVar_GenParamName;
                            }
                        }
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_GenParamValue = hv_GenParamValue.TupleConcat(
                                    hv_Box.TupleSelect(0));
                                hv_GenParamValue.Dispose();
                                hv_GenParamValue = ExpTmpLocalVar_GenParamValue;
                            }
                        }
                    }
                }
                if ((int)(new HTuple((new HTuple(hv_Box.TupleLength())).TupleGreater(1))) != 0)
                {
                    if ((int)(new HTuple(((hv_Box.TupleSelect(1))).TupleEqual("false"))) != 0)
                    {
                        //Display no shadow.
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_GenParamName = hv_GenParamName.TupleConcat(
                                    "shadow");
                                hv_GenParamName.Dispose();
                                hv_GenParamName = ExpTmpLocalVar_GenParamName;
                            }
                        }
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_GenParamValue = hv_GenParamValue.TupleConcat(
                                    "false");
                                hv_GenParamValue.Dispose();
                                hv_GenParamValue = ExpTmpLocalVar_GenParamValue;
                            }
                        }
                    }
                    else if ((int)(new HTuple(((hv_Box.TupleSelect(1))).TupleNotEqual(
                        "true"))) != 0)
                    {
                        //Set a shadow color other than the default.
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_GenParamName = hv_GenParamName.TupleConcat(
                                    "shadow_color");
                                hv_GenParamName.Dispose();
                                hv_GenParamName = ExpTmpLocalVar_GenParamName;
                            }
                        }
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_GenParamValue = hv_GenParamValue.TupleConcat(
                                    hv_Box.TupleSelect(1));
                                hv_GenParamValue.Dispose();
                                hv_GenParamValue = ExpTmpLocalVar_GenParamValue;
                            }
                        }
                    }
                }
                //Restore default CoordSystem behavior.
                if ((int)(new HTuple(hv_CoordSystem_COPY_INP_TMP.TupleNotEqual("window"))) != 0)
                {
                    hv_CoordSystem_COPY_INP_TMP.Dispose();
                    hv_CoordSystem_COPY_INP_TMP = "image";
                }
                //
                if ((int)(new HTuple(hv_Color_COPY_INP_TMP.TupleEqual(""))) != 0)
                {
                    //disp_text does not accept an empty string for Color.
                    hv_Color_COPY_INP_TMP.Dispose();
                    hv_Color_COPY_INP_TMP = new HTuple();
                }
                //
                HOperatorSet.DispText(hv_WindowHandle, hv_String, hv_CoordSystem_COPY_INP_TMP,
                    hv_Row_COPY_INP_TMP, hv_Column_COPY_INP_TMP, hv_Color_COPY_INP_TMP, hv_GenParamName,
                    hv_GenParamValue);

                hv_Color_COPY_INP_TMP.Dispose();
                hv_Column_COPY_INP_TMP.Dispose();
                hv_CoordSystem_COPY_INP_TMP.Dispose();
                hv_Row_COPY_INP_TMP.Dispose();
                hv_GenParamName.Dispose();
                hv_GenParamValue.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {

                hv_Color_COPY_INP_TMP.Dispose();
                hv_Column_COPY_INP_TMP.Dispose();
                hv_CoordSystem_COPY_INP_TMP.Dispose();
                hv_Row_COPY_INP_TMP.Dispose();
                hv_GenParamName.Dispose();
                hv_GenParamValue.Dispose();

                throw HDevExpDefaultException;
            }
        }

        // Chapter: Matching / Shape-Based
        // Short Description: Calculate the transformation matrix for results of Shape-Based Matching. 
        public void get_hom_mat2d_from_matching_result(HTuple hv_Row, HTuple hv_Column,
            HTuple hv_Angle, HTuple hv_ScaleR, HTuple hv_ScaleC, out HTuple hv_HomMat2D)
        {



            // Local control variables 

            HTuple hv_HomMat2DIdentity = new HTuple();
            HTuple hv_HomMat2DScale = new HTuple(), hv_HomMat2DRotate = new HTuple();
            // Initialize local and output iconic variables 
            hv_HomMat2D = new HTuple();
            try
            {
                //This procedure calculates the transformation matrix for the model contours
                //from the results of Shape-Based Matching.
                //
                hv_HomMat2DIdentity.Dispose();
                HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity);
                hv_HomMat2DScale.Dispose();
                HOperatorSet.HomMat2dScale(hv_HomMat2DIdentity, hv_ScaleR, hv_ScaleC, 0, 0,
                    out hv_HomMat2DScale);
                hv_HomMat2DRotate.Dispose();
                HOperatorSet.HomMat2dRotate(hv_HomMat2DScale, hv_Angle, 0, 0, out hv_HomMat2DRotate);
                hv_HomMat2D.Dispose();
                HOperatorSet.HomMat2dTranslate(hv_HomMat2DRotate, hv_Row, hv_Column, out hv_HomMat2D);

                hv_HomMat2DIdentity.Dispose();
                hv_HomMat2DScale.Dispose();
                hv_HomMat2DRotate.Dispose();

                return;


            }
            catch (HalconException HDevExpDefaultException)
            {

                hv_HomMat2DIdentity.Dispose();
                hv_HomMat2DScale.Dispose();
                hv_HomMat2DRotate.Dispose();

                throw HDevExpDefaultException;
            }
        }

        // Chapter: Graphics / Text
        // Short Description: Set font independent of OS 
        public void set_display_font(HTuple hv_WindowHandle, HTuple hv_Size, HTuple hv_Font,
            HTuple hv_Bold, HTuple hv_Slant)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_OS = new HTuple(), hv_Fonts = new HTuple();
            HTuple hv_Style = new HTuple(), hv_Exception = new HTuple();
            HTuple hv_AvailableFonts = new HTuple(), hv_Fdx = new HTuple();
            HTuple hv_Indices = new HTuple();
            HTuple hv_Font_COPY_INP_TMP = new HTuple(hv_Font);
            HTuple hv_Size_COPY_INP_TMP = new HTuple(hv_Size);

            // Initialize local and output iconic variables 
            try
            {
                //This procedure sets the text font of the current window with
                //the specified attributes.
                //
                //Input parameters:
                //WindowHandle: The graphics window for which the font will be set
                //Size: The font size. If Size=-1, the default of 16 is used.
                //Bold: If set to 'true', a bold font is used
                //Slant: If set to 'true', a slanted font is used
                //
                hv_OS.Dispose();
                HOperatorSet.GetSystem("operating_system", out hv_OS);
                if ((int)((new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(new HTuple()))).TupleOr(
                    new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(-1)))) != 0)
                {
                    hv_Size_COPY_INP_TMP.Dispose();
                    hv_Size_COPY_INP_TMP = 16;
                }
                if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Win"))) != 0)
                {
                    //Restore previous behavior
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        {
                            HTuple
                              ExpTmpLocalVar_Size = ((1.13677 * hv_Size_COPY_INP_TMP)).TupleInt()
                                ;
                            hv_Size_COPY_INP_TMP.Dispose();
                            hv_Size_COPY_INP_TMP = ExpTmpLocalVar_Size;
                        }
                    }
                }
                else
                {
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        {
                            HTuple
                              ExpTmpLocalVar_Size = hv_Size_COPY_INP_TMP.TupleInt()
                                ;
                            hv_Size_COPY_INP_TMP.Dispose();
                            hv_Size_COPY_INP_TMP = ExpTmpLocalVar_Size;
                        }
                    }
                }
                if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("Courier"))) != 0)
                {
                    hv_Fonts.Dispose();
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "Courier";
                    hv_Fonts[1] = "Courier 10 Pitch";
                    hv_Fonts[2] = "Courier New";
                    hv_Fonts[3] = "CourierNew";
                    hv_Fonts[4] = "Liberation Mono";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))) != 0)
                {
                    hv_Fonts.Dispose();
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "Consolas";
                    hv_Fonts[1] = "Menlo";
                    hv_Fonts[2] = "Courier";
                    hv_Fonts[3] = "Courier 10 Pitch";
                    hv_Fonts[4] = "FreeMono";
                    hv_Fonts[5] = "Liberation Mono";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
                {
                    hv_Fonts.Dispose();
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "Luxi Sans";
                    hv_Fonts[1] = "DejaVu Sans";
                    hv_Fonts[2] = "FreeSans";
                    hv_Fonts[3] = "Arial";
                    hv_Fonts[4] = "Liberation Sans";
                }
                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
                {
                    hv_Fonts.Dispose();
                    hv_Fonts = new HTuple();
                    hv_Fonts[0] = "Times New Roman";
                    hv_Fonts[1] = "Luxi Serif";
                    hv_Fonts[2] = "DejaVu Serif";
                    hv_Fonts[3] = "FreeSerif";
                    hv_Fonts[4] = "Utopia";
                    hv_Fonts[5] = "Liberation Serif";
                }
                else
                {
                    hv_Fonts.Dispose();
                    hv_Fonts = new HTuple(hv_Font_COPY_INP_TMP);
                }
                hv_Style.Dispose();
                hv_Style = "";
                if ((int)(new HTuple(hv_Bold.TupleEqual("true"))) != 0)
                {
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        {
                            HTuple
                              ExpTmpLocalVar_Style = hv_Style + "Bold";
                            hv_Style.Dispose();
                            hv_Style = ExpTmpLocalVar_Style;
                        }
                    }
                }
                else if ((int)(new HTuple(hv_Bold.TupleNotEqual("false"))) != 0)
                {
                    hv_Exception.Dispose();
                    hv_Exception = "Wrong value of control parameter Bold";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Slant.TupleEqual("true"))) != 0)
                {
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        {
                            HTuple
                              ExpTmpLocalVar_Style = hv_Style + "Italic";
                            hv_Style.Dispose();
                            hv_Style = ExpTmpLocalVar_Style;
                        }
                    }
                }
                else if ((int)(new HTuple(hv_Slant.TupleNotEqual("false"))) != 0)
                {
                    hv_Exception.Dispose();
                    hv_Exception = "Wrong value of control parameter Slant";
                    throw new HalconException(hv_Exception);
                }
                if ((int)(new HTuple(hv_Style.TupleEqual(""))) != 0)
                {
                    hv_Style.Dispose();
                    hv_Style = "Normal";
                }
                hv_AvailableFonts.Dispose();
                HOperatorSet.QueryFont(hv_WindowHandle, out hv_AvailableFonts);
                hv_Font_COPY_INP_TMP.Dispose();
                hv_Font_COPY_INP_TMP = "";
                for (hv_Fdx = 0; (int)hv_Fdx <= (int)((new HTuple(hv_Fonts.TupleLength())) - 1); hv_Fdx = (int)hv_Fdx + 1)
                {
                    hv_Indices.Dispose();
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_Indices = hv_AvailableFonts.TupleFind(
                            hv_Fonts.TupleSelect(hv_Fdx));
                    }
                    if ((int)(new HTuple((new HTuple(hv_Indices.TupleLength())).TupleGreater(
                        0))) != 0)
                    {
                        if ((int)(new HTuple(((hv_Indices.TupleSelect(0))).TupleGreaterEqual(0))) != 0)
                        {
                            hv_Font_COPY_INP_TMP.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_Font_COPY_INP_TMP = hv_Fonts.TupleSelect(
                                    hv_Fdx);
                            }
                            break;
                        }
                    }
                }
                if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual(""))) != 0)
                {
                    throw new HalconException("Wrong value of control parameter Font");
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    {
                        HTuple
                          ExpTmpLocalVar_Font = (((hv_Font_COPY_INP_TMP + "-") + hv_Style) + "-") + hv_Size_COPY_INP_TMP;
                        hv_Font_COPY_INP_TMP.Dispose();
                        hv_Font_COPY_INP_TMP = ExpTmpLocalVar_Font;
                    }
                }
                HOperatorSet.SetFont(hv_WindowHandle, hv_Font_COPY_INP_TMP);

                hv_Font_COPY_INP_TMP.Dispose();
                hv_Size_COPY_INP_TMP.Dispose();
                hv_OS.Dispose();
                hv_Fonts.Dispose();
                hv_Style.Dispose();
                hv_Exception.Dispose();
                hv_AvailableFonts.Dispose();
                hv_Fdx.Dispose();
                hv_Indices.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {

                hv_Font_COPY_INP_TMP.Dispose();
                hv_Size_COPY_INP_TMP.Dispose();
                hv_OS.Dispose();
                hv_Fonts.Dispose();
                hv_Style.Dispose();
                hv_Exception.Dispose();
                hv_AvailableFonts.Dispose();
                hv_Fdx.Dispose();
                hv_Indices.Dispose();

                throw HDevExpDefaultException;
            }
        }
        #endregion 
    }
}
